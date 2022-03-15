using District09.Messaging.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace District09.Messaging;

public class ListenerHostedService<TDataType, TMessageType> : BackgroundService
{
    private readonly ILogger<ListenerHostedService<TDataType, TMessageType>> _logger;
    private readonly IListener<TDataType> _listener;
    private readonly string _queueName;

    public ListenerHostedService(
        ILogger<ListenerHostedService<TDataType, TMessageType>> logger,
        IListener<TDataType> listener,
        string queueName)
    {
        _logger = logger;
        _listener = listener;
        _queueName = queueName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Background Service running");
            await StartListener();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Unexpected error occurred");
        }
    }

    private async Task StartListener()
    {
        _logger.LogInformation("Starting Listening on {Queue}", _queueName);
        await _listener.StartListener(_queueName);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Backgroung Service is stopping");
        await _listener.StopListener();
        await base.StopAsync(cancellationToken);
    }
}