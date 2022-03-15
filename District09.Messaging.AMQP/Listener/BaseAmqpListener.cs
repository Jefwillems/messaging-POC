using Apache.NMS;
using District09.Messaging.AMQP.Contracts;
using District09.Messaging.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.AMQP.Listener;

public abstract class BaseAmqpListener<TDataType> : IListener<TDataType>, IDisposable
{
    protected readonly ILogger<BaseAmqpListener<TDataType>> Logger;
    private readonly IAmqpWrapper _wrapper;
    private readonly IServiceProvider _serviceProvider;
    private ISession? _session;
    private IMessageConsumer? _consumer;

    protected BaseAmqpListener(
        ILogger<BaseAmqpListener<TDataType>> logger,
        IAmqpWrapper wrapper,
        IServiceProvider serviceProvider)
    {
        Logger = logger;
        _wrapper = wrapper;
        _serviceProvider = serviceProvider;
    }

    private void MessageReceivedListener(IMessage message)
    {
        using var scope = _serviceProvider.CreateScope();

        if (message is not ITextMessage textMessage) return;

        HandleMessage(textMessage, scope);
    }

    protected abstract void HandleMessage(ITextMessage message, IServiceScope scope);

    public Task StartListener(string queueName)
    {
        Logger.LogInformation("starting base amqp listener");
        _session = _wrapper.GetSession();
        var queue = _session.GetQueue(queueName);
        _consumer = _session.CreateConsumer(queue);
        _consumer.Listener += MessageReceivedListener;
        return Task.CompletedTask;
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