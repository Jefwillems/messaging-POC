using System.Text.Json;
using District09.Messaging.AMQP.Configuration;
using District09.Messaging.AMQP.Contracts;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.AMQP;

public class MessagePublisher<TDataType> : IMessagePublisher<TDataType>
{
    private readonly ILogger<MessagePublisher<TDataType>> _logger;
    private readonly IAmqWrapper _wrapper;
    private readonly string _queueName;

    public MessagePublisher(
        ILogger<MessagePublisher<TDataType>> logger,
        IFinishedConfig config,
        IAmqWrapper wrapper)
    {
        _logger = logger;
        _wrapper = wrapper;
        _queueName = config.GetPublishQueueForType(typeof(IMessagePublisher<TDataType>));
    }

    public void PublishMessage(TDataType message)
    {
        _logger.LogInformation("Publishing message to {Queue}", _queueName);
        var session = _wrapper.GetSession();
        using var queue = session.GetQueue(_queueName);
        using var prod = session.CreateProducer(queue);
        var msg = session.CreateTextMessage(JsonSerializer.Serialize(message));
        prod.Send(msg);
    }
}