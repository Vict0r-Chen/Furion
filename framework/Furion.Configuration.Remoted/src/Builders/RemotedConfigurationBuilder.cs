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
public sealed class RemotedConfigurationBuilder
{
    /// <summary>
    /// 待请求的 Url 地址集合
    /// </summary>
    internal readonly HashSet<string> _urlAddresses;

    /// <summary>
    /// 远程配置模型过滤器
    /// </summary>
    internal Func<RemotedConfigurationModel, bool>? _filterConfigure;

    /// <summary>
    /// 默认 HttpClient 配置委托
    /// </summary>
    internal Action<HttpClient>? _defaultHttpClientConfigure;

    /// <summary>
    /// 构造函数
    /// </summary>
    public RemotedConfigurationBuilder()
    {
        _urlAddresses = new(StringComparer.OrdinalIgnoreCase);

        DefaultHttpMethod = HttpMethod.Get;
        DefaultTimeout = TimeSpan.FromSeconds(30);
    }

    /// <summary>
    /// 默认 <see cref="HttpMethod"/>
    /// </summary>
    public HttpMethod DefaultHttpMethod { get; set; }

    /// <summary>
    /// 默认超时配置
    /// </summary>
    /// <remarks>默认值 30 秒</remarks>
    public TimeSpan DefaultTimeout { get; set; }

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

        _defaultHttpClientConfigure = configure;
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

        // 逐条添加到 Url 地址到集合中
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
    /// 构建模块服务
    /// </summary>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal List<RemotedConfigurationModel> Build()
    {
        // 创建远程配置模型集合并排序
        var remotedConfigurationModels = CreateModels()
            .OrderByDescending(m => m.Order)
            .ToList();

        // 释放对象
        Release();

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
            Timeout = DefaultTimeout
        });

        // 遍历远程配置模型集合并设置 HttpClient 配置委托
        foreach (var remotedConfigurationModel in remotedConfigurationModels)
        {
            // 过滤器检查
            if (_filterConfigure is not null && !_filterConfigure.Invoke(remotedConfigurationModel))
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
    /// 释放对象
    /// </summary>
    internal void Release()
    {
        _urlAddresses.Clear();
        _filterConfigure = null;
        _defaultHttpClientConfigure = null;
    }

    /// <summary>
    /// 设置远程配置模型 HttpClient 配置委托
    /// </summary>
    /// <param name="remotedConfigurationModel"><see cref="RemotedConfigurationModel"/></param>
    internal void SetClientConfigurator(RemotedConfigurationModel remotedConfigurationModel)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(remotedConfigurationModel);

        // 若默认 HttpClient 配置委托为空则跳过
        if (_defaultHttpClientConfigure is null)
        {
            return;
        }

        // 若远程配置模型未配置 HttpClient 委托则设置为默认值
        if (remotedConfigurationModel.ClientConfigurator is null)
        {
            remotedConfigurationModel.ConfigureClient(_defaultHttpClientConfigure);
            return;
        }

        // 若两者都配置了则创建级联调用委托
        var clientConfigurator = new[] { _defaultHttpClientConfigure, remotedConfigurationModel.ClientConfigurator }
            .Cast<Action<HttpClient>>()
            .Aggregate((previous, current) => (t) =>
            {
                previous(t);
                current(t);
            });

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
        var isValidUrl = Uri.TryCreate(urlAddress, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        if (isValidUrl)
        {
            return;
        }

        throw new ArgumentException($"The given address `{urlAddress}` is invalid.", nameof(urlAddress));
    }
}