using Apache.NMS;
using District09.Messaging.Contracts;

namespace District09.Messaging.Stomp.Contracts;

public interface IStompWrapper : IConnectionWrapper<ISession>, IDisposable
{
    ISession GetSession(AcknowledgementMode mode);
}