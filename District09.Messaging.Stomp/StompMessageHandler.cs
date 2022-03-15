using Apache.NMS;
using District09.Messaging.Pipeline;

namespace District09.Messaging.Stomp;

public abstract class StompMessageHandler<TDataType> : BaseMessageHandler<TDataType, ITextMessage
>
{
    protected override void HandleMessage(BaseMiddlewareContext<TDataType, ITextMessage> context)
    {
        HandleAmqpMessage((StompContext<TDataType>)context);
    }

    protected abstract void HandleAmqpMessage(StompContext<TDataType> context);
}