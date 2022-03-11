using Apache.NMS;
using Apache.NMS.AMQP;
using District09.Messaging.Configuration;
using District09.Messaging.Contracts;
using Microsoft.Extensions.Logging;

namespace District09.Messaging;

public sealed class AmqWrapper : IAmqWrapper, IDisposable
{
    private readonly ILogger<AmqWrapper> _logger;
    private readonly IFinishedConfig _config;
    private IConnection? _connection;
    private readonly object _connectionLock = new();

    public AmqWrapper(ILogger<AmqWrapper> logger, IFinishedConfig config)
    {
        _logger = logger;
        _config = config;
    }

    public bool IsConnectionStarted()
    {
        return _connection is { IsStarted: true };
    }

    private void StartConnection()
    {
        _logger.LogInformation("Trying to start connection");
        _connection!.Start();
        _logger.LogInformation("Connection started");
    }

    private IConnection CreateConnection()
    {
        if (_connection != null)
        {
            return _connection;
        }

        _logger.LogInformation("Trying to create connection");
        _logger.LogTrace("with settings {Uri}, {Username}", _config.BrokerOptions.Uri, _config.BrokerOptions.Username);
        var connectionFactory = new ConnectionFactory(_config.BrokerOptions.Uri);
        var connection = connectionFactory.CreateConnection(
            _config.BrokerOptions.Username,
            _config.BrokerOptions.Password);
        connection.ExceptionListener += ConnectionOnExceptionListener;
        _logger.LogInformation("Connection created");
        return connection;
    }

    public ISession GetSession(AcknowledgementMode mode = AcknowledgementMode.IndividualAcknowledge)
    {
        if (!IsConnectionStarted())
        {
            lock (_connectionLock)
            {
                _connection = CreateConnection();
            }

            StartConnection();
        }

        _logger.LogInformation("Connection started, setting up session");

        var session = _connection!.CreateSession(mode);

        _logger.LogInformation("Session created");

        return session;
    }

    private void ConnectionOnExceptionListener(Exception exception)
    {
        _logger.LogError(exception, "Unexpected error during connection with broker");
    }


    #region Dispose

    private void Dispose(bool disposing)
    {
        if (!disposing) return;
        _connection?.Close();
        _connection?.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~AmqWrapper()
    {
        Dispose(false);
    }

    #endregion
}