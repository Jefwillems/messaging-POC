namespace District09.Messaging.AMQP.Processors;

public interface IProcessor<TProcessType>
{
    TProcessType Process(TProcessType input);
}