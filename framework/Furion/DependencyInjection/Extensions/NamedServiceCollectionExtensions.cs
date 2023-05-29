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
/// 命名服务拓展类
/// </summary>
public static class NamedServiceCollectionExtensions
{
    /// <summary>
    /// 注册命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> - 服务描述器集合</param>
    /// <param name="name">服务命名键</param>
    /// <param name="descriptor">服务描述器</param>
    /// <returns><see cref="IServiceCollection"/> - 服务描述器集合</returns>
    public static IServiceCollection AddNamed(this IServiceCollection services, string name, ServiceDescriptor descriptor)
    {
        return services;
    }

    /// <summary>
    /// 添加暂时命名服务
    /// </summary>
    /// <typeparam name="TService">服务</typeparam>
    /// <typeparam name="TImplementation">服务实现类</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/> - 服务描述器集合</param>
    /// <param name="name">服务命名键</param>
    /// <returns><see cref="IServiceCollection"/> - 服务描述器集合</returns>
    public static IServiceCollection AddNamedTransient<TService, TImplementation>(this IServiceCollection services, string name)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddNamed(name, ServiceDescriptor.Transient<TService, TImplementation>());
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <typeparam name="TService">服务</typeparam>
    /// <typeparam name="TImplementation">服务实现类</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/> - 服务描述器集合</param>
    /// <param name="name">服务命名键</param>
    /// <returns><see cref="IServiceCollection"/> - 服务描述器集合</returns>
    public static IServiceCollection AddNamedScoped<TService, TImplementation>(this IServiceCollection services, string name)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddNamed(name, ServiceDescriptor.Scoped<TService, TImplementation>());
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <typeparam name="TService">服务</typeparam>
    /// <typeparam name="TImplementation">服务实现类</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/> - 服务描述器集合</param>
    /// <param name="name">服务命名键</param>
    /// <returns><see cref="IServiceCollection"/> - 服务描述器集合</returns>
    public static IServiceCollection AddNamedSingleton<TService, TImplementation>(this IServiceCollection services, string name)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddNamed(name, ServiceDescriptor.Singleton<TService, TImplementation>());
    }

    /// <summary>
    /// 添加暂时命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> - 服务描述器集合</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationType">服务实现类类型</param>
    /// <param name="name">服务命名键</param>
    /// <returns><see cref="IServiceCollection"/> - 服务描述器集合</returns>
    public static IServiceCollection AddNamedTransient(this IServiceCollection services, Type serviceType, Type implementationType, string name)
    {
        return services.AddNamed(name, ServiceDescriptor.Transient(serviceType, implementationType));
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> - 服务描述器集合</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationType">服务实现类类型</param>
    /// <param name="name">服务命名键</param>
    /// <returns><see cref="IServiceCollection"/> - 服务描述器集合</returns>
    public static IServiceCollection AddNamedScoped(this IServiceCollection services, Type serviceType, Type implementationType, string name)
    {
        return services.AddNamed(name, ServiceDescriptor.Scoped(serviceType, implementationType));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> - 服务描述器集合</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationType">服务实现类类型</param>
    /// <param name="name">服务命名键</param>
    /// <returns><see cref="IServiceCollection"/> - 服务描述器集合</returns>
    public static IServiceCollection AddNamedSingleton(this IServiceCollection services, Type serviceType, Type implementationType, string name)
    {
        return services.AddNamed(name, ServiceDescriptor.Singleton(serviceType, implementationType));
    }
}