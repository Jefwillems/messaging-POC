namespace District09.Messaging.AMQP.Processors;

public class ProcessorNode<TNodeType>
{
    private ProcessorNode<TNodeType>? Next { get; set; }
    private readonly IProcessor<TNodeType> _currentProcessor;

    public ProcessorNode(IEnumerable<IProcessor<TNodeType>> nextNodes)
    {
        var l = new Queue<IProcessor<TNodeType>>(nextNodes);
        _currentProcessor = l.Dequeue();
        if (l.Count <= 0) return;
        Next = new ProcessorNode<TNodeType>(l);
    }

    public TNodeType Process(TNodeType input)
    {
        var a = _currentProcessor.Process(input);
        return Next == null ? a : Next.Process(a);
    }
}