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

#pragma warning disable

public class SomeClass
{
}

public class CBaseComponent : ComponentBase
{
}

public class InheritComponent : CBaseComponent
{
}

public abstract class AbstractComponent : ComponentBase
{
}

public class PrivateNewComponent : ComponentBase
{
    private PrivateNewComponent()
    {
    }
}

public class CallOptions
{
    public List<string> CallRecords { get; } = new List<string>();
    public string CallName { get; set; }
}

[DependsOn<BComponent, CComponent>]
public class AComponent : ComponentBase
{
    public override void PreConfigureServices(ServiceComponentContext context)
    {
        Props<CallOptions>(options =>
        {
            options.CallRecords.Add($"{nameof(AComponent)}.{nameof(PreConfigureServices)}");
            options.CallName = nameof(AComponent);
        });

        context.Properties.Add(nameof(AComponent), nameof(PreConfigureServices));
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        Props<CallOptions>(options =>
        {
            options.CallRecords.Add($"{nameof(AComponent)}.{nameof(ConfigureServices)}");
        });

        // 重复调用
        context.Services.AddComponent<BComponent>(context.Configuration);

        context.Properties.Add(nameof(AComponent) + "1", nameof(ConfigureServices));
    }
}

[DependsOn<CComponent, DComponent>]
public class BComponent : ComponentBase
{
    public override void PreConfigureServices(ServiceComponentContext context)
    {
        Props<CallOptions>(options =>
        {
            options.CallRecords.Add($"{nameof(BComponent)}.{nameof(PreConfigureServices)}");
            options.CallName = nameof(BComponent);
        });

        context.Properties.Add(nameof(BComponent), nameof(PreConfigureServices));
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        Props<CallOptions>(options =>
        {
            options.CallRecords.Add($"{nameof(BComponent)}.{nameof(ConfigureServices)}");
        });

        context.Properties.Add(nameof(BComponent) + "1", nameof(ConfigureServices));
    }
}

[DependsOn<DComponent>]
public class CComponent : ComponentBase
{
    public override void PreConfigureServices(ServiceComponentContext context)
    {
        Props<CallOptions>(options =>
        {
            options.CallRecords.Add($"{nameof(CComponent)}.{nameof(PreConfigureServices)}");
            options.CallName = nameof(CComponent);
        });
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        Props<CallOptions>(options =>
        {
            options.CallRecords.Add($"{nameof(CComponent)}.{nameof(ConfigureServices)}");
        });
    }
}

public class DComponent : ComponentBase
{
    public override void PreConfigureServices(ServiceComponentContext context)
    {
        Props<CallOptions>(options =>
        {
            options.CallRecords.Add($"{nameof(DComponent)}.{nameof(PreConfigureServices)}");
            options.CallName = nameof(DComponent);
        });
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        Props<CallOptions>(options =>
        {
            options.CallRecords.Add($"{nameof(DComponent)}.{nameof(ConfigureServices)}");
        });
    }
}

[DependsOn<NotActivateComponent, FComponent>]
public class EComponent : ComponentBase
{
}

[DependsOn<FComponent, DComponent>]
public class NotActivateComponent : ComponentBase
{
    public override bool CanActivate(ComponentContext context)
    {
        return false;
    }
}

public class FComponent : ComponentBase
{
}

public class InvalidOptions
{
    protected InvalidOptions()
    {
    }
}

public class OkOptions
{
}

public class InvalidArgumentComponnet : ComponentBase
{
    public InvalidArgumentComponnet(InvalidOptions options)
    {
    }
}

public class InvalidArgument2Componnet : ComponentBase
{
    public InvalidArgument2Componnet(Action<InvalidOptions> options)
    {
    }
}

public class OkArgumentComponent : ComponentBase
{
    public OkArgumentComponent(Action<OkOptions> action, OkOptions okOptions)
    {
    }
}

public class OkArgument2Component : ComponentBase
{
    public OkArgument2Component(OkOptions okOptions)
    {
        throw new NotImplementedException();
    }

    public OkArgument2Component(Action<OkOptions> action, OkOptions okOptions)
    {
    }
}

public class OkArgument3Component : ComponentBase
{
    [ActivatorComponentConstructor]
    public OkArgument3Component(OkOptions okOptions)
    {
    }

    public OkArgument3Component(Action<OkOptions> action, OkOptions okOptions)
    {
        throw new NotImplementedException();
    }
}

public class PropertyComponent : ComponentBase
{
    [ComponentProps]
    private OkOptions Options { get; set; }

    [ComponentProps]
    internal Action<OkOptions> Action { get; set; }

    [ComponentProps]
    private OkOptions? Options2 { get; set; }

    [ComponentProps]
    public Action<OkOptions> Action2 { get; set; }

    public override void ConfigureServices(ServiceComponentContext context)
    {
    }
}

public class PropertyInvalidComponent : ComponentBase
{
    [ComponentProps]
    public InvalidOptions Options { get; set; }

    public override void ConfigureServices(ServiceComponentContext context)
    {
    }
}

public class PropertyReadonlyComponent : ComponentBase
{
    [ComponentProps]
    public InvalidOptions Options { get; }

    public override void ConfigureServices(ServiceComponentContext context)
    {
    }
}