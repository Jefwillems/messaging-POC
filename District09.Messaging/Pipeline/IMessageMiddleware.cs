namespace District09.Messaging.Pipeline;

public interface IMessageMiddleware<TDataType>
{
    MiddlewareContext<TDataType> Execute(MiddlewareContext<TDataType> context, Action next);
}