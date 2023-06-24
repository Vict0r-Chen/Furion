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
/// 配置模块文件扫描器构建器
/// </summary>
public sealed class FileScannerConfigurationBuilder
{
    /// <summary>
    /// 待扫描的目录集合
    /// </summary>
    internal readonly HashSet<string> _directories;

    /// <summary>
    /// 文件通配符
    /// </summary>
    /// <remarks><see href="https://learn.microsoft.com/zh-cn/dotnet/core/extensions/file-globbing">文件通配符</see></remarks>
    internal readonly HashSet<string> _fileGlobbing;

    /// <summary>
    /// 文件黑名单通配符
    /// </summary>
    /// <remarks>禁止已扫描的文件名作为配置文件</remarks>
    internal readonly HashSet<string> _fileBlacklistGlobbing;

    /// <summary>
    /// 文件扫描过滤器
    /// </summary>
    internal Func<FileConfigurationModel, bool>? _filterConfigure;

    /// <summary>
    /// 构造函数
    /// </summary>
    public FileScannerConfigurationBuilder()
    {
        _directories = new()
        {
            AppContext.BaseDirectory
        };

        _fileGlobbing = new()
        {
            "*.json"
        };

        _fileBlacklistGlobbing = new()
        {
            "*.runtimeconfig.json",
            "*.runtimeconfig.*.json",
            "*.deps.json",
            "*.staticwebassets.*.json",
            "*.nuget.dgspec.json",
            "launchSettings.json",
            "tsconfig.json",
            "package.json",
            "project.assets.json",
            "manifest.json"
        };
    }

    /// <summary>
    /// 文件扫描最大深度
    /// </summary>
    public int MaxDepth { get; set; }

    /// <summary>
    /// 添加文件扫描过滤器
    /// </summary>
    /// <param name="configure"><see cref="Func{T, TResult}"/></param>
    public void AddFilter(Func<FileConfigurationModel, bool> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));

        _filterConfigure = configure;
    }

    /// <summary>
    /// 添加文件扫描的目录
    /// </summary>
    /// <param name="directories"><see cref="string"/>[]</param>
    /// <returns><see cref="FileScannerConfigurationBuilder"/></returns>
    public FileScannerConfigurationBuilder AddDirectories(params string?[] directories)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(directories, nameof(directories));

        Array.ForEach(directories, directory =>
        {
            // 空检查
            if (string.IsNullOrWhiteSpace(directory))
            {
                return;
            }

            _directories.Add(directory);
        });

        return this;
    }

    /// <summary>
    /// 添加文件扫描的目录
    /// </summary>
    /// <param name="directories"><see cref="IEnumerable{T}"/></param>
    /// <returns><see cref="FileScannerConfigurationBuilder"/></returns>
    public FileScannerConfigurationBuilder AddDirectories(IEnumerable<string> directories)
    {
        return AddDirectories(directories.ToArray());
    }

    /// <summary>
    /// 添加文件通配符
    /// </summary>
    /// <param name="globbings"><see cref="string"/>[]</param>
    /// <returns><see cref="FileScannerConfigurationBuilder"/></returns>
    public FileScannerConfigurationBuilder AddGlobbings(params string[] globbings)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(globbings, nameof(globbings));

        Array.ForEach(globbings, globbing => _fileGlobbing.Add(globbing));

        return this;
    }

    /// <summary>
    /// 添加文件通配符
    /// </summary>
    /// <param name="globbings"><see cref="IEnumerable{T}"/></param>
    /// <returns><see cref="FileScannerConfigurationBuilder"/></returns>
    public FileScannerConfigurationBuilder AddGlobbings(IEnumerable<string> globbings)
    {
        return AddGlobbings(globbings.ToArray());
    }

    /// <summary>
    /// 构建模块服务
    /// </summary>
    /// <param name="builder"><see cref="IConfigurationBuilder"/></param>
    internal void Build(IConfigurationBuilder builder)
    {
        // 获取配置根结构
        var configurationRoot = builder is ConfigurationManager configurationManager
            ? configurationManager
            : builder.Build();

        // 添加内容目录扫描
        var contentRoot = configurationRoot["CONTENTROOT"];
        AddDirectories(contentRoot);

        // 扫描所有配置文件目录
        var files = ScanDirectories();

        // 获取环境的名称
        var environmentName = configurationRoot["ENVIRONMENT"];

        _directories.Clear();
        _fileGlobbing.Clear();
        _fileBlacklistGlobbing.Clear();
    }

    /// <summary>
    /// 扫描所有配置文件目录
    /// </summary>
    /// <returns><see cref="HashSet{T}"/></returns>
    internal HashSet<string> ScanDirectories()
    {
        // 初始化文件通配符匹配对象
        var matcher = new Matcher();
        matcher.AddIncludePatterns(_fileGlobbing);
        matcher.AddExcludePatterns(_fileBlacklistGlobbing);

        // 扫描目录配置文件
        var files = new HashSet<string>();
        foreach (var directory in _directories)
        {
            var matchFiles = ScanDirectory(directory, MaxDepth, matcher);
            matchFiles.ForEach(file => files.Add(file));
        }

        return files;
    }

    /// <summary>
    /// 执行目录扫描，返回符合匹配条件的文件路径列表
    /// </summary>
    /// <param name="folderPath">要扫描的目录路径</param>
    /// <param name="maxDepth">文件扫描的最大深度</param>
    /// <param name="matcher">可选的文件通配符匹配对象</param>
    /// <returns><see cref="List{T}"/></returns>
    internal static List<string> ScanDirectory(string folderPath, int maxDepth, Matcher? matcher = null)
    {
        // 创建一个空的文件列表和一个元组类型的栈，压入初始目录和深度值
        var files = new List<string>();
        var stack = new Stack<(string folderPath, int depth)>();
        stack.Push((folderPath, 0));

        // 循环取出栈内元素，直到栈为空
        while (stack.Count > 0)
        {
            // 取出栈顶元素表示当前要扫描的目录路径和深度值
            var (currentFolderPath, currentDepth) = stack.Pop();

            // 查找当前目录下所有匹配的文件
            var matchFiles = from file in Directory.EnumerateFiles(currentFolderPath)
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
            if (currentDepth >= maxDepth)
            {
                continue;
            }

            // 将当前目录下的子目录入栈，设置深度值加 1
            foreach (var subFolderPath in Directory.GetDirectories(currentFolderPath))
            {
                stack.Push((subFolderPath, currentDepth + 1));
            }
        }

        // 返回符合匹配条件的文件列表
        return files;
    }
}