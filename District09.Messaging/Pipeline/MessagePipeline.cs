namespace District09.Messaging.Pipeline;

public class MessagePipeline<TDataType, TMessageType>
{
    private readonly Queue<IMessageMiddleware<TDataType, TMessageType>> _middlewares;

    public MessagePipeline(IEnumerable<IMessageMiddleware<TDataType, TMessageType>> middlewares)
    {
        _middlewares = new Queue<IMessageMiddleware<TDataType, TMessageType>>(middlewares);
    }

    public BaseMiddlewareContext<TDataType, TMessageType> Run(BaseMiddlewareContext<TDataType, TMessageType> context)
    {
        return _middlewares.Any()
            ? RunInternal(context)
            : context;
    }

    private BaseMiddlewareContext<TDataType, TMessageType> RunInternal(
        BaseMiddlewareContext<TDataType, TMessageType> context)
    {
        return _middlewares.TryDequeue(out var m)
            ? m.Execute(context, () => RunInternal(context))
            : context;
    }
}