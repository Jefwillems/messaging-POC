namespace District09.Messaging.AMQP.Processors;

public interface IPostProcessor
{
    void PostProcess(ReceivedMessage message, Exception? error);
}