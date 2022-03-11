using District09.Messaging;
using District09.Messaging.Configuration;
using District09.Messaging.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace District09.Messaging.Extensions;

public static class MessagingExtensions
{
    public delegate IFinishedConfig ConfigureMessaging(IConfigBuilder opts);

    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        ConfigureMessaging delegateFunc)
    {
        var config = delegateFunc(new ConfigurationBuilder(services, configuration));
        services.AddSingleton(config);
        services.AddSingleton<IAmqWrapper, AmqWrapper>();

        return services;
    }
}