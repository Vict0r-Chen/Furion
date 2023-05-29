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
/// <see cref="Type"/> 类型拓展类
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// 判断类型是否是静态类
    /// </summary>
    /// <param name="type"><see cref="Type"/> - 类型</param>
    /// <returns><see cref="bool"/> - true 表示是静态类；false 表示非静态类</returns>
    internal static bool IsStatic(this Type type)
    {
        return type.IsSealed && type.IsAbstract;
    }

    /// <summary>
    /// 获取类型匹配的继承类型
    /// </summary>
    /// <remarks>包含派生类型和接口类型</remarks>
    /// <param name="type"><see cref="Type"/> - 类型</param>
    /// <param name="retrieveType">检索类型</param>
    /// <param name="excludeTypes">排除的类型</param>
    /// <returns><see cref="Tuple{T1, T2, T3}"/> - (定义类型，匹配的继承类型, 依赖类型）</returns>
    internal static (Type Definition, IEnumerable<Type>? InheritTypes, Type? DependencyType) GetMatchInheritTypes(this Type type, Type? retrieveType, IEnumerable<Type>? excludeTypes = default)
    {
        // 获取类型定义类型
        var typeDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

        // 获取类型定义参数
        var typeDefinitionParameters = type.GetTypeInfo().GenericTypeParameters;

        // 获取所有接口
        IEnumerable<Type> interfaces = type.GetInterfaces();

        // 获取依赖类型
        var dependencyType = retrieveType is null
                                    ? null
                                    : interfaces.SingleOrDefault(i => i != retrieveType && retrieveType.IsAssignableFrom(i));

        // 排除特定类型
        if (excludeTypes is not null)
        {
            interfaces = interfaces.Where(t => !excludeTypes.Contains(t) && dependencyType?.IsAssignableFrom(t) == false);
        }

        // 获取基类
        var baseType = type.BaseType is null
                             || type.BaseType == typeof(object)
                             || type.BaseType.IsNotPublic
                             || (type.IsGenericType && !type.BaseType.IsGenericType)
                           ? null
                           : type.BaseType;

        // 获取所有继承类
        var inheritTypes = baseType is null
                                            ? interfaces
                                            : interfaces.Concat(new[] { baseType });

        inheritTypes = !type.IsGenericType ?
                            inheritTypes
                            : inheritTypes.Where(i => i.IsGenericType
                                                                && i.GenericTypeArguments.Length == typeDefinitionParameters.Length
                                                                && i.GenericTypeArguments.SequenceEqual(typeDefinitionParameters))
                                          .Select(i => i.GetGenericTypeDefinition());

        // 处理没有接口或基类类型
        inheritTypes = inheritTypes.Any() ? inheritTypes : new[] { typeDefinition };

        return (typeDefinition, inheritTypes, dependencyType);
    }
}