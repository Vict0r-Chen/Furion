namespace Furion.Tests.DependencyInjection.Named;

public class AddNamedTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void AddNamed_EmptyName_ReturnOops(string? name)
    {
        var services = new ServiceCollection();

        if (name == null)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                services.AddNamed(name: name!, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
            });
        }

        if (name == string.Empty)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                services.AddNamed(name!, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
            });
        }
    }

    [Fact]
    public void AddNamed_EmptyServiceDescriptor_ReturnOops()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentNullException>(() =>
        {
            services.AddNamed("name1", null!);
        });
    }

    [Theory]
    [InlineData("name1", "name2")]
    [InlineData("name3", "name4")]
    public void AddNamed_NotRepeatName_ReturnOK(string name1, string name2)
    {
        var services = new ServiceCollection();
        services.AddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.AddNamed(name2, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var count = services.Count;
        Assert.Equal(3, count);
    }

    [Fact]
    public void AddNamed_RepeatName_ReturnOK()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass2, NamedClass2>());

        var count = services.Count;
        Assert.Equal(4, count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TryAddNamed_EmptyName_ReturnOops(string? name)
    {
        var services = new ServiceCollection();

        if (name == null)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                services.TryAddNamed(name!, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
            });
        }

        if (name == string.Empty)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                services.TryAddNamed(name!, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
            });
        }
    }

    [Fact]
    public void TryAddNamed_EmptyServiceDescriptor_ReturnOops()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentNullException>(() =>
        {
            services.TryAddNamed("name1", null!);
        });
    }

    [Theory]
    [InlineData("name1", "name2")]
    [InlineData("name3", "name4")]
    public void TryAddNamed_NotRepeatName_ReturnOK(string name1, string name2)
    {
        var services = new ServiceCollection();
        services.TryAddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.TryAddNamed(name2, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var count = services.Count;
        Assert.Equal(3, count);
    }

    [Fact]
    public void TryAddNamed_RepeatName_ReturnOK()
    {
        var name1 = "name1";
        var services = new ServiceCollection();
        services.TryAddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.TryAddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.TryAddNamed(name1, ServiceDescriptor.Scoped<INamedClass2, NamedClass2>());

        var count = services.Count;
        Assert.Equal(3, count);
    }
}