using System.Diagnostics;
using Apache.NMS;
using District09.Messaging.AMQP.Processors;
using Elastic.Apm;
using Elastic.Apm.Api;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace District09.Messaging.Extensions.Apm;

public class ApmPreProcessor : BasePreProcessor
{
    private readonly ILogger<ApmPreProcessor> _logger;

    public ApmPreProcessor(ILogger<ApmPreProcessor> logger)
    {
        _logger = logger;
    }

    protected override ITextMessage PreProcess(ITextMessage message)
    {
        var traceParent = ExtractTraceParent(message);
        var operationName = $"Received {message.NMSType} on {message.NMSDestination}";
        var activity = new Activity($"Received message on {message.NMSDestination}");

        if (traceParent != null)
        {
            activity.SetParentId(traceParent);
        }

        activity.SetIdFormat(ActivityIdFormat.W3C);
        activity.Start();

        var tracingData = CreateTracingData(traceParent);
        var apmTransaction = Agent.IsConfigured
            ? Agent.Tracer.StartTransaction(operationName, ApiConstants.TypeMessaging, tracingData)
            : null;

        using var traceIdLogContext = LogContext.PushProperty("TraceId", apmTransaction?.TraceId);
        var correlationId = GetCorrelationId(message);
        using var correlationIdLogContext = LogContext.PushProperty("CorrelationId", correlationId);

        return message;
    }

    private static string? ExtractTraceParent(IMessage message)
    {
        var traceParent = message.Properties.GetString("traceparent");

        return string.IsNullOrEmpty(traceParent) ? null : traceParent;
    }

    private DistributedTracingData? CreateTracingData(string? traceParentValue)
    {
        var tracingData = DistributedTracingData.TryDeserializeFromString(traceParentValue);

        if (tracingData == null)
        {
            _logger.LogError("Could not parse traceparent: {TraceParentValue}", traceParentValue);
        }

        return tracingData;
    }

    private static Guid GetCorrelationId(IMessage message)
    {
        var nmsguid = message.NMSCorrelationID;
        var nmst = Guid.TryParse(nmsguid, out var nmsg);
        if (nmst)
        {
            // Guid successfully parsed from NMSCorrelationId
            return nmsg;
        }

        var guidProp = message.Properties.GetString("CorrelationId");
        if (guidProp == null)
        {
            return Guid.NewGuid();
        }

        var tryParse = Guid.TryParse(guidProp, out var g);
        return tryParse ? g : Guid.Empty;
    }
}