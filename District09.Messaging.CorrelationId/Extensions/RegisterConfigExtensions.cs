using District09.Messaging.Configuration;
using District09.Messaging.CorrelationId.Middleware;

namespace District09.Messaging.CorrelationId.Extensions;

public static class RegisterConfigExtensions
{
    public static IRegisterConfig WithCorrelationId<TDataType>(this IRegisterConfig self)
    {
        return self.WithListenerMiddleware<CorrelationIdListenerMiddleware<TDataType>, TDataType>()
            .WithPublisherMiddleware<CorrelationIdPublisherMiddleware<TDataType>, TDataType>();
    }
}