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
    [Theory]
    [InlineData(typeof(AComponent))]
    [InlineData(typeof(BComponent))]
    [InlineData(typeof(AWebComponent))]
    [InlineData(typeof(BWebComponent))]
    [InlineData(typeof(OneEmptyConstructorComponent))]
    [InlineData(typeof(ManyConstructorComponent))]
    [InlineData(typeof(InnerCommponent))]
    public void CheckComponent_ReturnOK(Type componentType)
    {
        ComponentBase.CheckComponent(componentType);
    }

    [Theory]
    [InlineData(typeof(NotComponent))]
    [InlineData(typeof(ComponentBase))]
    [InlineData(typeof(WebComponent))]
    [InlineData(typeof(AbstractComponent))]
    [InlineData(typeof(InheritComponent))]
    [InlineData(typeof(OneConstructorComponent))]
    [InlineData(typeof(OneInternalConstructorComponent))]
    public void CheckComponent_ReturnOops(Type componentType)
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.CheckComponent(componentType);
        });
    }

    [Fact]
    public void CheckComponentDependencies_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
             { typeof(AComponent), new[]{ typeof(BComponent),typeof(CComponent)} },
             { typeof(BComponent), new[]{ typeof(CComponent),typeof(DComponent)} },
             { typeof(FComponent), new[]{ typeof(CComponent),typeof(EComponent)} },
             { typeof(GComponent), Array.Empty<Type>() }
        };

        ComponentBase.CheckComponentDependencies(dependencies);
    }

    [Fact]
    public void CheckComponentDependencies_Circular_ReturnOops()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            { typeof(AComponent), new[]{ typeof(BComponent),typeof(CComponent)} },
            { typeof(BComponent), new[]{ typeof(AComponent),typeof(DComponent)} },
            { typeof(DComponent), new[]{ typeof(AComponent) } }
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.CheckComponentDependencies(dependencies);
        });

        Assert.Equal("The dependency relationship has a circular dependency.", exception.Message);
    }

    [Fact]
    public void CheckComponentDependencies_Circular2_ReturnOops()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            { typeof(AComponent), new[]{ typeof(AComponent)} }
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.CheckComponentDependencies(dependencies);
        });

        Assert.Equal("The dependency relationship has a circular dependency.", exception.Message);
    }

    [Theory]
    [InlineData(typeof(NotComponent), "Type 'NotComponent' is not assignable from 'ComponentBase'.")]
    [InlineData(typeof(ComponentBase), "The component cannot be an abstract type or a ComponentBase or WebComponent type.")]
    [InlineData(typeof(WebComponent), "The component cannot be an abstract type or a ComponentBase or WebComponent type.")]
    [InlineData(typeof(AbstractComponent), "The component cannot be an abstract type or a ComponentBase or WebComponent type.")]
    [InlineData(typeof(InheritComponent), "Components are not allowed to inherit from each other.")]
    [InlineData(typeof(OneConstructorComponent), "A component must have at least one public parameterless constructor.")]
    [InlineData(typeof(OneInternalConstructorComponent), "A component must have at least one public parameterless constructor.")]
    public void CheckComponentDependencies_EveryComponent_ReturnOops(Type componentType, string message)
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            { typeof(AComponent), new[]{ componentType, typeof(CComponent)} },
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.CheckComponentDependencies(dependencies);
        });

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void GenerateComponentDependencies_ReturnOK()
    {
        var dependencies = ComponentBase.GenerateComponentDependencies(typeof(AComponent));
        Assert.Equal(4, dependencies.Count);

        Assert.Equal(typeof(AComponent), dependencies.Keys.ElementAt(0));
        Assert.Equal(typeof(BComponent), dependencies.Values.ElementAt(0)[0]);
        Assert.Equal(typeof(CComponent), dependencies.Values.ElementAt(0)[1]);

        Assert.Equal(typeof(BComponent), dependencies.Keys.ElementAt(1));
        Assert.Equal(typeof(CComponent), dependencies.Values.ElementAt(1)[0]);
        Assert.Equal(typeof(DComponent), dependencies.Values.ElementAt(1)[1]);

        Assert.Equal(typeof(CComponent), dependencies.Keys.ElementAt(2));
        Assert.Empty(dependencies.Values.ElementAt(2));

        Assert.Equal(typeof(DComponent), dependencies.Keys.ElementAt(3));
        Assert.Empty(dependencies.Values.ElementAt(3));
    }

    [Fact]
    public void GenerateComponentDependencies_NotComponent_ReturnOops()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var dependencies = ComponentBase.GenerateComponentDependencies(typeof(NotComponent));
        });

        Assert.Equal("Type 'NotComponent' is not assignable from 'ComponentBase'.", exception.Message);
    }

    [Fact]
    public void GenerateComponentDependencies_ReturnOops()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var dependencies = ComponentBase.GenerateComponentDependencies<DependencyNotComponent>();
        });

        Assert.Equal("Type 'NotComponent' is not assignable from 'ComponentBase'.", exception.Message);
    }

    [Fact]
    public void GenerateTopologicalSortedMap_ReturnOK()
    {
        var sortedList = ComponentBase.GenerateTopologicalSortedMap(typeof(AComponent));
        Assert.Equal(4, sortedList.Count);

        // C D B A
        Assert.Equal(typeof(CComponent), sortedList.ElementAt(0));
        Assert.Equal(typeof(DComponent), sortedList.ElementAt(1));
        Assert.Equal(typeof(BComponent), sortedList.ElementAt(2));
        Assert.Equal(typeof(AComponent), sortedList.ElementAt(3));
    }
}