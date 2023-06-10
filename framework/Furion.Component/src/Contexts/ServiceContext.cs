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
/// 服务组件上下文
/// </summary>
public sealed class ServiceContext
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    internal ServiceContext(IServiceCollection services, IConfiguration configuration)
    {
        Services = services;
        Configuration = configuration;
        Environment = services.GetHostEnvironment();
    }

    /// <summary>
    /// 析构实例
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    public void Deconstruct(out IServiceCollection services)
    {
        services = Services;
    }

    /// <inheritdoc cref="IServiceCollection"/>
    public IServiceCollection Services { get; }

    /// <inheritdoc cref="IConfiguration"/>
    public IConfiguration Configuration { get; }

    /// <inheritdoc cref="IHostEnvironment"/>
    public IHostEnvironment? Environment { get; }

    ///// <inheritdoc cref="ILoggingBuilder"/>
    //public ILoggingBuilder? Logging { get; init; }

    /// <summary>
    /// 获取组件参数
    /// </summary>
    /// <typeparam name="TOption">组件参数类型</typeparam>
    /// <returns><typeparamref name="TOption"/></returns>
    public TOption? GetOptions<TOption>()
        where TOption : class, new()
    {
        var componentOptions = Services.GetComponentOptions();
        return componentOptions?.OptionsActions.GetOptions<TOption>();
    }
}