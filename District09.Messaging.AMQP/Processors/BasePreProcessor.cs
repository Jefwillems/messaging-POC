using Apache.NMS;

namespace District09.Messaging.AMQP.Processors;

public abstract class BasePreProcessor : IProcessor<ITextMessage>
{
    public ITextMessage Process(ITextMessage input)
    {
        return PreProcess(input);
    }

    protected abstract ITextMessage PreProcess(ITextMessage message);
}