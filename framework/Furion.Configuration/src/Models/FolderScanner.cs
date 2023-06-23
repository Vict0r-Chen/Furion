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
/// 文件目录扫描器类
/// </summary>
/// <remarks>作用于扫描指定目录下符合匹配条件的文件</remarks>
internal sealed class FolderScanner
{
    /// <summary>
    /// 文件名匹配对象
    /// </summary>
    internal readonly Matcher _matcher;

    /// <summary>
    /// 文件黑名单匹配对象
    /// </summary>
    internal readonly Matcher _blacklistMatcher;

    /// <summary>
    /// 构造函数，用于初始化文件目录扫描器
    /// </summary>
    /// <param name="folderPath">要扫描的目录路径</param>
    /// <param name="maxDepth">文件扫描的最大深度</param>
    internal FolderScanner(string folderPath, int maxDepth)
    {
        _matcher = new();
        _blacklistMatcher = new();

        FolderPath = folderPath;
        MaxDepth = maxDepth;
    }

    /// <summary>
    /// 文件目录路径
    /// </summary>
    internal string FolderPath { get; }

    /// <summary>
    /// 文件扫描最大深度
    /// </summary>
    internal int MaxDepth { get; }

    /// <summary>
    /// 文件通配符
    /// </summary>
    internal string[] FileGlobbing { get; set; } = new[] { "*.*" };

    /// <summary>
    /// 文件黑名单通配符
    /// </summary>
    internal string[] FileBlacklistGlobbing { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 执行目录扫描
    /// </summary>
    /// <remarks>扫描符合匹配条件的文件路径列表</remarks>
    /// <returns><see cref="List{T}"/></returns>
    internal List<string> ScanFolder()
    {
        // 添加文件名通配符和黑名单匹配规则
        _matcher.AddIncludePatterns(FileGlobbing);
        _blacklistMatcher.AddIncludePatterns(FileBlacklistGlobbing);

        // 开始执行目录扫描
        var files = new List<string>();
        ScanFolderHelper(files, FolderPath, 0);

        return files;
    }

    /// <summary>
    /// 扫描目录辅助方法，用于递归查找符合匹配条件的文件
    /// </summary>
    /// <param name="files">符合匹配条件的文件路径列表</param>
    /// <param name="folderPath">要扫描的目录路径</param>
    /// <param name="currentDepth">当前扫描深度</param>
    private void ScanFolderHelper(List<string> files, string folderPath, int currentDepth)
    {
        // 查找所有匹配的文件
        var matchFiles = from file in Directory.EnumerateFiles(folderPath)
                         let fileName = Path.GetFileName(file)
                         let isXml = fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)
                         let dllFileExists = isXml && (File.Exists(Path.ChangeExtension(file, ".dll"))
                                                        || File.Exists(Path.ChangeExtension(file, ".pdb")))
                         where _matcher.Match(fileName).HasMatches
                                && !_blacklistMatcher.Match(fileName).HasMatches
                                && (!isXml || !dllFileExists)
                         select file;

        files.AddRange(matchFiles);

        // 如果深度大于或等于当前深度则停止扫描
        if (currentDepth >= MaxDepth)
        {
            return;
        }

        // 递归查找子目录
        foreach (string subFolderPath in Directory.GetDirectories(folderPath))
        {
            ScanFolderHelper(files, subFolderPath, currentDepth + 1);
        }
    }
}