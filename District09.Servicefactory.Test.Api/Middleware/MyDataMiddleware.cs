using District09.Messaging.AMQP;
using District09.Servicefactory.Test.Api.Handlers;

namespace District09.Servicefactory.Test.Api.Middleware;

public class MyDataMiddleware : AmqpListenerMiddleware<MyData>
{
    private readonly ILogger<MyDataMiddleware> _logger;

    public MyDataMiddleware(ILogger<MyDataMiddleware> logger)
    {
        _logger = logger;
    }
    
    protected override AmqpContext<MyData> Handle(AmqpContext<MyData> context, Action next)
    {
        _logger.LogInformation("mydataMiddleware called");
        context.Extra.Add("data", "Hello World");
        next();
        return context;
    }
}