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

namespace Furion.Component.AspNetCore.Tests;

public class WebWebComponentBuilderTests
{
    [Fact]
    public void NewInstance_Default()
    {
        var webComponentBuilder = new WebComponentBuilder();

        Assert.NotNull(webComponentBuilder);
        Assert.NotNull(webComponentBuilder._propsActions);
        Assert.Empty(webComponentBuilder._propsActions);
        Assert.True(webComponentBuilder.SuppressDuplicateCall);

        var webComponentBuilderBase = new WebComponentBuilderBase();
        Assert.NotNull(webComponentBuilderBase);
        Assert.NotNull(webComponentBuilderBase._propsActions);
        Assert.Empty(webComponentBuilderBase._propsActions);
    }

    [Fact]
    public void Props_Parameter_Null_Throw()
    {
        var webComponentBuilder = new WebComponentBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            webComponentBuilder.Props<ComponentActionOptions>(null!);
        });
    }

    [Fact]
    public void Props_NotExists_Add()
    {
        var webComponentBuilder = new WebComponentBuilder();
        webComponentBuilder.Props<ComponentActionOptions>(options =>
        {
        });
        Assert.Single(webComponentBuilder._propsActions);
        Assert.Single(webComponentBuilder._propsActions.First().Value);
    }

    [Fact]
    public void Props_Exists_Update()
    {
        var webComponentBuilder = new WebComponentBuilder();
        webComponentBuilder.Props<ComponentActionOptions>(options =>
        {
        });

        Assert.Single(webComponentBuilder._propsActions);
        Assert.Single(webComponentBuilder._propsActions.First().Value);

        webComponentBuilder.Props<ComponentActionOptions>(options =>
        {
        });

        Assert.Single(webComponentBuilder._propsActions);
        Assert.Equal(2, webComponentBuilder._propsActions.First().Value.Count);
    }

    [Fact]
    public void Build()
    {
        var webComponentBuilder = new WebComponentBuilder
        {
            SuppressDuplicateCall = false
        };
        var services = new ServiceCollection();
        var webApplicationBuilder = WebApplication.CreateBuilder().AddComponentCore();
        var webApplication = webApplicationBuilder.Build();

        webComponentBuilder.Build(webApplication);
        var componentOptions = webApplication.GetComponentOptions();

        Assert.Empty(webComponentBuilder._propsActions);
        Assert.Equal(webComponentBuilder.SuppressDuplicateCall, componentOptions.SuppressDuplicateCallForWeb);
        Assert.Equal(2, componentOptions.PropsActions.Count);
        Assert.Equal(typeof(ComponentBuilder), componentOptions.PropsActions.Keys.First());
        Assert.Equal(typeof(WebComponentBuilder), componentOptions.PropsActions.Keys.Last());

        var action = componentOptions.GetPropsAction<WebComponentBuilder>();
        Assert.NotNull(action);

        var options = new WebComponentBuilder();
        action(options);
        Assert.Equal(webComponentBuilder.SuppressDuplicateCall, options.SuppressDuplicateCall);
    }
}