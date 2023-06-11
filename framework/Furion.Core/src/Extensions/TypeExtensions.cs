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
/// <see cref="Type"/> 类型拓展
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// 判断类型是否是静态类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see langword="true"/> 标识为静态类型；<see langword="false"/> 标识为非静态类</returns>
    internal static bool IsStatic(this Type type)
    {
        return type.IsSealed && type.IsAbstract;
    }

    /// <summary>
    /// 判断类型是否是匿名类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see langword="true"/> 标识为匿名类型；<see langword="false"/> 标识为非匿名类型</returns>
    internal static bool IsAnonymousType(this Type type)
    {
        // 首先需要判断类型是否定义了 CompilerGeneratedAttribute 特性，因为匿名类型是通过编译器生成的，并且都会加上这个特性
        if (type.IsDefined(typeof(CompilerGeneratedAttribute), false))
        {
            // 接下来判断类型名称是否是以 "<>" 开头，以 Encoding 类型结尾，这是匿名类型名称的一般规则
            if (type.FullName != null && type.FullName.StartsWith("<>") && type.FullName.Contains("AnonymousType"))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 判断类型是否可实例化
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see langword="true"/> 标识可实例化；<see langword="false"/> 标识不可实例化</returns>
    internal static bool IsInstantiatedType(this Type type)
    {
        return !type.IsAbstract && !type.IsStatic() && type.IsClass;
    }

    /// <summary>
    /// 判断类型是否可实例化且派生自特定类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="derivedType">派生类型</param>
    /// <returns><see langword="true"/> 标识可实例化且派生特定类型；<see langword="false"/> 标识不能实例化或不派生特定类型</returns>
    internal static bool IsInstantiatedTypeWithAssignableFrom(this Type type, Type derivedType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(derivedType, nameof(derivedType));

        return type.IsInstantiatedType() && derivedType != type && derivedType.IsAssignableFrom(type);
    }

    /// <summary>
    /// 获取类型指定的特性
    /// </summary>
    /// <typeparam name="TAttribute">特性类型</typeparam>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="inherit">是否支持特性继承查找</param>
    /// <returns><typeparamref name="TAttribute"/></returns>
    internal static TAttribute? GetCustomAttributeIfIsDefined<TAttribute>(this Type type, bool inherit = false)
        where TAttribute : Attribute
    {
        // 如果未定义则直接返回
        if (!type.IsDefined(typeof(TAttribute), inherit))
        {
            return null;
        }

        return type.GetCustomAttribute<TAttribute>(inherit);
    }

    /// <summary>
    /// 获取公开的实例方法
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="name">方法名称</param>
    /// <returns><see cref="MethodInfo"/></returns>
    internal static MethodInfo? GetPublicInstanceMethod(this Type type, string name)
    {
        var methodInfo = type.GetMethod(name, BindingFlags.Public | BindingFlags.Instance);
        return methodInfo;
    }

    /// <summary>
    /// 查找所有的父类类型且包含自己
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="List{T}"/></returns>
    internal static List<Type> GetParentAndSelfTypes(this Type type)
    {
        var parentTypes = new List<Type>();
        var parentType = type.BaseType;

        // 递归查找所有的父类
        while (parentType is not null && parentType != typeof(object))
        {
            // 插入起始位置
            parentTypes.Insert(0, parentType);
            parentType = parentType.BaseType;
        }

        // 添加自身类型
        parentTypes.Add(type);

        return parentTypes;
    }
}