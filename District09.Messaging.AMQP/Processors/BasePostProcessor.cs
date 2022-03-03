namespace District09.Messaging.AMQP.Processors;

public abstract class BasePostProcessor : IProcessor<HandlerResult>
{
    public HandlerResult Process(HandlerResult input)
    {
        return PostProcess(input);
    }

    protected abstract HandlerResult PostProcess(HandlerResult message);
}