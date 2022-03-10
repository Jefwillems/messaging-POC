using District09.Messaging.AMQP;
using District09.Messaging.AMQP.Pipeline;

namespace District09.Servicefactory.Test.Api.Handlers;

public class MyMessageHandler : BaseMessageHandler<MyData>
{
    private readonly ILogger<MyMessageHandler> _logger;

    public MyMessageHandler(ILogger<MyMessageHandler> logger)
    {
        _logger = logger;
    }

    protected override void HandleMessage(MiddlewareContext<MyData> message)
    {
        _logger.LogInformation("doing stuff in my message handler");
        _logger.LogInformation("message content was: {Content}", message.Original.Text);
    }
}