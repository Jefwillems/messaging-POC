namespace District09.Servicefactory.Test.Api;

public class DoSomethingA : IDoSomething<DoSomethingA>
{
    public void DoSomething()
    {
        Console.WriteLine("Hello from A");
    }
}