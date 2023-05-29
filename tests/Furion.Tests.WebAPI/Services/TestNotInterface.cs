namespace Furion.Tests.WebAPI.Services;

public class TestNotInterface : IScopedDependency
{
}

[ServiceInjection(IncludingSelf = true)]
public class TestNotInterface2 : TestNotInterface
{
}