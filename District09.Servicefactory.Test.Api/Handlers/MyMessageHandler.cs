using District09.Messaging;
using District09.Messaging.Pipeline;

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
        message.Extra.TryGetValue("CorrelationId", out var corrId);
        _logger.LogInformation("CorrelationId was: {CorrId}", corrId);
        _logger.LogInformation("doing stuff in my message handler");
        _logger.LogInformation("message content was: {Content}", message.Original.Text);
    }
}