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

public class ComponentBuilderTests
{
    [Fact]
    public void NewInstance_Default()
    {
        var componentBuilder = new ComponentBuilder();

        Assert.NotNull(componentBuilder);
        Assert.NotNull(componentBuilder._optionsActions);
        Assert.Empty(componentBuilder._optionsActions);
        Assert.True(componentBuilder.SuppressDuplicateCall);

        var componentBuilderBase = new ComponentBuilderBase();
        Assert.NotNull(componentBuilderBase);
        Assert.NotNull(componentBuilderBase._optionsActions);
        Assert.Empty(componentBuilderBase._optionsActions);
    }

    [Fact]
    public void Configure_Parameter_Null_Throw()
    {
        var componentBuilder = new ComponentBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            componentBuilder.Configure<ComponentActionOptions>(null!);
        });
    }

    [Fact]
    public void Configure_NotExists_Add()
    {
        var componentBuilder = new ComponentBuilder();
        componentBuilder.Configure<ComponentActionOptions>(options =>
        {
        });
        Assert.Single(componentBuilder._optionsActions);
        Assert.Single(componentBuilder._optionsActions.First().Value);
    }

    [Fact]
    public void Configure_Exists_Update()
    {
        var componentBuilder = new ComponentBuilder();
        componentBuilder.Configure<ComponentActionOptions>(options =>
        {
        });

        Assert.Single(componentBuilder._optionsActions);
        Assert.Single(componentBuilder._optionsActions.First().Value);

        componentBuilder.Configure<ComponentActionOptions>(options =>
        {
        });

        Assert.Single(componentBuilder._optionsActions);
        Assert.Equal(2, componentBuilder._optionsActions.First().Value.Count);
    }

    [Fact]
    public void Build()
    {
        var componentBuilder = new ComponentBuilder();
        componentBuilder.SuppressDuplicateCall = false;
        var services = new ServiceCollection();

        componentBuilder.Build(services);
        var componentOptions = services.GetComponentOptions();

        Assert.Empty(componentBuilder._optionsActions);
        Assert.Equal(componentBuilder.SuppressDuplicateCall, componentOptions.SuppressDuplicateCall);
        Assert.Single(componentOptions.OptionsActions);
        Assert.Equal(typeof(ComponentBuilder), componentOptions.OptionsActions.Keys.First());

        var action = componentOptions.GetOptionsAction<ComponentBuilder>();
        Assert.NotNull(action);

        var options = new ComponentBuilder();
        action(options);
        Assert.Equal(componentBuilder.SuppressDuplicateCall, options.SuppressDuplicateCall);
    }
}