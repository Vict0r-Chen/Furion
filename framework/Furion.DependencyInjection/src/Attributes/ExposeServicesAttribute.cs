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
/// 服务导出配置
/// </summary>
/// <remarks>
/// <para>作用于程序集扫描，若类型配置了该特性则将指定的类型作为服务。</para>
/// <para>此配置优先级大于基类型和 <seealso cref="ServiceInjectionAttribute.IncludeSelf"/> 和 <seealso cref="ServiceInjectionAttribute.IncludeBase"/>。</para>
/// <para>配置的类型集合须包含在基类链类型集合中</para>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class ExposeServicesAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceTypes"><see cref="ServiceTypes"/></param>
    public ExposeServicesAttribute(params Type[] serviceTypes)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(serviceTypes, nameof(serviceTypes));

        ServiceTypes = serviceTypes;
    }

    /// <summary>
    /// 类型集合
    /// </summary>
    public Type[] ServiceTypes { get; init; }
}