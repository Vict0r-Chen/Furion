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
    /// 默认请求超时前等等的时间跨度
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
    /// 添加 Url 地址
    /// </summary>
    /// <param name="urlAddresses">Url 集合</param>
    /// <returns><see cref="RemotedConfigurationBuilder"/></returns>
    public RemotedConfigurationBuilder AddUrlAddresses(params string[] urlAddresses)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(urlAddresses);

        // 逐条添加到 Url 地址集合中
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
    /// <param name="urlAddresses">Url 集合</param>
    /// <returns><see cref="RemotedConfigurationBuilder"/></returns>
    public RemotedConfigurationBuilder AddUrlAddresses(IEnumerable<string> urlAddresses)
    {
        return AddUrlAddresses(urlAddresses.ToArray());
    }

    /// <summary>
    /// 构建模块服务
    /// </summary>
    /// <returns><see cref="RemotedConfigurationModel"/> 集合</returns>
    internal List<RemotedConfigurationModel> Build()
    {
        // 创建远程配置模型集合
        var remotedConfigurationModels = CreateModels();

        // 释放对象
        Release();

        return remotedConfigurationModels;
    }

    /// <summary>
    /// 创建远程配置模型集合
    /// </summary>
    /// <returns><see cref="RemotedConfigurationBuilder"/> 集合</returns>
    internal List<RemotedConfigurationModel> CreateModels()
    {
        var models = _urlAddresses.Select(urlAddress => new RemotedConfigurationModel(urlAddress, DefaultHttpMethod) { Timeout = DefaultTimeout })
            .Where(model => _filterConfigure is null || _filterConfigure.Invoke(model))
            .OrderByDescending(u => u.Order)
            .ToList();

        return models;
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    internal void Release()
    {
        _urlAddresses.Clear();
        _filterConfigure = null;
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