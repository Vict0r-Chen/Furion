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

[DependsOn<BComponent, CComponent, DComponent>]
public class AComponent : ComponentBase
{
    public List<string> Items { get; set; } = new();

    [ComponentProps]
    public Action<ComponentOptionsClass1>? CustomProps { get; set; }

    public override void PreConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(AComponent)}.{nameof(PreConfigureServices)}";
        context.Properties[record] = record;
        Items.Add($"{typeof(AComponent)}.{nameof(PreConfigureServices)}");
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(AComponent)}.{nameof(ConfigureServices)}";
        context.Properties[record] = record;
        Items.Add($"{typeof(AComponent)}.{nameof(ConfigureServices)}");
    }

    public override void OnDependencyInvocation(ComponentInvocationContext context)
    {
        Items.Add($"{context.Component.GetType()}.{context.MethodName}");
    }
}

[DependsOn<CComponent, FComponent>]
public class BComponent : ComponentBase
{
    public override void OnDependencyInvocation(ComponentInvocationContext context)
    {
    }

    public override void PreConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(BComponent)}.{nameof(PreConfigureServices)}";
        context.Properties[record] = record;
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(BComponent)}.{nameof(ConfigureServices)}";
        context.Properties[record] = record;
    }
}

[DependsOn<EComponent, DComponent>]
public class CComponent : ComponentBase
{
    public override void PreConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(CComponent)}.{nameof(PreConfigureServices)}";
        context.Properties[record] = record;
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(CComponent)}.{nameof(ConfigureServices)}";
        context.Properties[record] = record;
    }
}

public class DComponent : ComponentBase
{
    public override void PreConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(DComponent)}.{nameof(PreConfigureServices)}";
        context.Properties[record] = record;
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(DComponent)}.{nameof(ConfigureServices)}";
        context.Properties[record] = record;
    }
}

[DependsOn<FComponent>]
public class EComponent : ComponentBase
{
    public override void PreConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(EComponent)}.{nameof(PreConfigureServices)}";
        context.Properties[record] = record;
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(EComponent)}.{nameof(ConfigureServices)}";
        context.Properties[record] = record;
    }
}

public class FComponent : ComponentBase
{
    public override void PreConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(FComponent)}.{nameof(PreConfigureServices)}";
        context.Properties[record] = record;
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(FComponent)}.{nameof(ConfigureServices)}";
        context.Properties[record] = record;
    }
}

[DependsOn<FComponent, GComponent>]
public class GComponent : ComponentBase
{
}

[DependsOn<B1Component, C1Component, D1Component>]
public class A1Component : ComponentBase
{
    public override void PreConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(A1Component)}.{nameof(PreConfigureServices)}";
        context.Properties[record] = record;
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(A1Component)}.{nameof(ConfigureServices)}";
        context.Properties[record] = record;
    }
}

[DependsOn<C1Component>]
public class B1Component : ComponentBase
{
    public override bool CanActivate(ComponentContext context)
    {
        return false;
    }

    public override void PreConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(B1Component)}.{nameof(PreConfigureServices)}";
        context.Properties[record] = record;
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(B1Component)}.{nameof(ConfigureServices)}";
        context.Properties[record] = record;
    }
}

public class C1Component : ComponentBase
{
    public override void PreConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(C1Component)}.{nameof(PreConfigureServices)}";
        context.Properties[record] = record;
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(C1Component)}.{nameof(ConfigureServices)}";
        context.Properties[record] = record;
    }
}

[DependsOn<B1Component>]
public class D1Component : ComponentBase
{
    public override void PreConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(D1Component)}.{nameof(PreConfigureServices)}";
        context.Properties[record] = record;
    }

    public override void ConfigureServices(ServiceComponentContext context)
    {
        var record = $"{typeof(D1Component)}.{nameof(ConfigureServices)}";
        context.Properties[record] = record;
    }
}