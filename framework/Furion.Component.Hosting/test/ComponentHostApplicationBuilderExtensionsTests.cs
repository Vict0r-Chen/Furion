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

namespace Furion.Component.Hosting.Tests;

public class ComponentHostApplicationBuilderExtensionsTests
{
    [Fact]
    public void Entry_WebComponent_ReturnOK()
    {
        var hostApplicationBuilder = Host.CreateApplicationBuilder().Entry<AComponent>();
        var action = hostApplicationBuilder.GetComponentOptions().GetPropsAction<CallOptions>();
        Assert.NotNull(action);

        var callOptions = new CallOptions();
        action(callOptions);

        Assert.Equal(8, callOptions.CallRecords.Count);

        Assert.Equal($"{nameof(AComponent)}.{nameof(AComponent.PreConfigureServices)}", callOptions.CallRecords[0]);
        Assert.Equal($"{nameof(BComponent)}.{nameof(BComponent.PreConfigureServices)}", callOptions.CallRecords[1]);
        Assert.Equal($"{nameof(CComponent)}.{nameof(CComponent.PreConfigureServices)}", callOptions.CallRecords[2]);
        Assert.Equal($"{nameof(DComponent)}.{nameof(CComponent.PreConfigureServices)}", callOptions.CallRecords[3]);

        Assert.Equal($"{nameof(DComponent)}.{nameof(AComponent.ConfigureServices)}", callOptions.CallRecords[4]);
        Assert.Equal($"{nameof(CComponent)}.{nameof(BComponent.ConfigureServices)}", callOptions.CallRecords[5]);
        Assert.Equal($"{nameof(BComponent)}.{nameof(CComponent.ConfigureServices)}", callOptions.CallRecords[6]);
        Assert.Equal($"{nameof(AComponent)}.{nameof(CComponent.ConfigureServices)}", callOptions.CallRecords[7]);
    }

    [Fact]
    public void AddComponentCore_Null_Throw()
    {
        var hostApplicationBuilder = Host.CreateApplicationBuilder();
        Assert.Throws<ArgumentNullException>(() =>
        {
            hostApplicationBuilder.AddComponentCore((ComponentBuilder)null!);
        });
    }

    [Fact]
    public void AddComponentCore_ReturnOK()
    {
        var hostApplicationBuilder = Host.CreateApplicationBuilder();
        var componentBuilder = new ComponentBuilder()
        {
            SuppressDuplicateCall = false
        };

        hostApplicationBuilder.AddComponentCore(componentBuilder);

        var componentOptions = hostApplicationBuilder.Services.GetComponentOptions();
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
        var hostApplicationBuilder = Host.CreateApplicationBuilder();
        hostApplicationBuilder.AddComponentCore();

        var componentOptions = hostApplicationBuilder.Services.GetComponentOptions();
        Assert.Single(componentOptions.PropsActions);
    }

    [Fact]
    public void AddComponentCore_Action_ReturnOK()
    {
        var hostApplicationBuilder = Host.CreateApplicationBuilder();
        hostApplicationBuilder.AddComponentCore(builder =>
        {
            builder.SuppressDuplicateCall = false;
        });

        var componentOptions = hostApplicationBuilder.Services.GetComponentOptions();
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
        var hostApplicationBuilder = Host.CreateApplicationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            hostApplicationBuilder.AddComponent((Dictionary<Type, Type[]>)null!);
        });

        hostApplicationBuilder.AddComponent(new Dictionary<Type, Type[]>() { { typeof(AComponent), new[] { typeof(BComponent) } } });
    }

    [Fact]
    public void AddComponent_CallMethods()
    {
        // D C B A
        var hostApplicationBuilder = Host.CreateApplicationBuilder();
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));

        hostApplicationBuilder.AddComponentCore();
        hostApplicationBuilder.AddComponent(dependencies);
        var action = hostApplicationBuilder.Services.GetComponentOptions().GetPropsAction<CallOptions>();
        Assert.NotNull(action);

        var callOptions = new CallOptions();
        action(callOptions);

        Assert.Equal(8, callOptions.CallRecords.Count);

        Assert.Equal($"{nameof(AComponent)}.{nameof(AComponent.PreConfigureServices)}", callOptions.CallRecords[0]);
        Assert.Equal($"{nameof(BComponent)}.{nameof(BComponent.PreConfigureServices)}", callOptions.CallRecords[1]);
        Assert.Equal($"{nameof(CComponent)}.{nameof(CComponent.PreConfigureServices)}", callOptions.CallRecords[2]);
        Assert.Equal($"{nameof(DComponent)}.{nameof(CComponent.PreConfigureServices)}", callOptions.CallRecords[3]);

        Assert.Equal($"{nameof(DComponent)}.{nameof(AComponent.ConfigureServices)}", callOptions.CallRecords[4]);
        Assert.Equal($"{nameof(CComponent)}.{nameof(BComponent.ConfigureServices)}", callOptions.CallRecords[5]);
        Assert.Equal($"{nameof(BComponent)}.{nameof(CComponent.ConfigureServices)}", callOptions.CallRecords[6]);
        Assert.Equal($"{nameof(AComponent)}.{nameof(CComponent.ConfigureServices)}", callOptions.CallRecords[7]);
    }

    [Fact]
    public void AddComponent_DuplicateCallMethods()
    {
        // D C B A
        var hostApplicationBuilder = Host.CreateApplicationBuilder();
        var dependencies = ComponentBase.CreateDependencies(typeof(AComponent));

        hostApplicationBuilder.AddComponentCore(builder =>
        {
            builder.SuppressDuplicateCall = false;
        });
        hostApplicationBuilder.AddComponent(dependencies);
        var action = hostApplicationBuilder.Services.GetComponentOptions().GetPropsAction<CallOptions>();
        Assert.NotNull(action);

        var callOptions = new CallOptions();
        action(callOptions);

        Assert.True(callOptions.CallRecords.Count > 8);
    }

    [Fact]
    public void AddComponent_Generic()
    {
        // D C B A
        var hostApplicationBuilder = Host.CreateApplicationBuilder();
        hostApplicationBuilder.AddComponent<AComponent>();

        var action = hostApplicationBuilder.Services.GetComponentOptions().GetPropsAction<CallOptions>();
        Assert.NotNull(action);

        var callOptions = new CallOptions();
        action(callOptions);

        Assert.Equal(8, callOptions.CallRecords.Count);
    }

    [Fact]
    public void AddComponent_Type()
    {
        // D C B A
        var hostApplicationBuilder = Host.CreateApplicationBuilder();
        hostApplicationBuilder.AddComponent(typeof(AComponent));

        var action = hostApplicationBuilder.Services.GetComponentOptions().GetPropsAction<CallOptions>();
        Assert.NotNull(action);

        var callOptions = new CallOptions();
        action(callOptions);

        Assert.Equal(8, callOptions.CallRecords.Count);
    }
}