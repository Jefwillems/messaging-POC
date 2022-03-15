using Apache.NMS;
using District09.Messaging.Configuration;
using District09.Messaging.Contracts;
using District09.Messaging.Pipeline;
using District09.Messaging.Stomp.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.Stomp.Configuration;

public class ConfigBuilder : BaseConfigurationBuilder<ITextMessage>
{
    public ConfigBuilder(IServiceCollection services, IConfiguration configuration) : base(services, configuration)
    {
        services.AddSingleton<IStompWrapper, StompWrapper>();
    }

    public override IRegisterConfig<ITextMessage> WithListener<TDataType, THandlerType>(string queue)
    {
        var handlerType = typeof(IListenerMiddleware<TDataType, ITextMessage>);
        Services.AddScoped(handlerType, typeof(THandlerType));
        Services.AddSingleton<IListener<TDataType>, Listener<TDataType>>();

        Services.AddSingleton<IHostedService>(provider => new ListenerHostedService<TDataType, ITextMessage>(
            provider.GetRequiredService<ILogger<ListenerHostedService<TDataType, ITextMessage>>>(),
            provider.GetRequiredService<IListener<TDataType>>(),
            queue));
        Listeners.Add(handlerType, queue);
        return this;
    }

    public override IRegisterConfig<ITextMessage> WithPublisher<TDataType>(string queue)
    {
        var publisherType = typeof(IMessagePublisher<TDataType>);
        Services.AddScoped(publisherType, typeof(StompPublisher<TDataType>));
        Publishers.Add(publisherType, queue);
        return this;
    }

    public override IRegisterConfig<ITextMessage> WithListenerMiddleware<TMessageMiddleware, TForDataType>()
    {
        var mType = typeof(IListenerMiddleware<TForDataType, ITextMessage>);
        Services.AddScoped(mType, typeof(TMessageMiddleware));
        return this;
    }

    public override IRegisterConfig<ITextMessage> WithPublisherMiddleware<TMiddleware, TDataType>()
    {
        var mType = typeof(IPublisherMiddleware<TDataType, ITextMessage>);
        Services.AddScoped(mType, typeof(TMiddleware));
        return this;
    }
}