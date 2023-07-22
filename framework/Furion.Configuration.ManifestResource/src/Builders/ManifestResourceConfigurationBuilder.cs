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
/// 嵌入资源配置构建器
/// </summary>
public sealed class ManifestResourceConfigurationBuilder : ConfigurationBuilderBase
{
    /// <summary>
    /// 待扫描的程序集集合
    /// </summary>
    internal readonly HashSet<Assembly> _assemblies;

    /// <summary>
    /// 文件通配符
    /// </summary>
    internal readonly HashSet<string> _fileGlobbing;

    /// <summary>
    /// 黑名单文件通配符
    /// </summary>
    internal readonly HashSet<string> _blacklistFileGlobbing;

    /// <summary>
    /// 嵌入资源配置模型过滤器
    /// </summary>
    internal Func<ManifestResourceConfigurationModel, bool>? _filterConfigure;

    /// <summary>
    /// <inheritdoc cref="ManifestResourceConfigurationBuilder"/>
    /// </summary>
    public ManifestResourceConfigurationBuilder()
    {
        _assemblies = new();

        _fileGlobbing = new(StringComparer.OrdinalIgnoreCase)
        {
            "*.json"
        };

        _blacklistFileGlobbing = new(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 默认前缀
    /// </summary>
    public string? DefaultPrefix { get; set; }

    /// <summary>
    /// 添加嵌入资源配置模型过滤器
    /// </summary>
    /// <param name="configure"><see cref="Func{T1, T2, TResult}"/></param>
    public void AddFilter(Func<ManifestResourceConfigurationModel, bool> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        _filterConfigure = configure;
    }

    /// <summary>
    /// 添加程序集
    /// </summary>
    /// <param name="assemblies"><see cref="Assembly"/>[]</param>
    /// <returns><see cref="ManifestResourceConfigurationBuilder"/></returns>
    public ManifestResourceConfigurationBuilder AddAssemblies(params Assembly[] assemblies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(assemblies);

        Array.ForEach(assemblies, assembly =>
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(assembly);

            _assemblies.Add(assembly);
        });

        return this;
    }

    /// <summary>
    /// 添加程序集
    /// </summary>
    /// <param name="assemblies"><see cref="IEnumerable{T}"/></param>
    /// <returns><see cref="ManifestResourceConfigurationBuilder"/></returns>
    public ManifestResourceConfigurationBuilder AddAssemblies(IEnumerable<Assembly> assemblies)
    {
        return AddAssemblies(assemblies.ToArray());
    }

    /// <summary>
    /// 添加文件通配符
    /// </summary>
    /// <param name="globbings">文件通配符</param>
    /// <returns><see cref="ManifestResourceConfigurationBuilder"/></returns>
    public ManifestResourceConfigurationBuilder AddGlobbings(params string[] globbings)
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
    /// <returns><see cref="ManifestResourceConfigurationBuilder"/></returns>
    public ManifestResourceConfigurationBuilder AddGlobbings(IEnumerable<string> globbings)
    {
        return AddGlobbings(globbings.ToArray());
    }

    /// <summary>
    /// 添加黑名单文件通配符
    /// </summary>
    /// <param name="globbings"><see cref="string"/>[]</param>
    /// <returns><see cref="ManifestResourceConfigurationBuilder"/></returns>
    public ManifestResourceConfigurationBuilder AddBlacklistGlobbings(params string[] globbings)
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
    /// <returns><see cref="ManifestResourceConfigurationBuilder"/></returns>
    public ManifestResourceConfigurationBuilder AddBlacklistGlobbings(IEnumerable<string> globbings)
    {
        return AddBlacklistGlobbings(globbings.ToArray());
    }

    /// <summary>
    /// 构建模块服务
    /// </summary>
    /// <param name="manifestResourceConfigurationParser"><see cref="ManifestResourceConfigurationParser"/></param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<ManifestResourceConfigurationModel> Build(out ManifestResourceConfigurationParser? manifestResourceConfigurationParser)
    {
        // 扫描程序集并创建嵌入资源配置文件模型
        var manifestResourceConfigurationModels = CreateModels()
            .OrderBy(m => m.Order)
            .ToList();

        // 初始化嵌入资源配置解析器
        manifestResourceConfigurationParser = manifestResourceConfigurationModels.Count == 0
            ? null
            : new ManifestResourceConfigurationParser(InitializeParser());

        return manifestResourceConfigurationModels;
    }

    /// <summary>
    /// 创建嵌入资源配置模型集合
    /// </summary>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal IEnumerable<ManifestResourceConfigurationModel> CreateModels()
    {
        // 初始化文件通配符匹配对象
        var matcher = new Matcher();
        matcher.AddIncludePatterns(_fileGlobbing);
        matcher.AddExcludePatterns(_blacklistFileGlobbing);

        // 扫描程序集并创建嵌入资源配置模型集合
        var manifestResourceConfigurationModels = _assemblies
            .SelectMany(assembly => assembly.GetManifestResourceNames()
            .Where(resource => matcher.Match(resource).HasMatches)
            .Select(resource => new ManifestResourceConfigurationModel(assembly, resource)
            {
                Prefix = DefaultPrefix
            })
            .Where(model => _filterConfigure is null || _filterConfigure.Invoke(model)));

        return manifestResourceConfigurationModels;
    }
}