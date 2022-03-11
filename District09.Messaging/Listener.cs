using System.Text.Json;
using Apache.NMS;
using District09.Messaging.Contracts;
using District09.Messaging.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace District09.Messaging;

public class Listener<TDataType> : BaseListener<TDataType>
{
    public Listener(ILogger<Listener<TDataType>> logger,
        IAmqWrapper wrapper,
        IServiceProvider serviceProvider) :
        base(logger, wrapper, serviceProvider)
    {
    }

    protected override void HandleMessage(ITextMessage message, IServiceScope scope)
    {
        var content = JsonSerializer.Deserialize<TDataType>(message.Text);
        if (content == null) return;

        var pipelineMiddleware = scope.ServiceProvider.GetServices<IListenerMiddleware<TDataType>>();
        var pipeline = new MessagePipeline<TDataType>(pipelineMiddleware);
        var context = new MiddlewareContext<TDataType>(message);

        var result = pipeline.Run(context);
        if (result.IsFailed())
        {
            Logger.LogWarning(result.Exception, "Message processing failed");
        }
        else
        {
            Logger.LogInformation("Message processed successfully");
        }

        message.Acknowledge();
    }
}