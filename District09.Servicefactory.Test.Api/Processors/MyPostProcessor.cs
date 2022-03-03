using District09.Messaging.AMQP;
using District09.Messaging.AMQP.Processors;

namespace District09.Servicefactory.Test.Api.Processors;

public class MyPostProcessor : BasePostProcessor
{
    private readonly ILogger<MyPostProcessor> _logger;

    public MyPostProcessor(ILogger<MyPostProcessor> logger)
    {
        _logger = logger;
    }

    protected override HandlerResult PostProcess(HandlerResult message)
    {
        _logger.LogInformation("Post processing message");
        message.Exception = new Exception("Post process exception");
        return message;
    }
}