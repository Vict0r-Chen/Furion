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

namespace Furion.Kit;

/// <summary>
/// 组件模型
/// </summary>
internal sealed class ComponentModel
{
    /// <summary>
    /// <inheritdoc cref="ComponentModel"/>
    /// </summary>
    /// <param name="componentType">组件类型</param>
    internal ComponentModel(Type componentType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentType);

        // 获取组件所在程序集
        var assembly = componentType.Assembly;

        Name = componentType.Name;
        FullName = componentType.FullName;
        AssemblyName = assembly.GetName().Name;
        AssemblyDescription = assembly.GetDescription();
        AssemblyVersion = assembly.GetVersion()?.ToString();
        Guid = Guid.NewGuid();
    }

    /// <summary>
    /// 类型名称
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    /// 类型完全限定名
    /// </summary>
    public string? FullName { get; private set; }

    /// <summary>
    /// 程序集名称
    /// </summary>
    public string? AssemblyName { get; private set; }

    /// <summary>
    /// 程序集描述
    /// </summary>
    public string? AssemblyDescription { get; private set; }

    /// <summary>
    /// 程序集版本号
    /// </summary>
    public string? AssemblyVersion { get; private set; }

    /// <summary>
    /// 唯一标识
    /// </summary>
    public Guid Guid { get; private set; }

    /// <summary>
    /// 依赖组件集合
    /// </summary>
    public List<ComponentModel>? Dependencies { get; internal set; }
}