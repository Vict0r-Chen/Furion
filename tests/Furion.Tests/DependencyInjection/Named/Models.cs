namespace Furion.Tests.DependencyInjection.Named;

internal class NamedClass : INamedClass
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

internal interface INamedClass
{
    Guid Id { get; set; }
}

internal class NamedClass2 : INamedClass2
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

internal interface INamedClass2
{
}