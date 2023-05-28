namespace Furion.Tests.WebAPI.Services;

public class TestService : ITestService, IScopedDependency, IDisposable
{
    public void Dispose() => Console.WriteLine(nameof(TestService) + ": " + nameof(Dispose));
    public string GetName() => nameof(TestService);
}