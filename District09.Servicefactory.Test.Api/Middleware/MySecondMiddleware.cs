using District09.Messaging.AMQP.Pipeline;
using District09.Servicefactory.Test.Api.Handlers;

namespace District09.Servicefactory.Test.Api.Middleware;

public class MySecondMiddleware : IMessageMiddleware<MySecondData>
{
    private readonly ILogger<MySecondMiddleware> _logger;

    public MySecondMiddleware(ILogger<MySecondMiddleware> logger)
    {
        _logger = logger;
    }

    public MiddlewareContext<MySecondData> Execute(MiddlewareContext<MySecondData> context, Action next)
    {
        _logger.LogInformation("second middleware called");
        next();
        return context;
    }
}