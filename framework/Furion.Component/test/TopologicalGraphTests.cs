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

public class TopologicalGraphTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var topologicalGraph = new TopologicalGraph(null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(TopologicalGraph1), new[]{ typeof(TopologicalGraph2), typeof(TopologicalGraph3) }},
            {typeof(TopologicalGraph2), new[]{ typeof(TopologicalGraph4), typeof(TopologicalGraph5) }},
            {typeof(TopologicalGraph3), new[]{ typeof(TopologicalGraph4),typeof(TopologicalGraph6) }}
        };

        var topologicalGraph = new TopologicalGraph(dependencies);

        Assert.NotNull(topologicalGraph);
        Assert.NotNull(topologicalGraph._dependencies);
        Assert.NotEmpty(topologicalGraph._dependencies);
    }

    [Fact]
    public void Sort_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(TopologicalGraph1), new[]{ typeof(TopologicalGraph2), typeof(TopologicalGraph3) }},
            {typeof(TopologicalGraph2), new[]{ typeof(TopologicalGraph4), typeof(TopologicalGraph5) }},
            {typeof(TopologicalGraph3), new[]{ typeof(TopologicalGraph4),typeof(TopologicalGraph6) }}
        };

        var topologicalGraph = new TopologicalGraph(dependencies);

        var sortedList = topologicalGraph.Sort();

        Assert.NotNull(sortedList);
        Assert.Equal(new List<Type> { typeof(TopologicalGraph4), typeof(TopologicalGraph5), typeof(TopologicalGraph2), typeof(TopologicalGraph6), typeof(TopologicalGraph3), typeof(TopologicalGraph1) }, sortedList);
    }

    [Fact]
    public void VisitNode_Invalid_Parameters()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(TopologicalGraph1), new[]{ typeof(TopologicalGraph2), typeof(TopologicalGraph3) }},
            {typeof(TopologicalGraph2), new[]{ typeof(TopologicalGraph4), typeof(TopologicalGraph5) }},
            {typeof(TopologicalGraph3), new[]{ typeof(TopologicalGraph4),typeof(TopologicalGraph6) }}
        };

        var topologicalGraph = new TopologicalGraph(dependencies);

        Assert.Throws<ArgumentNullException>(() =>
        {
            topologicalGraph.VisitNode(null!, null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            topologicalGraph.VisitNode(typeof(TopologicalGraph1), null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            topologicalGraph.VisitNode(typeof(TopologicalGraph1), new(), null!);
        });
    }

    [Fact]
    public void VisitNode_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(TopologicalGraph1), new[]{ typeof(TopologicalGraph2), typeof(TopologicalGraph3) }},
            {typeof(TopologicalGraph2), new[]{ typeof(TopologicalGraph4), typeof(TopologicalGraph5) }},
            {typeof(TopologicalGraph3), new[]{ typeof(TopologicalGraph4),typeof(TopologicalGraph6) }}
        };

        var topologicalGraph = new TopologicalGraph(dependencies);
        var sortedNodes = new List<Type>();
        var visitedNodes = new HashSet<Type>();

        topologicalGraph.VisitNode(typeof(TopologicalGraph1), visitedNodes, sortedNodes);

        Assert.NotEmpty(sortedNodes);
        Assert.NotEmpty(visitedNodes);
    }

    [Fact]
    public void HasCycle_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(TopologicalGraph1), new[]{ typeof(TopologicalGraph2), typeof(TopologicalGraph3) }},
            {typeof(TopologicalGraph2), new[]{ typeof(TopologicalGraph4), typeof(TopologicalGraph5) }},
            {typeof(TopologicalGraph3), new[]{ typeof(TopologicalGraph4),typeof(TopologicalGraph6) }}
        };

        var topologicalGraph = new TopologicalGraph(dependencies);
        Assert.False(topologicalGraph.HasCycle());

        var dependencies2 = new Dictionary<Type, Type[]>
        {
            {typeof(TopologicalGraph1), new[]{ typeof(TopologicalGraph2), typeof(TopologicalGraph3) }},
            {typeof(TopologicalGraph2), new[]{ typeof(TopologicalGraph4), typeof(TopologicalGraph5) }},
            {typeof(TopologicalGraph3), new[]{ typeof(TopologicalGraph1), typeof(TopologicalGraph4),typeof(TopologicalGraph6) }}
        };

        var topologicalGraph2 = new TopologicalGraph(dependencies2);
        Assert.True(topologicalGraph2.HasCycle());
    }

    [Fact]
    public void HasCycleHelper_Invalid_Parameters()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(TopologicalGraph1), new[]{ typeof(TopologicalGraph2), typeof(TopologicalGraph3) }},
            {typeof(TopologicalGraph2), new[]{ typeof(TopologicalGraph4), typeof(TopologicalGraph5) }},
            {typeof(TopologicalGraph3), new[]{ typeof(TopologicalGraph4),typeof(TopologicalGraph6) }}
        };

        var topologicalGraph = new TopologicalGraph(dependencies);

        Assert.Throws<ArgumentNullException>(() =>
        {
            topologicalGraph.HasCycleHelper(null!, null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            topologicalGraph.HasCycleHelper(typeof(TopologicalGraph1), null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            topologicalGraph.HasCycleHelper(typeof(TopologicalGraph1), new(), null!);
        });
    }

    [Fact]
    public void HasCycleHelper_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(TopologicalGraph1), new[]{ typeof(TopologicalGraph2), typeof(TopologicalGraph3) }},
            {typeof(TopologicalGraph2), new[]{ typeof(TopologicalGraph4), typeof(TopologicalGraph5) }},
            {typeof(TopologicalGraph3), new[]{ typeof(TopologicalGraph4),typeof(TopologicalGraph6) }}
        };

        var topologicalGraph = new TopologicalGraph(dependencies);
        var pathNodes = new HashSet<Type>();
        var visitedNodes = new HashSet<Type>();

        topologicalGraph.HasCycleHelper(typeof(TopologicalGraph1), visitedNodes, pathNodes);

        Assert.Empty(pathNodes);
        Assert.NotEmpty(visitedNodes);
    }
}