namespace Furion.DependencyInjection.Tests;

public class NamedTypeTests
{
    [Fact]
    public void SameName_SameType_ReturnOK()
    {
        var name = "name1";
        var serviceType = typeof(INamedClass);
        var nameType1 = new NamedType(name, serviceType);
        var nameType2 = new NamedType(name, serviceType);

        Assert.Equal(nameType1, nameType2);
    }

    [Fact]
    public void SameName_NotSameType_ReturnOK()
    {
        var name = "name1";
        var serviceType1 = typeof(INamedClass);
        var serviceType2 = typeof(INamedClass2);
        var nameType1 = new NamedType(name, serviceType1);
        var nameType2 = new NamedType(name, serviceType2);

        Assert.NotEqual(nameType1, nameType2);
    }

    [Fact]
    public void NotSameName_SameType_ReturnOK()
    {
        var name1 = "name1";
        var name2 = "name2";
        var serviceType = typeof(INamedClass);
        var nameType1 = new NamedType(name1, serviceType);
        var nameType2 = new NamedType(name2, serviceType);

        Assert.NotEqual(nameType1, nameType2);
    }

    [Fact]
    public void NotSameName_NotSameType_ReturnOK()
    {
        var name1 = "name1";
        var name2 = "name2";
        var serviceType1 = typeof(INamedClass);
        var serviceType2 = typeof(INamedClass2);
        var nameType1 = new NamedType(name1, serviceType1);
        var nameType2 = new NamedType(name2, serviceType2);

        Assert.NotEqual(nameType1, nameType2);
    }
}