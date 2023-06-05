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
/// 服务添加方式
/// </summary>
/// <remarks>用于扫描程序集添加服务时配置</remarks>
public enum ServiceAddition : uint
{
    /// <summary>
    /// 添加
    /// </summary>
    /// <remarks>无论服务是否注册</remarks>
    Add = 0,

    /// <summary>
    /// 尝试添加
    /// </summary>
    /// <remarks>如果服务不存在则添加，否则不添加</remarks>
    TryAdd,

    /// <summary>
    /// 尝试添加
    /// </summary>
    /// <remarks>
    /// 如果服务和实现同时不存在则添加，否则不添加
    /// <para>注意：服务类型和实现类型不能相同</para>
    /// </remarks>
    TryAddEnumerable,

    /// <summary>
    /// 替换
    /// </summary>
    /// <remarks>如果服务存在则替换</remarks>
    Replace,

    /// <summary>
    /// 缺省值，同 <see cref="Add"/>
    /// </summary>
    Default = Add
}