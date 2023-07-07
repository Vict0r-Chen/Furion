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

public class ComponentBaseTests
{
    [Fact]
    public void NewInstance_Default()
    {
        var component = new CBaseComponent();

        Assert.NotNull(component);
        Assert.Null(component.Options);

        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        var componentContext = new ServiceComponentContext(services, configuration);

        component.PreConfigureServices(componentContext);
        component.ConfigureServices(componentContext);

        var webApplication = WebApplication.CreateBuilder().AddComponentCore().Build();
        var applicationContext = new ApplicationComponentContext(webApplication);

        component.PreConfigure(applicationContext);
        component.Configure(applicationContext);
    }

    [Theory]
    [InlineData(typeof(InvalidArgumentComponnet), "`InvalidOptions` parameter type is an invalid component options.")]
    [InlineData(typeof(InvalidArgument2Componnet), "`Action`1` parameter type is an invalid component options.")]
    public void CreateComponent_InvalidArgument_Throw(Type componentType, string message)
    {
        var webApplication = WebApplication.CreateBuilder().AddComponentCore().Build();
        var componentOptions = webApplication.GetComponentOptions();

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentBase.CreateComponent(componentType, componentOptions);
        });

        Assert.Equal(message, exception.Message);
    }

    [Theory]
    [InlineData(typeof(OkArgumentComponent))]
    [InlineData(typeof(OkArgument2Component))]
    [InlineData(typeof(OkArgument3Component))]
    public void CreateComponent_ReturnOK(Type componentType)
    {
        var webApplication = WebApplication.CreateBuilder().AddComponentCore().Build();
        var componentOptions = webApplication.GetComponentOptions();

        var component = ComponentBase.CreateComponent(componentType, componentOptions);
        Assert.NotNull(component);
        Assert.NotNull(component.Options);
    }

    [Theory]
    [InlineData(typeof(AComponent), true)]
    [InlineData(typeof(NotWebComponent), false)]
    [InlineData(typeof(ComponentBase), false)]
    [InlineData(typeof(WebComponent), false)]
    [InlineData(typeof(InvalidArgumentComponnet), true)]
    [InlineData(typeof(OkArgumentComponent), true)]
    public void IsWebComponent(Type componentType, bool result)
    {
        Assert.Equal(result, ComponentBase.IsWebComponent(componentType));
    }

    [Fact]
    public void CreateComponents()
    {
        var webApplication = WebApplication.CreateBuilder().AddComponentCore().Build();
        var componentContext = new ApplicationComponentContext(webApplication);
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));

        // D C B A
        var list = ComponentBase.CreateComponents<WebComponent>(dependencies, componentContext, topologicalGraphPredicate: ComponentBase.IsWebComponent);
        Assert.NotNull(list);

        Assert.Equal(4, list.Count);

        Assert.Equal(typeof(DComponent), list[0].GetType());
        Assert.Equal(typeof(CComponent), list[1].GetType());
        Assert.Equal(typeof(BComponent), list[2].GetType());
        Assert.Equal(typeof(AComponent), list[3].GetType());

        // 避免引发重复调用检查
        componentContext.Options.InvokeRecords.Clear();
        var list2 = ComponentBase.CreateComponents<WebComponent>(dependencies, componentContext, topologicalGraphPredicate: ComponentBase.IsWebComponent);
        Assert.NotNull(list2);

        Assert.Equal(4, list2.Count);

        Assert.Equal(typeof(DComponent), list2[0].GetType());
        Assert.Equal(typeof(CComponent), list2[1].GetType());
        Assert.Equal(typeof(BComponent), list2[2].GetType());
        Assert.Equal(typeof(AComponent), list2[3].GetType());
    }
}