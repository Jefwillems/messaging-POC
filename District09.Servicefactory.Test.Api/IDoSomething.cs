namespace District09.Servicefactory.Test.Api;

public interface IDoSomething<T> where T: class, IDoSomething<T>
{
    void DoSomething();
}