using District09.Messaging.AMQP.Pipeline;
using District09.Servicefactory.Test.Api.Handlers;

namespace District09.Servicefactory.Test.Api.Middleware;

public class MyDataMiddleware : IListenerMiddleware<MyData>
{
    private readonly ILogger<MyDataMiddleware> _logger;

    public MyDataMiddleware(ILogger<MyDataMiddleware> logger)
    {
        _logger = logger;
    }

    public MiddlewareContext<MyData> Execute(MiddlewareContext<MyData> context, Action next)
    {
        _logger.LogInformation("mydataMiddleware called");
        context.Extra.Add("data", "Hello World");
        next();
        return context;
    }
}