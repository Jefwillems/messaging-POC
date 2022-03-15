namespace District09.Messaging.Pipeline;

public abstract class BaseMiddlewareContext<TDataType, TMessageType>
{
    protected BaseMiddlewareContext(TMessageType textMessage)
    {
        Original = textMessage;
    }

    public IDictionary<string, object> Extra { get; set; } = new Dictionary<string, object>();
    public Exception? Exception { get; set; }
    public abstract TDataType? GetParsedMessage();

    public TMessageType Original { get; set; }

    public bool IsFailed() => Exception != null;
}