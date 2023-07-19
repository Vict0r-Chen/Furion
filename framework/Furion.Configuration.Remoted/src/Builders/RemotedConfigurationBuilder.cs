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
/// 远程配置构建器
/// </summary>
public sealed class RemotedConfigurationBuilder : ConfigurationBuilderBase
{
    /// <summary>
    /// 待请求的 Url 地址集合
    /// </summary>
    internal readonly HashSet<string> _urlAddresses;

    /// <summary>
    /// 媒体类型和文件拓展名映射集合
    /// </summary>
    internal readonly IDictionary<string, string> _mediaTypeMappings;

    /// <summary>
    /// 远程配置模型过滤器
    /// </summary>
    internal Func<RemotedConfigurationModel, bool>? _filterConfigure;

    /// <summary>
    /// 默认 HttpClient 配置委托
    /// </summary>
    internal Action<HttpClient>? _defaultClientConfigurator;

    /// <summary>
    /// <inheritdoc cref="RemotedConfigurationBuilder"/>
    /// </summary>
    public RemotedConfigurationBuilder()
    {
        _urlAddresses = new(StringComparer.OrdinalIgnoreCase);
        _mediaTypeMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 默认 <see cref="HttpMethod"/>
    /// </summary>
    public HttpMethod DefaultHttpMethod { get; set; } = HttpMethod.Get;

    /// <summary>
    /// 默认超时配置
    /// </summary>
    /// <remarks>默认值 30 秒</remarks>
    public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// 默认可选配置
    /// </summary>
    public bool DefaultOptional { get; set; }

    /// <summary>
    /// 默认前缀
    /// </summary>
    public string? DefaultPrefix { get; set; }

    /// <summary>
    /// 添加远程配置模型过滤器
    /// </summary>
    /// <param name="configure">自定义配置委托</param>
    public void AddFilter(Func<RemotedConfigurationModel, bool> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        _filterConfigure = configure;
    }

    /// <summary>
    /// 配置默认 HttpClient 客户端对象
    /// </summary>
    /// <param name="configure">自定义配置委托</param>
    public void ConfigureClient(Action<HttpClient> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        _defaultClientConfigurator = configure;
    }

    /// <summary>
    /// 添加 Url 地址
    /// </summary>
    /// <param name="urlAddresses">Url 地址</param>
    /// <returns><see cref="RemotedConfigurationBuilder"/></returns>
    public RemotedConfigurationBuilder AddUrlAddresses(params string[] urlAddresses)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(urlAddresses);

        // 逐条添加 Url 地址到集合中
        Array.ForEach(urlAddresses, urlAddress =>
        {
            // 检查 Url 地址有效性
            EnsureLegalUrlAddress(urlAddress);

            _urlAddresses.Add(urlAddress);
        });

        return this;
    }

    /// <summary>
    /// 添加 Url 地址
    /// </summary>
    /// <param name="urlAddresses">Url 地址</param>
    /// <returns><see cref="RemotedConfigurationBuilder"/></returns>
    public RemotedConfigurationBuilder AddUrlAddresses(IEnumerable<string> urlAddresses)
    {
        return AddUrlAddresses(urlAddresses.ToArray());
    }

    /// <summary>
    /// 添加媒体类型和文件拓展名映射
    /// </summary>
    /// <param name="mediaType">媒体类型</param>
    /// <param name="extension">文件拓展名</param>
    public RemotedConfigurationBuilder AddMediaTypeMapping(string mediaType, string extension)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(mediaType);
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);

        _mediaTypeMappings[mediaType] = extension;

        return this;
    }

    /// <summary>
    /// 构建模块服务
    /// </summary>
    /// <param name="remotedConfigurationParser"><see cref="RemotedConfigurationParser"/></param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<RemotedConfigurationModel> Build(out RemotedConfigurationParser? remotedConfigurationParser)
    {
        // 创建远程配置模型集合并排序
        var remotedConfigurationModels = CreateModels()
            .OrderBy(m => m.Order)
            .ToList();

        // 初始化远程配置解析器
        remotedConfigurationParser = remotedConfigurationModels.Count == 0
            ? null
            : new RemotedConfigurationParser(InitializeParser(), _mediaTypeMappings);

        return remotedConfigurationModels;
    }

    /// <summary>
    /// 创建远程配置模型集合
    /// </summary>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal IEnumerable<RemotedConfigurationModel> CreateModels()
    {
        // 根据 Url 地址集合生成远程配置模型
        var remotedConfigurationModels = _urlAddresses.Select(urlAddress => new RemotedConfigurationModel(urlAddress, DefaultHttpMethod)
        {
            Timeout = DefaultTimeout,
            Optional = DefaultOptional,
            Prefix = DefaultPrefix
        });

        // 遍历远程配置模型集合并设置 HttpClient 配置委托
        foreach (var remotedConfigurationModel in remotedConfigurationModels)
        {
            // 过滤器检查
            if (_filterConfigure is not null
                && !_filterConfigure.Invoke(remotedConfigurationModel))
            {
                continue;
            }

            // 设置 HttpClient 配置委托
            SetClientConfigurator(remotedConfigurationModel);

            // 返回当前集合项
            yield return remotedConfigurationModel;
        }
    }

    /// <summary>
    /// 设置远程配置模型 HttpClient 配置委托
    /// </summary>
    /// <param name="remotedConfigurationModel"><see cref="RemotedConfigurationModel"/></param>
    internal void SetClientConfigurator(RemotedConfigurationModel remotedConfigurationModel)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(remotedConfigurationModel);

        // 若默认 HttpClient 配置委托为空则直接返回
        if (_defaultClientConfigurator is null)
        {
            return;
        }

        // 复制一个新的委托避免死循环
        var modelClientConfigurator = remotedConfigurationModel.ClientConfigurator is null
            ? null
            : new Action<HttpClient>(remotedConfigurationModel.ClientConfigurator);

        // 创建级联调用委托
        void clientConfigurator(HttpClient t)
        {
            _defaultClientConfigurator(t);
            modelClientConfigurator?.Invoke(t);
        }

        remotedConfigurationModel.ConfigureClient(clientConfigurator);
    }

    /// <summary>
    /// 检查 Url 地址有效性
    /// </summary>
    /// <param name="urlAddress">Url 地址</param>
    /// <exception cref="ArgumentException"></exception>
    internal static void EnsureLegalUrlAddress(string urlAddress)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(urlAddress);

        // Url 合法性检查
        if (Uri.TryCreate(urlAddress, UriKind.RelativeOrAbsolute, out var uri)
            && (!uri.IsAbsoluteUri || uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            return;
        }

        throw new ArgumentException($"The given address `{urlAddress}` is invalid.", nameof(urlAddress));
    }
}