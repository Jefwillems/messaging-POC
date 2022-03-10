using District09.Messaging.AMQP.Pipeline;

namespace District09.Messaging.AMQP.Configuration;

public interface IRegisterConfig
{
    IRegisterConfig WithListener<TDataType, THandlerType>(string queue)
        where THandlerType : BaseMessageHandler<TDataType>;

    IRegisterConfig WithPublisher<TDataType>(string queue);

    IRegisterConfig WithListenerMiddleware<TMessageMiddleware, TForDataType>()
        where TMessageMiddleware : IListenerMiddleware<TForDataType>;

    IRegisterConfig WithPublisherMiddleware<TMiddleware, TDataType>()
        where TMiddleware : IPublisherMiddleware<TDataType>;

    IFinishedConfig Build();
}