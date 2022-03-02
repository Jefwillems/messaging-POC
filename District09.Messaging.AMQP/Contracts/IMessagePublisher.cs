namespace District09.Messaging.AMQP.Contracts;

public interface IMessagePublisher<in TDataType>
{
    void PublishMessage(TDataType message);
}