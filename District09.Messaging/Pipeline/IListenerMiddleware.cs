namespace District09.Messaging.Pipeline;

public interface IListenerMiddleware<TDataType, TMessageType> : IMessageMiddleware<TDataType, TMessageType>
{
}