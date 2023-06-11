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

namespace Furion.Component;

/// <summary>
/// 应用组件上下文
/// </summary>
public sealed class ApplicationContext : ComponentContext
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="application"><see cref="WebApplication"/></param>
    internal ApplicationContext(WebApplication application)
        : base(application.Configuration)
    {
        Application = application;
        Environment = application.Environment;
    }

    /// <summary>
    /// 析构实例
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/></param>
    public void Deconstruct(out WebApplication app)
    {
        app = Application;
    }

    /// <inheritdoc cref="WebApplication"/>
    public WebApplication Application { get; }

    /// <inheritdoc cref="IWebHostEnvironment"/>
    public new IWebHostEnvironment Environment { get; }

    /// <summary>
    /// 获取组件参数
    /// </summary>
    /// <typeparam name="TOption">组件参数类型</typeparam>
    /// <returns><typeparamref name="TOption"/></returns>
    public TOption? GetOptions<TOption>()
        where TOption : class, new()
    {
        var componentOptions = Application.GetComponentOptions();
        return componentOptions.OptionsActions.GetOptions<TOption>();
    }

    /// <summary>
    /// 获取组件参数
    /// </summary>
    /// <typeparam name="TOption">组件参数类型</typeparam>
    /// <returns><typeparamref name="TOption"/></returns>
    public TOption GetOptionsOrDefault<TOption>()
        where TOption : class, new()
    {
        return GetOptions<TOption>() ?? Activator.CreateInstance<TOption>();
    }
}