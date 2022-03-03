using System.Text.Json;
using Apache.NMS;
using District09.Messaging.AMQP.Contracts;
using District09.Messaging.AMQP.Processors;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.AMQP;

public class Listener<TDataType> : IListener<TDataType>, IDisposable
{
    private readonly ILogger<Listener<TDataType>> _logger;
    private readonly IAmqWrapper _wrapper;
    private readonly IMessageHandler<TDataType> _messageHandler;
    private readonly IEnumerable<IPreProcessor> _preProcessors;
    private readonly IEnumerable<IPostProcessor> _postProcessors;
    private ISession? _session;
    private IMessageConsumer? _consumer;


    public Listener(
        ILogger<Listener<TDataType>> logger,
        IAmqWrapper wrapper,
        IMessageHandler<TDataType> handler,
        IEnumerable<IPreProcessor> preProcessors,
        IEnumerable<IPostProcessor> postProcessors)
    {
        _logger = logger;
        _wrapper = wrapper;
        _messageHandler = handler;
        _preProcessors = preProcessors;
        _postProcessors = postProcessors;
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
        _logger.LogInformation("Received message: {@Message}", message);
        if (message is not ITextMessage textMessage) return;
        var content = JsonSerializer.Deserialize<TDataType>(textMessage.Text);
        if (content == null) return;

        foreach (var processor in _preProcessors)
        {
            processor.PreProcess(textMessage);
        }

        var msg = new ReceivedMessage<TDataType>(textMessage, content);
        _messageHandler.HandleMessage(msg);

        foreach (var processor in _postProcessors)
        {
            processor.PostProcess(msg, null);
        }
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