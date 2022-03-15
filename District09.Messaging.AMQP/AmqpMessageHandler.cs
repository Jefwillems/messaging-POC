using Apache.NMS;
using District09.Messaging.Pipeline;

namespace District09.Messaging.AMQP;

public abstract class
    AmqpMessageHandler<TDataType> : BaseMessageHandler<TDataType, ITextMessage>
{
    protected override void HandleMessage(BaseMiddlewareContext<TDataType, ITextMessage> context)
    {
        HandleAmqpMessage((AmqpContext<TDataType>)context);
    }

    protected abstract void HandleAmqpMessage(AmqpContext<TDataType> context);
}