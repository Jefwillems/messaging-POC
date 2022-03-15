using Apache.NMS;
using District09.Messaging.Configuration;
using District09.Messaging.Stomp.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace District09.Messaging.Stomp.Extensions;

public static class MessagingExtensions
{
    public delegate IFinishedConfig ConfigureMessaging(IRegisterConfig<ITextMessage> builder);

    public static IServiceCollection AddStompMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        ConfigureMessaging delegateFunc)
    {
        var configBuilder = new ConfigBuilder(services, configuration);
        var c = delegateFunc(configBuilder);
        return services;
    }
}