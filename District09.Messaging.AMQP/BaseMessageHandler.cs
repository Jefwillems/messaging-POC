using District09.Messaging.AMQP.Pipeline;

namespace District09.Messaging.AMQP;

public abstract class BaseMessageHandler<TDataType> : IListenerMiddleware<TDataType>
{
    protected abstract void HandleMessage(MiddlewareContext<TDataType> context);

    public MiddlewareContext<TDataType> Execute(MiddlewareContext<TDataType> context, Action next)
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