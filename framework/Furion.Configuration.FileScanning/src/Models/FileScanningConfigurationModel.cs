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

namespace Furion.Configuration;

/// <summary>
/// 文件扫描配置模型
/// </summary>
public sealed class FileScanningConfigurationModel
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="environmentFlag">环境标识</param>
    internal FileScanningConfigurationModel(string filePath, bool environmentFlag)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        FilePath = Path.GetFullPath(filePath);

        Extension = Path.GetExtension(FilePath);
        FileName = Path.GetFileName(FilePath);
        Group = ResolveGroup(FilePath);
        DirectoryName = Path.GetDirectoryName(FilePath) ?? string.Empty;

        EnvironmentFlag = environmentFlag;
    }

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; init; }

    /// <summary>
    /// 文件拓展名
    /// </summary>
    public string Extension { get; init; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; init; }

    /// <summary>
    /// 文件目录名
    /// </summary>
    public string DirectoryName { get; init; }

    /// <summary>
    /// 分组名
    /// </summary>
    public string Group { get; init; }

    /// <summary>
    /// 文件可选配置
    /// </summary>
    public bool Optional { get; set; }

    /// <summary>
    /// 文件变更时刷新配置
    /// </summary>
    public bool ReloadOnChange { get; set; }

    /// <summary>
    /// 文件变更延迟刷新毫秒数配置
    /// </summary>
    public int ReloadDelay { get; set; } = 250;

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>值越大则越后添加</remarks>
    public int Order { get; set; }

    /// <summary>
    /// 环境标识
    /// </summary>
    /// <remarks>内部维护属性，作用于 <see cref="FileScanningConfigurationBuilder.AllowEnvironmentSwitching"/></remarks>
    internal bool EnvironmentFlag { get; init; }

    /// <summary>
    /// 检查文件路径是否和模型文件路径匹配
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns><see cref="bool"/></returns>
    internal bool IsMatch(string filePath)
    {
        return Path.GetFullPath(FilePath)
            .Equals(Path.GetFullPath(filePath), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 解析文件分组
    /// </summary>
    /// <remarks>若文件名包含 () 则取其之间内容作为分组名，否则取第一个 . 前面的字符作为分组名</remarks>
    /// <param name="filePath">文件路径</param>
    /// <returns><see cref="string"/></returns>
    internal static string ResolveGroup(string filePath)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        // 获取文件名
        var fileName = Path.GetFileName(filePath);

        // 检查文件拓展名
        if (string.IsNullOrWhiteSpace(Path.GetExtension(fileName)))
        {
            return string.Empty;
        }

        return fileName.StartsWith('(') && fileName.Contains(')')
            ? fileName[fileName.IndexOf("(")..(fileName.IndexOf(")") + 1)]
            : fileName[..fileName.IndexOf(".")];
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Group = {Group}, FileName = {FileName}, Path = {FilePath}";
    }
}