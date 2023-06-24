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
/// 文件配置模型
/// </summary>
/// <remarks>作用于文件目录扫描</remarks>
public sealed class FileConfigurationModel
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="path">文件绝对路径</param>
    internal FileConfigurationModel(string path)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

        Path = path;
        Extension = System.IO.Path.GetExtension(path);
        FileName = System.IO.Path.GetFileName(path);
        Directory = System.IO.Path.GetDirectoryName(path)!;
        Group = ResolveGroup();
    }

    /// <summary>
    /// 文件绝对路径
    /// </summary>
    public string Path { get; init; }

    /// <summary>
    /// 拓展名
    /// </summary>
    public string Extension { get; init; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; init; }

    /// <summary>
    /// 文件目录
    /// </summary>
    public string Directory { get; init; }

    /// <summary>
    /// 分组名
    /// </summary>
    public string Group { get; init; }

    /// <summary>
    /// 文件可选配置
    /// </summary>
    public bool Optional { get; set; } = true;

    /// <summary>
    /// 文件变更时刷新
    /// </summary>
    public bool ReloadOnChang { get; set; } = true;

    /// <summary>
    /// 文件变更时刷新前等待毫秒数
    /// </summary>
    public int ReloadDelay { get; set; } = 250;

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>值越大则越后添加</remarks>
    public int Order { get; set; }

    /// <summary>
    /// 解析分组
    /// </summary>
    /// <returns></returns>
    internal string ResolveGroup()
    {
        return FileName.StartsWith('(') && FileName.Contains(')')
                ? FileName[FileName.IndexOf("(")..(FileName.IndexOf(")") + 1)]
                : FileName[..FileName.IndexOf(".")];
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"[{Group}] FileName: {FileName} Path: {Path}";
    }
}