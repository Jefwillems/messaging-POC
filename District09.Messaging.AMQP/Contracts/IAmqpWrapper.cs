using Apache.NMS;
using District09.Messaging.Contracts;

namespace District09.Messaging.AMQP.Contracts;

public interface IAmqpWrapper : IConnectionWrapper<ISession>, IDisposable
{
    ISession GetSession(AcknowledgementMode mode);
}