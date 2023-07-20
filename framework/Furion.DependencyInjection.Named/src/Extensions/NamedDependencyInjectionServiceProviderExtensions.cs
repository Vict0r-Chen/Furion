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

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 依赖注入命名模块 <see cref="IServiceProvider"/> 拓展类
/// </summary>
public static class NamedDependencyInjectionServiceProviderExtensions
{
    /// <summary>
    /// 获取命名服务
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="name">命名</param>
    /// <param name="serviceType">服务类型</param>
    /// <returns><see cref="object"/></returns>
    public static object? GetNamedService(this IServiceProvider serviceProvider
        , string name
        , Type serviceType)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(serviceType);

        return serviceProvider.GetService(new NamedTypeDelegator(name, serviceType));
    }

    /// <summary>
    /// 获取命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="name">命名</param>
    /// <returns><typeparamref name="TService"/></returns>
    public static TService? GetNamedService<TService>(this IServiceProvider serviceProvider, string name)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return (TService?)serviceProvider.GetNamedService(name, typeof(TService));
    }

    /// <summary>
    /// 获取命名服务
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="name">命名</param>
    /// <param name="serviceType">服务类型</param>
    /// <returns><see cref="object"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static object GetRequiredNamedService(this IServiceProvider serviceProvider
        , string name
        , Type serviceType)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(serviceType);

        return serviceProvider.GetRequiredService(new NamedTypeDelegator(name, serviceType));
    }

    /// <summary>
    /// 获取命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="name">命名</param>
    /// <returns><typeparamref name="TService"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TService GetRequiredNamedService<TService>(this IServiceProvider serviceProvider, string name)
        where TService : notnull
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return (TService)serviceProvider.GetRequiredNamedService(name, typeof(TService));
    }

    /// <summary>
    /// 获取命名服务集合
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="name">命名</param>
    /// <param name="serviceType">服务类型</param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    public static IEnumerable<object?> GetNamedServices(this IServiceProvider serviceProvider
        , string name
        , Type serviceType)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(serviceType);

        return serviceProvider.GetServices(new NamedTypeDelegator(name, serviceType))
            .Where(serviceType.IsInstanceOfType);
    }

    /// <summary>
    /// 获取命名服务集合
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="name">命名</param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    public static IEnumerable<TService> GetNamedServices<TService>(this IServiceProvider serviceProvider, string name)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return serviceProvider.GetServices(new NamedTypeDelegator(name, typeof(TService)))
            .OfType<TService>();
    }
}