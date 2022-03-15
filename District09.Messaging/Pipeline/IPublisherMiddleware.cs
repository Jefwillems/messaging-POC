namespace District09.Messaging.Pipeline;

public interface IPublisherMiddleware<TDataType, TMessageType> : IMessageMiddleware<TDataType, TMessageType>
{
}