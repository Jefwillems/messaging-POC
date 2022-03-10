using District09.Messaging.AMQP.Pipeline;
using Serilog.Context;

namespace District09.Messaging.CorrelationId.Middleware;

public class CorrelationIdListenerMiddleware<TDataType> : IListenerMiddleware<TDataType>
{
    public MiddlewareContext<TDataType> Execute(MiddlewareContext<TDataType> context, Action next)
    {
        var correlationId = context.Original.NMSCorrelationID ??
                            context.Original.Properties.GetString("CorrelationId");
        context.Extra.Add("CorrelationId", correlationId);
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            next();
        }
        return context;
    }
}