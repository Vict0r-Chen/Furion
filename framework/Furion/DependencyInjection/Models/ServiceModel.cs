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
/// 服务模型
/// </summary>
public sealed class ServiceModel
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceType">服务类型</param>
    /// <param name="implementationType">服务实现类型</param>
    /// <param name="serviceLifetime">服务生存期</param>
    /// <param name="serviceRegister">服务注册方式</param>
    public ServiceModel(Type serviceType
        , Type implementationType
        , ServiceLifetime serviceLifetime
        , ServiceRegister? serviceRegister = null)
    {
        ServiceDescriptor = ServiceDescriptor.Describe(serviceType, implementationType, serviceLifetime);
        ServiceRegister = serviceType == implementationType || serviceRegister is null
                                           ? ServiceRegister.Add // 处理 TryAddEnumerable 不能注册服务类型等于实现类型的问题
                                           : serviceRegister.Value;
    }

    /// <summary>
    /// 服务描述器
    /// </summary>
    public ServiceDescriptor ServiceDescriptor { get; init; }

    /// <summary>
    /// 服务注册方式
    /// </summary>
    public ServiceRegister ServiceRegister { get; init; }

    /// <summary>
    /// 检查服务模型是否可以注册
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> - 服务描述器集合</param>
    /// <returns><see cref="bool"/> - 返回 true 可以注册</returns>
    public bool CanRegister(IServiceCollection services)
    {
        return !services.Any(s => s.ServiceType == ServiceDescriptor.ServiceType
                                                      && (s.ImplementationType == ServiceDescriptor.ImplementationType || (s.ImplementationType is null && ServiceDescriptor.ServiceType == ServiceDescriptor.ImplementationType)));
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj != null
                && GetType() == obj.GetType()
                && obj is ServiceModel model
                && model.ServiceDescriptor.ServiceType == ServiceDescriptor.ServiceType
                && model.ServiceDescriptor.ImplementationType == ServiceDescriptor.ImplementationType;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return ServiceDescriptor.ServiceType.GetHashCode() + ServiceDescriptor.ImplementationType?.GetHashCode() ?? default;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ServiceRegister.ToString() + " " + ServiceDescriptor.ToString();
    }
}