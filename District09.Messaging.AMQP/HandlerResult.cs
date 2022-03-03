namespace District09.Messaging.AMQP;

public class HandlerResult
{
    public ReceivedMessage Original { get; set; }
    public Exception? Exception { get; set; }
}