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
/// 依赖注入服务构建器
/// </summary>
public sealed partial class DependencyInjectionBuilder
{
    /// <summary>
    /// 待注册的命名服务描述器集合
    /// </summary>
    private Dictionary<string, ServiceDescriptor>? _namedServices = new();

    /// <summary>
    /// 添加命名服务
    /// </summary>
    /// <param name="serviceKey">服务命名键</param>
    /// <param name="descriptor">服务描述器</param>
    /// <returns><see cref="DependencyInjectionBuilder"/> - 依赖注入服务构建器</returns>
    public DependencyInjectionBuilder AddNamed(string serviceKey, ServiceDescriptor descriptor)
    {
        return this;
    }

    /// <summary>
    /// 添加暂时命名服务
    /// </summary>
    /// <typeparam name="TService">服务</typeparam>
    /// <typeparam name="TImplementation">服务实现类</typeparam>
    /// <param name="serviceKey">服务命名键</param>
    /// <returns><see cref="DependencyInjectionBuilder"/> - 依赖注入服务构建器</returns>
    public DependencyInjectionBuilder AddNamedTransient<TService, TImplementation>(string serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        return AddNamed(serviceKey, ServiceDescriptor.Transient<TService, TImplementation>());
    }

    /// <summary>
    /// 添加范围命名服务
    /// </summary>
    /// <typeparam name="TService">服务</typeparam>
    /// <typeparam name="TImplementation">服务实现类</typeparam>
    /// <param name="serviceKey">服务命名键</param>
    /// <returns><see cref="DependencyInjectionBuilder"/> - 依赖注入服务构建器</returns>
    public DependencyInjectionBuilder AddNamedScoped<TService, TImplementation>(string serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        return AddNamed(serviceKey, ServiceDescriptor.Scoped<TService, TImplementation>());
    }

    /// <summary>
    /// 添加单例命名服务
    /// </summary>
    /// <typeparam name="TService">服务</typeparam>
    /// <typeparam name="TImplementation">服务实现类</typeparam>
    /// <param name="serviceKey">服务命名键</param>
    /// <returns><see cref="DependencyInjectionBuilder"/> - 依赖注入服务构建器</returns>
    public DependencyInjectionBuilder AddNamedSingleton<TService, TImplementation>(string serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        return AddNamed(serviceKey, ServiceDescriptor.Singleton<TService, TImplementation>());
    }
}