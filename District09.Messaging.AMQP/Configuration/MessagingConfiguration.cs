using System.Collections.Immutable;

namespace District09.Messaging.AMQP.Configuration;

public class MessagingConfiguration : IFinishedConfig
{
    private readonly ImmutableDictionary<Type, string> _listeners;
    private readonly ImmutableDictionary<Type, string> _publishers;
    public BrokerOptions BrokerOptions { get; set; }

    public MessagingConfiguration(
        IDictionary<Type, string> listeners,
        IDictionary<Type, string> publishers,
        BrokerOptions options)
    {
        _listeners = listeners.ToImmutableDictionary();
        _publishers = publishers.ToImmutableDictionary();
        BrokerOptions = options;
    }

    public IDictionary<Type, string> GetListeners() => _listeners;

    public string GetPublishQueueForType(Type messageType)
    {
        return GetQueueForType(_publishers, messageType);
    }

    public string GetListenQueueForType(Type messageType)
    {
        return GetQueueForType(_listeners, messageType);
    }

    private static string GetQueueForType(IDictionary<Type, string> lookup, Type t)
    {
        return lookup.FirstOrDefault(pair => pair.Key == t).Value;
    }
}