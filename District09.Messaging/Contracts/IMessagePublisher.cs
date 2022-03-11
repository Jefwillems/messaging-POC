namespace District09.Messaging.Contracts;

public interface IMessagePublisher<in TDataType>
{
    void PublishMessage(TDataType message);
}