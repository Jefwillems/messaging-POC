using Apache.NMS;

namespace District09.Messaging.AMQP.Contracts;

public interface IAmqWrapper
{
    bool IsConnectionStarted();
    ISession GetSession(AcknowledgementMode mode = AcknowledgementMode.IndividualAcknowledge);
}