namespace Furion.Tests.WebAPI.Services;

public class TestNotInterface : IScopedDependency
{
}

[ServiceInjection(IncludingSelf = true, Order = 2)]
public class TestNotInterface2 : TestNotInterface
{
}

public class TestNotInterface3 : TestNotInterface
{
}