using District09.Messaging.AMQP.Processors;

namespace District09.Messaging.AMQP.Configuration;

public interface IRegisterConfig
{
    IRegisterConfig WithListener<TDataType, THandlerType>(string queue);
    IRegisterConfig WithPublisher<TDataType>(string queue);
    IRegisterConfig WithPreProcessor<TProcessorType>() where TProcessorType : BasePreProcessor;
    IRegisterConfig WithPostProcessor<TProcessorType>() where TProcessorType : BasePostProcessor;
    IFinishedConfig Build();
}