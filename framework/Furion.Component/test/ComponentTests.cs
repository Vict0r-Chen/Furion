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

public class ComponentTests
{
    [Fact]
    public void AddComponent_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddComponent<AComponent>(new ConfigurationManager());

        var componentOptions = services.GetComponentOptions();
        Assert.NotNull(componentOptions);
        Assert.Single(componentOptions.OptionsActions);
        Assert.Equal(8, componentOptions.OptionsActions.First().Value.Count);

        // C D B A
        var addComponentOptions = componentOptions.OptionsActions.GetOptions<AddComponentOptions>();
        Assert.NotNull(addComponentOptions);
        Assert.Equal(4, addComponentOptions.PreConfigureServices.Count);
        Assert.Equal(nameof(CComponent), addComponentOptions.PreConfigureServices.ElementAt(0));
        Assert.Equal(nameof(DComponent), addComponentOptions.PreConfigureServices.ElementAt(1));
        Assert.Equal(nameof(BComponent), addComponentOptions.PreConfigureServices.ElementAt(2));
        Assert.Equal(nameof(AComponent), addComponentOptions.PreConfigureServices.ElementAt(3));

        Assert.Equal(4, addComponentOptions.ConfigureServices.Count);
        Assert.Equal(nameof(CComponent), addComponentOptions.ConfigureServices.ElementAt(0));
        Assert.Equal(nameof(DComponent), addComponentOptions.ConfigureServices.ElementAt(1));
        Assert.Equal(nameof(BComponent), addComponentOptions.ConfigureServices.ElementAt(2));
        Assert.Equal(nameof(AComponent), addComponentOptions.ConfigureServices.ElementAt(3));
    }

    [Fact]
    public void AddWebComponent_ReturnOK()
    {
        var builder = WebApplication.CreateBuilder().AddComponentService();
        var app = builder.Build();
        app.AddComponent<AWebComponent>();

        var componentOptions = app.GetComponentOptions();
        Assert.NotNull(componentOptions);
        Assert.Single(componentOptions.OptionsActions);
        Assert.Equal(8, componentOptions.OptionsActions.First().Value.Count);

        // C D B A
        var addComponentOptions = componentOptions.OptionsActions.GetOptions<AddComponentOptions>();
        Assert.NotNull(addComponentOptions);
        Assert.Equal(4, addComponentOptions.PreConfigure.Count);
        Assert.Equal(nameof(CWebComponent), addComponentOptions.PreConfigure.ElementAt(0));
        Assert.Equal(nameof(DWebComponent), addComponentOptions.PreConfigure.ElementAt(1));
        Assert.Equal(nameof(BWebComponent), addComponentOptions.PreConfigure.ElementAt(2));
        Assert.Equal(nameof(AWebComponent), addComponentOptions.PreConfigure.ElementAt(3));

        Assert.Equal(4, addComponentOptions.Configure.Count);
        Assert.Equal(nameof(CWebComponent), addComponentOptions.Configure.ElementAt(0));
        Assert.Equal(nameof(DWebComponent), addComponentOptions.Configure.ElementAt(1));
        Assert.Equal(nameof(BWebComponent), addComponentOptions.Configure.ElementAt(2));
        Assert.Equal(nameof(AWebComponent), addComponentOptions.Configure.ElementAt(3));
    }

    [Fact]
    public void WebApplicationBuilder_Entry_ReturnOK()
    {
        var app = WebApplication.CreateBuilder().Entry<AWebComponent>();

        var componentOptions = app.GetComponentOptions();
        Assert.NotNull(componentOptions);
        Assert.Single(componentOptions.OptionsActions);
        Assert.Equal(16, componentOptions.OptionsActions.First().Value.Count);

        // C D B A
        var addComponentOptions = componentOptions.OptionsActions.GetOptions<AddComponentOptions>();
        Assert.NotNull(addComponentOptions);

        Assert.Equal(4, addComponentOptions.PreConfigureServices.Count);
        Assert.Equal(nameof(CWebComponent), addComponentOptions.PreConfigureServices.ElementAt(0));
        Assert.Equal(nameof(DWebComponent), addComponentOptions.PreConfigureServices.ElementAt(1));
        Assert.Equal(nameof(BWebComponent), addComponentOptions.PreConfigureServices.ElementAt(2));
        Assert.Equal(nameof(AWebComponent), addComponentOptions.PreConfigureServices.ElementAt(3));

        Assert.Equal(4, addComponentOptions.ConfigureServices.Count);
        Assert.Equal(nameof(CWebComponent), addComponentOptions.ConfigureServices.ElementAt(0));
        Assert.Equal(nameof(DWebComponent), addComponentOptions.ConfigureServices.ElementAt(1));
        Assert.Equal(nameof(BWebComponent), addComponentOptions.ConfigureServices.ElementAt(2));
        Assert.Equal(nameof(AWebComponent), addComponentOptions.ConfigureServices.ElementAt(3));

        Assert.Equal(4, addComponentOptions.PreConfigure.Count);
        Assert.Equal(nameof(CWebComponent), addComponentOptions.PreConfigure.ElementAt(0));
        Assert.Equal(nameof(DWebComponent), addComponentOptions.PreConfigure.ElementAt(1));
        Assert.Equal(nameof(BWebComponent), addComponentOptions.PreConfigure.ElementAt(2));
        Assert.Equal(nameof(AWebComponent), addComponentOptions.PreConfigure.ElementAt(3));

        Assert.Equal(4, addComponentOptions.Configure.Count);
        Assert.Equal(nameof(CWebComponent), addComponentOptions.Configure.ElementAt(0));
        Assert.Equal(nameof(DWebComponent), addComponentOptions.Configure.ElementAt(1));
        Assert.Equal(nameof(BWebComponent), addComponentOptions.Configure.ElementAt(2));
        Assert.Equal(nameof(AWebComponent), addComponentOptions.Configure.ElementAt(3));
    }

    [Fact]
    public void HostApplicationBuilder_Entry_ReturnOK()
    {
        var host = Host.CreateApplicationBuilder().Entry<AComponent>();

        var componentOptions = host.GetComponentOptions();
        Assert.NotNull(componentOptions);
        Assert.Single(componentOptions.OptionsActions);
        Assert.Equal(8, componentOptions.OptionsActions.First().Value.Count);

        // C D B A
        var addComponentOptions = componentOptions.OptionsActions.GetOptions<AddComponentOptions>();
        Assert.NotNull(addComponentOptions);

        Assert.Equal(4, addComponentOptions.PreConfigureServices.Count);
        Assert.Equal(nameof(CComponent), addComponentOptions.PreConfigureServices.ElementAt(0));
        Assert.Equal(nameof(DComponent), addComponentOptions.PreConfigureServices.ElementAt(1));
        Assert.Equal(nameof(BComponent), addComponentOptions.PreConfigureServices.ElementAt(2));
        Assert.Equal(nameof(AComponent), addComponentOptions.PreConfigureServices.ElementAt(3));

        Assert.Equal(4, addComponentOptions.ConfigureServices.Count);
        Assert.Equal(nameof(CComponent), addComponentOptions.ConfigureServices.ElementAt(0));
        Assert.Equal(nameof(DComponent), addComponentOptions.ConfigureServices.ElementAt(1));
        Assert.Equal(nameof(BComponent), addComponentOptions.ConfigureServices.ElementAt(2));
        Assert.Equal(nameof(AComponent), addComponentOptions.ConfigureServices.ElementAt(3));
    }
}