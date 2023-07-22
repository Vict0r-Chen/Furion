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

public class ComponentBuilderTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var componentBuilder = new ComponentBuilder();

        Assert.NotNull(componentBuilder);
        Assert.NotNull(componentBuilder._propsActions);
        Assert.Empty(componentBuilder._propsActions);
    }

    [Fact]
    public void Props_Invalid_Parameters()
    {
        var componentBuilder = new ComponentBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            componentBuilder.Props((Action<ComponentOptionsClass1>)null!);
        });
    }

    [Fact]
    public void Props_ReturnOK()
    {
        var componentBuilder = new ComponentBuilder();

        componentBuilder.Props<ComponentOptionsClass1>(props => { });
        Assert.Single(componentBuilder._propsActions);

        componentBuilder.Props<ComponentOptionsClass1>(props => { });
        Assert.Single(componentBuilder._propsActions);
        Assert.Equal(2, componentBuilder._propsActions[typeof(ComponentOptionsClass1)].Count);
    }

    [Fact]
    public void PropsConfiguration_Invalid_Parameters()
    {
        var componentBuilder = new ComponentBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            componentBuilder.Props<ComponentOptionsClass1>((IConfiguration)null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            componentBuilder.Props<ComponentOptionsClass1>(new ConfigurationManager());
        });
    }

    [Fact]
    public void PropsConfiguration_ReturnOK()
    {
        var componentBuilder = new ComponentBuilder();
        var configurationManager = new ConfigurationManager();
        configurationManager.AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"Data:Items:0", "action1"},
            {"Data:Items:1", "action2"}
        });

        componentBuilder.Props<ComponentOptionsClass1>(configurationManager.GetSection("Data"));
        Assert.Single(componentBuilder._propsActions);

        var propsAction = componentBuilder._propsActions[typeof(ComponentOptionsClass1)].First() as Action<ComponentOptionsClass1>;
        Assert.NotNull(propsAction);

        var componentOptionsClass1 = new ComponentOptionsClass1();
        propsAction(componentOptionsClass1);

        Assert.NotNull(componentOptionsClass1.Items);
        Assert.Equal(2, componentOptionsClass1.Items.Count);
    }

    [Fact]
    public void Build_Invalid_Parameters()
    {
        var componentBuilder = new ComponentBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            componentBuilder.Build(null!);
        });
    }

    [Fact]
    public void Build_ReturnOK()
    {
        var componentBuilder = new ComponentBuilder();
        componentBuilder.Props<ComponentOptionsClass1>(props => { });

        var hostApplicationBuilder = Host.CreateApplicationBuilder();

        componentBuilder.Build(hostApplicationBuilder);

        Assert.Contains(hostApplicationBuilder.Services, s => s.ServiceType == typeof(CoreOptions) && s.ImplementationInstance is not null);
        Assert.Contains(hostApplicationBuilder.Services, s => s.ImplementationType == typeof(ComponentReleaser));

        var componentOptions = hostApplicationBuilder.GetComponentOptions();
        Assert.Single(componentOptions.PropsActions);
        Assert.NotEmpty(componentOptions.PropsActions[typeof(ComponentOptionsClass1)]);
    }
}