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

public class DependencyGraphTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var dependencyGraph = new DependencyGraph(null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(DependencyGraph1), new[]{ typeof(DependencyGraph2), typeof(DependencyGraph3) }},
            {typeof(DependencyGraph2), new[]{ typeof(DependencyGraph4), typeof(DependencyGraph5) }},
            {typeof(DependencyGraph3), new[]{ typeof(DependencyGraph4),typeof(DependencyGraph6) }}
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        Assert.NotNull(dependencyGraph);
        Assert.NotNull(dependencyGraph._dependencies);
        Assert.NotEmpty(dependencyGraph._dependencies);
        Assert.NotNull(dependencyGraph._ancestorsNodes);
        Assert.NotEmpty(dependencyGraph._ancestorsNodes);
        Assert.NotNull(dependencyGraph._descendantsNodes);
        Assert.NotEmpty(dependencyGraph._descendantsNodes);
    }

    [Fact]
    public void BuildAncestorsAndDescendantsNodes_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(DependencyGraph1), new[]{ typeof(DependencyGraph2), typeof(DependencyGraph3) }},
            {typeof(DependencyGraph2), new[]{ typeof(DependencyGraph4), typeof(DependencyGraph5) }},
            {typeof(DependencyGraph3), new[]{ typeof(DependencyGraph4),typeof(DependencyGraph6) }}
        };

        var dependencyGraph = new DependencyGraph(dependencies);
        dependencyGraph._ancestorsNodes.Clear();
        dependencyGraph._descendantsNodes.Clear();

        dependencyGraph.BuildAncestorsAndDescendantsNodes();

        Assert.Equal(5, dependencyGraph._ancestorsNodes.Count);
        Assert.Equal(typeof(DependencyGraph2), dependencyGraph._ancestorsNodes.Keys.ElementAt(0));
        Assert.Equal(new[] { typeof(DependencyGraph1) }, dependencyGraph._ancestorsNodes.Values.ElementAt(0));

        Assert.Equal(typeof(DependencyGraph3), dependencyGraph._ancestorsNodes.Keys.ElementAt(1));
        Assert.Equal(new[] { typeof(DependencyGraph1) }, dependencyGraph._ancestorsNodes.Values.ElementAt(1));

        Assert.Equal(typeof(DependencyGraph4), dependencyGraph._ancestorsNodes.Keys.ElementAt(2));
        Assert.Equal(new[] { typeof(DependencyGraph2), typeof(DependencyGraph3) }, dependencyGraph._ancestorsNodes.Values.ElementAt(2));

        Assert.Equal(typeof(DependencyGraph5), dependencyGraph._ancestorsNodes.Keys.ElementAt(3));
        Assert.Equal(new[] { typeof(DependencyGraph2) }, dependencyGraph._ancestorsNodes.Values.ElementAt(3));

        Assert.Equal(typeof(DependencyGraph6), dependencyGraph._ancestorsNodes.Keys.ElementAt(4));
        Assert.Equal(new[] { typeof(DependencyGraph3) }, dependencyGraph._ancestorsNodes.Values.ElementAt(4));

        Assert.Equal(3, dependencyGraph._descendantsNodes.Count);
        Assert.Equal(typeof(DependencyGraph1), dependencyGraph._descendantsNodes.Keys.ElementAt(0));
        Assert.Equal(new[] { typeof(DependencyGraph2), typeof(DependencyGraph3) }, dependencyGraph._descendantsNodes.Values.ElementAt(0));

        Assert.Equal(typeof(DependencyGraph2), dependencyGraph._descendantsNodes.Keys.ElementAt(1));
        Assert.Equal(new[] { typeof(DependencyGraph4), typeof(DependencyGraph5) }, dependencyGraph._descendantsNodes.Values.ElementAt(1));

        Assert.Equal(typeof(DependencyGraph3), dependencyGraph._descendantsNodes.Keys.ElementAt(2));
        Assert.Equal(new[] { typeof(DependencyGraph4), typeof(DependencyGraph6) }, dependencyGraph._descendantsNodes.Values.ElementAt(2));
    }

    [Fact]
    public void FindAllAncestors_Invalid_Parameters()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(DependencyGraph1), new[]{ typeof(DependencyGraph2), typeof(DependencyGraph3) }},
            {typeof(DependencyGraph2), new[]{ typeof(DependencyGraph4), typeof(DependencyGraph5) }},
            {typeof(DependencyGraph3), new[]{ typeof(DependencyGraph4),typeof(DependencyGraph6) }}
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        Assert.Throws<ArgumentNullException>(() =>
        {
            dependencyGraph.FindAllAncestors(null!);
        });
    }

    [Fact]
    public void FindAllAncestors_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(DependencyGraph1), new[]{ typeof(DependencyGraph2), typeof(DependencyGraph3) }},
            {typeof(DependencyGraph2), new[]{ typeof(DependencyGraph4), typeof(DependencyGraph5) }},
            {typeof(DependencyGraph3), new[]{ typeof(DependencyGraph4),typeof(DependencyGraph6) }}
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        Assert.Equal(new List<Type>(), dependencyGraph.FindAllAncestors(typeof(DependencyGraph1)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph1) }, dependencyGraph.FindAllAncestors(typeof(DependencyGraph2)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph1) }, dependencyGraph.FindAllAncestors(typeof(DependencyGraph3)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph2), typeof(DependencyGraph3), typeof(DependencyGraph1), typeof(DependencyGraph1) }, dependencyGraph.FindAllAncestors(typeof(DependencyGraph4)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph2), typeof(DependencyGraph1) }, dependencyGraph.FindAllAncestors(typeof(DependencyGraph5)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph3), typeof(DependencyGraph1) }, dependencyGraph.FindAllAncestors(typeof(DependencyGraph6)));
    }

    [Fact]
    public void FindAllDescendants_Invalid_Parameters()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(DependencyGraph1), new[]{ typeof(DependencyGraph2), typeof(DependencyGraph3) }},
            {typeof(DependencyGraph2), new[]{ typeof(DependencyGraph4), typeof(DependencyGraph5) }},
            {typeof(DependencyGraph3), new[]{ typeof(DependencyGraph4),typeof(DependencyGraph6) }}
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        Assert.Throws<ArgumentNullException>(() =>
        {
            dependencyGraph.FindAllDescendants(null!);
        });
    }

    [Fact]
    public void FindAllDescendants_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(DependencyGraph1), new[]{ typeof(DependencyGraph2), typeof(DependencyGraph3) }},
            {typeof(DependencyGraph2), new[]{ typeof(DependencyGraph4), typeof(DependencyGraph5) }},
            {typeof(DependencyGraph3), new[]{ typeof(DependencyGraph4),typeof(DependencyGraph6) }}
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        Assert.Equal(new List<Type> { typeof(DependencyGraph2), typeof(DependencyGraph3), typeof(DependencyGraph4), typeof(DependencyGraph5), typeof(DependencyGraph4), typeof(DependencyGraph6), }, dependencyGraph.FindAllDescendants(typeof(DependencyGraph1)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph4), typeof(DependencyGraph5) }, dependencyGraph.FindAllDescendants(typeof(DependencyGraph2)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph4), typeof(DependencyGraph6) }, dependencyGraph.FindAllDescendants(typeof(DependencyGraph3)));
        Assert.Equal(new List<Type> { }, dependencyGraph.FindAllDescendants(typeof(DependencyGraph4)));
        Assert.Equal(new List<Type> { }, dependencyGraph.FindAllDescendants(typeof(DependencyGraph5)));
        Assert.Equal(new List<Type> { }, dependencyGraph.FindAllDescendants(typeof(DependencyGraph6)));
    }

    [Fact]
    public void FindAncestors_Invalid_Parameters()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(DependencyGraph1), new[]{ typeof(DependencyGraph2), typeof(DependencyGraph3) }},
            {typeof(DependencyGraph2), new[]{ typeof(DependencyGraph4), typeof(DependencyGraph5) }},
            {typeof(DependencyGraph3), new[]{ typeof(DependencyGraph4),typeof(DependencyGraph6) }}
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        Assert.Throws<ArgumentNullException>(() =>
        {
            dependencyGraph.FindAncestors(null!);
        });
    }

    [Fact]
    public void FindAncestors_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(DependencyGraph1), new[]{ typeof(DependencyGraph2), typeof(DependencyGraph3) }},
            {typeof(DependencyGraph2), new[]{ typeof(DependencyGraph4), typeof(DependencyGraph5) }},
            {typeof(DependencyGraph3), new[]{ typeof(DependencyGraph4),typeof(DependencyGraph6) }}
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        Assert.Equal(new List<Type>(), dependencyGraph.FindAncestors(typeof(DependencyGraph1)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph1) }, dependencyGraph.FindAncestors(typeof(DependencyGraph2)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph1) }, dependencyGraph.FindAncestors(typeof(DependencyGraph3)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph2), typeof(DependencyGraph3), typeof(DependencyGraph1) }, dependencyGraph.FindAncestors(typeof(DependencyGraph4)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph2), typeof(DependencyGraph1) }, dependencyGraph.FindAncestors(typeof(DependencyGraph5)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph3), typeof(DependencyGraph1) }, dependencyGraph.FindAncestors(typeof(DependencyGraph6)));
    }

    [Fact]
    public void FindDescendants_Invalid_Parameters()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(DependencyGraph1), new[]{ typeof(DependencyGraph2), typeof(DependencyGraph3) }},
            {typeof(DependencyGraph2), new[]{ typeof(DependencyGraph4), typeof(DependencyGraph5) }},
            {typeof(DependencyGraph3), new[]{ typeof(DependencyGraph4),typeof(DependencyGraph6) }}
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        Assert.Throws<ArgumentNullException>(() =>
        {
            dependencyGraph.FindDescendants(null!);
        });
    }

    [Fact]
    public void FindDescendants_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            {typeof(DependencyGraph1), new[]{ typeof(DependencyGraph2), typeof(DependencyGraph3) }},
            {typeof(DependencyGraph2), new[]{ typeof(DependencyGraph4), typeof(DependencyGraph5) }},
            {typeof(DependencyGraph3), new[]{ typeof(DependencyGraph4),typeof(DependencyGraph6) }}
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        Assert.Equal(new List<Type> { typeof(DependencyGraph2), typeof(DependencyGraph3), typeof(DependencyGraph4), typeof(DependencyGraph5), typeof(DependencyGraph6), }, dependencyGraph.FindDescendants(typeof(DependencyGraph1)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph4), typeof(DependencyGraph5) }, dependencyGraph.FindDescendants(typeof(DependencyGraph2)));
        Assert.Equal(new List<Type> { typeof(DependencyGraph4), typeof(DependencyGraph6) }, dependencyGraph.FindDescendants(typeof(DependencyGraph3)));
        Assert.Equal(new List<Type> { }, dependencyGraph.FindDescendants(typeof(DependencyGraph4)));
        Assert.Equal(new List<Type> { }, dependencyGraph.FindDescendants(typeof(DependencyGraph5)));
        Assert.Equal(new List<Type> { }, dependencyGraph.FindDescendants(typeof(DependencyGraph6)));
    }
}