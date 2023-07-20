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

public class NamedDependencyInjectionServiceProviderExtensionsTests
{
    [Fact]
    public void GetNamedService_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedService(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetNamedService(string.Empty, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetNamedService("", null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedService("name", null!);
        });
    }

    [Fact]
    public void GetNamedService_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");
        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetNamedService("name", typeof(ITestNamedService));
        Assert.NotNull(service);
        Assert.True(service is TestNamedService);

        var service1 = serviceProvider.GetNamedService("name", typeof(INotRegisterNamedService));
        Assert.Null(service1);
    }

    [Fact]
    public void GetNamedServiceGeneric_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedService<ITestNamedService>(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetNamedService<ITestNamedService>(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetNamedService<ITestNamedService>("");
        });
    }

    [Fact]
    public void GetNamedServiceGeneric_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");
        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetNamedService<ITestNamedService>("name");
        Assert.NotNull(service);
        Assert.True(service is TestNamedService);

        var service1 = serviceProvider.GetNamedService<INotRegisterNamedService>("name");
        Assert.Null(service1);
    }

    [Fact]
    public void GetRequiredNamedService_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetRequiredNamedService(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetRequiredNamedService(string.Empty, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetRequiredNamedService("", null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetRequiredNamedService("name", null!);
        });
    }

    [Fact]
    public void GetRequiredNamedService_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");
        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetRequiredNamedService("name", typeof(ITestNamedService));
        Assert.NotNull(service);
        Assert.True(service is TestNamedService);

        var service1 = serviceProvider.GetNamedService("name", typeof(INotRegisterNamedService));
        Assert.Null(service1);
    }

    [Fact]
    public void GetRequiredNamedService_ThrowIfNull()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() =>
        {
            var service1 = serviceProvider.GetRequiredNamedService("name", typeof(INotRegisterNamedService));
        });
    }

    [Fact]
    public void GetNamedRequiredServiceGeneric_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetRequiredNamedService<ITestNamedService>(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetRequiredNamedService<ITestNamedService>(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetRequiredNamedService<ITestNamedService>("");
        });
    }

    [Fact]
    public void GetRequiredNamedServiceGeneric_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");
        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetRequiredNamedService<ITestNamedService>("name");
        Assert.NotNull(service);
        Assert.True(service is TestNamedService);
    }

    [Fact]
    public void GetRequiredNamedServiceGeneric_ThrowIfNull()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() =>
        {
            var service1 = serviceProvider.GetRequiredNamedService<INotRegisterNamedService>("name");
        });
    }

    [Fact]
    public void GetNamedServices_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedServices(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetNamedServices(string.Empty, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetNamedServices("", null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedServices("name", null!);
        });
    }

    [Fact]
    public void GetNamedServices_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");
        services.AddNamedTransient<ITestNamedService, TestOtherNamedService>("name");
        var serviceProvider = services.BuildServiceProvider();

        var serviceList = serviceProvider.GetNamedServices("name", typeof(ITestNamedService));
        Assert.NotNull(serviceList);
        Assert.Equal(2, serviceList.Count());
        Assert.True(serviceList.ElementAt(0) is TestNamedService);
        Assert.True(serviceList.ElementAt(1) is TestOtherNamedService);

        var serviceList1 = serviceProvider.GetNamedServices("name", typeof(INotRegisterNamedService));
        Assert.NotNull(serviceList1);
        Assert.Empty(serviceList1);
    }

    [Fact]
    public void GetNamedServicesGeneric_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedServices<ITestNamedService>(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetNamedServices<ITestNamedService>(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            serviceProvider.GetNamedServices<ITestNamedService>("");
        });
    }

    [Fact]
    public void GetNamedServicesGeneric_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");
        services.AddNamedTransient<ITestNamedService, TestOtherNamedService>("name");
        var serviceProvider = services.BuildServiceProvider();

        var serviceList = serviceProvider.GetNamedServices<ITestNamedService>("name");
        Assert.NotNull(serviceList);
        Assert.Equal(2, serviceList.Count());
        Assert.True(serviceList.ElementAt(0) is TestNamedService);
        Assert.True(serviceList.ElementAt(1) is TestOtherNamedService);

        var serviceList1 = serviceProvider.GetNamedServices<INotRegisterNamedService>("name");
        Assert.NotNull(serviceList1);
        Assert.Empty(serviceList1);
    }
}