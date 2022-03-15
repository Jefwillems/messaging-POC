using System.Text.Json;
using Apache.NMS;
using District09.Messaging.Pipeline;

namespace District09.Messaging.AMQP;

public class AmqpContext<TDataType> : BaseMiddlewareContext<TDataType, ITextMessage>
{
    public AmqpContext(ITextMessage message) : base(message)
    {
    }

    public override TDataType? GetParsedMessage()
    {
        return JsonSerializer.Deserialize<TDataType>(Original.Text);
    }
}