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

public class DependencyGraphTests
{
    [Fact]
    public void NewInstance_Null_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var dependencyGraph = new DependencyGraph(null!);
        });
    }

    [Fact]
    public void NewInstance_Default()
    {
        var dependencyGraph = new DependencyGraph(new());

        Assert.NotNull(dependencyGraph);
        Assert.NotNull(dependencyGraph._dependencies);
        Assert.NotNull(dependencyGraph._ancestorsNodes);
        Assert.NotNull(dependencyGraph._descendantsNodes);

        Assert.Empty(dependencyGraph._dependencies);
        Assert.Empty(dependencyGraph._ancestorsNodes);
        Assert.Empty(dependencyGraph._descendantsNodes);
    }

    [Fact]
    public void BuildAncestorsAndDescendantsNodes()
    {
        var dependencies = new Dictionary<Type, List<Type>>()
        {
            { typeof(AType), new List<Type> { typeof(BType), typeof(CType) } },
            { typeof(BType), new List<Type> { typeof(DType), typeof(FType) } },
            { typeof(DType), new List<Type> { typeof(CType) } }
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        Assert.NotNull(dependencyGraph);
        Assert.NotNull(dependencyGraph._dependencies);
        Assert.NotNull(dependencyGraph._ancestorsNodes);
        Assert.NotNull(dependencyGraph._descendantsNodes);

        Assert.Equal(3, dependencyGraph._dependencies.Count);
        Assert.Equal(4, dependencyGraph._ancestorsNodes.Count);
        Assert.Equal(3, dependencyGraph._descendantsNodes.Count);
    }

    [Fact]
    public void FindAllAncestors()
    {
        var dependencies = new Dictionary<Type, List<Type>>()
        {
            { typeof(AType), new List<Type> { typeof(BType), typeof(CType) } },
            { typeof(BType), new List<Type> { typeof(DType), typeof(FType) } },
            { typeof(DType), new List<Type> { typeof(CType) } }
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        var aAncestors = dependencyGraph.FindAllAncestors(typeof(AType));
        var bAncestors = dependencyGraph.FindAllAncestors(typeof(BType));
        var cAncestors = dependencyGraph.FindAllAncestors(typeof(CType));
        var dAncestors = dependencyGraph.FindAllAncestors(typeof(DType));
        var fAncestors = dependencyGraph.FindAllAncestors(typeof(FType));

        Assert.Empty(aAncestors);

        Assert.Single(bAncestors);
        Assert.True(bAncestors.SequenceEqual(new List<Type> { typeof(AType) }));

        Assert.Equal(4, cAncestors.Count);
        Assert.True(cAncestors.SequenceEqual(new List<Type> { typeof(AType), typeof(DType), typeof(BType), typeof(AType) }));

        Assert.Equal(2, dAncestors.Count);
        Assert.True(dAncestors.SequenceEqual(new List<Type> { typeof(BType), typeof(AType) }));

        Assert.Equal(2, fAncestors.Count);
        Assert.True(fAncestors.SequenceEqual(new List<Type> { typeof(BType), typeof(AType) }));
    }

    [Fact]
    public void FindAllDescendants()
    {
        var dependencies = new Dictionary<Type, List<Type>>()
        {
            { typeof(AType), new List<Type> { typeof(BType), typeof(CType) } },
            { typeof(BType), new List<Type> { typeof(DType), typeof(FType) } },
            { typeof(DType), new List<Type> { typeof(CType) } }
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        var aAncestors = dependencyGraph.FindAllDescendants(typeof(AType));
        var bAncestors = dependencyGraph.FindAllDescendants(typeof(BType));
        var cAncestors = dependencyGraph.FindAllDescendants(typeof(CType));
        var dAncestors = dependencyGraph.FindAllDescendants(typeof(DType));
        var fAncestors = dependencyGraph.FindAllDescendants(typeof(FType));

        Assert.Equal(5, aAncestors.Count);
        Assert.True(aAncestors.SequenceEqual(new List<Type> { typeof(BType), typeof(CType), typeof(DType), typeof(FType), typeof(CType) }));

        Assert.Equal(3, bAncestors.Count);
        Assert.True(bAncestors.SequenceEqual(new List<Type> { typeof(DType), typeof(FType), typeof(CType) }));

        Assert.Empty(cAncestors);

        Assert.Single(dAncestors);
        Assert.True(dAncestors.SequenceEqual(new List<Type> { typeof(CType) }));

        Assert.Empty(fAncestors);
    }

    [Fact]
    public void FindAncestors()
    {
        var dependencies = new Dictionary<Type, List<Type>>()
        {
            { typeof(AType), new List<Type> { typeof(BType), typeof(CType) } },
            { typeof(BType), new List<Type> { typeof(DType), typeof(FType) } },
            { typeof(DType), new List<Type> { typeof(CType) } }
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        var aAncestors = dependencyGraph.FindAncestors(typeof(AType));
        var bAncestors = dependencyGraph.FindAncestors(typeof(BType));
        var cAncestors = dependencyGraph.FindAncestors(typeof(CType));
        var dAncestors = dependencyGraph.FindAncestors(typeof(DType));
        var fAncestors = dependencyGraph.FindAncestors(typeof(FType));

        Assert.Empty(aAncestors);

        Assert.Single(bAncestors);
        Assert.True(bAncestors.SequenceEqual(new List<Type> { typeof(AType) }));

        Assert.Equal(3, cAncestors.Count);
        Assert.True(cAncestors.SequenceEqual(new List<Type> { typeof(AType), typeof(DType), typeof(BType) }));

        Assert.Equal(2, dAncestors.Count);
        Assert.True(dAncestors.SequenceEqual(new List<Type> { typeof(BType), typeof(AType) }));

        Assert.Equal(2, fAncestors.Count);
        Assert.True(fAncestors.SequenceEqual(new List<Type> { typeof(BType), typeof(AType) }));
    }

    [Fact]
    public void FindDescendants()
    {
        var dependencies = new Dictionary<Type, List<Type>>()
        {
            { typeof(AType), new List<Type> { typeof(BType), typeof(CType) } },
            { typeof(BType), new List<Type> { typeof(DType), typeof(FType) } },
            { typeof(DType), new List<Type> { typeof(CType) } }
        };

        var dependencyGraph = new DependencyGraph(dependencies);

        var aAncestors = dependencyGraph.FindDescendants(typeof(AType));
        var bAncestors = dependencyGraph.FindDescendants(typeof(BType));
        var cAncestors = dependencyGraph.FindDescendants(typeof(CType));
        var dAncestors = dependencyGraph.FindDescendants(typeof(DType));
        var fAncestors = dependencyGraph.FindDescendants(typeof(FType));

        Assert.Equal(4, aAncestors.Count);
        Assert.True(aAncestors.SequenceEqual(new List<Type> { typeof(BType), typeof(CType), typeof(DType), typeof(FType) }));

        Assert.Equal(3, bAncestors.Count);
        Assert.True(bAncestors.SequenceEqual(new List<Type> { typeof(DType), typeof(FType), typeof(CType) }));

        Assert.Empty(cAncestors);

        Assert.Single(dAncestors);
        Assert.True(dAncestors.SequenceEqual(new List<Type> { typeof(CType) }));

        Assert.Empty(fAncestors);
    }
}