namespace District09.Messaging.Contracts;

public interface IListener<TDataType>
{
    Task StartListener(string queueName);
    Task StopListener();
}