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

public class NamedServiceProviderExtensionsTests
{
    [Fact]
    public void GetNamedService_Parameters_Null_Throw()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var serviceType = typeof(INamedServiceClass);
        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedService(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetNamedService(string.Empty, serviceType);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedService(null!, serviceType);
        });
    }

    [Fact]
    public void GetNamedService_NotExists_ReturnNull()
    {
        var name = "name1";
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetNamedService(name, typeof(INamedServiceClass));
        Assert.Null(service);

        var service1 = serviceProvider.GetNamedService<INamedServiceClass>(name);
        Assert.Null(service1);
    }

    [Fact]
    public void GetNamedService_Exists_ReturnService()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamedTransient<INamedServiceClass, NamedServiceClass>(name);
        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetNamedService(name, typeof(INamedServiceClass));
        Assert.NotNull(service);
        Assert.Equal(typeof(NamedServiceClass), service.GetType());

        var service1 = serviceProvider.GetNamedService<INamedServiceClass>(name);
        Assert.NotNull(service1);
        Assert.Equal(typeof(NamedServiceClass), service.GetType());
    }

    [Fact]
    public void GetRequiredNamedService_Parameters_Null_Throw()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var serviceType = typeof(INamedServiceClass);
        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetRequiredNamedService(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetRequiredNamedService(string.Empty, serviceType);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetRequiredNamedService(null!, serviceType);
        });
    }

    [Fact]
    public void GetRequiredNamedService_NotExists_Throw()
    {
        var name = "name1";
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() =>
        {
            var service = serviceProvider.GetRequiredNamedService(name, typeof(INamedServiceClass));
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            var service1 = serviceProvider.GetRequiredNamedService<INamedServiceClass>(name);
        });
    }

    [Fact]
    public void GetRequiredNamedService_Exists_ReturnService()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamedTransient<INamedServiceClass, NamedServiceClass>(name);
        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetRequiredNamedService(name, typeof(INamedServiceClass));
        Assert.NotNull(service);
        Assert.Equal(typeof(NamedServiceClass), service.GetType());

        var service1 = serviceProvider.GetRequiredNamedService<INamedServiceClass>(name);
        Assert.NotNull(service1);
        Assert.Equal(typeof(NamedServiceClass), service.GetType());
    }

    [Fact]
    public void GetNamedServices_Parameters_Null_Throw()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var serviceType = typeof(INamedServiceClass);
        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedServices(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetNamedServices(string.Empty, serviceType);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedServices(null!, serviceType);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedServices<INamedServiceClass>(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetNamedServices<INamedServiceClass>(string.Empty);
        });
    }

    [Fact]
    public void GetNamedServices_NotExists_ReturnNull()
    {
        var name = "name1";
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var collection = serviceProvider.GetNamedServices(name, typeof(INamedServiceClass));
        Assert.NotNull(collection);
        Assert.Empty(collection);

        var collection1 = serviceProvider.GetNamedServices<INamedServiceClass>(name);
        Assert.NotNull(collection1);
        Assert.Empty(collection1);
    }

    [Fact]
    public void GetNamedServices_Exists_ReturnService()
    {
        var name = "name1";
        var services = new ServiceCollection();

        services.AddNamedScoped<INamedServiceClass, NamedServiceClass>(name);
        services.AddNamedScoped<INamedServiceClass, NamedServiceClass2>(name);

        var serviceProvider = services.BuildServiceProvider();

        var collection = serviceProvider.GetNamedServices(name, typeof(INamedServiceClass));
        Assert.NotNull(collection);
        Assert.Equal(2, collection.Count());

        var first = collection.First();
        var last = collection.Last();
        Assert.NotNull(first);
        Assert.NotNull(last);
        Assert.Equal(typeof(NamedServiceClass), first.GetType());
        Assert.Equal(typeof(NamedServiceClass2), last.GetType());

        var collection2 = serviceProvider.GetNamedServices<INamedServiceClass>(name);
        Assert.NotNull(collection2);
        Assert.Equal(2, collection2.Count());

        var first2 = collection2.First();
        var last2 = collection2.Last();
        Assert.NotNull(first2);
        Assert.NotNull(last2);
        Assert.Equal(typeof(NamedServiceClass), first2.GetType());
        Assert.Equal(typeof(NamedServiceClass2), last2.GetType());

        Assert.True(collection.SequenceEqual(collection2));
    }
}