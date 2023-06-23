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
    /// 构造函数，用于初始化文件目录扫描器
    /// </summary>
    /// <param name="folderPath">要扫描的目录路径</param>
    /// <param name="maxDepth">文件扫描的最大深度</param>
    internal FolderScanner(string folderPath, int maxDepth)
    {
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
    /// 执行目录扫描，返回符合匹配条件的文件路径列表
    /// </summary>
    /// <param name="matcher">可选的文件通配符匹配对象</param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<string> ScanFolder(Matcher? matcher = null)
    {
        // 创建一个空的文件列表和一个元组类型的栈，压入初始目录和深度值
        var files = new List<string>();
        var stack = new Stack<(string folderPath, int depth)>();
        stack.Push((FolderPath, 0));

        // 循环取出栈内元素，直到栈为空
        while (stack.Count > 0)
        {
            // 取出栈顶元素表示当前要扫描的目录路径和深度值
            var (folderPath, currentDepth) = stack.Pop();

            // 查找当前目录下所有匹配的文件
            var matchFiles = from file in Directory.EnumerateFiles(folderPath)
                             let fileName = Path.GetFileName(file)
                             let isXml = fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)
                             let dllFileExists = isXml && (File.Exists(Path.ChangeExtension(file, ".dll"))
                                                            || File.Exists(Path.ChangeExtension(file, ".pdb")))
                             where (matcher is null || matcher.Match(fileName).HasMatches)
                                    && (!isXml || !dllFileExists)
                             select file;

            // 将匹配的文件添加到列表中
            files.AddRange(matchFiles);

            // 如果已经达到最大深度则跳过当前目录的子目录
            if (currentDepth >= MaxDepth)
            {
                continue;
            }

            // 将当前目录下的子目录入栈，设置深度值加 1
            foreach (string subFolderPath in Directory.GetDirectories(folderPath))
            {
                stack.Push((subFolderPath, currentDepth + 1));
            }
        }

        // 返回符合匹配条件的文件列表
        return files;
    }
}