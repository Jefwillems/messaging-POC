using District09.Messaging.AMQP.Contracts;
using District09.Messaging.AMQP.Processors;
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
    {
        var handlerType = typeof(IMessageHandler<TDataType>);
        _services.AddScoped(handlerType, typeof(THandlerType));
        _services.AddScoped<IListener<TDataType>, Listener<TDataType>>();

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

    public IRegisterConfig WithPreProcessor<TProcessorType>() where TProcessorType : IPreProcessor
    {
        _services.AddScoped(typeof(IPreProcessor), typeof(TProcessorType));
        return this;
    }

    public IRegisterConfig WithPostProcessor<TProcessorType>() where TProcessorType : IPostProcessor
    {
        _services.AddScoped(typeof(IPostProcessor), typeof(TProcessorType));
        return this;
    }


    public IFinishedConfig Build()
    {
        return new MessagingConfiguration(_listeners, _publishers, _options);
    }
}