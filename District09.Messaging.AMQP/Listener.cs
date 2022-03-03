using System.Text.Json;
using Apache.NMS;
using District09.Messaging.AMQP.Contracts;
using District09.Messaging.AMQP.Processors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.AMQP;

public class Listener<TDataType> : IListener<TDataType>, IDisposable
{
    private readonly ILogger<Listener<TDataType>> _logger;
    private readonly IAmqWrapper _wrapper;
    private readonly IServiceProvider _serviceProvider;
    private ISession? _session;
    private IMessageConsumer? _consumer;


    public Listener(
        ILogger<Listener<TDataType>> logger,
        IAmqWrapper wrapper,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _wrapper = wrapper;
        _serviceProvider = serviceProvider;
    }

    public Task StartListener(string queueName)
    {
        _session = _wrapper.GetSession();
        var queue = _session.GetQueue(queueName);
        _consumer = _session.CreateConsumer(queue);
        _consumer.Listener += MessageReceivedListener;
        return Task.CompletedTask;
    }

    private void MessageReceivedListener(IMessage message)
    {
        using var scope = _serviceProvider.CreateScope();

        if (message is not ITextMessage textMessage) return;
        var content = JsonSerializer.Deserialize<TDataType>(textMessage.Text);
        if (content == null) return;

        // Pre process with possible plugins (Apm, tracing, logging, etc)
        var preProcessors = scope.ServiceProvider.GetServices<BasePreProcessor>();
        var preQueue = new ProcessorQueue<ITextMessage>(preProcessors);
        var preProcessedMessage = preQueue.Execute(textMessage);

        var handler = scope.ServiceProvider.GetService<IMessageHandler<TDataType>>() ??
                      throw new Exception($"Message hanndler for type {typeof(TDataType)} is not registered");
        var msg = new ReceivedMessage<TDataType>(preProcessedMessage, content);
        var result = handler.HandleMessage(msg);

        // Post process (no use case yet but might be useful later on)
        var postProcessors = scope.ServiceProvider.GetServices<BasePostProcessor>();
        var postQueue = new ProcessorQueue<HandlerResult>(postProcessors);
        var postProcessed = postQueue.Execute(result);
        // TODO finalize?
        message.Acknowledge();
    }

    public Task StopListener()
    {
        _session?.Close();
        _consumer?.Close();
        return Task.CompletedTask;
    }

    private void Dispose(bool disposing)
    {
        if (!disposing) return;
        _session?.Dispose();
        _consumer?.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}