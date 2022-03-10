namespace District09.Messaging.AMQP.Pipeline;

public interface IMessageMiddleware<TDataType>
{
    MiddlewareContext<TDataType> Execute(MiddlewareContext<TDataType> context, Action next);
}