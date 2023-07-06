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

namespace Furion.DependencyInjection.AspNetCore;

/// <inheritdoc />
internal sealed class TypeActivatorCache : ITypeActivatorCache
{
    /// <summary>
    /// 对象创建工厂
    /// </summary>
    private readonly Func<Type, ObjectFactory> _createFactory = (type) => ActivatorUtilities.CreateFactory(type, Type.EmptyTypes);

    /// <summary>
    /// 类型缓存集合
    /// </summary>
    private readonly ConcurrentDictionary<Type, ObjectFactory> _typeActivatorCache = new();

    /// <inheritdoc/>
    public TInstance CreateInstance<TInstance>(IServiceProvider serviceProvider, Type implementationType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(implementationType);

        // 获取或创建对象实例
        var createFactory = _typeActivatorCache.GetOrAdd(implementationType, _createFactory);
        return (TInstance)createFactory(serviceProvider, arguments: null);
    }
}