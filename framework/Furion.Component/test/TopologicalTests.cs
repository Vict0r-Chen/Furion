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
    public void Sort_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            { typeof(AComponent), new[]{ typeof(BComponent),typeof(CComponent)} },
            { typeof(BComponent), new[]{ typeof(CComponent),typeof(DComponent)} },
            { typeof(FComponent), new[]{ typeof(CComponent),typeof(EComponent)} },
            { typeof(GComponent), Array.Empty<Type>() }
        };

        var sortedTypes = Topological.Sort(dependencies);
        // C D B A E F G
        Assert.Equal(typeof(CComponent), sortedTypes[0]);
        Assert.Equal(typeof(DComponent), sortedTypes[1]);
        Assert.Equal(typeof(BComponent), sortedTypes[2]);
        Assert.Equal(typeof(AComponent), sortedTypes[3]);
        Assert.Equal(typeof(EComponent), sortedTypes[4]);
        Assert.Equal(typeof(FComponent), sortedTypes[5]);
        Assert.Equal(typeof(GComponent), sortedTypes[6]);
    }

    [Fact]
    public void Sort_Circular_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            { typeof(AComponent), new[]{ typeof(BComponent),typeof(CComponent)} },
            { typeof(BComponent), new[]{ typeof(AComponent),typeof(DComponent)} },
            { typeof(DComponent), new[]{ typeof(AComponent) } }
        };

        var sortedTypes = Topological.Sort(dependencies);
        Assert.Equal(4, sortedTypes.Count);
    }

    [Fact]
    public void HasNotCycle_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            { typeof(AComponent), new[]{ typeof(BComponent),typeof(CComponent)} },
            { typeof(BComponent), new[]{ typeof(CComponent),typeof(DComponent)} },
            { typeof(FComponent), new[]{ typeof(CComponent),typeof(EComponent)} },
            { typeof(GComponent), Array.Empty<Type>() }
        };

        var result = Topological.HasCycle(dependencies);
        Assert.False(result);
    }

    [Fact]
    public void HasCycle_ReturnOK()
    {
        var dependencies = new Dictionary<Type, Type[]>
        {
            { typeof(AComponent), new[]{ typeof(BComponent),typeof(CComponent)} },
            { typeof(BComponent), new[]{ typeof(AComponent),typeof(DComponent)} },
            { typeof(DComponent), new[]{ typeof(AComponent) } }
        };

        var result = Topological.HasCycle(dependencies);
        Assert.True(result);
    }
}