using Apache.NMS;
using District09.Messaging.AMQP.Apm.Middleware;
using District09.Messaging.Configuration;

namespace District09.Messaging.AMQP.Apm.Extensions;

public static class MessagingExtensions
{
    public static IRegisterConfig<ITextMessage> WithApmTracing<TDataType>(this IRegisterConfig<ITextMessage> self)
    {
        self.WithListenerMiddleware<ListenerApmTracingMiddleware<TDataType>, TDataType>();
        return self;
    }
}