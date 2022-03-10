using District09.Messaging.AMQP.Contracts;
using District09.Messaging.AMQP.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.AMQP.Configuration;

public class ConfigurationBuilder : IConfigBuilder
{
    private readonly IServiceCollection _services;
    private readonly IDictionary<Type, string> _listeners;
    private readonly IDictionary<Type, string> _publishers;

    private readonly BrokerOptions _options;

    public ConfigurationBuilder(IServiceCollection services, IConfiguration configuration)
    {
        _services = services;
        _listeners = new Dictionary<Type, string>();
        _publishers = new Dictionary<Type, string>();
        _options = configuration.GetSection(BrokerOptions.Prefix).Get<BrokerOptions>();
    }

    public IRegisterConfig WithListener<TDataType, THandlerType>(string queue)
        where THandlerType : BaseMessageHandler<TDataType>
    {
        var handlerType = typeof(IListenerMiddleware<TDataType>);
        _services.AddScoped(handlerType, typeof(THandlerType));
        _services.AddSingleton<IListener<TDataType>, Listener<TDataType>>();

        _services.AddSingleton<IHostedService>(provider => new ListenerHostedService<TDataType>(
            provider.GetRequiredService<ILogger<ListenerHostedService<TDataType>>>(),
            provider.GetRequiredService<IListener<TDataType>>(),
            queue
        ));

        _listeners.Add(handlerType, queue);
        return this;
    }

    public IRegisterConfig WithPublisher<TDataType>(string queue)
    {
        var publisherType = typeof(IMessagePublisher<TDataType>);
        _services.AddScoped(publisherType, typeof(MessagePublisher<TDataType>));
        _publishers.Add(publisherType, queue);
        return this;
    }

    public IRegisterConfig WithListenerMiddleware<TMessageMiddleware, TForDataType>()
        where TMessageMiddleware : IListenerMiddleware<TForDataType>
    {
        var middlewareType = typeof(IListenerMiddleware<TForDataType>);
        _services.AddScoped(middlewareType, typeof(TMessageMiddleware));
        return this;
    }

    public IRegisterConfig WithPublisherMiddleware<TMiddleware, TDataType>()
        where TMiddleware : IPublisherMiddleware<TDataType>
    {
        var mType = typeof(IPublisherMiddleware<TDataType>);
        _services.AddScoped(mType, typeof(TMiddleware));
        return this;
    }

    public IFinishedConfig Build()
    {
        var config = new MessagingConfiguration(_listeners, _publishers, _options);
        _services.AddSingleton(config);
        return config;
    }
}