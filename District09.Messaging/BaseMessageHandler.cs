using District09.Messaging.Pipeline;

namespace District09.Messaging;

public abstract class BaseMessageHandler<TDataType, TMessageType>
    : IListenerMiddleware<TDataType, TMessageType>

{
    protected abstract void HandleMessage(BaseMiddlewareContext<TDataType, TMessageType> context);

    public BaseMiddlewareContext<TDataType, TMessageType> Execute(
        BaseMiddlewareContext<TDataType, TMessageType> context,
        Action next)
    {
        try
        {
            HandleMessage(context);
            next();
            return context;
        }
        catch (Exception e)
        {
            context.Exception = e;
        }

        return context;
    }
}