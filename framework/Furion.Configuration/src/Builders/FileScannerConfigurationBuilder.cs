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
public sealed partial class FileScannerConfigurationBuilder
{
    /// <summary>
    /// 待扫描的目录集合
    /// </summary>
    internal readonly HashSet<string> _directories;

    /// <summary>
    /// 文件通配符
    /// </summary>
    internal readonly HashSet<string> _fileGlobbing;

    /// <summary>
    /// 文件黑名单通配符
    /// </summary>
    /// <remarks>禁止已扫描的文件名作为配置文件</remarks>
    internal readonly HashSet<string> _fileBlacklistGlobbing;

    /// <summary>
    /// 文件配置程序集合
    /// </summary>
    internal readonly Dictionary<string, Type> _fileConfigurationSources;

    /// <summary>
    /// 文件配置模型过滤器
    /// </summary>
    internal Func<FileConfigurationModel, bool>? _filterConfigure;

    /// <summary>
    /// 构造函数
    /// </summary>
    public FileScannerConfigurationBuilder()
    {
        _directories = new(StringComparer.OrdinalIgnoreCase);

        _fileGlobbing = new(StringComparer.OrdinalIgnoreCase)
        {
            "*.json"
        };

        _fileBlacklistGlobbing = new(StringComparer.OrdinalIgnoreCase)
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

        _fileConfigurationSources = new(StringComparer.OrdinalIgnoreCase)
        {
            {".json", typeof(JsonConfigurationSource) },
            {".xml", typeof(XmlConfigurationSource) },
            {".ini", typeof(IniConfigurationSource) }
        };
    }

    /// <summary>
    /// 文件扫描最大深度
    /// </summary>
    public uint MaxDepth { get; set; }

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
            ArgumentException.ThrowIfNullOrWhiteSpace(directory, nameof(directory));

            // 检查绝对路径
            if (!Path.IsPathRooted(directory))
            {
                throw new ArgumentException($"The path `{directory}` is not an absolute path.", nameof(directory));
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

        Array.ForEach(globbings, globbing =>
        {
            // 空检查
            ArgumentException.ThrowIfNullOrWhiteSpace(globbing, nameof(globbing));

            _fileGlobbing.Add(globbing);
        });

        return this;
    }

    /// <summary>
    /// 添加配置文件处理程序
    /// </summary>
    /// <typeparam name="TConfigurationSource"><see cref="FileConfigurationSource"/></typeparam>
    /// <param name="extension">文件拓展名</param>
    /// <returns><see cref="FileScannerConfigurationBuilder"/></returns>
    public FileScannerConfigurationBuilder AddConfigurationSources<TConfigurationSource>(string extension)
        where TConfigurationSource : FileConfigurationSource
    {
        return AddConfigurationSources(extension, typeof(TConfigurationSource));
    }

    /// <summary>
    /// 添加配置文件处理程序
    /// </summary>
    /// <param name="extension">文件拓展名</param>
    /// <param name="configurationSourceType"><see cref="FileConfigurationSource"/></param>
    /// <returns><see cref="FileScannerConfigurationBuilder"/></returns>
    public FileScannerConfigurationBuilder AddConfigurationSources(string extension, Type configurationSourceType)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(extension, nameof(extension));
        ArgumentNullException.ThrowIfNull(configurationSourceType, nameof(configurationSourceType));

        // 检查拓展名有效性
        if (!FileExtensionRegex().IsMatch(extension))
        {
            throw new ArgumentException($"`{extension}` is not a valid file extension.", nameof(extension));
        }

        // 检查配置文件处理程序类型有效性
        if (!typeof(FileConfigurationSource).IsAssignableFrom(configurationSourceType))
        {
            throw new ArgumentException($"`{configurationSourceType.Name}` type is not assignable from `{typeof(FileConfigurationSource).Name}`.", nameof(configurationSourceType));
        }

        // 添加或更新配置文件处理程序
        _fileConfigurationSources[extension] = configurationSourceType;

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
        AddDirectories(contentRoot ?? AppContext.BaseDirectory, AppContext.BaseDirectory);

        // 获取环境名称
        var environmentName = configurationRoot["ENVIRONMENT"];

        // 扫描所有配置文件目录，根据拓展名、文件目录、文件名排序
        var files = ScanDirectories(builder, contentRoot)
                                                                                                           .OrderBy(f => f.Order)
                                                                                                           .GroupBy(f => new { f.Extension, f.DirectoryName, f.Group });

        // 遍历分组并添加配置文件
        foreach (var fileGroup in files)
        {
            var filesInGroup = fileGroup.ToList();
            var groupPath = Path.Combine(fileGroup.Key.DirectoryName, fileGroup.Key.Group);

            // 添加基础配置文件
            var baseFileModel = filesInGroup.Find(f => f.FilePath.Equals(groupPath + fileGroup.Key.Extension));
            AddFileConfigurationSource(builder, baseFileModel);

            // 添加基于环境配置文件
            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                var envFileModel = filesInGroup.Find(f => f.FilePath.Equals(groupPath + "." + environmentName + fileGroup.Key.Extension));
                AddFileConfigurationSource(builder, envFileModel);
            }
        }

        // 释放对象
        Release();
    }

    /// <summary>
    /// 扫描所有配置文件目录
    /// </summary>
    /// <param name="builder"><see cref="IConfigurationBuilder"/></param>
    /// <param name="conentRoot">应用程序内容目录</param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal IEnumerable<FileConfigurationModel> ScanDirectories(IConfigurationBuilder builder, string? conentRoot = null)
    {
        // 查找所有配置的文件配置提供程序
        var filesExists = builder.Sources.OfType<FileConfigurationSource>()
                                                       .Select(s => (s.Path, s.FileProvider as PhysicalFileProvider))
                                                       .OfType<(string Path, PhysicalFileProvider Provider)>()
                                                       .SelectMany(t => GetFilePublishPaths(Path.Combine(t.Provider.Root, t.Path), conentRoot))
                                                       .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // 初始化文件通配符匹配对象
        var matcher = new Matcher();
        matcher.AddIncludePatterns(_fileGlobbing);
        matcher.AddExcludePatterns(_fileBlacklistGlobbing);

        // 扫描目录配置文件
        var files = _directories.SelectMany(directory => ScanDirectory(directory, MaxDepth, matcher))
                                                 .Distinct(StringComparer.OrdinalIgnoreCase)
                                                 .Except(filesExists, StringComparer.OrdinalIgnoreCase);

        // 用于存储发布后的文件路径
        var filePublishPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // 遍历所有配置文件
        foreach (var file in files)
        {
            // 检查文件路径是否已添加进集合中
            var filePublishFile = GetFilePublishPaths(file, conentRoot)[^1];
            if (!filePublishPaths.Add(filePublishFile))
            {
                continue;
            }

            // 创建文件配置模型
            var fileConfigurationModel = new FileConfigurationModel(file);

            // 调用文件配置模型过滤器
            if (_filterConfigure is null || _filterConfigure.Invoke(fileConfigurationModel))
            {
                yield return fileConfigurationModel;
            }
        }

        filePublishPaths.Clear();
    }

    /// <summary>
    /// 添加文件配置提供程序
    /// </summary>
    /// <param name="builder"><see cref="IConfigurationBuilder"/></param>
    /// <param name="model"><see cref="FileConfigurationModel"/></param>
    /// <exception cref="InvalidOperationException"></exception>
    internal void AddFileConfigurationSource(IConfigurationBuilder builder, FileConfigurationModel? model)
    {
        // 空检查
        if (model is null)
        {
            return;
        }

        // 拓展配置提供程序检查
        if (!_fileConfigurationSources.TryGetValue(model.Extension, out var fileConfigurationSourceType))
        {
            throw new InvalidOperationException($"Configuration provider for `{model.Extension}` extension not found.");
        }

        // 创建文件配置源
        var fileConfigurationSource = Activator.CreateInstance(fileConfigurationSourceType) as FileConfigurationSource;

        // 空检查
        ArgumentNullException.ThrowIfNull(fileConfigurationSource, nameof(fileConfigurationSourceType));

        // 初始化
        fileConfigurationSource.FileProvider = null;
        fileConfigurationSource.Path = model.FilePath;
        fileConfigurationSource.Optional = model.Optional;
        fileConfigurationSource.ReloadOnChange = model.ReloadOnChange;
        fileConfigurationSource.ReloadDelay = model.ReloadDelay;
        fileConfigurationSource.ResolveFileProvider();

        // 添加到配置构建器中
        builder.Add(fileConfigurationSource);

        // 输出调试事件
        Debugging.File("The file `{0}` has been successfully added to the configuration.", model.FilePath);
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    internal void Release()
    {
        _directories.Clear();
        _fileGlobbing.Clear();
        _fileBlacklistGlobbing.Clear();
        _fileConfigurationSources.Clear();
        _filterConfigure = null;
    }

    /// <summary>
    /// 执行目录扫描，返回符合匹配条件的文件路径列表
    /// </summary>
    /// <param name="folderPath">要扫描的目录路径</param>
    /// <param name="maxDepth">文件扫描的最大深度，默认 0</param>
    /// <param name="matcher">可选的文件通配符匹配对象</param>
    /// <returns><see cref="List{T}"/></returns>
    internal static List<string> ScanDirectory(string folderPath, uint maxDepth = 0, Matcher? matcher = null)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(folderPath, nameof(folderPath));

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
            var matchFiles = from filePath in Directory.EnumerateFiles(currentFolderPath)
                             let fileName = Path.GetFileName(filePath)
                             let isXmlFile = fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)
                             let dllFileExists = isXmlFile && File.Exists(Path.ChangeExtension(filePath, ".dll"))
                             where (matcher is null || matcher.Match(fileName).HasMatches)
                                    && (!isXmlFile || !dllFileExists)
                             select filePath;

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

    /// <summary>
    /// 获取文件发布后的路径
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="contentRoot">应用程序内容目录</param>
    /// <returns><see cref="string"/>[]</returns>
    internal static string[] GetFilePublishPaths(string filePath, string? contentRoot = null)
    {
        // 获取应用程序执行目录
        var baseDirectory = AppContext.BaseDirectory;

        // 处理不同操作系统目录分隔符
        var osFilePath = filePath.Replace('/', Path.DirectorySeparatorChar);

        // 若文件路径以 baseDirectory 开头，则直接返回
        if (osFilePath.StartsWith(baseDirectory, StringComparison.OrdinalIgnoreCase))
        {
            return new[] { osFilePath };
        }

        // 若内容目录不为空
        if (!string.IsNullOrWhiteSpace(contentRoot) && osFilePath.StartsWith(contentRoot, StringComparison.OrdinalIgnoreCase))
        {
            // 生成文件发布后路径
            var filePublishPath = Path.Combine(baseDirectory, osFilePath[contentRoot.Length..].TrimStart(Path.DirectorySeparatorChar));
            return new[] { osFilePath, filePublishPath };
        }

        return new[] { osFilePath };
    }

    /// <summary>
    /// 文件拓展名正则表达式
    /// </summary>
    /// <returns><see cref="Regex"/></returns>
    [GeneratedRegex("^\\.[a-zA-Z0-9]+$")]
    private static partial Regex FileExtensionRegex();
}