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
/// <see cref="IServiceCollection"/> 类型拓展
/// </summary>
/// <remarks>支持基于名称注册服务</remarks>
public static class NamedServiceCollectionExtensions
{
    /// <summary>
    /// 添加命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamed(this IServiceCollection services)
    {
        // 注册泛型命名服务
        services.TryAddTransient(typeof(INamedService<>), typeof(NamedService<>));

        return services;
    }

    /// <summary>
    /// 添加命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceDescriptor"><see cref="ServiceDescriptor"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IServiceCollection AddNamed(this IServiceCollection services, string name, ServiceDescriptor serviceDescriptor)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(serviceDescriptor);

        // 创建服务描述器代理类型
        var serviceDescriptorDelegator = CreateDelegator(serviceDescriptor, name);
        ArgumentNullException.ThrowIfNull(serviceDescriptorDelegator);

        // 日志事件记录
        DependencyInjectionEventSource.Log.AddNamed($"{serviceDescriptorDelegator}");

        services.Add(serviceDescriptorDelegator);

        // 注册泛型命名服务
        services.AddNamed();

        return services;
    }

    /// <summary>
    /// 添加命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceDescriptor"><see cref="ServiceDescriptor"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamed(this IServiceCollection services, string name, ServiceDescriptor serviceDescriptor)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(serviceDescriptor);

        // 创建服务描述器代理类型
        var serviceDescriptorDelegator = CreateDelegator(serviceDescriptor, name);
        ArgumentNullException.ThrowIfNull(serviceDescriptorDelegator);

        // 日志事件记录
        DependencyInjectionEventSource.Log.TryAddNamed($"{serviceDescriptorDelegator}");

        services.TryAdd(serviceDescriptorDelegator);

        // 注册泛型命名服务
        services.AddNamed();

        return services;
    }

    /// <summary>
    /// 添加瞬时命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedTransient(this IServiceCollection services, string name, Type serviceType, Func<IServiceProvider, object> implementationFactory)
    {
        return services.AddNamed(name, ServiceDescriptor.Transient(serviceType, implementationFactory));
    }

    /// <summary>
    /// 添加瞬时命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationType">实现类类型</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedTransient(this IServiceCollection services, string name, Type serviceType, Type implementationType)
    {
        return services.AddNamed(name, ServiceDescriptor.Transient(serviceType, implementationType));
    }

    /// <summary>
    /// 添加瞬时命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedTransient<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        return services.AddNamed(name, ServiceDescriptor.Transient(implementationFactory));
    }

    /// <summary>
    /// 添加瞬时命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedTransient<TService, TImplementation>(this IServiceCollection services, string name)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddNamed(name, ServiceDescriptor.Transient<TService, TImplementation>());
    }

    /// <summary>
    /// 添加瞬时命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedTransient<TService, TImplementation>(this IServiceCollection services, string name, Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddNamed(name, ServiceDescriptor.Transient<TService, TImplementation>(implementationFactory));
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedScoped<TService, TImplementation>(this IServiceCollection services, string name)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddNamed(name, ServiceDescriptor.Scoped<TService, TImplementation>());
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedScoped<TService, TImplementation>(this IServiceCollection services, string name, Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddNamed(name, ServiceDescriptor.Scoped<TService, TImplementation>(implementationFactory));
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedScoped(this IServiceCollection services, string name, Type serviceType, Func<IServiceProvider, object> implementationFactory)
    {
        return services.AddNamed(name, ServiceDescriptor.Scoped(serviceType, implementationFactory));
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationType">实现类类型</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedScoped(this IServiceCollection services, string name, Type serviceType, Type implementationType)
    {
        return services.AddNamed(name, ServiceDescriptor.Scoped(serviceType, implementationType));
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedScoped<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        return services.AddNamed(name, ServiceDescriptor.Scoped(implementationFactory));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedSingleton<TService, TImplementation>(this IServiceCollection services, string name, Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddNamed(name, ServiceDescriptor.Singleton<TService, TImplementation>(implementationFactory));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedSingleton<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        return services.AddNamed(name, ServiceDescriptor.Singleton(implementationFactory));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationInstance">实现类实例</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedSingleton<TService>(this IServiceCollection services, string name, TService implementationInstance)
        where TService : class
    {
        return services.AddNamed(name, ServiceDescriptor.Singleton(implementationInstance));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationType">实现类类型</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedSingleton(this IServiceCollection services, string name, Type serviceType, Type implementationType)
    {
        return services.AddNamed(name, ServiceDescriptor.Singleton(serviceType, implementationType));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationInstance">实现类实例</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedSingleton(this IServiceCollection services, string name, Type serviceType, object implementationInstance)
    {
        return services.AddNamed(name, ServiceDescriptor.Singleton(serviceType, implementationInstance));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedSingleton(this IServiceCollection services, string name, Type serviceType, Func<IServiceProvider, object> implementationFactory)
    {
        return services.AddNamed(name, ServiceDescriptor.Singleton(serviceType, implementationFactory));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddNamedSingleton<TService, TImplementation>(this IServiceCollection services, string name)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddNamed(name, ServiceDescriptor.Singleton<TService, TImplementation>());
    }

    /// <summary>
    /// 添加瞬时命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedTransient(this IServiceCollection services, string name, Type serviceType, Func<IServiceProvider, object> implementationFactory)
    {
        return services.TryAddNamed(name, ServiceDescriptor.Transient(serviceType, implementationFactory));
    }

    /// <summary>
    /// 添加瞬时命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationType">实现类类型</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedTransient(this IServiceCollection services, string name, Type serviceType, Type implementationType)
    {
        return services.TryAddNamed(name, ServiceDescriptor.Transient(serviceType, implementationType));
    }

    /// <summary>
    /// 添加瞬时命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedTransient<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        return services.TryAddNamed(name, ServiceDescriptor.Transient(implementationFactory));
    }

    /// <summary>
    /// 添加瞬时命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedTransient<TService, TImplementation>(this IServiceCollection services, string name)
        where TService : class
        where TImplementation : class, TService
    {
        return services.TryAddNamed(name, ServiceDescriptor.Transient<TService, TImplementation>());
    }

    /// <summary>
    /// 添加瞬时命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedTransient<TService, TImplementation>(this IServiceCollection services, string name, Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        return services.TryAddNamed(name, ServiceDescriptor.Transient<TService, TImplementation>(implementationFactory));
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedScoped<TService, TImplementation>(this IServiceCollection services, string name)
        where TService : class
        where TImplementation : class, TService
    {
        return services.TryAddNamed(name, ServiceDescriptor.Scoped<TService, TImplementation>());
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedScoped<TService, TImplementation>(this IServiceCollection services, string name, Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        return services.TryAddNamed(name, ServiceDescriptor.Scoped<TService, TImplementation>(implementationFactory));
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedScoped(this IServiceCollection services, string name, Type serviceType, Func<IServiceProvider, object> implementationFactory)
    {
        return services.TryAddNamed(name, ServiceDescriptor.Scoped(serviceType, implementationFactory));
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationType">实现类类型</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedScoped(this IServiceCollection services, string name, Type serviceType, Type implementationType)
    {
        return services.TryAddNamed(name, ServiceDescriptor.Scoped(serviceType, implementationType));
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedScoped<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        return services.TryAddNamed(name, ServiceDescriptor.Scoped(implementationFactory));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedSingleton<TService, TImplementation>(this IServiceCollection services, string name, Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        return services.TryAddNamed(name, ServiceDescriptor.Singleton<TService, TImplementation>(implementationFactory));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedSingleton<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        return services.TryAddNamed(name, ServiceDescriptor.Singleton(implementationFactory));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="implementationInstance">实现类实例</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedSingleton<TService>(this IServiceCollection services, string name, TService implementationInstance)
        where TService : class
    {
        return services.TryAddNamed(name, ServiceDescriptor.Singleton(implementationInstance));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationType">实现类类型</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedSingleton(this IServiceCollection services, string name, Type serviceType, Type implementationType)
    {
        return services.TryAddNamed(name, ServiceDescriptor.Singleton(serviceType, implementationType));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationInstance">实现类实例</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedSingleton(this IServiceCollection services, string name, Type serviceType, object implementationInstance)
    {
        return services.TryAddNamed(name, ServiceDescriptor.Singleton(serviceType, implementationInstance));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationFactory">实现类工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedSingleton(this IServiceCollection services, string name, Type serviceType, Func<IServiceProvider, object> implementationFactory)
    {
        return services.TryAddNamed(name, ServiceDescriptor.Singleton(serviceType, implementationFactory));
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <typeparam name="TImplementation">实现类类型</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="name">服务名称</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection TryAddNamedSingleton<TService, TImplementation>(this IServiceCollection services, string name)
        where TService : class
        where TImplementation : class, TService
    {
        return services.TryAddNamed(name, ServiceDescriptor.Singleton<TService, TImplementation>());
    }

    /// <summary>
    /// 创建服务描述器代理
    /// </summary>
    /// <param name="serviceDescriptor"><see cref="ServiceDescriptor"/></param>
    /// <param name="name">服务名称</param>
    /// <returns><see cref="ServiceDescriptor"/></returns>
    private static ServiceDescriptor? CreateDelegator(ServiceDescriptor serviceDescriptor, string name)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(serviceDescriptor);

        var serviceTypeDelegator = new NamedType(name, serviceDescriptor.ServiceType);

        // 返回实现类类型服务描述器
        if (serviceDescriptor.ImplementationType is not null)
        {
            return new(serviceTypeDelegator, serviceDescriptor.ImplementationType, serviceDescriptor.Lifetime);
        }

        // 返回对象实例服务描述器
        if (serviceDescriptor.ImplementationInstance is not null)
        {
            return new(serviceTypeDelegator, serviceDescriptor.ImplementationInstance);
        }

        // 返回泛型工厂实例服务描述器
        if (serviceDescriptor.ImplementationFactory is not null)
        {
            return new(serviceTypeDelegator, serviceDescriptor.ImplementationFactory, serviceDescriptor.Lifetime);
        }

        return null;
    }
}