namespace Furion.Tests.DependencyInjection.Named;

public class NamedServiceProviderTests
{
    /// <summary>
    /// 测试添加命名服务空字符串
    /// </summary>
    /// <param name="name"></param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetNamedService_EmptyName_ReturnOops(string name)
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        if (name == null)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                serviceProvider.GetNamedService(name!, null!);
            });
        }

        if (name == string.Empty)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                serviceProvider.GetNamedService(name!, null!);
            });
        }
    }

    /// <summary>
    /// 测试添加命名服务空字符串
    /// </summary>
    /// <param name="name"></param>
    [Fact]
    public void GetNamedService_EmptyType_ReturnOops()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedService("name1", null!);
        });
    }

    /// <summary>
    /// 解析存在的命名服务
    /// </summary>
    /// <param name="name"></param>
    [Theory]
    [InlineData("name1")]
    [InlineData("name2")]
    public void GetNamedService_ExistsResolve_ReturnOK(string name)
    {
        var services = new ServiceCollection();
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetNamedService<INamedClass>(name);
        Assert.NotNull(service1);

        var service2 = serviceProvider.GetNamedService(name, typeof(INamedClass));
        Assert.NotNull(service2);
    }

    /// <summary>
    /// 解析不存在的命名服务
    /// </summary>
    [Fact]
    public void GetNamedService_NotExistsResolve_ReturnOK()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamed(name + "_miss", ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetNamedService<INamedClass>(name);

        Assert.Null(service1);
    }

    /// <summary>
    /// 解析命名对象
    /// </summary>
    [Fact]
    public void GetNamedService_ResolveObject_ReturnOk()
    {
        var name = "name1";
        var obj = new NamedClass();

        var services = new ServiceCollection();
        services.AddNamedSingleton("name1", obj);

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetNamedService<NamedClass>(name);

        Assert.NotNull(service1);
        Assert.Equal(obj, service1);
    }

    /// <summary>
    /// 测试命名工厂
    /// </summary>
    [Fact]
    public void GetNamedService_ResolveFactory_ReturnOk()
    {
        var name = "name1";
        var obj = new NamedClass();

        var services = new ServiceCollection();
        services.AddNamedScoped<INamedClass, NamedClass>("name1", sp =>
        {
            return new NamedClass();
        });

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetNamedService<NamedClass>(name);

        Assert.NotNull(service1);
    }

    /// <summary>
    /// 解析存在的命名服务
    /// </summary>
    /// <param name="name"></param>
    [Theory]
    [InlineData("name1")]
    [InlineData("name2")]
    public void GetNamedRequiredService_ExistsResolve_ReturnOK(string name)
    {
        var services = new ServiceCollection();
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetNamedRequiredService<INamedClass>(name);
        Assert.NotNull(service1);

        var service2 = serviceProvider.GetNamedRequiredService(name, typeof(INamedClass));
        Assert.NotNull(service2);
    }

    /// <summary>
    /// 解析不存在的命名服务
    /// </summary>
    [Fact]
    public void GetNamedRequiredService_NotExistsResolve_ReturnOops()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamed(name + "_miss", ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var service1 = serviceProvider.GetNamedRequiredService<INamedClass>(name);
        });
    }
}