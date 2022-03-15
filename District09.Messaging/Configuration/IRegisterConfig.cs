using District09.Messaging.Pipeline;

namespace District09.Messaging.Configuration;

public interface IRegisterConfig<TMessageType>
{
    IRegisterConfig<TMessageType> WithListener<TDataType, THandlerType>(string queue)
        where THandlerType : BaseMessageHandler<TDataType, TMessageType>;

    IRegisterConfig<TMessageType> WithPublisher<TDataType>(string queue);

    IRegisterConfig<TMessageType> WithListenerMiddleware<TMessageMiddleware, TForDataType>()
        where TMessageMiddleware : IListenerMiddleware<TForDataType, TMessageType>;

    IRegisterConfig<TMessageType> WithPublisherMiddleware<TMiddleware, TDataType>()
        where TMiddleware : IPublisherMiddleware<TDataType, TMessageType>;

    IFinishedConfig Build();
}