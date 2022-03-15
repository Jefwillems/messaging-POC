using System.Text.Json;
using Apache.NMS;
using District09.Messaging.Pipeline;

namespace District09.Messaging.Stomp;

public class StompContext<TDataType> : BaseMiddlewareContext<TDataType, ITextMessage>
{
    public StompContext(ITextMessage textMessage) : base(textMessage)
    {
    }

    public override TDataType? GetParsedMessage()
    {
        return JsonSerializer.Deserialize<TDataType>(Original.Text);
    }
}