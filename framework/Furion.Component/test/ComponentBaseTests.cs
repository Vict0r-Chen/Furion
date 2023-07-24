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

public class ComponentBaseTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var component = new AComponent();

        Assert.NotNull(component);
        Assert.Null(component.Options);
        Assert.True(component.CanActivate(new ServiceComponentContext(Host.CreateApplicationBuilder())));
    }

    [Fact]
    public void Props_Invalid_Parameters()
    {
        var component = new AComponent
        {
            Options = new ComponentOptions()
        };

        Assert.Throws<ArgumentNullException>(() =>
        {
            component.Props((Action<ComponentOptionsClass1>)null!);
        });
    }

    [Fact]
    public void Props_ReturnOK()
    {
        var component = new AComponent
        {
            Options = new ComponentOptions()
        };

        component.Props<ComponentOptionsClass1>(props => { });
        Assert.Single(component.Options.PropsActions);

        component.Props<ComponentOptionsClass1>(props => { });
        Assert.Single(component.Options.PropsActions);
        Assert.Equal(2, component.Options.PropsActions[typeof(ComponentOptionsClass1)].Count);
    }

    [Fact]
    public void PropsConfiguration_Invalid_Parameters()
    {
        var component = new AComponent
        {
            Options = new ComponentOptions()
        };

        Assert.Throws<ArgumentNullException>(() =>
        {
            component.Props<ComponentOptionsClass1>((IConfiguration)null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            component.Props<ComponentOptionsClass1>(new ConfigurationManager());
        });
    }

    [Fact]
    public void PropsConfiguration_ReturnOK()
    {
        var component = new AComponent
        {
            Options = new ComponentOptions()
        };

        var configurationManager = new ConfigurationManager();
        configurationManager.AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"Data:Items:0", "action1"},
            {"Data:Items:1", "action2"}
        });

        component.Props<ComponentOptionsClass1>(configurationManager.GetSection("Data"));
        Assert.Single(component.Options.PropsActions);

        var propsAction = component.Options.PropsActions[typeof(ComponentOptionsClass1)].First() as Action<ComponentOptionsClass1>;
        Assert.NotNull(propsAction);

        var componentOptionsClass1 = new ComponentOptionsClass1();
        propsAction(componentOptionsClass1);

        Assert.NotNull(componentOptionsClass1.Items);
        Assert.Equal(2, componentOptionsClass1.Items.Count);
    }

    [Fact]
    public void PreConfigureServices_ReturnOK()
    {
        var component = new AComponent
        {
            Options = new ComponentOptions()
        };

        component.PreConfigureServices(new ServiceComponentContext(Host.CreateApplicationBuilder()));

        Assert.Equal("Furion.Component.Tests.AComponent.PreConfigureServices", component.Items.ElementAt(0));
    }

    [Fact]
    public void ConfigureServices_ReturnOK()
    {
        var component = new AComponent
        {
            Options = new ComponentOptions()
        };

        component.ConfigureServices(new ServiceComponentContext(Host.CreateApplicationBuilder()));

        Assert.Equal("Furion.Component.Tests.AComponent.ConfigureServices", component.Items.ElementAt(0));
    }

    [Fact]
    public void OnDependencyInvocation_ReturnOK()
    {
        var component = new AComponent
        {
            Options = new ComponentOptions()
        };

        component.OnDependencyInvocation(new ComponentInvocationContext(component, new ServiceComponentContext(Host.CreateApplicationBuilder()), "PreConfigureServices"));

        Assert.Equal("Furion.Component.Tests.AComponent.PreConfigureServices", component.Items.ElementAt(0));
    }

    [Fact]
    public void ReloadProps_ReturnOK()
    {
        var component = new AComponent
        {
            Options = new ComponentOptions()
        };
        component.Props<ComponentOptionsClass1>(props => { });

        Assert.Null(component.CustomProps);

        component.ReloadProps();

        Assert.NotNull(component.CustomProps);
    }

    [Fact]
    public void CreateDependencies_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.CreateDependencies(null!);
        });
    }

    [Fact]
    public void CreateDependencies_ReturnOK()
    {
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));

        Assert.NotNull(dependencies);
        Assert.NotEmpty(dependencies);

        var resultDependencies = new Dictionary<Type, Type[]>
        {
            {typeof(AComponent), new [] { typeof(BComponent), typeof(CComponent), typeof(DComponent) } },
            {typeof(BComponent), new [] { typeof(CComponent), typeof(FComponent) } },
            {typeof(CComponent), new [] { typeof(EComponent), typeof(DComponent) } },
            {typeof(DComponent), Array.Empty<Type>() },
            {typeof(EComponent), new [] { typeof(FComponent) } },
            {typeof(FComponent), Array.Empty<Type>() },
        };

        Assert.Equal(resultDependencies, dependencies);
    }

    [Fact]
    public void CreateEntry_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.CreateEntry(null!, null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.CreateEntry(typeof(AComponent), null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.CreateEntry(typeof(AComponent), new ServiceComponentContext(Host.CreateApplicationBuilder()), null!);
        });

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.CreateEntry(typeof(GComponent), new ServiceComponentContext(Host.CreateApplicationBuilder()), new[] { nameof(ComponentBase.PreConfigureServices), nameof(ComponentBase.ConfigureServices) });
        });
        Assert.Equal("The dependency relationship has a circular dependency.", exception.Message);
    }

    [Fact]
    public void CreateEntry_ReturnOK()
    {
        var componentContext = new ServiceComponentContext(Host.CreateApplicationBuilder());
        ComponentBase.CreateEntry(typeof(AComponent), componentContext, new[] { nameof(ComponentBase.PreConfigureServices), nameof(ComponentBase.ConfigureServices) });

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
    public void CreateEntry_CanActivate_ReturnOK()
    {
        var componentContext = new ServiceComponentContext(Host.CreateApplicationBuilder());
        ComponentBase.CreateEntry(typeof(A1Component), componentContext, new[] { nameof(ComponentBase.PreConfigureServices), nameof(ComponentBase.ConfigureServices) });

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
        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.NotifyInvocation(null!, null!, null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.NotifyInvocation(new(new()), null!, null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.NotifyInvocation(new(new()), new AComponent(), null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.NotifyInvocation(new(new()), new AComponent(), new ServiceComponentContext(Host.CreateApplicationBuilder()), null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            ComponentBase.NotifyInvocation(new(new()), new AComponent(), new ServiceComponentContext(Host.CreateApplicationBuilder()), string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            ComponentBase.NotifyInvocation(new(new()), new AComponent(), new ServiceComponentContext(Host.CreateApplicationBuilder()), "");
        });
    }

    [Fact]
    public void NotifyInvocation_ReturnOK()
    {
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));
        var dependencyGraph = new DependencyGraph(dependencies);
        var componentContext = new ServiceComponentContext(Host.CreateApplicationBuilder());
        var component = new AComponent
        {
            Options = componentContext.Options
        };
        componentContext.Options.Components.TryAdd(typeof(AComponent), component);
        var methodName = nameof(AComponent.PreConfigureServices);

        ComponentBase.NotifyInvocation(dependencyGraph, component, componentContext, methodName);
        Assert.Equal("Furion.Component.Tests.AComponent.PreConfigureServices", component.Items.ElementAt(0));

        var component2 = new BComponent
        {
            Options = componentContext.Options
        };
        componentContext.Options.Components.TryAdd(typeof(BComponent), component2);
        ComponentBase.NotifyInvocation(dependencyGraph, component2, componentContext, methodName);
        Assert.Equal("Furion.Component.Tests.BComponent.PreConfigureServices", component.Items.ElementAt(1));
    }

    [Fact]
    public void InvokeMethod_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.InvokeMethod(null!, null!, null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.InvokeMethod(new(new()), null!, null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.InvokeMethod(new(new()), new AComponent(), null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.InvokeMethod(new(new()), new AComponent(), new ServiceComponentContext(Host.CreateApplicationBuilder()), null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            ComponentBase.InvokeMethod(new(new()), new AComponent(), new ServiceComponentContext(Host.CreateApplicationBuilder()), string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            ComponentBase.InvokeMethod(new(new()), new AComponent(), new ServiceComponentContext(Host.CreateApplicationBuilder()), "");
        });
    }

    [Fact]
    public void InvokeMethod_ReturnOK()
    {
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));
        var dependencyGraph = new DependencyGraph(dependencies);
        var componentContext = new ServiceComponentContext(Host.CreateApplicationBuilder());
        var component = new AComponent
        {
            Options = componentContext.Options
        };
        componentContext.Options.Components.TryAdd(typeof(AComponent), component);
        var methodName = nameof(AComponent.PreConfigureServices);

        ComponentBase.InvokeMethod(dependencyGraph, component, componentContext, methodName);
        Assert.Equal("Furion.Component.Tests.AComponent.PreConfigureServices", component.Items.ElementAt(0));
    }

    [Fact]
    public void IsWebComponent_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.IsWebComponent(null!);
        });
    }

    [Fact]
    public void IsWebComponent_ReturnOK()
    {
        Assert.False(ComponentBase.IsWebComponent(typeof(AComponent)));
    }
}