using Apache.NMS;
using District09.Messaging.AMQP.Configuration;
using District09.Messaging.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace District09.Messaging.AMQP.Extensions;

public static class MessagingExtensions
{
    public delegate IFinishedConfig ConfigureMessaging(IRegisterConfig<ITextMessage> builder);

    public static IServiceCollection AddAmqpMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        ConfigureMessaging delegateFunc)
    {
        var configBuilder = new ConfigBuilder(services, configuration);
        var c = delegateFunc(configBuilder);
        return services;
    }
}