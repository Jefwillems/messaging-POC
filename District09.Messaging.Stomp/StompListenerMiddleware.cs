using Apache.NMS;
using District09.Messaging.Pipeline;

namespace District09.Messaging.Stomp;

public abstract class StompListenerMiddleware<TDataType> : IListenerMiddleware<TDataType, ITextMessage>
{
    public BaseMiddlewareContext<TDataType, ITextMessage> Execute(
        BaseMiddlewareContext<TDataType, ITextMessage> context, Action next)
    {
        return Handle((StompContext<TDataType>)context, next);
    }

    protected abstract StompContext<TDataType> Handle(StompContext<TDataType> context, Action next);
}