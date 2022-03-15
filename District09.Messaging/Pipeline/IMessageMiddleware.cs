namespace District09.Messaging.Pipeline;

public interface IMessageMiddleware<TDataType, TMessageType>
{
    BaseMiddlewareContext<TDataType, TMessageType> Execute(BaseMiddlewareContext<TDataType, TMessageType> context,
        Action next);
}