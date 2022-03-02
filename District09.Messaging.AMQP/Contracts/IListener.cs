namespace District09.Messaging.AMQP.Contracts;

public interface IListener<TDataType>
{
    Task StartListener(string queueName);
    Task StopListener();
}