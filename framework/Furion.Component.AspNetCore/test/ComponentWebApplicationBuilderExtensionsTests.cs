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

public class ComponentWebApplicationBuilderExtensionsTests
{
    [Fact]
    public void Entry_WebComponent_ReturnOK()
    {
        var webApplication = WebApplication.CreateBuilder().Entry<AComponent>();
        var action = webApplication.GetComponentOptions().GetPropsAction<CallOptions>();
        Assert.NotNull(action);

        var callOptions = new CallOptions();
        action(callOptions);

        Assert.Equal(8, callOptions.CallRecords.Count);

        Assert.Equal($"{nameof(AComponent)}.{nameof(AComponent.PreConfigure)}", callOptions.CallRecords[0]);
        Assert.Equal($"{nameof(BComponent)}.{nameof(BComponent.PreConfigure)}", callOptions.CallRecords[1]);
        Assert.Equal($"{nameof(CComponent)}.{nameof(CComponent.PreConfigure)}", callOptions.CallRecords[2]);
        Assert.Equal($"{nameof(DComponent)}.{nameof(CComponent.PreConfigure)}", callOptions.CallRecords[3]);

        Assert.Equal($"{nameof(DComponent)}.{nameof(AComponent.Configure)}", callOptions.CallRecords[4]);
        Assert.Equal($"{nameof(CComponent)}.{nameof(BComponent.Configure)}", callOptions.CallRecords[5]);
        Assert.Equal($"{nameof(BComponent)}.{nameof(CComponent.Configure)}", callOptions.CallRecords[6]);
        Assert.Equal($"{nameof(AComponent)}.{nameof(CComponent.Configure)}", callOptions.CallRecords[7]);
    }

    [Fact]
    public void Entry_Components_ReturnOK()
    {
        var webApplication = WebApplication.CreateBuilder().Entry<AComponent, AComponent>();
        var action = webApplication.GetComponentOptions().GetPropsAction<CallOptions>();
        Assert.NotNull(action);

        var callOptions = new CallOptions();
        action(callOptions);

        Assert.Equal(8, callOptions.CallRecords.Count);

        Assert.Equal($"{nameof(AComponent)}.{nameof(AComponent.PreConfigure)}", callOptions.CallRecords[0]);
        Assert.Equal($"{nameof(BComponent)}.{nameof(BComponent.PreConfigure)}", callOptions.CallRecords[1]);
        Assert.Equal($"{nameof(CComponent)}.{nameof(CComponent.PreConfigure)}", callOptions.CallRecords[2]);
        Assert.Equal($"{nameof(DComponent)}.{nameof(CComponent.PreConfigure)}", callOptions.CallRecords[3]);

        Assert.Equal($"{nameof(DComponent)}.{nameof(AComponent.Configure)}", callOptions.CallRecords[4]);
        Assert.Equal($"{nameof(CComponent)}.{nameof(BComponent.Configure)}", callOptions.CallRecords[5]);
        Assert.Equal($"{nameof(BComponent)}.{nameof(CComponent.Configure)}", callOptions.CallRecords[6]);
        Assert.Equal($"{nameof(AComponent)}.{nameof(CComponent.Configure)}", callOptions.CallRecords[7]);
    }

    [Fact]
    public void AddComponentCore_Null_Throw()
    {
        var webApplicationBuilder = WebApplication.CreateBuilder();
        Assert.Throws<ArgumentNullException>(() =>
        {
            webApplicationBuilder.AddComponentCore((ComponentBuilder)null!);
        });
    }

    [Fact]
    public void AddComponentCore_ReturnOK()
    {
        var webApplicationBuilder = WebApplication.CreateBuilder();
        var componentBuilder = new ComponentBuilder()
        {
            SuppressDuplicateCall = false
        };

        webApplicationBuilder.AddComponentCore(componentBuilder);

        var componentOptions = webApplicationBuilder.Services.GetComponentOptions();
        Assert.Single(componentOptions.PropsActions);
        Assert.False(componentOptions.SuppressDuplicateCall);

        var action = componentOptions.GetPropsAction<ComponentBuilder>();
        Assert.NotNull(action);

        var builder = new ComponentBuilder();
        action(builder);
        Assert.False(builder.SuppressDuplicateCall);
    }

    [Fact]
    public void AddComponentCore_Action_Null_ReturnOK()
    {
        var webApplicationBuilder = WebApplication.CreateBuilder();
        webApplicationBuilder.AddComponentCore();

        var componentOptions = webApplicationBuilder.Services.GetComponentOptions();
        Assert.Single(componentOptions.PropsActions);
    }

    [Fact]
    public void AddComponentCore_Action_ReturnOK()
    {
        var webApplicationBuilder = WebApplication.CreateBuilder();
        webApplicationBuilder.AddComponentCore(builder =>
        {
            builder.SuppressDuplicateCall = false;
        });

        var componentOptions = webApplicationBuilder.Services.GetComponentOptions();
        Assert.Single(componentOptions.PropsActions);
        Assert.False(componentOptions.SuppressDuplicateCall);

        var action = componentOptions.GetPropsAction<ComponentBuilder>();
        Assert.NotNull(action);

        var builder = new ComponentBuilder();
        action(builder);
        Assert.False(builder.SuppressDuplicateCall);
    }

    [Fact]
    public void AddComponent_Dependencies_Null_Throw()
    {
        var webApplicationBuilder = WebApplication.CreateBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            webApplicationBuilder.AddComponent((Dictionary<Type, Type[]>)null!);
        });

        webApplicationBuilder.AddComponent(new Dictionary<Type, Type[]>());
    }

    [Fact]
    public void AddComponent_CallMethods()
    {
        // D C B A
        var webApplication = WebApplication.CreateBuilder().AddComponentCore().Build();
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));

        webApplication.UseComponent(dependencies);
        var action = webApplication.GetComponentOptions().GetPropsAction<CallOptions>();
        Assert.NotNull(action);

        var callOptions = new CallOptions();
        action(callOptions);

        Assert.Equal(8, callOptions.CallRecords.Count);

        Assert.Equal($"{nameof(AComponent)}.{nameof(AComponent.PreConfigure)}", callOptions.CallRecords[0]);
        Assert.Equal($"{nameof(BComponent)}.{nameof(BComponent.PreConfigure)}", callOptions.CallRecords[1]);
        Assert.Equal($"{nameof(CComponent)}.{nameof(CComponent.PreConfigure)}", callOptions.CallRecords[2]);
        Assert.Equal($"{nameof(DComponent)}.{nameof(CComponent.PreConfigure)}", callOptions.CallRecords[3]);

        Assert.Equal($"{nameof(DComponent)}.{nameof(AComponent.Configure)}", callOptions.CallRecords[4]);
        Assert.Equal($"{nameof(CComponent)}.{nameof(BComponent.Configure)}", callOptions.CallRecords[5]);
        Assert.Equal($"{nameof(BComponent)}.{nameof(CComponent.Configure)}", callOptions.CallRecords[6]);
        Assert.Equal($"{nameof(AComponent)}.{nameof(CComponent.Configure)}", callOptions.CallRecords[7]);
    }

    [Fact]
    public void AddComponent_DuplicateCallMethods()
    {
        // D C B A
        var webApplication = WebApplication.CreateBuilder().AddComponentCore().Build();
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));

        webApplication.UseComponentCore(builder =>
        {
            builder.SuppressDuplicateCall = false;
        });
        webApplication.UseComponent(dependencies);
        var action = webApplication.GetComponentOptions().GetPropsAction<CallOptions>();
        Assert.NotNull(action);

        var callOptions = new CallOptions();
        action(callOptions);

        Assert.True(callOptions.CallRecords.Count > 8);
    }

    [Fact]
    public void AddComponent_Generic()
    {
        // D C B A
        var webApplicationBuilder = WebApplication.CreateBuilder();
        webApplicationBuilder.AddComponent<AComponent>();

        var webApplication = webApplicationBuilder.Build();
        webApplication.UseComponent<AComponent>();
        var action = webApplication.GetComponentOptions().GetPropsAction<CallOptions>();
        Assert.NotNull(action);

        var callOptions = new CallOptions();
        action(callOptions);

        Assert.Equal(8, callOptions.CallRecords.Count);
    }

    [Fact]
    public void AddComponent_Type()
    {
        // D C B A
        var webApplicationBuilder = WebApplication.CreateBuilder();
        webApplicationBuilder.AddComponent(typeof(AComponent));

        var webApplication = webApplicationBuilder.Build();
        webApplication.UseComponent(typeof(AComponent));
        var action = webApplication.GetComponentOptions().GetPropsAction<CallOptions>();
        Assert.NotNull(action);

        var callOptions = new CallOptions();
        action(callOptions);

        Assert.Equal(8, callOptions.CallRecords.Count);
    }
}