namespace District09.Messaging.Pipeline;

public interface IPublisherMiddleware<TDataType> : IMessageMiddleware<TDataType>
{
}