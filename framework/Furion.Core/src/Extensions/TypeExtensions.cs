// 麻省理工学院许可证
//
// 版权所有 © 2020-2023 百小僧，百签科技（广东）有限公司
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
    /// 检查类型是否是静态类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsStatic(this Type type)
    {
        return type is { IsSealed: true, IsAbstract: true };
    }

    /// <summary>
    /// 检查类型是否是匿名类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsAnonymous(this Type type)
    {
        // 检查是否贴有 [CompilerGenerated] 特性
        if (!type.IsDefined(typeof(CompilerGeneratedAttribute), false))
        {
            return false;
        }

        // 类型限定名是否以 <> 开头且以 AnonymousType 结尾
        return type.FullName is not null
            && type.FullName.StartsWith("<>")
            && type.FullName.Contains("AnonymousType");
    }

    /// <summary>
    /// 检查类型是否可实例化
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsInstantiable(this Type type)
    {
        return type is { IsClass: true, IsAbstract: false }
            && !type.IsStatic();
    }

    /// <summary>
    /// 检查类型是否派生自指定类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="fromType"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsAlienAssignableTo(this Type type, Type fromType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(fromType);

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
        return !type.IsDefined(typeof(TAttribute), inherit)
            ? null
            : type.GetCustomAttribute<TAttribute>(inherit);
    }

    /// <summary>
    /// 检查类型是否定义了公开无参构造函数
    /// </summary>
    /// <remarks>用于 <see cref="Activator.CreateInstance(Type)"/> 实例化</remarks>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool HasDefinePublicParameterlessConstructor(this Type type)
    {
        return type.IsInstantiable()
            && type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes) is not null;
    }

    /// <summary>
    /// 检查类型和指定类型定义是否相等
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="compareType"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsDefinitionEqual(this Type type, Type? compareType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(compareType);

        return type == compareType
            || (type.IsGenericType
                && compareType.IsGenericType
                && type.IsGenericTypeDefinition // 💡
                && type == compareType.GetGenericTypeDefinition());
    }

    /// <summary>
    /// 检查类型和指定继承类型是否兼容
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="inheritType"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsCompatibilityTo(this Type type, Type? inheritType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(inheritType);

        return inheritType != typeof(object)
            && inheritType.IsAssignableFrom(type)
            && (!type.IsGenericType
                || (type.IsGenericType
                    && inheritType.IsGenericType
                    && type.GetTypeInfo().GenericTypeParameters.SequenceEqual(inheritType.GenericTypeArguments)));
    }

    /// <summary>
    /// 检查类型是否定义了指定方法
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="name">方法名称</param>
    /// <param name="accessibilityBindingFlags">可访问性成员绑定标记</param>
    /// <param name="methodInfo"><see cref="MethodInfo"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsDeclarationMethod(this Type type
        , string name
        , BindingFlags accessibilityBindingFlags
        , out MethodInfo? methodInfo)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(type);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        methodInfo = type.GetMethod(name, accessibilityBindingFlags | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        return methodInfo is not null;
    }

    /// <summary>
    /// 检查类型是否是整数类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsInteger(this Type type)
    {
        // 如果是浮点类型则直接返回
        if (type.IsDecimal())
        {
            return false;
        }

        // 检查 TypeCode
        return Type.GetTypeCode(type) is TypeCode.Byte
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
    /// 检查类型是否是小数类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsDecimal(this Type type)
    {
        // 如果是浮点类型则直接返回
        if (type == typeof(decimal)
            || type == typeof(double)
            || type == typeof(float))
        {
            return true;
        }

        // 检查 TypeCode
        return Type.GetTypeCode(type) is TypeCode.Double or TypeCode.Decimal;
    }

    /// <summary>
    /// 检查类型是否是数值类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsNumeric(this Type type)
    {
        return type.IsInteger()
            || type.IsDecimal();
    }

    /// <summary>
    /// 创建属性设置器
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/></param>
    /// <returns><see cref="Action{T1, T2}"/></returns>
    internal static Action<object, object?> CreatePropertySetter(this Type type, PropertyInfo propertyInfo)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(propertyInfo);

        // 创建一个动态方法来设置属性值
        var setterMethod = new DynamicMethod(
            $"{type.FullName}_Set_{propertyInfo.Name}",
            null,
            new Type[] { typeof(object), typeof(object) },
            typeof(TypeExtensions).Module
        );

        // 获取动态方法的 ILGenerator，用于生成方法体指令
        var ilGenerator = setterMethod.GetILGenerator();

        // 获取属性的 set 方法
        var setMethod = propertyInfo.GetSetMethod(nonPublic: true);

        // 空检查
        ArgumentNullException.ThrowIfNull(setMethod);

        // 将第一个参数（即 obj）转换为实际的对象类型
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Castclass, type);

        // 将第二个参数（即 value）加载到堆栈上
        ilGenerator.Emit(OpCodes.Ldarg_1);

        // 如果属性类型是值类型，则执行拆箱操作（unbox.any）
        if (propertyInfo.PropertyType.IsValueType)
        {
            ilGenerator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
        }
        else // 否则，将值强制转换为属性类型
        {
            ilGenerator.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
        }

        // 调用属性的 set 方法
        ilGenerator.Emit(OpCodes.Callvirt, setMethod);

        // 返回
        ilGenerator.Emit(OpCodes.Ret);

        return (Action<object, object?>)setterMethod.CreateDelegate(typeof(Action<object, object>));
    }

    /// <summary>
    /// 创建字段设置器
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="fieldInfo"><see cref="FieldInfo"/></param>
    /// <returns><see cref="Action{T1, T2}"/></returns>
    internal static Action<object, object?> CreateFieldSetter(this Type type, FieldInfo fieldInfo)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(fieldInfo);

        // 创建一个动态方法来设置字段值
        var setterMethod = new DynamicMethod(
            $"{type.FullName}_Set_{fieldInfo.Name}",
            null,
            new Type[] { typeof(object), typeof(object) },
            typeof(TypeExtensions).Module
        );

        // 获取动态方法的 ILGenerator，用于生成方法体指令
        var ilGenerator = setterMethod.GetILGenerator();

        // 将第一个参数（即 obj）转换为实际的对象类型
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Castclass, type);

        // 将第二个参数（即 value）加载到堆栈上
        ilGenerator.Emit(OpCodes.Ldarg_1);

        // 如果字段类型是值类型，则执行拆箱操作（unbox.any）
        if (fieldInfo.FieldType.IsValueType)
        {
            ilGenerator.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
        }
        else // 否则，将值强制转换为字段类型
        {
            ilGenerator.Emit(OpCodes.Castclass, fieldInfo.FieldType);
        }

        // 将字段的值设置为堆栈上的值
        ilGenerator.Emit(OpCodes.Stfld, fieldInfo);

        // 返回
        ilGenerator.Emit(OpCodes.Ret);

        return (Action<object, object?>)setterMethod.CreateDelegate(typeof(Action<object, object>));
    }
}