using System.Text.Json;
using Apache.NMS;
using District09.Messaging.AMQP.Contracts;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.AMQP;

public class Listener<TDataType> : IListener<TDataType>, IDisposable
{
    private readonly ILogger<Listener<TDataType>> _logger;
    private readonly IAmqWrapper _wrapper;
    private readonly IMessageHandler<TDataType> _messageHandler;
    private ISession? _session;
    private IMessageConsumer? _consumer;


    public Listener(
        ILogger<Listener<TDataType>> logger,
        IAmqWrapper wrapper,
        IMessageHandler<TDataType> handler)
    {
        _logger = logger;
        _wrapper = wrapper;
        _messageHandler = handler;
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
        if (content != null)
        {
            _messageHandler.HandleMessage(new ReceivedMessage<TDataType>(textMessage, content));
            // TODO: tracing, correlation etc
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