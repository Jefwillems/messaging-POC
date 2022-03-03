using District09.Messaging.AMQP;
using District09.Messaging.AMQP.Contracts;

namespace District09.Servicefactory.Test.Api.Handlers;

public class MyMessageHandler : IMessageHandler<MyData>
{
    private readonly ILogger<MyMessageHandler> _logger;

    public MyMessageHandler(ILogger<MyMessageHandler> logger)
    {
        _logger = logger;
    }

    public HandlerResult HandleMessage(ReceivedMessage<MyData> message)
    {
        return new HandlerResult()
        {
            Original = message,
            Exception = new Exception("some error occured")
        };
    }
}