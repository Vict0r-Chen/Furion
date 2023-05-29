namespace Furion.Tests.WebAPI.Services;

//[ServiceInjection(Ignore = true)]
internal class TestGeneric<T, B> : TestBaseSerivce<T, B>, ITestGeneric<T, B>, IScopedDependency
{
    public string GetT() => typeof(T).Name;
}

public abstract class TestBaseSerivce<T, B>
{
    public string GetBase() => nameof(TestBaseSerivce);
}