using District09.Messaging.AMQP;
using District09.Messaging.AMQP.Processors;

namespace District09.Servicefactory.Test.Api.Processors;

public class MyPostProcessor : BasePostProcessor
{
    protected override HandlerResult PostProcess(HandlerResult message)
    {
        message.Exception = new Exception("Post process exception");
        return message;
    }
}