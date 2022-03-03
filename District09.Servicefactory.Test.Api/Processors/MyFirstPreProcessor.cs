using Apache.NMS;
using District09.Messaging.AMQP.Processors;

namespace District09.Servicefactory.Test.Api.Processors;

public class MyFirstPreProcessor : BasePreProcessor
{
    private readonly ILogger<MyFirstPreProcessor> _logger;

    public MyFirstPreProcessor(ILogger<MyFirstPreProcessor> logger)
    {
        _logger = logger;
    }

    protected override ITextMessage PreProcess(ITextMessage message)
    {
        _logger.LogInformation("first pre processing");
        return message;
    }
}