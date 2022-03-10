using District09.Messaging.AMQP;
using District09.Messaging.AMQP.Pipeline;

namespace District09.Servicefactory.Test.Api.Handlers;

public class MySecondHandler : BaseMessageHandler<MySecondData>
{
    private readonly ILogger<MySecondHandler> _logger;

    public MySecondHandler(ILogger<MySecondHandler> logger)
    {
        _logger = logger;
    }

    protected override void HandleMessage(MiddlewareContext<MySecondData> message)
    {
        _logger.LogInformation("MySecondhandler called");
        _logger.LogInformation("Content was: {Content}", message.Original.Text);
    }
}