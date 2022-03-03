using System.Diagnostics;
using Apache.NMS;
using District09.Messaging.AMQP.Processors;
using Elastic.Apm;
using Elastic.Apm.Api;
using Microsoft.Extensions.Logging;

namespace District09.Messaging.Extensions.Apm;

public class ApmPreProcessor : IPreProcessor
{
    private readonly ILogger<ApmPreProcessor> _logger;

    public ApmPreProcessor(ILogger<ApmPreProcessor> logger)
    {
        _logger = logger;
    }

    public void PreProcess(ITextMessage message)
    {
        var traceParent = ExtractTraceParent(message);
        var operationName = $"Received {message.NMSType} on {message.NMSDestination}";
        var activity = new Activity($"Received message on {message.NMSDestination}");

        if (traceParent != null)
        {
            activity.SetParentId(traceParent);
            var tracingData = CreateTracingData(traceParent);
            var apmTransaction = Agent.IsConfigured
                ? Agent.Tracer.StartTransaction(operationName, ApiConstants.TypeMessaging, tracingData)
                : null;
        }

        activity.SetIdFormat(ActivityIdFormat.W3C);
        activity.Start();
        // using var traceIdLogContext = LogContext.PushProperty("TraceId", apmTransaction?.TraceId);
    }

    private static string? ExtractTraceParent(IMessage message)
    {
        var traceParent = message.Properties.GetString("traceparent");

        return string.IsNullOrEmpty(traceParent) ? null : traceParent;
    }

    private DistributedTracingData? CreateTracingData(string traceParentValue)
    {
        var tracingData = DistributedTracingData.TryDeserializeFromString(traceParentValue);

        if (tracingData == null)
        {
            _logger.LogError("Could not parse traceparent: {TraceParentValue}", traceParentValue);
        }

        return tracingData;
    }
}