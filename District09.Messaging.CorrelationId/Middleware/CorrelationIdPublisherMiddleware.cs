using CorrelationId.Abstractions;
using District09.Messaging.Pipeline;
using Serilog.Context;

namespace District09.Messaging.CorrelationId.Middleware;

public class CorrelationIdPublisherMiddleware<TDataType> : IPublisherMiddleware<TDataType>
{
    private readonly ICorrelationContextAccessor _contextAccessor;

    public CorrelationIdPublisherMiddleware(ICorrelationContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public MiddlewareContext<TDataType> Execute(MiddlewareContext<TDataType> context, Action next)
    {
        var corrId = _contextAccessor.CorrelationContext.CorrelationId;
        context.Extra["CorrelationId"] = corrId;
        context.Original.Properties.SetString("CorrelationId", corrId);
        context.Original.NMSCorrelationID = corrId;
        using (LogContext.PushProperty("CorrelationId", corrId))
        {
            next();
        }

        return context;
    }
}