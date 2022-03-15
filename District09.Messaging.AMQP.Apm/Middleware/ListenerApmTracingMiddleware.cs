using Microsoft.Extensions.Logging;

namespace District09.Messaging.AMQP.Apm.Middleware;

public class ListenerApmTracingMiddleware<TDataType> : AmqpListenerMiddleware<TDataType>
{
    private readonly ILogger<ListenerApmTracingMiddleware<TDataType>> _logger;

    public ListenerApmTracingMiddleware(ILogger<ListenerApmTracingMiddleware<TDataType>> logger)
    {
        _logger = logger;
    }

    protected override AmqpContext<TDataType> Handle(AmqpContext<TDataType> context, Action next)
    {
        // TODO setup
        _logger.LogInformation("setting up apm tracing for message");
        // continue middleware chain
        next();

        // TODO cleanup
        _logger.LogInformation("cleaning up apm tracing for message");
        return context;
    }
}