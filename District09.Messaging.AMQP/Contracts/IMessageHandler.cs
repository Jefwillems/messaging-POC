namespace District09.Messaging.AMQP.Contracts;

public interface IMessageHandler<TMessageDataType>
{
    void HandleMessage(ReceivedMessage<TMessageDataType> message);
}