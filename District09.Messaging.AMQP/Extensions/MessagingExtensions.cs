using District09.Messaging.AMQP;
using District09.Messaging.AMQP.Configuration;
using District09.Messaging.AMQP.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace District09.Messaging.AMQP.Extensions;

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

    private static T CastObject<T>(object input)
    {
        return (T)input;
    }
}