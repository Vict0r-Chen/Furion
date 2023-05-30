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
/// <see cref="Type"/> 类型拓展类
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// 获取类型匹配的所有继承类型
    /// </summary>
    /// <remarks>包含派生类型和所有有效接口类型</remarks>
    /// <param name="type"><see cref="Type"/> - 类型</param>
    /// <param name="excludeTypes">排除的类型集合</param>
    /// <returns><see cref="Tuple{T1, T2, T3}"/> - (定义类型，匹配的继承类型, 服务生存期依赖类型）</returns>
    internal static (Type TypeDefinition, IEnumerable<Type> InheritTypes, Type? LifetimeDependencyType) GetMatchInheritTypes(this Type type, IEnumerable<Type>? excludeTypes = default)
    {
        // 获取类型定义
        var typeDefinition = type.IsGenericType
                                    ? type.GetGenericTypeDefinition()
                                    : type;

        // 获取类型定义参数
        var typeDefinitionParameters = type.GetTypeInfo().GenericTypeParameters;

        // 获取类型实现的所有接口
        var interfaces = type.GetInterfaces();

        // 解析服务生存期类型
        var lifetimeDependencyType = interfaces.SingleOrDefault(DependencyInjectionBuilder.CheckIsAssignableFromILifetimeDependency);

        // 过滤无效接口
        var filterInterfaces = excludeTypes is null
                                    ? interfaces
                                    : interfaces.Where(t => !excludeTypes.Contains(t) && lifetimeDependencyType?.IsAssignableFrom(t) == false);

        // 获取派生类型
        var baseType = type.BaseType is null
                                 || type.BaseType == typeof(object)
                                 || type.BaseType.IsNotPublic
                                 || (type.IsGenericType && !type.BaseType.IsGenericType)
                               ? null
                               : type.BaseType;

        // 获取所有继承类型，包含派生类型和所有有效接口
        var inheritTypes = baseType is null
                                            ? filterInterfaces
                                            : filterInterfaces.Concat(new[] { baseType });

        // 处理泛型类型
        inheritTypes = !type.IsGenericType
                         ? inheritTypes
                         : inheritTypes.Where(i => i.IsGenericType
                                                             && i.GenericTypeArguments.Length == typeDefinitionParameters.Length
                                                             && i.GenericTypeArguments.SequenceEqual(typeDefinitionParameters))
                                        .Select(i => i.GetGenericTypeDefinition());

        // 组合最终继承类型
        inheritTypes = inheritTypes.Any()
                         ? inheritTypes
                         : new[] { typeDefinition };

        // 返回元组
        return (typeDefinition, inheritTypes, lifetimeDependencyType);
    }
}