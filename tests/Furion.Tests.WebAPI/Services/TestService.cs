namespace Furion.Tests.WebAPI.Services;

public class TestService : ITestService, ITestExtraService, IScopedDependency, IDisposable
{
    public void Dispose() => Console.WriteLine(nameof(TestService) + ": " + nameof(Dispose));
    public string Extra() => nameof(ITestExtraService);
    public string GetName() => nameof(ITestService);
}