
namespace District09.Messaging.AMQP.Configuration;

public interface IRegisterConfig
{
    IRegisterConfig WithListener<TDataType, THandlerType>(string queue);
    IRegisterConfig WithPublisher<TDataType>(string queue);
    IFinishedConfig Build();
}