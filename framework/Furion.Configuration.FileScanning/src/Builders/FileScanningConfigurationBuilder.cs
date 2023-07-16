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
/// 文件扫描配置构建器
/// </summary>
public sealed partial class FileScanningConfigurationBuilder : ConfigurationBuilderBase
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
    /// 黑名单文件通配符
    /// </summary>
    internal readonly HashSet<string> _fileBlacklistGlobbing;

    /// <summary>
    /// 文件扫描配置模型过滤器
    /// </summary>
    internal Func<FileScanningConfigurationModel, bool>? _filterConfigure;

    /// <summary>
    /// 构造函数
    /// </summary>
    public FileScanningConfigurationBuilder()
    {
        _directories = new(StringComparer.OrdinalIgnoreCase);

        _fileGlobbing = new(StringComparer.OrdinalIgnoreCase)
        {
            "**/**.json"
        };

        _fileBlacklistGlobbing = new(StringComparer.OrdinalIgnoreCase)
        {
            "**/**.runtimeconfig.json",
            "**/**.runtimeconfig.*.json",
            "**/**.deps.json",
            "**/**.staticwebassets.*.json",
            "**/**.nuget.dgspec.json",
            "**/project.assets.json",
            "**/MvcTestingAppManifest.json"
        };
    }

    /// <summary>
    /// 扫描最大深度
    /// </summary>
    public uint MaxScanDepth { get; set; }

    /// <summary>
    /// 默认文件可选配置
    /// </summary>
    public bool DefaultOptional { get; set; }

    /// <summary>
    /// 默认文件变更时刷新配置
    /// </summary>
    public bool DefaultReloadOnChange { get; set; }

    /// <summary>
    /// 默认文件变更延迟刷新毫秒数配置
    /// </summary>
    public int DefaultReloadDelay { get; set; } = 250;

    /// <summary>
    /// 允许基于环境切换
    /// </summary>
    public bool AllowEnvironmentSwitching { get; set; } = true;

    /// <summary>
    /// 添加文件扫描配置模型过滤器
    /// </summary>
    /// <param name="configure">自定义配置委托</param>
    public void AddFilter(Func<FileScanningConfigurationModel, bool> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        _filterConfigure = configure;
    }

    /// <summary>
    /// 添加扫描目录
    /// </summary>
    /// <param name="directories">目录</param>
    /// <returns><see cref="FileScanningConfigurationBuilder"/></returns>
    public FileScanningConfigurationBuilder AddDirectories(params string[] directories)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(directories);

        // 逐条添加目录到集合中
        Array.ForEach(directories, directory =>
        {
            // 检查目录有效性
            EnsureLegalDirectory(directory);

            _directories.Add(directory);
        });

        return this;
    }

    /// <summary>
    /// 添加扫描目录
    /// </summary>
    /// <param name="directories"><see cref="IEnumerable{T}"/></param>
    /// <returns><see cref="FileScanningConfigurationBuilder"/></returns>
    public FileScanningConfigurationBuilder AddDirectories(IEnumerable<string> directories)
    {
        return AddDirectories(directories.ToArray());
    }

    /// <summary>
    /// 添加文件通配符
    /// </summary>
    /// <param name="globbings">文件通配符</param>
    /// <returns><see cref="FileScanningConfigurationBuilder"/></returns>
    public FileScanningConfigurationBuilder AddGlobbings(params string[] globbings)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(globbings, nameof(globbings));

        // 逐条添加文件通配符到集合中
        Array.ForEach(globbings, globbing =>
        {
            // 空检查
            ArgumentException.ThrowIfNullOrWhiteSpace(globbing);

            _fileGlobbing.Add(globbing);
        });

        return this;
    }

    /// <summary>
    /// 添加文件通配符
    /// </summary>
    /// <param name="globbings"><see cref="IEnumerable{T}"/></param>
    /// <returns><see cref="FileScanningConfigurationBuilder"/></returns>
    public FileScanningConfigurationBuilder AddGlobbings(IEnumerable<string> globbings)
    {
        return AddGlobbings(globbings.ToArray());
    }

    /// <summary>
    /// 添加黑名单文件通配符
    /// </summary>
    /// <param name="globbings"><see cref="string"/>[]</param>
    /// <returns><see cref="FileScanningConfigurationBuilder"/></returns>
    public FileScanningConfigurationBuilder AddBlacklistGlobbings(params string[] globbings)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(globbings, nameof(globbings));

        // 逐条添加黑名单文件通配符到集合中
        Array.ForEach(globbings, globbing =>
        {
            // 空检查
            ArgumentException.ThrowIfNullOrWhiteSpace(globbing);

            _fileBlacklistGlobbing.Add(globbing);
        });

        return this;
    }

    /// <summary>
    /// 添加黑名单文件通配符
    /// </summary>
    /// <param name="globbings"><see cref="IEnumerable{T}"/></param>
    /// <returns><see cref="FileScanningConfigurationBuilder"/></returns>
    public FileScanningConfigurationBuilder AddBlacklistGlobbings(IEnumerable<string> globbings)
    {
        return AddBlacklistGlobbings(globbings.ToArray());
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

        // 获取应用程序内容目录
        var contentRoot = configurationRoot["CONTENTROOT"];

        // 默认添加应用程序内容目录和应用程序执行目录作为扫描
        AddDirectories(contentRoot ?? AppContext.BaseDirectory, AppContext.BaseDirectory);

        // 获取环境变量名称
        var environmentName = configurationRoot["ENVIRONMENT"];

        // 创建文件扫描配置模型集合同时排序并分组
        var groupedFileScanningConfigurationModels = CreateModels(builder, contentRoot)
            .OrderBy(model => model.Order)
            .ThenBy(model => model.FileName.Length)
            .GroupBy(model => new
            {
                model.Extension,
                model.DirectoryName,
                model.Group
            });

        // 创建文件配置解析器
        var fileConfigurationParser = new FileConfigurationParser();

        // 逐条添加配置文件
        foreach (var groupedFileScanningConfigurationModel in groupedFileScanningConfigurationModels)
        {
            // 获取当前分组的文件扫描配置模型集合
            var fileInGroupModels = groupedFileScanningConfigurationModel.ToList();

            // 文件拓展名
            var extension = groupedFileScanningConfigurationModel.Key.Extension;

            // 文件路径不带拓展名
            var filePathWithoutExtension = Path.Combine(groupedFileScanningConfigurationModel.Key.DirectoryName, groupedFileScanningConfigurationModel.Key.Group);

            // 组合不携带环境变量名的文件路径和对应模型
            var baseFile = filePathWithoutExtension + extension;
            var baseFileModel = fileInGroupModels.Find(model => model.IsMatch(baseFile));

            // 添加配置文件
            AddFileByEnvironment(builder, fileConfigurationParser, baseFileModel, baseFile);

            // 检查是否定义环境变量名称
            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                // 组合携带环境变量名的文件路径和对应模型
                var environmentFile = filePathWithoutExtension + "." + environmentName + extension;
                var environmentFileModel = fileInGroupModels.Find(model => model.IsMatch(environmentFile));

                // 添加配置文件
                AddFileByEnvironment(builder, fileConfigurationParser, environmentFileModel, environmentFile);
            }
        }
    }

    /// <summary>
    /// 创建文件扫描配置模型集合
    /// </summary>
    /// <param name="builder"><see cref="IConfigurationBuilder"/></param>
    /// <param name="contentRoot">应用程序内容目录</param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal IEnumerable<FileScanningConfigurationModel> CreateModels(IConfigurationBuilder builder, string? contentRoot = null)
    {
        // 初始化文件通配符匹配对象
        var matcher = new Matcher();
        matcher.AddIncludePatterns(_fileGlobbing);
        matcher.AddExcludePatterns(_fileBlacklistGlobbing);

        // 扫描所有目录文件
        var files = _directories.SelectMany(directory => ScanDirectory(directory, matcher, MaxScanDepth))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Except(GetAddedFiles(builder, contentRoot), StringComparer.OrdinalIgnoreCase);

        //  创建一个 Set 来存储已访问过的文件
        var visitedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // 遍历所有文件创建文件扫描配置模型集合
        foreach (var filePath in files)
        {
            // 检查文件是否已添加进集合中
            if (!visitedFiles.Add(ResolvePublicationFile(filePath, contentRoot)[^1]))
            {
                continue;
            }

            // 创建文件扫描配置模型
            var fileScanningConfigurationModel = new FileScanningConfigurationModel(filePath, true)
            {
                Optional = DefaultOptional,
                ReloadOnChange = DefaultReloadOnChange,
                ReloadDelay = DefaultReloadDelay
            };

            // 调用文件扫描配置模型过滤器
            if (_filterConfigure is null || _filterConfigure.Invoke(fileScanningConfigurationModel))
            {
                yield return fileScanningConfigurationModel;
            }
        }
    }

    /// <summary>
    /// 添加配置文件（支持环境切换）
    /// </summary>
    /// <param name="builder"><see cref="IConfigurationBuilder"/></param>
    /// <param name="fileConfigurationParser"><see cref="FileConfigurationParser"/></param>
    /// <param name="fileScanningConfigurationModel"><see cref="FileScanningConfigurationModel"/></param>
    /// <param name="filePath">文件路径</param>
    internal void AddFileByEnvironment(IConfigurationBuilder builder
        , FileConfigurationParser fileConfigurationParser
        , FileScanningConfigurationModel? fileScanningConfigurationModel
        , string filePath)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        // 检查是否启用支持环境切换，如果启用则创建默认值
        fileScanningConfigurationModel = !AllowEnvironmentSwitching
            ? fileScanningConfigurationModel
            : fileScanningConfigurationModel ?? new FileScanningConfigurationModel(filePath, false)
            {
                Optional = DefaultOptional,
                ReloadOnChange = DefaultReloadOnChange,
                ReloadDelay = DefaultReloadDelay
            };

        // 空检查
        if (fileScanningConfigurationModel is null)
        {
            return;
        }

        // 添加配置
        AddFile(builder, fileConfigurationParser, fileScanningConfigurationModel);
    }

    /// <summary>
    /// 添加配置文件
    /// </summary>
    /// <param name="builder"><see cref="IConfigurationBuilder"/></param>
    /// <param name="fileConfigurationParser"><see cref="FileConfigurationParser"/></param>
    /// <param name="fileScanningConfigurationModel"><see cref="FileScanningConfigurationModel"/></param>
    internal static void AddFile(IConfigurationBuilder builder
        , FileConfigurationParser fileConfigurationParser
        , FileScanningConfigurationModel fileScanningConfigurationModel)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(fileConfigurationParser);
        ArgumentNullException.ThrowIfNull(fileScanningConfigurationModel);

        // 创建文件拓展名配置源对象
        var fileConfigurationSource = fileConfigurationParser.CreateSourceInstance(fileScanningConfigurationModel.Extension
            , fileConfigurationSource =>
            {
                fileConfigurationSource.Path = fileScanningConfigurationModel.FilePath;
                fileConfigurationSource.Optional = fileScanningConfigurationModel.Optional;
                fileConfigurationSource.ReloadOnChange = fileScanningConfigurationModel.ReloadOnChange;
                fileConfigurationSource.ReloadDelay = fileScanningConfigurationModel.ReloadDelay;
            });

        // 添加配置文件
        builder.Add(fileConfigurationSource);

        // 调试事件消息
        var debugMessage = "The file `{0}` has been successfully added to the configuration.";
        if (!fileScanningConfigurationModel.EnvironmentFlag)
        {
            debugMessage += " (File does not actually exist)";
        }

        // 输出调试事件
        Debugging.File(debugMessage, fileScanningConfigurationModel.FilePath);
    }

    /// <summary>
    /// 查找已注册的配置文件集合
    /// </summary>
    /// <param name="builder"><see cref="IConfigurationBuilder"/></param>
    /// <param name="contentRoot">应用程序内容目录</param>
    /// <returns><see cref="HashSet{T}"/></returns>
    internal static HashSet<string> GetAddedFiles(IConfigurationBuilder builder, string? contentRoot)
    {
        // 查找所有实现 FileConfigurationSource 的配置
        var addedFiles = builder.Sources.OfType<FileConfigurationSource>()
            .Select(s => (s.Path, s.FileProvider as PhysicalFileProvider))
            .OfType<(string Path, PhysicalFileProvider Provider)>()
            .SelectMany(t => ResolvePublicationFile(Path.Combine(t.Provider.Root, t.Path), contentRoot))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return addedFiles;
    }

    /// <summary>
    /// 根据文件路径解析发布后的对应路径
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="contentRoot">应用程序内容目录</param>
    /// <returns><see cref="string"/>[]</returns>
    internal static string[] ResolvePublicationFile(string filePath, string? contentRoot = null)
    {
        // 获取应用程序执行目录
        var baseDirectory = AppContext.BaseDirectory;

        // 获取不同操作系统下的文件路径
        var originalFile = filePath.Replace('/', Path.DirectorySeparatorChar);

        // 若文件路径以 baseDirectory 开头，则直接返回
        if (originalFile.StartsWith(baseDirectory, StringComparison.OrdinalIgnoreCase))
        {
            return new[] { originalFile };
        }

        // 若应用程序内容目录不为空
        if (!string.IsNullOrWhiteSpace(contentRoot)
            && originalFile.StartsWith(contentRoot, StringComparison.OrdinalIgnoreCase))
        {
            // 生成发布后的文件路径
            var publicationFilePath = Path.Combine(baseDirectory, originalFile[contentRoot.Length..]
                .TrimStart(Path.DirectorySeparatorChar));

            return new[] { originalFile, publicationFilePath };
        }

        return new[] { originalFile };
    }

    /// <summary>
    /// 扫描目录文件
    /// </summary>
    /// <param name="matcher"><see cref="Matcher"/></param>
    /// <param name="directory">目录</param>
    /// <param name="maxScanDepth">扫描最大深度</param>
    /// <returns><see cref="List{T}"/></returns>
    internal static List<string> ScanDirectory(string directory
        , Matcher matcher
        , uint maxScanDepth = 0)
    {
        // 检查目录有效性
        EnsureLegalDirectory(directory);

        // 空检查
        ArgumentNullException.ThrowIfNull(matcher);

        // 初始化目录文件集合对象
        var files = new List<string>();

        // 初始化栈结构对象并将当前目录和深度值压入栈中
        var stack = new Stack<(string folderPath, int depth)>(new[] { (directory, 0) });

        // 循环取出栈内元素，直到栈为空
        while (stack.Count > 0)
        {
            // 取出栈顶元素中的当前目录和深度值
            var (currentDirectory, currentDepth) = stack.Pop();

            // 查找当前目录下所有匹配的文件
            var matchFiles = Directory.EnumerateFiles(currentDirectory)
                .Where(filePath => matcher.Match(filePath).HasMatches);

            // 查找匹配的文件添加到集合中
            files.AddRange(matchFiles);

            // 检查扫描最大深度
            if (currentDepth >= maxScanDepth)
            {
                continue;
            }

            // 将当前目录下的子目录入栈，设置深度值加 1
            foreach (var subDirectory in Directory.GetDirectories(currentDirectory))
            {
                stack.Push((subDirectory, currentDepth + 1));
            }
        }

        return files;
    }

    /// <summary>
    /// 检查目录有效性
    /// </summary>
    /// <param name="directory">目录</param>
    /// <exception cref="ArgumentException"></exception>
    internal static void EnsureLegalDirectory(string directory)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);

        // 检查是否为绝对路径
        if (Path.IsPathRooted(directory))
        {
            return;
        }

        throw new ArgumentException($"The path `{directory}` is not an absolute path.", nameof(directory));
    }
}