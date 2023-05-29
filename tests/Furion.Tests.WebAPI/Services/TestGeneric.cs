namespace Furion.Tests.WebAPI.Services;

[ServiceInjection(Ignore = true)]
internal class TestGeneric<T> : ITestGeneric<T>, IScopedDependency
{
    public string GetT() => typeof(T).Name;
}