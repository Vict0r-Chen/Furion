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

namespace Furion.DependencyInjection;

/// <summary>
/// <see cref="ServiceDescriptor"/> 类型拓展
/// </summary>
internal static class ServiceDescriptorExtensions
{
    /// <summary>
    /// 创建服务描述器代理
    /// </summary>
    /// <param name="serviceDescriptor"><see cref="ServiceDescriptor"/></param>
    /// <param name="name">服务名称</param>
    /// <returns><see cref="ServiceDescriptor"/></returns>
    internal static ServiceDescriptor? CreateDelegator(this ServiceDescriptor serviceDescriptor, string name)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(serviceDescriptor);

        var serviceTypeDelegator = new NamedType(name, serviceDescriptor.ServiceType);

        if (serviceDescriptor.ImplementationType is not null)
        {
            return new(serviceTypeDelegator, serviceDescriptor.ImplementationType, serviceDescriptor.Lifetime);
        }

        if (serviceDescriptor.ImplementationInstance is not null)
        {
            return new(serviceTypeDelegator, serviceDescriptor.ImplementationInstance);
        }

        if (serviceDescriptor.ImplementationFactory is not null)
        {
            return new(serviceTypeDelegator, serviceDescriptor.ImplementationFactory, serviceDescriptor.Lifetime);
        }

        return null;
    }

    /// <summary>
    /// 获取服务描述器实现类型
    /// </summary>
    /// <param name="serviceDescriptor"><see cref="ServiceDescriptor"/></param>
    /// <returns><see cref="Type"/></returns>
    internal static Type? GetImplementationType(this ServiceDescriptor serviceDescriptor)
    {
        // 如果实现类类型不为空直接返回
        if (serviceDescriptor.ImplementationType is not null)
        {
            return serviceDescriptor.ImplementationType;
        }

        // 如果实现类实例不为空直接返回
        if (serviceDescriptor.ImplementationInstance is not null)
        {
            return serviceDescriptor.ImplementationInstance.GetType();
        }

        // 如果实现类工厂不为空则获取泛型参数第二个参数返回
        if (serviceDescriptor.ImplementationFactory is not null)
        {
            var genericTypeArguments = serviceDescriptor.ImplementationFactory.GetType().GenericTypeArguments;
            return genericTypeArguments[1];
        }

        return null;
    }
}