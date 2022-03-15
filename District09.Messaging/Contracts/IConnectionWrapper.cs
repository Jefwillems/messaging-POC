namespace District09.Messaging.Contracts;

public interface IConnectionWrapper<out TSessionType>
{
    bool IsConnectionStarted();
    TSessionType GetSession();
}