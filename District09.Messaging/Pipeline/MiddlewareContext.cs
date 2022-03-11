using System.Text.Json;
using Apache.NMS;

namespace District09.Messaging.Pipeline;

public class MiddlewareContext<TDataType>
{
    public IDictionary<string, object> Extra { get; set; } = new Dictionary<string, object>();
    public ITextMessage Original { get; set; }
    public Exception? Exception { get; set; }

    public MiddlewareContext(ITextMessage original)
    {
        Original = original;
    }

    public TDataType? GetParsedMessage()
    {
        return JsonSerializer.Deserialize<TDataType>(Original.Text);
    }

    public bool IsFailed() => Exception != null;
}