
namespace District09.Messaging.AMQP.Configuration;

public interface IFinishedConfig
{
    string GetPublishQueueForType(Type messageType);
    string GetListenQueueForType(Type messageType);
    public BrokerOptions BrokerOptions { get; set; }
    IDictionary<Type, string> GetListeners();
}