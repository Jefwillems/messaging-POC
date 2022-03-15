using Apache.NMS;
using District09.Messaging.Pipeline;

namespace District09.Messaging.AMQP;

public abstract class AmqpListenerMiddleware<TDataType> : IListenerMiddleware<TDataType, ITextMessage>
{
    public BaseMiddlewareContext<TDataType, ITextMessage> Execute(
        BaseMiddlewareContext<TDataType, ITextMessage> context, Action next)
    {
        return Handle((AmqpContext<TDataType>)context, next);
    }

    protected abstract AmqpContext<TDataType> Handle(AmqpContext<TDataType> context, Action next);
}