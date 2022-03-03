namespace District09.Messaging.AMQP.Processors;

public class ProcessorQueue<TProcessor>
{
    private ProcessorNode<TProcessor>? First { get; set; }

    public ProcessorQueue(IEnumerable<IProcessor<TProcessor>> processors)
    {
        var p = processors.ToList();
        if (p.Any())
        {
            First = new ProcessorNode<TProcessor>(p.ToList());
        }
    }

    public TProcessor Execute(TProcessor input)
    {
        return First == null ? input : First.Process(input);
    }
}