using Apache.NMS;

namespace District09.Messaging.AMQP.Processors;

public interface IPreProcessor
{
    void PreProcess(ITextMessage message);
}