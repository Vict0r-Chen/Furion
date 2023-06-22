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
    /// 文件扫描通配符
    /// </summary>
    /// <remarks><see href="https://learn.microsoft.com/zh-cn/dotnet/core/extensions/file-globbing">文件通配符</see></remarks>
    internal readonly HashSet<string> _fileGlobbing;

    /// <summary>
    /// 文件名黑名单
    /// </summary>
    /// <remarks>禁止已扫描的文件名作为配置文件</remarks>
    internal readonly HashSet<string> _fileBlacklistGlobbing;

    /// <summary>
    /// 文件扫描过滤器
    /// </summary>
    internal Func<FileScannerModel, bool>? _filterConfigure;

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
            "*.json",
            "*.xml",
            "*.ini",
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
            "project.assets.json",
            "manifest.json"
        };
    }

    /// <summary>
    /// 扫描文件最大深度
    /// </summary>
    public int MaxDepthScanner { get; set; } = 2;

    /// <summary>
    /// 添文件扫描过滤器
    /// </summary>
    /// <param name="configure"><see cref="Func{T, TResult}"/></param>
    public void AddFilter(Func<FileScannerModel, bool> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));

        _filterConfigure = configure;
    }

    /// <summary>
    /// 添加待扫描的目录
    /// </summary>
    /// <param name="directories"><see cref="string"/>[]</param>
    /// <returns><see cref="FileScannerConfigurationBuilder"/></returns>
    public FileScannerConfigurationBuilder AddDirectories(params string[] directories)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(directories, nameof(directories));

        Array.ForEach(directories, directory => _directories.Add(directory));

        return this;
    }

    /// <summary>
    /// 添加待扫描的目录
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

        // 获取运行环境
        var environment = configurationRoot["ENVIRONMENT"];
    }
}