namespace Furion.Tests.WebAPI.Services;

//[ServiceInjection(Ignore = true)]
internal sealed class TestService : TestBaseSerivce, ITestService, ITestExtraService, IScopedDependency, IDisposable
{
    public void Dispose() => Console.WriteLine(nameof(TestService) + ": " + nameof(Dispose));

    public string Extra() => nameof(ITestExtraService);

    public string GetName() => nameof(ITestService);
}

public abstract class TestBaseSerivce
{
    public string GetBase() => nameof(TestBaseSerivce);
}

internal sealed class TestGService : TestSingleGeneric<string>, IScopedDependency
{

}

public class TestSingleGeneric<T>
{

}