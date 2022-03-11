namespace District09.Messaging.Contracts;

internal interface IListener<TDataType>
{
    Task StartListener(string queueName);
    Task StopListener();
}