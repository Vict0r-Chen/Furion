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
    public void Configure_ReturnOK()
    {
        var builder = new ComponentBuilder();

        builder.Configure<DefaultOptions>(options => { });
        Assert.Single(builder._optionsActions);

        builder.Configure<DefaultOptions>(options => { });
        Assert.Single(builder._optionsActions);

        builder.Configure<WithParameterlessOptions>(options => { });
        Assert.Equal(2, builder._optionsActions.Count);
    }

    [Fact]
    public void Build_ReturnOK()
    {
        var builder = new ComponentBuilder();
        builder.Configure<DefaultOptions>(options => { });
        builder.Configure<WithParameterlessOptions>(options => { });

        var services = new ServiceCollection();
        builder.Build(services);

        Assert.Empty(builder._optionsActions);

        var componentOptions = services.GetComponentOptions();
        Assert.NotNull(componentOptions);

        Assert.Equal(3, componentOptions.OptionsActions.Count);
    }

    [Fact]
    public void WebConfigure_ReturnOK()
    {
        var builder = new WebComponentBuilder();

        builder.Configure<DefaultOptions>(options => { });
        Assert.Single(builder._optionsActions);

        builder.Configure<DefaultOptions>(options => { });
        Assert.Single(builder._optionsActions);

        builder.Configure<WithParameterlessOptions>(options => { });
        Assert.Equal(2, builder._optionsActions.Count);
    }

    [Fact]
    public void WebBuild_ReturnOK()
    {
        var builder = new WebComponentBuilder();
        builder.Configure<DefaultOptions>(options => { });
        builder.Configure<WithParameterlessOptions>(options => { });

        var webApplication = WebApplication.CreateBuilder().AddComponentService().Build();
        builder.Build(webApplication);

        Assert.Empty(builder._optionsActions);

        var componentOptions = webApplication.GetComponentOptions();
        Assert.NotNull(componentOptions);

        Assert.Equal(4, componentOptions.OptionsActions.Count);
    }

    [Fact]
    public void SuppressDuplicateCall_ReturnOK()
    {
        var builder = new ComponentBuilder
        {
            SuppressDuplicateCall = false
        };

        var services = new ServiceCollection();
        builder.Build(services);

        var componentOptions = services.GetComponentOptions();
        Assert.NotNull(componentOptions);

        Assert.False(componentOptions.SuppressDuplicateCall);
    }

    [Fact]
    public void Web_SuppressDuplicateCall_ReturnOK()
    {
        var builder = new WebComponentBuilder
        {
            SuppressDuplicateCall = false
        };

        var webApplication = WebApplication.CreateBuilder().AddComponentService().Build();
        builder.Build(webApplication);

        var componentOptions = webApplication.GetComponentOptions();
        Assert.NotNull(componentOptions);

        Assert.False(componentOptions.SuppressDuplicateCallForWeb);
    }

    [Theory]
    [InlineData(true, 2)]
    [InlineData(false, 3)]
    public void AddComponent_SuppressDuplicate_ReturnOK(bool supress, int count)
    {
        var services = new ServiceCollection();
        services.AddComponentService(builder =>
        {
            builder.SuppressDuplicateCall = supress;
            builder.Configure<SuppressDuplicateOptions>(o =>
            {
                o.Num = 1;
            });
        });
        services.AddComponent<SuppressDuplicateComponent>(new ConfigurationManager());
        services.AddComponent<SuppressDuplicateComponent>(new ConfigurationManager());

        var options = services.GetComponentOptions().OptionsActions.GetOptions<SuppressDuplicateOptions>();
        Assert.NotNull(options);
        Assert.Equal(count, options.Num);
    }

    [Theory]
    [InlineData(true, 2)]
    [InlineData(false, 3)]
    public void AddComponent_SuppressDuplicate_ForWeb_ReturnOK(bool supress, int count)
    {
        var builder = WebApplication.CreateBuilder();
        builder.AddComponentService(builder =>
        {
            builder.SuppressDuplicateCall = supress;
            builder.Configure<SuppressDuplicateOptions>(o =>
            {
                o.Num = 1;
            });
        });
        builder.AddComponent<SuppressDuplicateComponent>();
        builder.AddComponent<SuppressDuplicateComponent>();

        var options = builder.Services.GetComponentOptions().OptionsActions.GetOptions<SuppressDuplicateOptions>();
        Assert.NotNull(options);
        Assert.Equal(count, options.Num);

        var app = builder.Build();
        app.UseComponentMiddleware(b =>
        {
            b.SuppressDuplicateCall = supress;
            b.Configure<SuppressDuplicateForWebOptions>(o =>
            {
                o.Num = 1;
            });
        });

        app.UseComponent<SuppressDuplicateForWebComponent>();
        app.UseComponent<SuppressDuplicateForWebComponent>();

        var options2 = app.GetComponentOptions().OptionsActions.GetOptions<SuppressDuplicateForWebOptions>();
        Assert.NotNull(options2);
        Assert.Equal(count, options2.Num);
    }
}