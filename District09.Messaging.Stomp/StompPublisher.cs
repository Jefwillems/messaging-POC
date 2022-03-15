using System.Text.Json;
using Apache.NMS;
using District09.Messaging.Configuration;
using District09.Messaging.Contracts;
using District09.Messaging.Pipeline;
using District09.Messaging.Stomp.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.Stomp;

public class StompPublisher<TDataType> : BaseMessagePublisher<TDataType>
{
    private readonly ILogger<StompPublisher<TDataType>> _logger;
    private readonly string _queueName;
    private readonly IStompWrapper _wrapper;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StompPublisher(
        ILogger<StompPublisher<TDataType>> logger,
        IFinishedConfig config,
        IStompWrapper wrapper,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _wrapper = wrapper;
        _serviceScopeFactory = serviceScopeFactory;
        _queueName = config.GetPublishQueueForType(typeof(BaseMessagePublisher<TDataType>));
    }

    public override void PublishMessage(TDataType message)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        _logger.LogInformation("Publishing message to {Queue}", _queueName);
        var session = _wrapper.GetSession();
        var queue = session.GetQueue(_queueName);
        using var prod = session.CreateProducer(queue);
        var msg = session.CreateTextMessage(JsonSerializer.Serialize(message));

        var publisherMw = scope.ServiceProvider.GetServices<IPublisherMiddleware<TDataType, ITextMessage>>();
        var pipeline = new MessagePipeline<TDataType, ITextMessage>(publisherMw);
        var context = new StompContext<TDataType>(msg);
        var result = pipeline.Run(context);

        prod.Send(result.Original);
    }
}