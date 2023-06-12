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

[DependsOn(
    typeof(BWebComponent)
    , typeof(CWebComponent)
    )]
public class AWebComponent : WebComponent
{
    public override void PreConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigureServices.Add(nameof(AWebComponent));
        });
    }

    public override void ConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.ConfigureServices.Add(nameof(AWebComponent));
        });
    }

    public override void PreConfigure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigure.Add(nameof(AWebComponent));
        });
    }

    public override void Configure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.Configure.Add(nameof(AWebComponent));
        });
    }
}

[DependsOn(
    typeof(CWebComponent)
    , typeof(DWebComponent)
    )]
public class BWebComponent : WebComponent
{
    public override void PreConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigureServices.Add(nameof(BWebComponent));
        });
    }

    public override void ConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.ConfigureServices.Add(nameof(BWebComponent));
        });
    }

    public override void PreConfigure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigure.Add(nameof(BWebComponent));
        });
    }

    public override void Configure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.Configure.Add(nameof(BWebComponent));
        });
    }
}

public class CWebComponent : WebComponent
{
    public override void PreConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigureServices.Add(nameof(CWebComponent));
        });
    }

    public override void ConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.ConfigureServices.Add(nameof(CWebComponent));
        });
    }

    public override void PreConfigure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigure.Add(nameof(CWebComponent));
        });
    }

    public override void Configure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.Configure.Add(nameof(CWebComponent));
        });
    }
}

public class DWebComponent : WebComponent
{
    public override void PreConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigureServices.Add(nameof(DWebComponent));
        });
    }

    public override void ConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.ConfigureServices.Add(nameof(DWebComponent));
        });
    }

    public override void PreConfigure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigure.Add(nameof(DWebComponent));
        });
    }

    public override void Configure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.Configure.Add(nameof(DWebComponent));
        });
    }
}

public class EWebComponent : WebComponent
{
    public override void PreConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigureServices.Add(nameof(EWebComponent));
        });
    }

    public override void ConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.ConfigureServices.Add(nameof(EWebComponent));
        });
    }

    public override void PreConfigure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigure.Add(nameof(EWebComponent));
        });
    }

    public override void Configure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.Configure.Add(nameof(EWebComponent));
        });
    }
}

[DependsOn(
    typeof(CWebComponent)
    , typeof(EWebComponent)
    )]
public class FWebComponent : WebComponent
{
    public override void PreConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigureServices.Add(nameof(FWebComponent));
        });
    }

    public override void ConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.ConfigureServices.Add(nameof(FWebComponent));
        });
    }

    public override void PreConfigure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigure.Add(nameof(FWebComponent));
        });
    }

    public override void Configure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.Configure.Add(nameof(FWebComponent));
        });
    }
}

public class GWebComponent : WebComponent
{
    public override void PreConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigureServices.Add(nameof(GWebComponent));
        });
    }

    public override void ConfigureServices(ServiceContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Services);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.ConfigureServices.Add(nameof(GWebComponent));
        });
    }

    public override void PreConfigure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.PreConfigure.Add(nameof(GWebComponent));
        });
    }

    public override void Configure(ApplicationContext context)
    {
        Assert.NotNull(context);
        Assert.NotNull(context.Application);
        Assert.NotNull(context.Configuration);
        Assert.NotNull(context.Environment);

        Configure<AddComponentOptions>(options =>
        {
            options.Configure.Add(nameof(GWebComponent));
        });
    }
}

public class InheritWebComponent : AWebComponent
{
}

public class NotWebComponent
{
}

public class SuppressDuplicateForWebOptions
{
    public int Num { get; set; }
}

public class SuppressDuplicateForWebComponent : WebComponent
{
    public override void PreConfigure(ApplicationContext context)
    {
        Configure<SuppressDuplicateForWebOptions>(o =>
        {
            o.Num += 1;
        });
    }
}

public class CustomOptionsForWeb
{
    public int Num { get; set; }
}

public class CustomOptionsForWebComponent : WebComponent
{
    public override void Configure(ApplicationContext context)
    {
        var options = context.GetOptions<CustomOptionsForWeb>();
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        Assert.Equal(10, options.Num);
    }
}