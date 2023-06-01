namespace Furion.Tests.DependencyInjection.Named;

public class AddNamedTests
{
    /// <summary>
    /// 测试添加命名服务空字符串
    /// </summary>
    /// <param name="name"></param>
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
                services.AddNamed(name!, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
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

    /// <summary>
    /// 测试添加命名服务空服务描述器
    /// </summary>
    [Fact]
    public void AddNamed_EmptyServiceDescriptor_ReturnOops()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentNullException>(() =>
        {
            services.AddNamed("name1", null!);
        });
    }

    /// <summary>
    /// 测试不同的服务命名成功添加
    /// </summary>
    /// <param name="name1"></param>
    /// <param name="name2"></param>
    [Theory]
    [InlineData("name1", "name2")]
    [InlineData("name3", "name4")]
    public void AddNamed_NotRepeat_ReturnOK(string name1, string name2)
    {
        var services = new ServiceCollection();
        services.AddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.AddNamed(name2, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var namedServiceCollectionOptions = serviceProvider.GetRequiredService<IOptionsMonitor<NamedServiceCollectionOptions>>().CurrentValue;
        var count = namedServiceCollectionOptions.NamedServices.Count;

        var hasName1 = namedServiceCollectionOptions.NamedServices.ContainsKey(name1);
        var hasName2 = namedServiceCollectionOptions.NamedServices.ContainsKey(name2);

        Assert.Equal(2, count);
        Assert.True(hasName1);
        Assert.True(hasName2);
    }

    /// <summary>
    /// 测试相同的服务命名抛异常
    /// </summary>
    [Fact]
    public void AddNamed_Repeat_ReturnOops()
    {
        var name1 = "name1";
        var services = new ServiceCollection();
        services.AddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.AddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var namedServiceCollectionOptions = serviceProvider.GetRequiredService<IOptionsMonitor<NamedServiceCollectionOptions>>().CurrentValue;
        });

        Assert.Equal($"The service named '{name1}' already exists.", exception.Message);
    }

    /// <summary>
    /// 测试添加命名服务空字符串
    /// </summary>
    /// <param name="name"></param>
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

    /// <summary>
    /// 测试添加命名服务空服务描述器
    /// </summary>
    [Fact]
    public void TryAddNamed_EmptyServiceDescriptor_ReturnOops()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentNullException>(() =>
        {
            services.TryAddNamed("name1", null!);
        });
    }

    /// <summary>
    /// 测试不同的服务命名成功添加
    /// </summary>
    /// <param name="name1"></param>
    /// <param name="name2"></param>
    [Theory]
    [InlineData("name1", "name2")]
    [InlineData("name3", "name4")]
    public void TryAddNamed_NotRepeat_ReturnOK(string name1, string name2)
    {
        var services = new ServiceCollection();
        services.TryAddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.TryAddNamed(name2, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var namedServiceCollectionOptions = serviceProvider.GetRequiredService<IOptionsMonitor<NamedServiceCollectionOptions>>().CurrentValue;
        var count = namedServiceCollectionOptions.NamedServices.Count;

        var hasName1 = namedServiceCollectionOptions.NamedServices.ContainsKey(name1);
        var hasName2 = namedServiceCollectionOptions.NamedServices.ContainsKey(name2);

        Assert.Equal(2, count);
        Assert.True(hasName1);
        Assert.True(hasName2);
    }

    /// <summary>
    /// 测试相同的服务命名抛异常
    /// </summary>
    [Fact]
    public void TryAddNamed_Repeat_ReturnOK()
    {
        var name1 = "name1";
        var services = new ServiceCollection();
        services.TryAddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.TryAddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var namedServiceCollectionOptions = serviceProvider.GetRequiredService<IOptionsMonitor<NamedServiceCollectionOptions>>().CurrentValue;
        var count = namedServiceCollectionOptions.NamedServices.Count;

        var hasName1 = namedServiceCollectionOptions.NamedServices.ContainsKey(name1);

        Assert.Equal(1, count);
        Assert.True(hasName1);
    }

    /// <summary>
    /// 测试多线程相同的服务命名抛异常
    /// </summary>
    [Fact]
    public void AddNamed_ThreadRepeat_ReturnOops()
    {
        var name1 = "name1";
        var services = new ServiceCollection();

        new Thread(new ThreadStart(() =>
        {
            services.AddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        })).Start();

        new Thread(new ThreadStart(() =>
        {
            services.AddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        })).Start();

        Thread.Sleep(1000);

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var namedServiceCollectionOptions = serviceProvider.GetRequiredService<IOptionsMonitor<NamedServiceCollectionOptions>>().CurrentValue;
        });

        Assert.Equal($"The service named '{name1}' already exists.", exception.Message);
    }
}