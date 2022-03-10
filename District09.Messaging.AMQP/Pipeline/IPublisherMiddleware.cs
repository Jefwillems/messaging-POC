namespace District09.Messaging.AMQP.Pipeline;

public interface IPublisherMiddleware<TDataType> : IMessageMiddleware<TDataType>
{
}