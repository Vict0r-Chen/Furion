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

namespace Furion.Component.Tests;

public class EntryComponentTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var entryComponent = new EntryComponent(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var entryComponent = new EntryComponent(typeof(AComponent), null!);
        });

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var entryComponent = new EntryComponent(typeof(GComponent), new ServiceComponentContext(Host.CreateApplicationBuilder()));
        });
        Assert.Equal("The dependency relationship has a circular dependency.", exception.Message);
    }

    [Fact]
    public void New_ReturnOK()
    {
        var entryComponent = new EntryComponent(typeof(AComponent), new ServiceComponentContext(Host.CreateApplicationBuilder()));

        Assert.NotNull(entryComponent);
        Assert.NotNull(entryComponent.ComponentType);
        Assert.Equal(typeof(AComponent), entryComponent.ComponentType);
        Assert.NotNull(entryComponent.ComponentContext);
        Assert.True(entryComponent.ComponentContext is ServiceComponentContext);

        Assert.NotNull(entryComponent._dependencies);
        var resultDependencies = new Dictionary<Type, Type[]>
        {
            {typeof(AComponent), new [] { typeof(BComponent), typeof(CComponent), typeof(DComponent) } },
            {typeof(BComponent), new [] { typeof(CComponent), typeof(FComponent) } },
            {typeof(CComponent), new [] { typeof(EComponent), typeof(DComponent) } },
            {typeof(DComponent), Array.Empty<Type>() },
            {typeof(EComponent), new [] { typeof(FComponent) } },
            {typeof(FComponent), Array.Empty<Type>() },
        };

        Assert.Equal(resultDependencies, entryComponent._dependencies);

        Assert.NotNull(entryComponent._sortedDependencies);
        Assert.Equal(new List<Type> { typeof(FComponent), typeof(EComponent), typeof(DComponent), typeof(CComponent), typeof(BComponent), typeof(AComponent) }, entryComponent._sortedDependencies);

        Assert.NotNull(entryComponent._dependencyGraph);
    }

    [Fact]
    public void Start_ReturnOK()
    {
        var componentContext = new ServiceComponentContext(Host.CreateApplicationBuilder());
        var entryComponent = new EntryComponent(typeof(AComponent), componentContext);
        entryComponent.Start();

        Assert.Equal(12, componentContext.Properties.Count);

        var properties = new[]
        {
            "Furion.Component.Tests.AComponent.PreConfigureServices",
            "Furion.Component.Tests.BComponent.PreConfigureServices",
            "Furion.Component.Tests.CComponent.PreConfigureServices",
            "Furion.Component.Tests.DComponent.PreConfigureServices",
            "Furion.Component.Tests.EComponent.PreConfigureServices",
            "Furion.Component.Tests.FComponent.PreConfigureServices",
            "Furion.Component.Tests.FComponent.ConfigureServices",
            "Furion.Component.Tests.EComponent.ConfigureServices",
            "Furion.Component.Tests.DComponent.ConfigureServices",
            "Furion.Component.Tests.CComponent.ConfigureServices",
            "Furion.Component.Tests.BComponent.ConfigureServices",
            "Furion.Component.Tests.AComponent.ConfigureServices",
        };

        Assert.Equal(properties, componentContext.Properties.Keys.ToArray());
    }

    [Fact]
    public void Start_CanActivate_ReturnOK()
    {
        var componentContext = new ServiceComponentContext(Host.CreateApplicationBuilder());
        var entryComponent = new EntryComponent(typeof(A1Component), componentContext);
        entryComponent.Start();

        var properties = new[]
        {
            "Furion.Component.Tests.A1Component.PreConfigureServices",
            "Furion.Component.Tests.D1Component.PreConfigureServices",
            "Furion.Component.Tests.C1Component.PreConfigureServices",
            "Furion.Component.Tests.C1Component.ConfigureServices",
            "Furion.Component.Tests.D1Component.ConfigureServices",
            "Furion.Component.Tests.A1Component.ConfigureServices"
        };

        Assert.Equal(properties, componentContext.Properties.Keys.ToArray());
    }

    [Fact]
    public void NotifyInvocation_Invalid_Parameters()
    {
        var componentContext = new ServiceComponentContext(Host.CreateApplicationBuilder());
        var entryComponent = new EntryComponent(typeof(A1Component), componentContext);

        Assert.Throws<ArgumentNullException>(() =>
        {
            entryComponent.NotifyInvocation(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            entryComponent.NotifyInvocation(new AComponent(), null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            entryComponent.NotifyInvocation(new AComponent(), string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            entryComponent.NotifyInvocation(new AComponent(), "");
        });
    }

    [Fact]
    public void NotifyInvocation_ReturnOK()
    {
        var componentContext = new ServiceComponentContext(Host.CreateApplicationBuilder());
        var entryComponent = new EntryComponent(typeof(AComponent), componentContext);

        var component = new AComponent
        {
            Options = componentContext.Options
        };
        componentContext.Options.Components.TryAdd(typeof(AComponent), component);
        var methodName = nameof(AComponent.PreConfigureServices);

        entryComponent.NotifyInvocation(component, methodName);
        Assert.Equal("Furion.Component.Tests.AComponent.PreConfigureServices", component.Items.ElementAt(0));

        var component2 = new BComponent
        {
            Options = componentContext.Options
        };
        componentContext.Options.Components.TryAdd(typeof(BComponent), component2);
        entryComponent.NotifyInvocation(component2, methodName);
        Assert.Equal("Furion.Component.Tests.BComponent.PreConfigureServices", component.Items.ElementAt(1));
    }

    [Fact]
    public void InvokeMethod_Invalid_Parameters()
    {
        var componentContext = new ServiceComponentContext(Host.CreateApplicationBuilder());
        var entryComponent = new EntryComponent(typeof(AComponent), componentContext);

        Assert.Throws<ArgumentNullException>(() =>
        {
            entryComponent.InvokeMethod(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            entryComponent.InvokeMethod(new AComponent(), null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            entryComponent.InvokeMethod(new AComponent(), string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            entryComponent.InvokeMethod(new AComponent(), "");
        });
    }

    [Fact]
    public void InvokeMethod_ReturnOK()
    {
        var componentContext = new ServiceComponentContext(Host.CreateApplicationBuilder());
        var entryComponent = new EntryComponent(typeof(AComponent), componentContext);

        var component = new AComponent
        {
            Options = componentContext.Options
        };
        componentContext.Options.Components.TryAdd(typeof(AComponent), component);
        var methodName = nameof(AComponent.PreConfigureServices);

        entryComponent.InvokeMethod(component, methodName);
        Assert.Equal("Furion.Component.Tests.AComponent.PreConfigureServices", component.Items.ElementAt(0));
    }

    [Fact]
    public void GetInvokeMethodNames_ReturnOK()
    {
        var componentContext = new ServiceComponentContext(Host.CreateApplicationBuilder());
        var entryComponent = new EntryComponent(typeof(AComponent), componentContext);

        Assert.Equal(new[] { nameof(ComponentBase.PreConfigureServices), nameof(ComponentBase.ConfigureServices) }, entryComponent.GetInvokeMethodNames());
    }
}