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

public class TopologicalTests
{
    [Fact]
    public void GenerateTopologicalMap_ReturnOK()
    {
        var topologicalMap = ComponentBase.GenerateTopologicalMap(typeof(AComponent));
        Assert.NotNull(topologicalMap);

        var firstComponent = topologicalMap.First();
        Assert.Equal(typeof(FComponent), firstComponent);
    }

    [Fact]
    public void GenerateTopologicalMap_ReturnOops()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var topologicalMap = ComponentBase.GenerateTopologicalMap(typeof(NotComponent));
        });

        Assert.Equal("Type 'NotComponent' is not assignable from 'ComponentBase'.", exception.Message);
    }

    [Fact]
    public void Topological_Sort_EmptyDependences_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(AComponent), Array.Empty<Type>() },
            {typeof(BComponent), Array.Empty<Type>() },
            {typeof(CComponent), Array.Empty<Type>() },
            {typeof(DComponent), Array.Empty<Type>() },
            {typeof(EComponent), Array.Empty<Type>() },
            {typeof(FComponent), Array.Empty<Type>() },
            {typeof(GComponent), Array.Empty<Type>() },
        };

        // A B C D E F G
        var sortedNodes = ComponentBase.GenerateTopologicalMap(dependencies);
        Assert.Equal(dependencies.Keys.ElementAt(0), sortedNodes[0]);
        Assert.Equal(dependencies.Keys.ElementAt(1), sortedNodes[1]);
        Assert.Equal(dependencies.Keys.ElementAt(2), sortedNodes[2]);
        Assert.Equal(dependencies.Keys.ElementAt(3), sortedNodes[3]);
        Assert.Equal(dependencies.Keys.ElementAt(4), sortedNodes[4]);
        Assert.Equal(dependencies.Keys.ElementAt(5), sortedNodes[5]);
        Assert.Equal(dependencies.Keys.ElementAt(6), sortedNodes[6]);
    }

    [Fact]
    public void Topological_Sort_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(AComponent),new[]{ typeof(BComponent), typeof(CComponent) } },
            {typeof(CComponent),new[]{ typeof(FComponent), typeof(DComponent), typeof(EComponent) } },
            {typeof(BComponent),new[]{ typeof(CComponent), typeof(EComponent) } },
            {typeof(GComponent), Array.Empty<Type>() }
        };

        // F D E C B A G
        var sortedNodes = ComponentBase.GenerateTopologicalMap(dependencies);
        Assert.Equal(typeof(FComponent), sortedNodes[0]);
        Assert.Equal(typeof(DComponent), sortedNodes[1]);
        Assert.Equal(typeof(EComponent), sortedNodes[2]);
        Assert.Equal(typeof(CComponent), sortedNodes[3]);
        Assert.Equal(typeof(BComponent), sortedNodes[4]);
        Assert.Equal(typeof(AComponent), sortedNodes[5]);
        Assert.Equal(typeof(GComponent), sortedNodes[6]);
    }

    [Fact]
    public void Topological_HasCycle_ReturnOops()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(AComponent),new[]{ typeof(BComponent), typeof(CComponent) } },
            {typeof(CComponent),new[]{ typeof(FComponent), typeof(DComponent), typeof(EComponent), typeof(AComponent) } },
            {typeof(BComponent),new[]{ typeof(CComponent), typeof(EComponent) } },
            {typeof(GComponent), Array.Empty<Type>() }
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var sortedNodes = ComponentBase.GenerateTopologicalMap(dependencies);
        });

        Assert.Equal("The dependency relationship has a circular dependency.", exception.Message);
    }

    [Fact]
    public void CheckComponent_Inherited_ReturnOK()
    {
        ComponentBase.CheckComponent(typeof(AComponent));
    }

    [Fact]
    public void CheckComponent_Inherited_ReturnOops()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.CheckComponent(typeof(InheritedComponent));
        });

        Assert.Equal("Components are not allowed to inherit from each other.", exception.Message);
    }
}