using District09.Messaging.AMQP;

namespace District09.Servicefactory.Test.Api.Handlers;

public class MySecondHandler : AmqpMessageHandler<MySecondData>
{
    private readonly ILogger<MySecondHandler> _logger;

    public MySecondHandler(ILogger<MySecondHandler> logger)
    {
        _logger = logger;
    }


    protected override void HandleAmqpMessage(AmqpContext<MySecondData> context)
    {
        _logger.LogInformation("MySecondhandler called");
        _logger.LogInformation("Content was: {Content}", context.Original.Text);
    }
}