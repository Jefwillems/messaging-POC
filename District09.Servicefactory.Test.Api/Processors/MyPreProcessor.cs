using Apache.NMS;
using District09.Messaging.AMQP.Processors;

namespace District09.Servicefactory.Test.Api.Processors;

public class MyPreProcessor : BasePreProcessor
{
    private readonly ILogger<MyPreProcessor> _logger;

    public MyPreProcessor(ILogger<MyPreProcessor> logger)
    {
        _logger = logger;
    }

    protected override ITextMessage PreProcess(ITextMessage message)
    {
        _logger.LogInformation("pre-processing message");
        return message;
    }
}