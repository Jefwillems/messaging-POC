namespace District09.Messaging.AMQP.Contracts;

internal interface IListener<TDataType>
{
    Task StartListener(string queueName);
    Task StopListener();
}