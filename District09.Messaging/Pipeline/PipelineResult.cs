using Apache.NMS;

namespace District09.Messaging.Pipeline;

public class PipelineResult<TDataType>
{
    public ITextMessage Original { get; set; }
    public TDataType Result { get; set; }
    public Exception? Exception { get; set; }

    public bool IsFailed() => Exception != null;
}