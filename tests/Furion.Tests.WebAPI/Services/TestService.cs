namespace Furion.Tests.WebAPI.Services;

[ServiceInjection(ServiceRegister.Default)]
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