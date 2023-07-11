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

public class ServiceComponentContextTests
{
    [Fact]
    public void NewInstance_Default()
    {
        var services = new ServiceCollection();
        var serviceComponentContext = new ServiceComponentContext(services, new ConfigurationManager());

        Assert.NotNull(serviceComponentContext);
        Assert.NotNull(serviceComponentContext.Services);
        Assert.NotNull(serviceComponentContext.Configuration);
        Assert.NotNull(serviceComponentContext.Options);
        Assert.NotNull(serviceComponentContext.Items);
        Assert.Empty(serviceComponentContext.Items);
        Assert.Null(serviceComponentContext.Environment);
    }

    [Fact]
    public void GetPropsAction_NotExists_ReturnNull()
    {
        var services = new ServiceCollection();
        var serviceComponentContext = new ServiceComponentContext(services, new ConfigurationManager());

        var action = serviceComponentContext.GetPropsAction<ComponentActionOptions>();
        Assert.Null(action);
    }

    [Fact]
    public void GetPropsAction_Exists_ReturnAction()
    {
        var services = new ServiceCollection();
        var serviceComponentContext = new ServiceComponentContext(services, new ConfigurationManager());

        static void Action(ComponentActionOptions options)
        {
        }
        serviceComponentContext.Options.PropsActions.Add(typeof(ComponentActionOptions), new List<Delegate> { Action });

        var action = serviceComponentContext.GetPropsAction<ComponentActionOptions>();
        Assert.NotNull(action);
    }

    [Fact]
    public void GetProps_NotExists_ReturnNull()
    {
        var services = new ServiceCollection();
        var serviceComponentContext = new ServiceComponentContext(services, new ConfigurationManager());

        var action = serviceComponentContext.GetProps<ComponentActionOptions>();
        Assert.Null(action);
    }

    [Fact]
    public void GetProps_Exists_ReturnAction()
    {
        var services = new ServiceCollection();
        var serviceComponentContext = new ServiceComponentContext(services, new ConfigurationManager());

        static void Action(ComponentActionOptions options)
        {
        }
        serviceComponentContext.Options.PropsActions.Add(typeof(ComponentActionOptions), new List<Delegate> { Action });

        var action = serviceComponentContext.GetProps<ComponentActionOptions>();
        Assert.NotNull(action);
    }

    [Fact]
    public void GetPropsOrNew_NotExists_ReturnNull()
    {
        var services = new ServiceCollection();
        var serviceComponentContext = new ServiceComponentContext(services, new ConfigurationManager());

        var action = serviceComponentContext.GetPropsOrNew<ComponentActionOptions>();
        Assert.NotNull(action);
    }

    [Fact]
    public void GetPropsOrNew_Exists_ReturnAction()
    {
        var services = new ServiceCollection();
        var serviceComponentContext = new ServiceComponentContext(services, new ConfigurationManager());

        static void Action(ComponentActionOptions options)
        {
        }
        serviceComponentContext.Options.PropsActions.Add(typeof(ComponentActionOptions), new List<Delegate> { Action });

        var action = serviceComponentContext.GetPropsOrNew<ComponentActionOptions>();
        Assert.NotNull(action);
    }

    [Fact]
    public void Props_Null_Throw()
    {
        var services = new ServiceCollection();
        var serviceComponentContext = new ServiceComponentContext(services, new ConfigurationManager());
        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceComponentContext.Props<ComponentActionOptions>(null!);
        });
    }

    [Fact]
    public void Props_ReturnOK()
    {
        var services = new ServiceCollection();
        var serviceComponentContext = new ServiceComponentContext(services, new ConfigurationManager());

        Assert.NotNull(serviceComponentContext.Options);
        Assert.Empty(serviceComponentContext.Options.PropsActions);

        serviceComponentContext.Props<ComponentActionOptions>(options =>
        {
        });

        Assert.Single(serviceComponentContext.Options.PropsActions);
    }
}