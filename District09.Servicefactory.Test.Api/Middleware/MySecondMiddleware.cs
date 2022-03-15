using District09.Messaging.AMQP;
using District09.Servicefactory.Test.Api.Handlers;

namespace District09.Servicefactory.Test.Api.Middleware;

public class MySecondMiddleware : AmqpListenerMiddleware<MySecondData>
{
    private readonly ILogger<MySecondMiddleware> _logger;

    public MySecondMiddleware(ILogger<MySecondMiddleware> logger)
    {
        _logger = logger;
    }

    protected override AmqpContext<MySecondData> Handle(AmqpContext<MySecondData> context, Action next)
    {
        _logger.LogInformation("second middleware called");
        next();
        return context;
    }
}