using System.Text.Json;
using Apache.NMS;
using District09.Messaging.AMQP.Contracts;
using District09.Messaging.AMQP.Listener;
using District09.Messaging.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.AMQP;

public class Listener<TDataType> : BaseAmqpListener<TDataType>
{
    public Listener(
        ILogger<Listener<TDataType>> logger,
        IAmqpWrapper wrapper,
        IServiceProvider serviceProvider) : base(
        logger, wrapper, serviceProvider)
    {
    }

    protected override void HandleMessage(ITextMessage message, IServiceScope scope)
    {
        var content = JsonSerializer.Deserialize<TDataType>(message.Text);
        if (content == null) return;

        var pipelineMiddleware = scope.ServiceProvider.GetServices<IListenerMiddleware<TDataType, ITextMessage>>();
        var pipeline = new MessagePipeline<TDataType, ITextMessage>(pipelineMiddleware);
        var context = new AmqpContext<TDataType>(message);

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