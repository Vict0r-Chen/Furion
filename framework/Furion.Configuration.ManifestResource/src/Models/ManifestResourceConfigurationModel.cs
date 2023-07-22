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

namespace Furion.Configuration;

/// <summary>
/// 嵌入资源配置模型
/// </summary>
public sealed class ManifestResourceConfigurationModel
{
    /// <summary>
    /// <inheritdoc cref="ManifestResourceConfigurationModel"/>
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <param name="resourceName">嵌入资源名称</param>
    internal ManifestResourceConfigurationModel(Assembly assembly, string resourceName)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceName);

        Assembly = assembly;
        ResourceName = resourceName;

        Extension = Path.GetExtension(resourceName);
    }

    /// <summary>
    /// 程序集
    /// </summary>
    public Assembly Assembly { get; init; }

    /// <summary>
    /// 嵌入资源名称
    /// </summary>
    public string ResourceName { get; init; }

    /// <summary>
    /// 文件拓展名
    /// </summary>
    public string Extension { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>值越大则注册越晚</remarks>
    public int Order { get; set; }

    /// <summary>
    /// 配置前缀
    /// </summary>
    public string? Prefix { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return ResourceName;
    }
}