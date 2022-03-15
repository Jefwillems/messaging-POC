using District09.Messaging.AMQP;

namespace District09.Servicefactory.Test.Api.Handlers;

public class MyMessageHandler : AmqpMessageHandler<MyData>
{
    private readonly ILogger<MyMessageHandler> _logger;

    public MyMessageHandler(ILogger<MyMessageHandler> logger)
    {
        _logger = logger;
    }


    protected override void HandleAmqpMessage(AmqpContext<MyData> context)
    {
        context.Extra.TryGetValue("CorrelationId", out var corrId);
        _logger.LogInformation("CorrelationId was: {CorrId}", corrId);
        _logger.LogInformation("doing stuff in my message handler");
        _logger.LogInformation("message content was: {Content}", context.Original.Text);
    }
}