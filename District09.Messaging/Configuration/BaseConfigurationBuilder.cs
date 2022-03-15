using District09.Messaging.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace District09.Messaging.Configuration;

public abstract class BaseConfigurationBuilder<TMessageType> :
    IRegisterConfig<TMessageType>
{
    protected readonly IServiceCollection Services;
    protected readonly IDictionary<Type, string> Listeners;
    protected readonly IDictionary<Type, string> Publishers;
    protected readonly BrokerOptions Options;

    protected BaseConfigurationBuilder(IServiceCollection services, IConfiguration configuration)
    {
        Services = services;
        Listeners = new Dictionary<Type, string>();
        Publishers = new Dictionary<Type, string>();
        Options = configuration.GetSection(BrokerOptions.Prefix).Get<BrokerOptions>();
    }

    public abstract IRegisterConfig<TMessageType> WithListener<TDataType, THandlerType>(string queue)
        where THandlerType : BaseMessageHandler<TDataType, TMessageType>;

    public abstract IRegisterConfig<TMessageType> WithPublisher<TDataType>(string queue);

    public abstract IRegisterConfig<TMessageType> WithListenerMiddleware<TMessageMiddleware, TForDataType>()
        where TMessageMiddleware : IListenerMiddleware<TForDataType, TMessageType>;

    public abstract IRegisterConfig<TMessageType> WithPublisherMiddleware<TMiddleware, TDataType>()
        where TMiddleware : IPublisherMiddleware<TDataType, TMessageType>;

    public IFinishedConfig Build()
    {
        var config = new MessagingConfiguration(Listeners, Publishers, Options);
        Services.AddSingleton<IFinishedConfig>(config);
        return config;
    }
}