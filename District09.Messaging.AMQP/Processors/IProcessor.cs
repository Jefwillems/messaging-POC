namespace District09.Messaging.AMQP.Processors;

internal interface IProcessor<TProcessType>
{
    TProcessType Process(TProcessType input);
}