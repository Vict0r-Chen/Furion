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

public class ComponentOptionsTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var componentOptions = new ComponentOptions();

        Assert.NotNull(componentOptions);
        Assert.NotNull(componentOptions.PropsActions);
        Assert.Empty(componentOptions.PropsActions);
        Assert.NotNull(componentOptions.Components);
        Assert.Empty(componentOptions.Components);
        Assert.NotNull(componentOptions._GetPropsActionMethod);
    }

    [Fact]
    public void Props_Invalid_Parameters()
    {
        var componentOptions = new ComponentOptions();

        Assert.Throws<ArgumentNullException>(() =>
        {
            componentOptions.Props((Action<ComponentOptionsClass1>)null!);
        });
    }

    [Fact]
    public void Props_ReturnOK()
    {
        var componentOptions = new ComponentOptions();

        componentOptions.Props<ComponentOptionsClass1>(props => { });
        Assert.Single(componentOptions.PropsActions);

        componentOptions.Props<ComponentOptionsClass1>(props => { });
        Assert.Single(componentOptions.PropsActions);
        Assert.Equal(2, componentOptions.PropsActions[typeof(ComponentOptionsClass1)].Count);
    }

    [Fact]
    public void PropsConfiguration_Invalid_Parameters()
    {
        var componentOptions = new ComponentOptions();

        Assert.Throws<ArgumentNullException>(() =>
        {
            componentOptions.Props<ComponentOptionsClass1>((IConfiguration)null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            componentOptions.Props<ComponentOptionsClass1>(new ConfigurationManager());
        });
    }

    [Fact]
    public void PropsConfiguration_ReturnOK()
    {
        var componentOptions = new ComponentOptions();
        var configurationManager = new ConfigurationManager();
        configurationManager.AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"Data:Items:0", "action1"},
            {"Data:Items:1", "action2"}
        });

        componentOptions.Props<ComponentOptionsClass1>(configurationManager.GetSection("Data"));
        Assert.Single(componentOptions.PropsActions);

        var propsAction = componentOptions.PropsActions[typeof(ComponentOptionsClass1)].First() as Action<ComponentOptionsClass1>;
        Assert.NotNull(propsAction);

        var componentOptionsClass1 = new ComponentOptionsClass1();
        propsAction(componentOptionsClass1);

        Assert.NotNull(componentOptionsClass1.Items);
        Assert.Equal(2, componentOptionsClass1.Items.Count);
    }

    [Fact]
    public void GetPropsActionGeneric_ReturnOK()
    {
        var componentOptions = new ComponentOptions();

        var optionsAction = componentOptions.GetPropsAction<ComponentOptionsClass1>();
        Assert.Null(optionsAction);

        var action1 = new Action<ComponentOptionsClass1>(p =>
        {
            p.Items.Add("action1");
        });

        var action2 = new Action<ComponentOptionsClass1>(p =>
        {
            p.Items.Add("action2");
        });

        var action3 = new Action<ComponentOptionsClass2>(p =>
        {
            p.Items.Add("action3");
        });

        componentOptions.PropsActions.AddOrUpdate(typeof(ComponentOptionsClass1), action1);
        componentOptions.PropsActions.AddOrUpdate(typeof(ComponentOptionsClass1), action2);
        componentOptions.PropsActions.AddOrUpdate(typeof(ComponentOptionsClass2), action3);

        Assert.Equal(2, componentOptions.PropsActions.Count);
        Assert.Equal(2, componentOptions.PropsActions[typeof(ComponentOptionsClass1)].Count);

        var optionsAction1 = componentOptions.GetPropsAction<ComponentOptionsClass1>();
        Assert.NotNull(optionsAction1);

        var componentOptionsClass1 = new ComponentOptionsClass1();
        optionsAction1(componentOptionsClass1);
        Assert.Equal(2, componentOptionsClass1.Items.Count);
        Assert.Equal(new List<string> { "action1", "action2" }, componentOptionsClass1.Items);

        var optionsAction2 = componentOptions.GetPropsAction<ComponentOptionsClass2>();
        Assert.NotNull(optionsAction2);

        var componentOptionsClass2 = new ComponentOptionsClass2();
        optionsAction2(componentOptionsClass2);
        Assert.Single(componentOptionsClass2.Items);
        Assert.Equal(new List<string> { "action3" }, componentOptionsClass2.Items);
    }

    [Fact]
    public void GetPropsAction_Invalid_Parameters()
    {
        var componentOptions = new ComponentOptions();
        Assert.Throws<ArgumentNullException>(() =>
        {
            componentOptions.GetPropsAction(null!);
        });
    }

    [Fact]
    public void GetPropsAction_ReturnOK()
    {
        var componentOptions = new ComponentOptions();

        var optionsAction = componentOptions.GetPropsAction(typeof(ComponentOptionsClass1));
        Assert.Null(optionsAction);

        var action1 = new Action<ComponentOptionsClass1>(p =>
        {
            p.Items.Add("action1");
        });

        var action2 = new Action<ComponentOptionsClass1>(p =>
        {
            p.Items.Add("action2");
        });

        var action3 = new Action<ComponentOptionsClass2>(p =>
        {
            p.Items.Add("action3");
        });

        componentOptions.PropsActions.AddOrUpdate(typeof(ComponentOptionsClass1), action1);
        componentOptions.PropsActions.AddOrUpdate(typeof(ComponentOptionsClass1), action2);
        componentOptions.PropsActions.AddOrUpdate(typeof(ComponentOptionsClass2), action3);

        Assert.Equal(2, componentOptions.PropsActions.Count);
        Assert.Equal(2, componentOptions.PropsActions[typeof(ComponentOptionsClass1)].Count);

        var optionsAction1 = componentOptions.GetPropsAction(typeof(ComponentOptionsClass1)) as Action<ComponentOptionsClass1>;
        Assert.NotNull(optionsAction1);

        var componentOptionsClass1 = new ComponentOptionsClass1();
        optionsAction1(componentOptionsClass1);
        Assert.Equal(2, componentOptionsClass1.Items.Count);
        Assert.Equal(new List<string> { "action1", "action2" }, componentOptionsClass1.Items);

        var optionsAction2 = componentOptions.GetPropsAction(typeof(ComponentOptionsClass2)) as Action<ComponentOptionsClass2>;
        Assert.NotNull(optionsAction2);

        var componentOptionsClass2 = new ComponentOptionsClass2();
        optionsAction2(componentOptionsClass2);
        Assert.Single(componentOptionsClass2.Items);
        Assert.Equal(new List<string> { "action3" }, componentOptionsClass2.Items);
    }
}