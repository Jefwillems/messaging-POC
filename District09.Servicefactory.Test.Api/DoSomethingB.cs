namespace District09.Servicefactory.Test.Api;

public class DoSomethingB : IDoSomething<DoSomethingB>
{
    public void DoSomething()
    {
        Console.WriteLine("Hello from B");
    }
}