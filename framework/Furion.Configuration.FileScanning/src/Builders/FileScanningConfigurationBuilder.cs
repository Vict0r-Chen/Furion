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
    internal readonly HashSet<string> _blacklistFileGlobbing;

    /// <summary>
    /// 文件扫描配置模型过滤器
    /// </summary>
    internal Func<FileScanningConfigurationModel, bool>? _filterConfigure;

    /// <summary>
    /// <inheritdoc cref="FileScanningConfigurationBuilder"/>
    /// </summary>
    public FileScanningConfigurationBuilder()
    {
        _directories = new(StringComparer.OrdinalIgnoreCase);

        _fileGlobbing = new(StringComparer.OrdinalIgnoreCase)
        {
            "**/**.json"
        };

        _blacklistFileGlobbing = new(StringComparer.OrdinalIgnoreCase)
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
    /// 文件加载异常委托
    /// </summary>
    public Action<FileLoadExceptionContext>? OnLoadException { get; set; }

    /// <summary>
    /// 允许基于环境切换
    /// </summary>
    public bool AllowEnvironmentSwitching { get; set; }

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
            FileScanningConfigurationScanner.EnsureLegalDirectory(directory);

            _directories.Add(Path.GetFullPath(directory));
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
        ArgumentNullException.ThrowIfNull(globbings);

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
        ArgumentNullException.ThrowIfNull(globbings);

        // 逐条添加黑名单文件通配符到集合中
        Array.ForEach(globbings, globbing =>
        {
            // 空检查
            ArgumentException.ThrowIfNullOrWhiteSpace(globbing);

            _blacklistFileGlobbing.Add(globbing);
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
    /// <param name="configurationBuilder"><see cref="IConfigurationBuilder"/></param>
    internal void Build(IConfigurationBuilder configurationBuilder)
    {
        // 创建文件扫描配置扫描器对象
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, this);

        // 扫描并添加配置文件
        fileScanningConfigurationScanner.ScanToAddFiles();
    }
}