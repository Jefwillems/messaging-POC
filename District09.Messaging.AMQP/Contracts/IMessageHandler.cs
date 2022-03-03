namespace District09.Messaging.AMQP.Contracts;

public interface IMessageHandler<TMessageDataType>
{
    HandlerResult HandleMessage(ReceivedMessage<TMessageDataType> message);
}