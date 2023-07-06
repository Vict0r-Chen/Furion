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
/// <see cref="Type"/> 拓展类
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// 是否为静态类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsStatic(this Type type)
    {
        return type.IsSealed && type.IsAbstract;
    }

    /// <summary>
    /// 是否为匿名类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsAnonymousType(this Type type)
    {
        // 是否贴有 [CompilerGenerated] 特性
        if (type.IsDefined(typeof(CompilerGeneratedAttribute), false))
        {
            // 类型限定名是否以 <> 开头且以 AnonymousType 结尾
            if (type.FullName is not null
                && type.FullName.StartsWith("<>")
                && type.FullName.Contains("AnonymousType"))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 是否可实例化
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsInstantiable(this Type type)
    {
        return type.IsClass
                && !type.IsAbstract
                && !type.IsStatic();
    }

    /// <summary>
    /// 是否派生自特定类型
    /// </summary>
    /// <remarks>排除特定类型本身</remarks>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="fromType"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsAlienAssignableTo(this Type type, Type fromType)
    {
        return fromType != type
            && fromType.IsAssignableFrom(type);
    }

    /// <summary>
    /// 获取指定特性实例
    /// </summary>
    /// <remarks>若特性不存在则返回 null</remarks>
    /// <typeparam name="TAttribute">特性类型</typeparam>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="inherit">是否查找基类型特性</param>
    /// <returns><typeparamref name="TAttribute"/></returns>
    internal static TAttribute? GetDefinedCustomAttribute<TAttribute>(this Type type, bool inherit = false)
        where TAttribute : Attribute
    {
        // 检查是否定义
        if (!type.IsDefined(typeof(TAttribute), inherit))
        {
            return null;
        }

        return type.GetCustomAttribute<TAttribute>(inherit);
    }

    /// <summary>
    /// 是否定义多个相同的特性
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="attributeType"><see cref="Attribute"/></param>
    /// <param name="inherit">是否查找基类型特性</param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsMultipleSameDefined(this Type type, Type attributeType, bool inherit = false)
    {
        // 检查是否定义
        if (!type.IsDefined(attributeType, inherit))
        {
            return false;
        }

        // 只查找当前类型
        return type.GetCustomAttributes(attributeType, false).Length > 1;
    }

    /// <summary>
    /// 获取指定特性实例
    /// </summary>
    /// <remarks>若特性不存在则返回默认实例</remarks>
    /// <typeparam name="TAttribute">特性类型</typeparam>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="inherit">是否查找基类型特性</param>
    /// <returns><typeparamref name="TAttribute"/></returns>
    internal static TAttribute GetDefinedCustomAttributeOrNew<TAttribute>(this Type type, bool inherit = false)
        where TAttribute : Attribute, new()
    {
        return type.GetDefinedCustomAttribute<TAttribute>(inherit) ?? new();
    }

    /// <summary>
    /// 是否包含公开无参构造函数
    /// </summary>
    /// <remarks>用于 <see cref="Activator.CreateInstance(Type)"/> 实例化</remarks>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool HasParameterlessConstructorDefined(this Type type)
    {
        return type.IsInstantiable()
                && type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes) is not null;
    }

    /// <summary>
    /// 类型定义是否相等
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="compareType"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsEqualTypeDefinition(this Type type, Type compareType)
    {
        return type == compareType
                || (type.IsGenericType
                    && compareType.IsGenericType
                    && type.IsGenericTypeDefinition
                    && type == compareType.GetGenericTypeDefinition());
    }

    /// <summary>
    /// 类型是否兼容
    /// </summary>
    /// <remarks>检查泛型定义参数</remarks>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="baseType"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsTypeCompatibilityTo(this Type type, Type? baseType)
    {
        return baseType is not null && baseType != typeof(object)
                && baseType.IsAssignableFrom(type)
                && (
                    !type.IsGenericType
                    || (type.IsGenericType
                        && baseType.IsGenericType
                        && type.GetTypeInfo().GenericTypeParameters.SequenceEqual(baseType.GenericTypeArguments)
                       )
                   );
    }

    /// <summary>
    /// 是否定义了指定方法
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="name">方法名称</param>
    /// <param name="accessibilityBindingFlags">可访问性成员绑定标记</param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsDeclareOnlyMethod(this Type type, string name, BindingFlags accessibilityBindingFlags)
    {
        return type.GetMethod(name, accessibilityBindingFlags | BindingFlags.Instance | BindingFlags.DeclaredOnly) is not null;
    }

    /// <summary>
    /// 是否为整数类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsInteger(this Type type)
    {
        if (type.IsDecimal())
        {
            return false;
        }

        // 获取类型代码
        var typeCode = Type.GetTypeCode(type);
        return typeCode is TypeCode.Byte
            or TypeCode.SByte
            or TypeCode.Int16
            or TypeCode.Int32
            or TypeCode.Int64
            or TypeCode.UInt16
            or TypeCode.UInt32
            or TypeCode.UInt64
            or TypeCode.Single;
    }

    /// <summary>
    /// 是否为小数类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsDecimal(this Type type)
    {
        if (type == typeof(decimal)
            || type == typeof(double)
            || type == typeof(float))
        {
            return true;
        }

        // 获取类型代码
        var typeCode = Type.GetTypeCode(type);
        return typeCode == TypeCode.Double
            || typeCode == TypeCode.Decimal;
    }

    /// <summary>
    /// 是否为数值类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsNumeric(this Type type)
    {
        return type.IsInteger()
            || type.IsDecimal();
    }
}