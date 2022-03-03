using District09.Messaging.AMQP.Configuration;

namespace District09.Messaging.Extensions.Apm.Extensions;

public static class ApmExtensions
{
    public static IRegisterConfig WithElasticApm(this IRegisterConfig config)
    {
        config.WithPreProcessor<ApmPreProcessor>();
        return config;
    }
}