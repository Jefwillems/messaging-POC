using System.Text.Json;
using Apache.NMS;
using District09.Messaging.Pipeline;
using District09.Messaging.Stomp.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.Stomp;

public class Listener<TDatatype> : BaseStompListener<TDatatype>
{
    public Listener(
        ILogger<Listener<TDatatype>> logger,
        IStompWrapper wrapper, IServiceProvider serviceProvider) :
        base(logger, wrapper, serviceProvider)
    {
    }

    protected override void HandleMessage(ITextMessage message, IServiceScope scope)
    {
        var content = JsonSerializer.Deserialize<TDatatype>(message.Text);
        if (content == null) return;

        var pipelineMiddleware = scope.ServiceProvider.GetServices<IListenerMiddleware<TDatatype, ITextMessage>>();
        var pipeline = new MessagePipeline<TDatatype, ITextMessage>(pipelineMiddleware);
        var context = new StompContext<TDatatype>(message);

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