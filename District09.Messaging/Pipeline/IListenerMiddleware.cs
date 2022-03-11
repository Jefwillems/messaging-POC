namespace District09.Messaging.Pipeline;

public interface IListenerMiddleware<TDataType> : IMessageMiddleware<TDataType>
{
}