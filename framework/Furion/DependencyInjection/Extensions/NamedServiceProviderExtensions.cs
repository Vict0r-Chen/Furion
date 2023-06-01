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

namespace System;

/// <summary>
/// <see cref="IServiceProvider"/> 类型拓展
/// </summary>
/// <remarks>支持基于名称解析服务</remarks>
public static class NamedServiceProviderExtensions
{
    /// <summary>
    /// 解析命名服务
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="name">服务命名</param>
    /// <param name="serviceType">服务类型</param>
    /// <returns><see cref="object"/></returns>
    public static object? GetNamedService(this IServiceProvider serviceProvider, string name, Type serviceType)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(serviceType);

        // 获取命名服务描述器选项
        var namedServiceCollectionOptions = serviceProvider.GetRequiredService<IOptionsMonitor<NamedServiceCollectionOptions>>().CurrentValue;

        // 查找命名配置是否存在
        var isExists = namedServiceCollectionOptions.NamedServices.TryGetValue(name, out var serviceDescriptor);
        if (!isExists || serviceDescriptor is null)
        {
            return null;
        }

        // 解析实现类型
        var implementationType = serviceDescriptor.ImplementationType;
        if (implementationType is not null)
        {
            // 事件记录
            NamedServiceProviderEventSource.Log.ResolveTypeStarted();

            return serviceProvider.GetRequiredService<IServiceProviderIsService>().IsService(implementationType)
                   ? serviceProvider.GetService(implementationType)
                   : ActivatorUtilities.CreateInstance(serviceProvider, implementationType);
        }

        // 解析实例类型
        if (serviceDescriptor.ImplementationInstance is not null)
        {
            // 事件记录
            NamedServiceProviderEventSource.Log.ResolveInstanceStarted();

            return serviceDescriptor.ImplementationInstance;
        }

        // 解析实现工厂
        if (serviceDescriptor.ImplementationFactory is not null)
        {
            // 事件记录
            NamedServiceProviderEventSource.Log.ResolveFactoryStarted();

            return serviceDescriptor.ImplementationFactory(serviceProvider);
        }

        // 事件记录
        NamedServiceProviderEventSource.Log.ResolveNullStarted();

        return null;
    }

    /// <summary>
    /// 解析命名服务
    /// </summary>
    /// <typeparam name="TService"><typeparamref name="TService"/></typeparam>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="name">服务命名</param>
    /// <returns><typeparamref name="TService"/></returns>
    public static TService? GetNamedService<TService>(this IServiceProvider serviceProvider, string name)
        where TService : class
    {
        return serviceProvider.GetNamedService(name, typeof(TService)) as TService;
    }

    /// <summary>
    /// 解析命名服务
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="name">服务命名</param>
    /// <param name="serviceType">服务类型</param>
    /// <returns><see cref="object"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static object GetNamedRequiredService(this IServiceProvider serviceProvider, string name, Type serviceType)
    {
        var service = serviceProvider.GetNamedService(name, serviceType);
        return service is null
            ? throw new InvalidOperationException($"No service for type '{serviceType.FullName}' has been registered.")
            : service;
    }

    /// <summary>
    /// 解析命名服务
    /// </summary>
    /// <typeparam name="TService"><typeparamref name="TService"/></typeparam>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="name">服务命名</param>
    /// <returns><typeparamref name="TService"/></returns>
    public static TService GetNamedRequiredService<TService>(this IServiceProvider serviceProvider, string name)
        where TService : class
    {
        return (TService)serviceProvider.GetNamedRequiredService(name, typeof(TService));
    }
}