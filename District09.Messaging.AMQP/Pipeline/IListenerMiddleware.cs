namespace District09.Messaging.AMQP.Pipeline;

public interface IListenerMiddleware<TDataType> : IMessageMiddleware<TDataType>
{
}