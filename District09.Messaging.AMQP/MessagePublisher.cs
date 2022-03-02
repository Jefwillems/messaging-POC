using District09.Messaging.AMQP.Configuration;
using District09.Messaging.AMQP.Contracts;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.AMQP;

public class MessagePublisher<TDataType> : IMessagePublisher<TDataType>
{
    private readonly ILogger<MessagePublisher<TDataType>> _logger;
    private readonly string _queueName;

    public MessagePublisher(ILogger<MessagePublisher<TDataType>> logger, IFinishedConfig config)
    {
        _logger = logger;
        _queueName = config.GetPublishQueueForType(typeof(TDataType));
    }

    public void PublishMessage(TDataType message)
    {
        _logger.LogInformation("Publishing message to {Queue}", _queueName);
    }
}