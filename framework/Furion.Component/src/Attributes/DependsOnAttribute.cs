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

namespace Furion.Component;

/// <summary>
/// 组件依赖配置
/// </summary>
/// <remarks>作用域派生自 <see cref="ComponentBase"/> 的类型</remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class DependsOnAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
    {
        DependedTypes = Array.Empty<Type>();
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dependedTypes">依赖类型集合</param>
    public DependsOnAttribute(params Type[] dependedTypes)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(dependedTypes, nameof(dependedTypes));

        DependedTypes = dependedTypes;
    }

    /// <summary>
    /// 依赖类型集合
    /// </summary>
    public Type[] DependedTypes { get; init; }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent> : DependsOnAttribute
    where TComponent : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent6"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
    where TComponent6 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5), typeof(TComponent6))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent6"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent7"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
    where TComponent6 : ComponentBase, new()
    where TComponent7 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5), typeof(TComponent6), typeof(TComponent7))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent6"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent7"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent8"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
    where TComponent6 : ComponentBase, new()
    where TComponent7 : ComponentBase, new()
    where TComponent8 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5), typeof(TComponent6), typeof(TComponent7), typeof(TComponent8))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent6"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent7"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent8"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent9"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8, TComponent9> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
    where TComponent6 : ComponentBase, new()
    where TComponent7 : ComponentBase, new()
    where TComponent8 : ComponentBase, new()
    where TComponent9 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5), typeof(TComponent6), typeof(TComponent7), typeof(TComponent8), typeof(TComponent9))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent6"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent7"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent8"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent9"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent10"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8, TComponent9, TComponent10> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
    where TComponent6 : ComponentBase, new()
    where TComponent7 : ComponentBase, new()
    where TComponent8 : ComponentBase, new()
    where TComponent9 : ComponentBase, new()
    where TComponent10 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5), typeof(TComponent6), typeof(TComponent7), typeof(TComponent8), typeof(TComponent9), typeof(TComponent10))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent6"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent7"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent8"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent9"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent10"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent11"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8, TComponent9, TComponent10, TComponent11> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
    where TComponent6 : ComponentBase, new()
    where TComponent7 : ComponentBase, new()
    where TComponent8 : ComponentBase, new()
    where TComponent9 : ComponentBase, new()
    where TComponent10 : ComponentBase, new()
    where TComponent11 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5), typeof(TComponent6), typeof(TComponent7), typeof(TComponent8), typeof(TComponent9), typeof(TComponent10), typeof(TComponent11))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent6"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent7"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent8"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent9"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent10"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent11"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent12"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8, TComponent9, TComponent10, TComponent11, TComponent12> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
    where TComponent6 : ComponentBase, new()
    where TComponent7 : ComponentBase, new()
    where TComponent8 : ComponentBase, new()
    where TComponent9 : ComponentBase, new()
    where TComponent10 : ComponentBase, new()
    where TComponent11 : ComponentBase, new()
    where TComponent12 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5), typeof(TComponent6), typeof(TComponent7), typeof(TComponent8), typeof(TComponent9), typeof(TComponent10), typeof(TComponent11), typeof(TComponent12))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent6"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent7"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent8"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent9"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent10"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent11"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent12"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent13"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8, TComponent9, TComponent10, TComponent11, TComponent12, TComponent13> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
    where TComponent6 : ComponentBase, new()
    where TComponent7 : ComponentBase, new()
    where TComponent8 : ComponentBase, new()
    where TComponent9 : ComponentBase, new()
    where TComponent10 : ComponentBase, new()
    where TComponent11 : ComponentBase, new()
    where TComponent12 : ComponentBase, new()
    where TComponent13 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5), typeof(TComponent6), typeof(TComponent7), typeof(TComponent8), typeof(TComponent9), typeof(TComponent10), typeof(TComponent11), typeof(TComponent12), typeof(TComponent13))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent6"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent7"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent8"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent9"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent10"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent11"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent12"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent13"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent14"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8, TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
    where TComponent6 : ComponentBase, new()
    where TComponent7 : ComponentBase, new()
    where TComponent8 : ComponentBase, new()
    where TComponent9 : ComponentBase, new()
    where TComponent10 : ComponentBase, new()
    where TComponent11 : ComponentBase, new()
    where TComponent12 : ComponentBase, new()
    where TComponent13 : ComponentBase, new()
    where TComponent14 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5), typeof(TComponent6), typeof(TComponent7), typeof(TComponent8), typeof(TComponent9), typeof(TComponent10), typeof(TComponent11), typeof(TComponent12), typeof(TComponent13), typeof(TComponent14))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent6"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent7"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent8"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent9"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent10"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent11"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent12"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent13"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent14"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent15"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8, TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
    where TComponent6 : ComponentBase, new()
    where TComponent7 : ComponentBase, new()
    where TComponent8 : ComponentBase, new()
    where TComponent9 : ComponentBase, new()
    where TComponent10 : ComponentBase, new()
    where TComponent11 : ComponentBase, new()
    where TComponent12 : ComponentBase, new()
    where TComponent13 : ComponentBase, new()
    where TComponent14 : ComponentBase, new()
    where TComponent15 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5), typeof(TComponent6), typeof(TComponent7), typeof(TComponent8), typeof(TComponent9), typeof(TComponent10), typeof(TComponent11), typeof(TComponent12), typeof(TComponent13), typeof(TComponent14), typeof(TComponent15))
    {
    }
}

/// <summary>
/// 组件依赖配置
/// </summary>
/// <typeparam name="TComponent1"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent2"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent3"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent4"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent5"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent6"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent7"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent8"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent9"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent10"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent11"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent12"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent13"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent14"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent15"><see cref="ComponentBase"/></typeparam>
/// <typeparam name="TComponent16"><see cref="ComponentBase"/></typeparam>
public sealed class DependsOnAttribute<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8, TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15, TComponent16> : DependsOnAttribute
    where TComponent1 : ComponentBase, new()
    where TComponent2 : ComponentBase, new()
    where TComponent3 : ComponentBase, new()
    where TComponent4 : ComponentBase, new()
    where TComponent5 : ComponentBase, new()
    where TComponent6 : ComponentBase, new()
    where TComponent7 : ComponentBase, new()
    where TComponent8 : ComponentBase, new()
    where TComponent9 : ComponentBase, new()
    where TComponent10 : ComponentBase, new()
    where TComponent11 : ComponentBase, new()
    where TComponent12 : ComponentBase, new()
    where TComponent13 : ComponentBase, new()
    where TComponent14 : ComponentBase, new()
    where TComponent15 : ComponentBase, new()
    where TComponent16 : ComponentBase, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DependsOnAttribute()
        : base(typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4), typeof(TComponent5), typeof(TComponent6), typeof(TComponent7), typeof(TComponent8), typeof(TComponent9), typeof(TComponent10), typeof(TComponent11), typeof(TComponent12), typeof(TComponent13), typeof(TComponent14), typeof(TComponent15), typeof(TComponent16))
    {
    }
}