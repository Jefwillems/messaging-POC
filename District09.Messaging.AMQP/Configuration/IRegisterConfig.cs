using District09.Messaging.AMQP.Pipeline;

namespace District09.Messaging.AMQP.Configuration;

public interface IRegisterConfig
{
    IRegisterConfig WithListener<TDataType, THandlerType>(string queue)
        where THandlerType : BaseMessageHandler<TDataType>;

    IRegisterConfig WithPublisher<TDataType>(string queue);

    IRegisterConfig WithMiddleware<TMessageMiddleware, TForDataType>()
        where TMessageMiddleware : IMessageMiddleware<TForDataType>;

    IFinishedConfig Build();
}