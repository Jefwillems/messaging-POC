using Apache.NMS;

namespace District09.Messaging.Contracts;

public interface IAmqWrapper
{
    bool IsConnectionStarted();
    ISession GetSession(AcknowledgementMode mode = AcknowledgementMode.IndividualAcknowledge);
}