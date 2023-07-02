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

namespace Furion.Component.Tests;

public class ComponentBaseTests
{
    [Fact]
    public void NewInstance_Default()
    {
        var component = new CBaseComponent();

        Assert.NotNull(component);
        Assert.Null(component.Options);

        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        var componentContext = new ServiceComponentContext(services, configuration);

        component.PreConfigureServices(componentContext);
        component.ConfigureServices(componentContext);
        Assert.True(component.CanActivate(componentContext));
    }

    [Fact]
    public void Props_Null_Throw()
    {
        var component = new CBaseComponent();
        Assert.Throws<ArgumentNullException>(() =>
        {
            component.Props<ComponentActionOptions>(null!);
        });
    }

    [Fact]
    public void Props_Options_Null_Throw()
    {
        var component = new CBaseComponent();
        Assert.Throws<ArgumentNullException>(() =>
        {
            component.Props<ComponentActionOptions>(options =>
            {
            });
        });
    }

    [Fact]
    public void Props_ReturnOK()
    {
        var services = new ServiceCollection();
        var component = new CBaseComponent
        {
            Options = services.GetComponentOptions()
        };

        Assert.NotNull(component.Options);
        Assert.Empty(component.Options.PropsActions);

        component.Props<ComponentActionOptions>(options =>
        {
        });

        Assert.Single(component.Options.PropsActions);
    }

    [Fact]
    public void Check_Null_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.Check(null!);
        });
    }

    [Theory]
    [InlineData(typeof(SomeClass), "`SomeClass` component type is not assignable from `ComponentBase`.")]
    [InlineData(typeof(ComponentBase), "Component type cannot be a `ComponentBase` or `WebComponent`.")]
    [InlineData(typeof(InheritComponent), "`InheritComponent` component type cannot inherit from other component types.")]
    [InlineData(typeof(AbstractComponent), "`AbstractComponent` component type must be able to be instantiated.")]
    public void Check_Throw(Type componentType, string message)
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.Check(componentType);
        });

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void CheckDependencies_Null_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentBase.CheckDependencies(null!);
        });
    }

    [Fact]
    public void CheckDependencies_Check_Throw()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(AComponent),new[]{typeof(BComponent),typeof(CComponent)} },
            {typeof(CComponent),new[]{typeof(SomeClass),typeof(BComponent)} }
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.CheckDependencies(dependencies);
        });

        Assert.Equal("`SomeClass` component type is not assignable from `ComponentBase`.", exception.Message);
    }

    [Fact]
    public void CheckDependencies_CircularDependency_Throw()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(AComponent),new[]{typeof(BComponent),typeof(CComponent)} },
            {typeof(CComponent),new[]{typeof(AComponent),typeof(BComponent)} }
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.CheckDependencies(dependencies);
        });

        Assert.Equal("The dependency relationship has a circular dependency.", exception.Message);
    }

    [Fact]
    public void CreateDependencies()
    {
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));

        Assert.Equal(4, dependencies.Count);

        Assert.Equal(typeof(AComponent), dependencies.Keys.ElementAt(0));
        Assert.Equal(typeof(BComponent), dependencies.Values.ElementAt(0)[0]);
        Assert.Equal(typeof(CComponent), dependencies.Values.ElementAt(0)[1]);

        Assert.Equal(typeof(BComponent), dependencies.Keys.ElementAt(1));
        Assert.Equal(typeof(CComponent), dependencies.Values.ElementAt(1)[0]);
        Assert.Equal(typeof(DComponent), dependencies.Values.ElementAt(1)[1]);

        Assert.Equal(typeof(CComponent), dependencies.Keys.ElementAt(2));
        Assert.Equal(typeof(DComponent), dependencies.Values.ElementAt(2)[0]);

        Assert.Equal(typeof(DComponent), dependencies.Keys.ElementAt(3));
        Assert.Empty(dependencies.Values.ElementAt(3));
    }

    [Fact]
    public void CreateTopological()
    {
        // D C B A
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));
        var list = ComponentBase.CreateTopological(dependencies);

        Assert.Equal(4, list.Count);
        Assert.Equal(typeof(DComponent), list.ElementAt(0));
        Assert.Equal(typeof(CComponent), list.ElementAt(1));
        Assert.Equal(typeof(BComponent), list.ElementAt(2));
        Assert.Equal(typeof(AComponent), list.ElementAt(3));
    }

    [Fact]
    public void CreateTopological_Predicate()
    {
        // D C B A
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));
        var list = ComponentBase.CreateTopological(dependencies, t => t == typeof(AComponent));

        Assert.Single(list);
        Assert.Equal(typeof(AComponent), list.ElementAt(0));
    }

    [Fact]
    public void CreateTopological_ForType()
    {
        // D C B A
        var list = ComponentBase.CreateTopological(typeof(AComponent));

        Assert.Equal(4, list.Count);
        Assert.Equal(typeof(DComponent), list.ElementAt(0));
        Assert.Equal(typeof(CComponent), list.ElementAt(1));
        Assert.Equal(typeof(BComponent), list.ElementAt(2));
        Assert.Equal(typeof(AComponent), list.ElementAt(3));
    }

    [Fact]
    public void CreateTopological_ForType_Predicate()
    {
        // D C B A
        var list = ComponentBase.CreateTopological(typeof(AComponent), t => t == typeof(AComponent));

        Assert.Single(list);
        Assert.Equal(typeof(AComponent), list.ElementAt(0));
    }

    [Theory]
    [InlineData(typeof(InvalidArgumentComponnet), "`InvalidOptions` parameter type is an invalid component options.")]
    [InlineData(typeof(InvalidArgument2Componnet), "`Action`1` parameter type is an invalid component options.")]
    public void CreateInstance_InvalidArgument_Throw(Type componentType, string message)
    {
        var services = new ServiceCollection();
        var componentOptions = services.GetComponentOptions();

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.CreateInstance(componentType, componentOptions);
        });

        Assert.Equal(message, exception.Message);
    }

    [Theory]
    [InlineData(typeof(OkArgumentComponent))]
    [InlineData(typeof(OkArgument2Component))]
    [InlineData(typeof(OkArgument3Component))]
    [InlineData(typeof(PrivateNewComponent))]
    public void CreateInstance_ReturnOK(Type componentType)
    {
        var services = new ServiceCollection();
        var componentOptions = services.GetComponentOptions();

        var component = ComponentBase.CreateInstance(componentType, componentOptions);
        Assert.NotNull(component);
        Assert.NotNull(component.Options);
    }

    [Fact]
    public void CreateInstance_PropertyProps()
    {
        var services = new ServiceCollection();
        var componentOptions = services.GetComponentOptions();

        var component = ComponentBase.CreateInstance(typeof(PropertyComponent), componentOptions);
        Assert.NotNull(component);
        Assert.NotNull(component.Options);
    }

    [Theory]
    [InlineData(typeof(PropertyInvalidComponent))]
    [InlineData(typeof(PropertyReadonlyComponent))]
    public void CreateInstance_PropertyProps_Throw(Type componentType)
    {
        var services = new ServiceCollection();
        var componentOptions = services.GetComponentOptions();

        Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.CreateInstance(componentType, componentOptions);
        });
    }

    [Theory]
    [InlineData(typeof(AComponent), false)]
    [InlineData(typeof(ComponentBase), false)]
    [InlineData(typeof(InvalidArgumentComponnet), false)]
    [InlineData(typeof(OkArgumentComponent), false)]
    public void IsWebComponent(Type componentType, bool result)
    {
        Assert.Equal(result, ComponentBase.IsWebComponent(componentType));
    }

    [Fact]
    public void CreateComponents()
    {
        var services = new ServiceCollection();
        var componentContext = new ServiceComponentContext(services, new ConfigurationManager());
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));

        // D C B A
        var list = ComponentBase.CreateComponents(dependencies, componentContext);
        Assert.NotNull(list);

        Assert.Equal(4, list.Count);

        Assert.Equal(typeof(DComponent), list[0].GetType());
        Assert.Equal(typeof(CComponent), list[1].GetType());
        Assert.Equal(typeof(BComponent), list[2].GetType());
        Assert.Equal(typeof(AComponent), list[3].GetType());

        // 避免引发重复调用检查
        componentContext.Options.InvokeRecords.Clear();
        var list2 = ComponentBase.CreateComponents<ComponentBase>(dependencies, componentContext);
        Assert.NotNull(list2);

        Assert.Equal(4, list2.Count);

        Assert.Equal(typeof(DComponent), list2[0].GetType());
        Assert.Equal(typeof(CComponent), list2[1].GetType());
        Assert.Equal(typeof(BComponent), list2[2].GetType());
        Assert.Equal(typeof(AComponent), list2[3].GetType());
    }

    [Fact]
    public void CreateComponents_CanActivate()
    {
        var services = new ServiceCollection();
        var componentContext = new ServiceComponentContext(services, new ConfigurationManager());
        var dependencies = ComponentBase.CreateDependencies(typeof(EComponent));

        var list = ComponentBase.CreateComponents(dependencies, componentContext);
        Assert.NotNull(list);

        Assert.Equal(2, list.Count);
        Assert.DoesNotContain(list, c => c.GetType() == typeof(DComponent));
    }

    [Fact]
    public void GetProps_Valid()
    {
        var componentOptions = new ComponentOptions();

        ComponentBase.GetProps(typeof(OkOptions), componentOptions);
        ComponentBase.GetProps(typeof(Action<OkOptions>), componentOptions);
    }

    [Fact]
    public void GetProps_invalid_Throw()
    {
        var componentOptions = new ComponentOptions();

        Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.GetProps(typeof(InvalidOptions), componentOptions);
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.GetProps(typeof(Action<InvalidOptions>), componentOptions);
        });
    }
}