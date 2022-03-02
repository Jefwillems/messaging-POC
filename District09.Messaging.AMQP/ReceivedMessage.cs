using Apache.NMS;

namespace District09.Messaging.AMQP;

public class ReceivedMessage<TDataType>
{
    private ITextMessage _original;
    public TDataType Data { get; }

    public ReceivedMessage(ITextMessage original, TDataType data)
    {
        _original = original;
        Data = data;
    }
}