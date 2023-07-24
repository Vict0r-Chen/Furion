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

namespace Furion.Component.AspNetCore.Tests;

public class AComponent : ComponentBase
{ }

[DependsOn<BWebComponent, CWebComponent, DWebComponent>]
public class AWebComponent : WebComponent
{
    public override void PreConfigure(ApplicationComponentContext context)
    {
        context.Application.UseHttpsRedirection();

        var record = $"{typeof(AWebComponent)}.{nameof(PreConfigure)}";
        context.Properties[record] = record;
    }

    public override void Configure(ApplicationComponentContext context)
    {
        var record = $"{typeof(AWebComponent)}.{nameof(Configure)}";
        context.Properties[record] = record;
    }
}

[DependsOn<CWebComponent, FWebComponent>]
public class BWebComponent : WebComponent
{
    public override void PreConfigure(ApplicationComponentContext context)
    {
        var record = $"{typeof(BWebComponent)}.{nameof(PreConfigure)}";
        context.Properties[record] = record;
    }

    public override void Configure(ApplicationComponentContext context)
    {
        var record = $"{typeof(BWebComponent)}.{nameof(Configure)}";
        context.Properties[record] = record;
    }
}

[DependsOn<EWebComponent, DWebComponent>]
public class CWebComponent : WebComponent
{
    public override void PreConfigure(ApplicationComponentContext context)
    {
        var record = $"{typeof(CWebComponent)}.{nameof(PreConfigure)}";
        context.Properties[record] = record;
    }

    public override void Configure(ApplicationComponentContext context)
    {
        var record = $"{typeof(CWebComponent)}.{nameof(Configure)}";
        context.Properties[record] = record;
    }
}

public class DWebComponent : WebComponent
{
    public override void PreConfigure(ApplicationComponentContext context)
    {
        var record = $"{typeof(DWebComponent)}.{nameof(PreConfigure)}";
        context.Properties[record] = record;
    }

    public override void Configure(ApplicationComponentContext context)
    {
        var record = $"{typeof(DWebComponent)}.{nameof(Configure)}";
        context.Properties[record] = record;
    }
}

[DependsOn<FWebComponent>]
public class EWebComponent : WebComponent
{
    public override void PreConfigure(ApplicationComponentContext context)
    {
        var record = $"{typeof(EWebComponent)}.{nameof(PreConfigure)}";
        context.Properties[record] = record;
    }

    public override void Configure(ApplicationComponentContext context)
    {
        var record = $"{typeof(EWebComponent)}.{nameof(Configure)}";
        context.Properties[record] = record;
    }
}

public class FWebComponent : WebComponent
{
    public override void PreConfigure(ApplicationComponentContext context)
    {
        var record = $"{typeof(FWebComponent)}.{nameof(PreConfigure)}";
        context.Properties[record] = record;
    }

    public override void Configure(ApplicationComponentContext context)
    {
        var record = $"{typeof(FWebComponent)}.{nameof(Configure)}";
        context.Properties[record] = record;
    }
}