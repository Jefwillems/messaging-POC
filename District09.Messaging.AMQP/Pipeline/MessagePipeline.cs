namespace District09.Messaging.AMQP.Pipeline;

internal class MessagePipeline<TDataType>
{
    private readonly Queue<IMessageMiddleware<TDataType>> _middlewares;

    public MessagePipeline(IEnumerable<IMessageMiddleware<TDataType>> middlewares)
    {
        _middlewares = new Queue<IMessageMiddleware<TDataType>>(middlewares);
    }

    public MiddlewareContext<TDataType> Run(MiddlewareContext<TDataType> context)
    {
        return _middlewares.Any()
            ? RunInternal(context)
            : context;
    }

    private MiddlewareContext<TDataType> RunInternal(MiddlewareContext<TDataType> context)
    {
        return _middlewares.TryDequeue(out var m)
            ? m.Execute(context, () => RunInternal(context))
            : context;
    }
}