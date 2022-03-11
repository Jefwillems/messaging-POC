using System.Text.Json;
using District09.Messaging.Configuration;
using District09.Messaging.Contracts;
using District09.Messaging.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace District09.Messaging;

public class MessagePublisher<TDataType> : IMessagePublisher<TDataType>
{
    private readonly ILogger<MessagePublisher<TDataType>> _logger;
    private readonly IAmqWrapper _wrapper;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _queueName;

    public MessagePublisher(
        ILogger<MessagePublisher<TDataType>> logger,
        IFinishedConfig config,
        IAmqWrapper wrapper,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _wrapper = wrapper;
        _serviceScopeFactory = serviceScopeFactory;
        _queueName = config.GetPublishQueueForType(typeof(IMessagePublisher<TDataType>));
    }

    public void PublishMessage(TDataType message)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        _logger.LogInformation("Publishing message to {Queue}", _queueName);
        var session = _wrapper.GetSession();
        using var queue = session.GetQueue(_queueName);
        using var prod = session.CreateProducer(queue);
        var msg = session.CreateTextMessage(JsonSerializer.Serialize(message));

        var publisherMw = scope.ServiceProvider.GetServices<IPublisherMiddleware<TDataType>>();
        var pipeline = new MessagePipeline<TDataType>(publisherMw);
        var context = new MiddlewareContext<TDataType>(msg);
        var result = pipeline.Run(context);


        prod.Send(result.Original);
    }
}