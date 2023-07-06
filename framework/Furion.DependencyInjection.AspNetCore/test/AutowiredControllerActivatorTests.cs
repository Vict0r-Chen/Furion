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

namespace Furion.DependencyInjection.AspNetCore.Tests;

public class AutowiredControllerActivatorTests
{
    [Fact]
    public void NewInstance_ReturnOK()
    {
        var autowiredControllerActivator = new AutowiredControllerActivator(new TypeActivatorCache());
        Assert.IsAssignableFrom<IControllerActivator>(autowiredControllerActivator);

        Assert.NotNull(autowiredControllerActivator._typeActivatorCache);
    }

    [Fact]
    public void Create_Null_Throw()
    {
        var autowiredControllerActivator = new AutowiredControllerActivator(new TypeActivatorCache());

        Assert.Throws<ArgumentNullException>(() =>
        {
            autowiredControllerActivator.Create(null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            autowiredControllerActivator.Create(new ControllerContext());
        });
        Assert.Equal($"The '{nameof(ControllerContext.ActionDescriptor)}' property of '{nameof(ControllerContext)}' must not be null.", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            autowiredControllerActivator.Create(new ControllerContext
            {
                ActionDescriptor = new ControllerActionDescriptor()
            });
        });
        Assert.Equal($"The '{nameof(ControllerContext.ActionDescriptor.ControllerTypeInfo)}' property of '{nameof(ControllerContext.ActionDescriptor)}' must not be null.", exception2.Message);
    }

    [Fact]
    public void Release_Null_Throw()
    {
        var autowiredControllerActivator = new AutowiredControllerActivator(new TypeActivatorCache());

        Assert.Throws<ArgumentNullException>(() =>
        {
            autowiredControllerActivator.Release(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            autowiredControllerActivator.Release(new ControllerContext(), null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            autowiredControllerActivator.Release(null!, new ControllerB());
        });
    }

    [Fact]
    public async Task ReleaseAsync_Null_Throw()
    {
        var autowiredControllerActivator = new AutowiredControllerActivator(new TypeActivatorCache());

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await autowiredControllerActivator.ReleaseAsync(null!, null!);
        });

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await autowiredControllerActivator.ReleaseAsync(new ControllerContext(), null!);
        });

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await autowiredControllerActivator.ReleaseAsync(null!, new ControllerB());
        });
    }

    [Fact]
    public void AutowriedFields_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddTransient<IServiceA, ServiceA>();
        var serviceProvider = services.BuildServiceProvider();

        var controller = new ControllerB();
        var typeinfo = typeof(ControllerB).GetTypeInfo();

        AutowiredControllerActivator.AutowriedFields(controller, typeinfo, serviceProvider);

        Assert.NotNull(controller._serviceA);
    }

    [Fact]
    public void AutowriedFields_Readonly_Throw()
    {
        var services = new ServiceCollection();
        services.AddTransient<IServiceA, ServiceA>();
        var serviceProvider = services.BuildServiceProvider();

        var controller = new ControllerC();
        var typeinfo = typeof(ControllerC).GetTypeInfo();

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            AutowiredControllerActivator.AutowriedFields(controller, typeinfo, serviceProvider);
        });
        Assert.Equal($"It is not possible to inject a service into a read-only `{nameof(ControllerC._serviceA)}` field of type `{nameof(ControllerC)}`.", exception.Message);
    }

    [Fact]
    public void AutowriedFields_NoRegister_Throw()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var controller = new ControllerE();
        var typeinfo = typeof(ControllerE).GetTypeInfo();

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            AutowiredControllerActivator.AutowriedFields(controller, typeinfo, serviceProvider);
        });
        Assert.Equal($"No service for type '{typeof(IServiceC).FullName}' has been registered.", exception.Message);
    }

    [Fact]
    public void AutowriedProperties_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddTransient<IServiceB, ServiceB>();
        var serviceProvider = services.BuildServiceProvider();

        var controller = new ControllerB();
        var typeinfo = typeof(ControllerB).GetTypeInfo();

        AutowiredControllerActivator.AutowriedProperties(controller, typeinfo, serviceProvider);

        Assert.NotNull(controller.ServiceB);
    }

    [Fact]
    public void AutowriedProperties_Readonly_Throw()
    {
        var services = new ServiceCollection();
        services.AddTransient<IServiceB, ServiceB>();
        var serviceProvider = services.BuildServiceProvider();

        var controller = new ControllerD();
        var typeinfo = typeof(ControllerD).GetTypeInfo();

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            AutowiredControllerActivator.AutowriedProperties(controller, typeinfo, serviceProvider);
        });
        Assert.Equal($"It is not possible to inject a service into a read-only `{nameof(ControllerD.ServiceB)}` property of type `{nameof(ControllerD)}`.", exception.Message);
    }

    [Fact]
    public void AutowriedProperties_NoRegister_Throw()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var controller = new ControllerE();
        var typeinfo = typeof(ControllerE).GetTypeInfo();

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            AutowiredControllerActivator.AutowriedProperties(controller, typeinfo, serviceProvider);
        });
        Assert.Equal($"No service for type '{typeof(IServiceC).FullName}' has been registered.", exception.Message);
    }

    [Fact]
    public void Autowried_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddTransient<IServiceA, ServiceA>();
        services.AddTransient<IServiceB, ServiceB>();
        var serviceProvider = services.BuildServiceProvider();

        var controller = new ControllerB();
        var typeinfo = typeof(ControllerB).GetTypeInfo();

        AutowiredControllerActivator.Autowried(controller, typeinfo, serviceProvider);

        Assert.NotNull(controller._serviceA);
        Assert.NotNull(controller.ServiceB);
    }
}