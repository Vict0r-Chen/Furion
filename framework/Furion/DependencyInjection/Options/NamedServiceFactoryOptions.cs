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

using Microsoft.Extensions.DependencyInjection;

namespace Furion.DependencyInjection;

/// <summary>
/// 命名服务工厂选项
/// </summary>
public sealed class NamedServiceFactoryOptions
{
    /// <summary>
    /// 命名服务描述器集合
    /// </summary>
    public Dictionary<string, ServiceDescriptor> NamedServiceDescriptors { get; } = new();

    /// <summary>
    /// 添加命名服务
    /// </summary>
    /// <param name="name">服务命名</param>
    /// <param name="descriptor">服务描述器</param>
    /// <returns><see cref="bool"/> - 返回 true 成功添加</returns>
    public bool TryAdd(string name, ServiceDescriptor descriptor)
    {
        return NamedServiceDescriptors.TryAdd(name, descriptor);
    }

    /// <summary>
    /// 查找命名服务
    /// </summary>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="descriptor">服务描述器</param>
    /// <returns><see cref="bool"/> - 返回 true 则存在</returns>
    public bool TryGet(string name, Type serviceType, out ServiceDescriptor? descriptor)
    {
        var isExists = NamedServiceDescriptors.TryGetValue(name, out descriptor);
        if (!isExists
            || descriptor is null
            || descriptor is { ImplementationType: null }
            || descriptor.ServiceType != serviceType) return false;

        return true;
    }

    /// <summary>
    /// 移除命名服务
    /// </summary>
    /// <param name="name">服务命名</param>
    /// <returns><see cref="bool"/> - 返回 true 成功移除</returns>
    public bool Remove(string name)
    {
        return NamedServiceDescriptors.Remove(name);
    }

    /// <summary>
    /// 解析服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="name">服务名称</param>
    /// <param name="serviceProvider">服务提供器</param>
    /// <returns><typeparamref name="TService"/> - 服务实例</returns>
    public TService? GetService<TService>(string name, IServiceProvider serviceProvider)
        where TService : class
    {
        return GetService(name, typeof(TService), serviceProvider) as TService;
    }

    /// <summary>
    /// 解析服务
    /// </summary>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="serviceProvider">服务提供器</param>
    /// <returns><see cref="object"/> - 服务实例</returns>
    public object? GetService(string name, Type serviceType, IServiceProvider serviceProvider)
    {
        if (TryGet(name, serviceType, out var descriptor))
        {
            // 解析服务
            return serviceProvider.GetService(descriptor!.ImplementationType!);
        }

        return null;
    }

    /// <summary>
    /// 解析服务
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    /// <param name="name">服务名称</param>
    /// <param name="serviceProvider">服务提供器</param>
    /// <returns><typeparamref name="TService"/> - 服务实例</returns>
    public TService GetRequiredService<TService>(string name, IServiceProvider serviceProvider)
        where TService : class
    {
        return (GetRequiredService(name, typeof(TService), serviceProvider) as TService)!;
    }

    /// <summary>
    /// 解析服务
    /// </summary>
    /// <param name="name">服务名称</param>
    /// <param name="serviceType">服务类型</param>
    /// <param name="serviceProvider">服务提供器</param>
    /// <returns><see cref="object"/> - 服务实例</returns>
    public object GetRequiredService(string name, Type serviceType, IServiceProvider serviceProvider)
    {
        if (TryGet(name, serviceType, out var descriptor))
        {
            // 解析服务
            return serviceProvider.GetRequiredService(descriptor!.ImplementationType!);
        }

        return null!;
    }
}