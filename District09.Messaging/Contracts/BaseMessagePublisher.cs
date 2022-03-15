namespace District09.Messaging.Contracts;

public abstract class BaseMessagePublisher<TDataType> : IMessagePublisher<TDataType>
{
    public abstract void PublishMessage(TDataType message);
}