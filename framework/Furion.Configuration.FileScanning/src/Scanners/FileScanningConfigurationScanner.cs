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
/// 文件扫描配置扫描器
/// </summary>
internal sealed class FileScanningConfigurationScanner
{
    /// <inheritdoc cref="IConfigurationBuilder"/>
    internal readonly IConfigurationBuilder _configurationBuilder;

    /// <inheritdoc cref="FileScanningConfigurationBuilder"/>
    internal readonly FileScanningConfigurationBuilder _fileScanningConfigurationBuilder;

    /// <summary>
    /// <inheritdoc cref="FileScanningConfigurationScanner"/>
    /// </summary>
    /// <param name="configurationBuilder"><see cref="IConfigurationBuilder"/></param>
    /// <param name="fileScanningConfigurationBuilder"><see cref="FileScanningConfigurationBuilder"/></param>
    internal FileScanningConfigurationScanner(IConfigurationBuilder configurationBuilder
        , FileScanningConfigurationBuilder fileScanningConfigurationBuilder)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configurationBuilder);
        ArgumentNullException.ThrowIfNull(fileScanningConfigurationBuilder);

        _configurationBuilder = configurationBuilder;
        _fileScanningConfigurationBuilder = fileScanningConfigurationBuilder;

        // 初始化
        Initialize();
    }

    /// <summary>
    /// 应用程序内容目录
    /// </summary>
    internal string? ContentRoot { get; private set; }

    /// <summary>
    /// 环境变量名称
    /// </summary>
    internal string? EnvironmentName { get; private set; }

    /// <summary>
    /// 初始化
    /// </summary>
    internal void Initialize()
    {
        // 获取配置根对象
        var configurationRoot = _configurationBuilder is ConfigurationManager configurationManager
        ? configurationManager
            : _configurationBuilder.Build();

        // 初始化应用程序内容目录
        ContentRoot = configurationRoot["CONTENTROOT"];

        // 默认添加应用程序内容目录和应用程序执行目录作为扫描
        _fileScanningConfigurationBuilder.AddDirectories(ContentRoot ?? AppContext.BaseDirectory, AppContext.BaseDirectory);

        // 初始化环境变量名称
        EnvironmentName = configurationRoot["ENVIRONMENT"]
            ?? (_fileScanningConfigurationBuilder.AllowEnvironmentSwitching ? "Production" : null);
    }

    /// <summary>
    /// 扫描并添加配置文件
    /// </summary>
    internal void ScanToAddFiles()
    {
        // 创建文件扫描配置模型集合同时排序并分组
        var groupedFileScanningConfigurationModels = CreateModels()
            .OrderBy(model => model.Order)
            .ThenBy(model => model.FileName.Length)
            .GroupBy(model => new
            {
                model.Extension,
                model.DirectoryName,
                model.Group
            });

        // 初始化文件配置解析器
        var fileConfigurationParser = _fileScanningConfigurationBuilder.InitializeParser();

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

            // 处理不携带环境变量名且文件不存在的情况
            // 不存在则跳过，存在则添加
            if (baseFileModel is not null)
            {
                // 添加配置文件
                AddFileByEnvironment(fileConfigurationParser
                    , baseFileModel
                    , baseFile);
            }

            // 检查是否定义环境变量名称
            if (!string.IsNullOrWhiteSpace(EnvironmentName))
            {
                // 组合携带环境变量名的文件路径和对应模型
                var environmentFile = filePathWithoutExtension + "." + EnvironmentName + extension;
                var environmentFileModel = fileInGroupModels.Find(model => model.IsMatch(environmentFile));

                // 添加配置文件
                AddFileByEnvironment(fileConfigurationParser
                    , environmentFileModel
                    , environmentFile);
            }
        }
    }

    /// <summary>
    /// 创建文件扫描配置模型集合
    /// </summary>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal IEnumerable<FileScanningConfigurationModel> CreateModels()
    {
        // 初始化文件通配符匹配对象
        var matcher = new Matcher();
        matcher.AddIncludePatterns(_fileScanningConfigurationBuilder._fileGlobbing);
        matcher.AddExcludePatterns(_fileScanningConfigurationBuilder._blacklistFileGlobbing);

        // 扫描所有目录文件
        var files = _fileScanningConfigurationBuilder._directories
            .SelectMany(directory => ScanDirectory(directory, matcher, _fileScanningConfigurationBuilder.MaxScanDepth))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Except(GetAddedFiles(), StringComparer.OrdinalIgnoreCase);

        //  创建一个 Set 来存储已访问过的文件
        var visitedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // 遍历所有文件创建文件扫描配置模型集合
        foreach (var filePath in files)
        {
            // 检查文件是否已添加进集合中
            if (!visitedFiles.Add(ResolvePublicationFile(filePath).Last()))
            {
                continue;
            }

            // 创建文件扫描配置模型
            var fileScanningConfigurationModel = CreateModel(filePath, true);

            // 检查文件拓展名
            if (string.IsNullOrWhiteSpace(fileScanningConfigurationModel.Extension))
            {
                continue;
            }

            // 调用文件扫描配置模型过滤器
            if (_fileScanningConfigurationBuilder._filterConfigure is null
                || _fileScanningConfigurationBuilder._filterConfigure.Invoke(fileScanningConfigurationModel))
            {
                yield return fileScanningConfigurationModel;
            }
        }
    }

    /// <summary>
    /// 创建文件扫描配置模型
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="environmentFlag">环境标识</param>
    /// <returns><see cref="FileScanningConfigurationModel"/></returns>
    internal FileScanningConfigurationModel CreateModel(string filePath, bool environmentFlag)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        return new FileScanningConfigurationModel(filePath, environmentFlag)
        {
            Optional = _fileScanningConfigurationBuilder.DefaultOptional,
            ReloadOnChange = _fileScanningConfigurationBuilder.DefaultReloadOnChange,
            ReloadDelay = _fileScanningConfigurationBuilder.DefaultReloadDelay,
            OnLoadException = _fileScanningConfigurationBuilder.OnLoadException ?? _fileScanningConfigurationBuilder.OnLoadException
        };
    }

    /// <summary>
    /// 添加配置文件（支持环境切换）
    /// </summary>
    /// <param name="fileConfigurationParser"><see cref="FileConfigurationParser"/></param>
    /// <param name="fileScanningConfigurationModel"><see cref="FileScanningConfigurationModel"/></param>
    /// <param name="filePath">文件路径</param>
    internal void AddFileByEnvironment(FileConfigurationParser fileConfigurationParser
        , FileScanningConfigurationModel? fileScanningConfigurationModel
        , string filePath)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(fileConfigurationParser);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        // 检查是否启用支持环境切换，如果启用则创建默认值
        fileScanningConfigurationModel = !_fileScanningConfigurationBuilder.AllowEnvironmentSwitching
            ? fileScanningConfigurationModel
            : fileScanningConfigurationModel ?? CreateModel(filePath, false);

        // 空检查
        if (fileScanningConfigurationModel is null)
        {
            return;
        }

        // 添加配置
        AddFile(fileConfigurationParser, fileScanningConfigurationModel);
    }

    /// <summary>
    /// 添加配置文件
    /// </summary>
    /// <param name="fileConfigurationParser"><see cref="FileConfigurationParser"/></param>
    /// <param name="fileScanningConfigurationModel"><see cref="FileScanningConfigurationModel"/></param>
    internal void AddFile(FileConfigurationParser fileConfigurationParser
        , FileScanningConfigurationModel fileScanningConfigurationModel)
    {
        // 空检查
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
                fileConfigurationSource.OnLoadException = fileScanningConfigurationModel.OnLoadException;
            });

        // 添加配置文件
        _configurationBuilder.Add(fileConfigurationSource);

        // 调试事件消息
        var debugMessage = "The file `{0}` has been successfully added to the configuration.";
        if (!fileScanningConfigurationModel.EnvironmentFlag)
        {
            debugMessage += " (File does not exist)";
        }

        // 输出调试事件
        Debugging.File(debugMessage, fileScanningConfigurationModel.FilePath);
    }

    /// <summary>
    /// 查找已注册的配置文件集合
    /// </summary>
    /// <returns><see cref="HashSet{T}"/></returns>
    internal HashSet<string> GetAddedFiles()
    {
        // 查找所有实现 FileConfigurationSource 的配置
        var addedFiles = _configurationBuilder.Sources
            .OfType<FileConfigurationSource>()
            .Select(s => (s.Path, s.FileProvider as PhysicalFileProvider))
            .OfType<(string Path, PhysicalFileProvider Provider)>()
            .SelectMany(t => ResolvePublicationFile(Path.Combine(t.Provider.Root, t.Path)))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return addedFiles;
    }

    /// <summary>
    /// 根据文件路径解析发布后的对应路径
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns><see cref="string"/>[]</returns>
    internal string[] ResolvePublicationFile(string filePath)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

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
        if (!string.IsNullOrWhiteSpace(ContentRoot)
            && originalFile.StartsWith(ContentRoot, StringComparison.OrdinalIgnoreCase))
        {
            // 生成发布后的文件路径
            var publicationFile = Path.Combine(baseDirectory, originalFile[ContentRoot.Length..]
                .TrimStart(Path.DirectorySeparatorChar));

            return new[] { originalFile, publicationFile };
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
        if (!Path.IsPathRooted(directory))
        {
            throw new ArgumentException($"The path `{directory}` is not an absolute path.", nameof(directory));
        }
    }
}