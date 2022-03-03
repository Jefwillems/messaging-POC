using System.Text.Json;
using Apache.NMS;
using District09.Messaging.AMQP.Processors;
using District09.Servicefactory.Test.Api.Handlers;

namespace District09.Servicefactory.Test.Api.Processors;

public class MyPreProcessor : BasePreProcessor
{
    protected override ITextMessage PreProcess(ITextMessage message)
    {
        var n = JsonSerializer.Deserialize<MyData>(message.Text);
        n.Hello += " (pre-processed)";
        message.Text += JsonSerializer.Serialize(n);
        return message;
    }
}