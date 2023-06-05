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
/// 服务注册特性配置
/// </summary>
/// <remarks>用于扫描程序集添加服务时配置</remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class ServiceInjectionAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public ServiceInjectionAttribute()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="addition">服务注册方式</param>
    public ServiceInjectionAttribute(ServiceAddition addition)
    {
        Addition = addition;
    }

    /// <inheritdoc cref="ServiceAddition"/>
    public ServiceAddition Addition { get; init; } = ServiceAddition.Default;

    /// <summary>
    /// 忽略注册
    /// </summary>
    public bool Ignore { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>数值越大越靠后注册</remarks>
    public int Order { get; init; }

    /// <summary>
    /// 将自身作为服务
    /// </summary>
    public bool IncludingSelf { get; init; }

    /// <summary>
    /// 将基类作为自身服务
    /// </summary>
    public bool IncludingBase { get; init; }

    /// <summary>
    /// 禁用指定派生类型作为服务注册
    /// </summary>
    public Type[]? SuppressDerivedTypes { get; init; }
}