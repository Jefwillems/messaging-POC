using Apache.NMS;

namespace District09.Messaging.AMQP;

public class ReceivedMessage
{
}

public class ReceivedMessage<TDataType> : ReceivedMessage
{
    private ITextMessage _original;
    public TDataType Data { get; }

    public ReceivedMessage(ITextMessage original, TDataType data)
    {
        _original = original;
        Data = data;
    }
}