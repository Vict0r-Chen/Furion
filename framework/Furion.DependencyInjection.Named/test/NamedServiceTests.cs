// 麻省理工学院许可证
//
// 版权所有 (c) 2020-2023 百小僧，百签科技（广东）有限公司
//
// 特此免费授予获得本软件及其相关文档文件（以下简称“软件”）副本的任何人以处理本软件的权利，
// 包括但不限于使用、复制、修改、合并、发布、分发、再许可、销售软件的副本，
// 以及允许拥有软件副本的个人进行上述行为，但须遵守以下条件：
//
// 在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，
// 无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Furion.DependencyInjection.Named.Tests;

public class NamedServiceTests
{
    [Fact]
    public void NotAddNamedService_Throw()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() =>
        {
            var service = serviceProvider.GetRequiredService(typeof(INamedService<>));
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            var service = serviceProvider.GetRequiredService(typeof(INamedService<INamedServiceClass>));
        });
    }

    [Fact]
    public void AddNamedService_ReturnNotNull()
    {
        var services = new ServiceCollection();
        services.AddNamed();
        var serviceProvider = services.BuildServiceProvider();

        var namedService = serviceProvider.GetRequiredService(typeof(INamedService<INamedServiceClass>));
        Assert.NotNull(namedService);

        var namedService2 = serviceProvider.GetRequiredService<INamedService<INamedServiceClass>>();
        Assert.NotNull(namedService2);
    }

    [Fact]
    public void GetIndex_IfNotExists_Throw()
    {
        var services = new ServiceCollection();
        services.AddNamed();
        var serviceProvider = services.BuildServiceProvider();

        var namedService = serviceProvider.GetRequiredService<INamedService<INamedServiceClass>>();
        Assert.NotNull(namedService);

        Assert.Throws<InvalidOperationException>(() =>
        {
            var service = namedService["name"];
        });
    }

    [Fact]
    public void GetIndex_IfExists_ReturnOK()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamed();
        services.AddNamedScoped<INamedServiceClass, NamedServiceClass>(name);
        var serviceProvider = services.BuildServiceProvider();

        var namedService = serviceProvider.GetRequiredService<INamedService<INamedServiceClass>>();
        Assert.NotNull(namedService);

        var service = namedService[name];
        Assert.NotNull(service);
        Assert.Equal(typeof(NamedServiceClass), service.GetType());
    }

    [Fact]
    public void Get_IfNotExists_ReturnNull()
    {
        var services = new ServiceCollection();
        services.AddNamed();
        var serviceProvider = services.BuildServiceProvider();

        var namedService = serviceProvider.GetRequiredService<INamedService<INamedServiceClass>>();
        Assert.NotNull(namedService);

        var service = namedService.Get("name");
        Assert.Null(service);
    }

    [Fact]
    public void Get_IfExists_ReturnNotNull()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamed();
        services.AddNamedScoped<INamedServiceClass, NamedServiceClass>(name);
        var serviceProvider = services.BuildServiceProvider();

        var namedService = serviceProvider.GetRequiredService<INamedService<INamedServiceClass>>();
        Assert.NotNull(namedService);

        var service = namedService.Get(name);
        Assert.NotNull(service);
        Assert.Equal(typeof(NamedServiceClass), service.GetType());
    }

    [Fact]
    public void GetEnumerator_IfNotExists_ReturnEmpty()
    {
        var services = new ServiceCollection();
        services.AddNamed();
        var serviceProvider = services.BuildServiceProvider();

        var namedService = serviceProvider.GetRequiredService<INamedService<INamedServiceClass>>();
        Assert.NotNull(namedService);

        var service = namedService.GetEnumerator("name");
        Assert.Empty(service);
    }

    [Fact]
    public void GetEnumerator_IfExists_ReturnNotEmpty()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamed();
        services.AddNamedScoped<INamedServiceClass, NamedServiceClass>(name);
        services.AddNamedScoped<INamedServiceClass, NamedServiceClass2>(name);
        var serviceProvider = services.BuildServiceProvider();

        var namedService = serviceProvider.GetRequiredService<INamedService<INamedServiceClass>>();
        Assert.NotNull(namedService);

        var collection = namedService.GetEnumerator(name);
        Assert.NotNull(collection);
        Assert.Equal(2, collection.Count());

        var first2 = collection.First();
        var last2 = collection.Last();
        Assert.NotNull(first2);
        Assert.NotNull(last2);
        Assert.Equal(typeof(NamedServiceClass), first2.GetType());
        Assert.Equal(typeof(NamedServiceClass2), last2.GetType());
    }
}