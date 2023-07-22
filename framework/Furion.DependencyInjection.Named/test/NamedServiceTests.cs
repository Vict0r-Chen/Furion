// 麻省理工学院许可证
//
// 版权所有 © 2020-2023 百小僧，百签科技（广东）有限公司
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
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var namedServcie = new NamedService<ITestNamedService>(null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var namedService = new NamedService<ITestNamedService>(serviceProvider);

        Assert.NotNull(namedService);
        Assert.NotNull(namedService._serviceProvider);
    }

    [Fact]
    public void Index_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var namedService = new NamedService<ITestNamedService>(serviceProvider);

        Assert.Throws<ArgumentNullException>(() =>
        {
            var service = namedService[null!];
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var service = namedService[string.Empty];
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var service = namedService[""];
        });
    }

    [Fact]
    public void Index_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");
        var serviceProvider = services.BuildServiceProvider();

        var namedService = new NamedService<ITestNamedService>(serviceProvider);
        var service = namedService["name"];

        Assert.NotNull(service);
        Assert.True(service is TestNamedService);

        var namedService2 = serviceProvider.GetRequiredService<INamedService<ITestNamedService>>();
        var service2 = namedService2["name"];
        Assert.NotNull(service2);
        Assert.True(service is TestNamedService);
    }

    [Fact]
    public void Index_ThrowIfNotRegister()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var namedService = new NamedService<INotRegisterNamedService>(serviceProvider);

        Assert.Throws<InvalidOperationException>(() =>
        {
            var service = namedService["name"];
        });
    }

    [Fact]
    public void Get_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var namedService = new NamedService<ITestNamedService>(serviceProvider);

        Assert.Throws<ArgumentNullException>(() =>
        {
            var service = namedService.Get(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var service = namedService.Get(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var service = namedService.Get("");
        });
    }

    [Fact]
    public void Get_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");
        var serviceProvider = services.BuildServiceProvider();

        var namedService = new NamedService<ITestNamedService>(serviceProvider);
        var service = namedService.Get("name");

        Assert.NotNull(service);
        Assert.True(service is TestNamedService);

        var namedService1 = new NamedService<INotRegisterNamedService>(serviceProvider);
        var service1 = namedService1.Get("name");

        Assert.Null(service1);

        var namedService2 = serviceProvider.GetRequiredService<INamedService<ITestNamedService>>();
        var service2 = namedService2.Get("name");
        Assert.NotNull(service2);
        Assert.True(service is TestNamedService);
    }

    [Fact]
    public void GetEnumerator_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var namedService = new NamedService<ITestNamedService>(serviceProvider);

        Assert.Throws<ArgumentNullException>(() =>
        {
            var service = namedService.GetEnumerator(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var service = namedService.GetEnumerator(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var service = namedService.GetEnumerator("");
        });
    }

    [Fact]
    public void GetEnumerator_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");
        services.AddNamedTransient<ITestNamedService, TestOtherNamedService>("name");
        var serviceProvider = services.BuildServiceProvider();

        var namedService = new NamedService<ITestNamedService>(serviceProvider);
        var serviceList = namedService.GetEnumerator("name");

        Assert.NotNull(serviceList);
        Assert.Equal(2, serviceList.Count());
        Assert.True(serviceList.ElementAt(0) is TestNamedService);
        Assert.True(serviceList.ElementAt(1) is TestOtherNamedService);

        var namedService1 = new NamedService<INotRegisterNamedService>(serviceProvider);
        var serviceList1 = namedService1.GetEnumerator("name");

        Assert.NotNull(serviceList1);
        Assert.Empty(serviceList1);

        var namedService2 = serviceProvider.GetRequiredService<INamedService<ITestNamedService>>();
        var serviceList2 = namedService2.GetEnumerator("name");
        Assert.NotNull(serviceList2);
        Assert.Equal(2, serviceList2.Count());
        Assert.True(serviceList2.ElementAt(0) is TestNamedService);
        Assert.True(serviceList2.ElementAt(1) is TestOtherNamedService);
    }
}