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
/// 远程配置解析器
/// </summary>
internal sealed class RemotedConfigurationParser
{
    /// <summary>
    /// Content-Type 和文件拓展名映射集合
    /// </summary>
    internal readonly IDictionary<string, string> _contentTypeMappings;

    /// <summary>
    /// <see cref="FileConfigurationParser"/>
    /// </summary>
    internal readonly FileConfigurationParser _fileConfigurationParser;

    /// <summary>
    /// 构造函数
    /// </summary>
    public RemotedConfigurationParser()
    {
        _contentTypeMappings = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            {"application/json", ".json" },
            {"application/vnd.api+json", ".json" },
            {"application/x-json", ".json" },
            {"text/json", ".json" },
            {"text/x-json", ".json" }
        };

        _fileConfigurationParser = new();
    }

    /// <summary>
    /// 解析远程请求地址并返回配置集合
    /// </summary>
    /// <param name="remotedConfigurationModel"><see cref="RemotedConfigurationModel"/></param>
    /// <returns><see cref="IDictionary{TKey, TValue}"/></returns>
    internal IDictionary<string, string?> ParseRequestUri(RemotedConfigurationModel remotedConfigurationModel)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(remotedConfigurationModel);

        // 请求远程地址并返回响应流
        using var stream = ReadAsStream(remotedConfigurationModel, out var extension);

        // 调用文件配置解析器对象进行解析
        var keyValues = _fileConfigurationParser.Parse(extension, stream);

        // 检查是否定义了配置前缀
        if (string.IsNullOrWhiteSpace(remotedConfigurationModel.Prefix))
        {
            // 输出调试事件
            Debugging.File("The remote address `{0}` has been successfully loaded into the configuration.", remotedConfigurationModel.RequestUri);

            return keyValues;
        }

        // 遍历字典集合并包装 Key
        var data = keyValues.ToDictionary(u => $"{remotedConfigurationModel.Prefix}:{u.Key}"
            , u => u.Value
            , StringComparer.OrdinalIgnoreCase);

        // 输出调试事件
        Debugging.File("The remote address `{0}` has been successfully loaded into the configuration with the prefix `{1}`."
            , remotedConfigurationModel.RequestUri
            , remotedConfigurationModel.Prefix);

        return data;
    }

    /// <summary>
    /// 请求远程地址并返回响应流
    /// </summary>
    /// <param name="remotedConfigurationModel"><see cref="RemotedConfigurationModel"/></param>
    /// <param name="extension">文件拓展名</param>
    /// <returns><see cref="Stream"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    internal Stream ReadAsStream(RemotedConfigurationModel remotedConfigurationModel, out string extension)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(remotedConfigurationModel);

        // 创建 HttpClient 请求对象
        using var httpClient = new HttpClient
        {
            Timeout = remotedConfigurationModel.Timeout
        };

        // 若请求为 GET 请求则禁用 HTTP 缓存
        if (remotedConfigurationModel.HttpMethod == HttpMethod.Get)
        {
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };
        }

        // 调用自定义 HttpClient 配置委托
        remotedConfigurationModel.ClientConfigurator?.Invoke(httpClient);

        // 发送 HTTP 请求
        var httpResponseMessage = httpClient.Send(
            new HttpRequestMessage(remotedConfigurationModel.HttpMethod, remotedConfigurationModel.RequestUri));

        // 确保请求成功
        httpResponseMessage.EnsureSuccessStatusCode();

        // 读取响应报文中的 Content-Type
        if (!httpResponseMessage.Content.Headers.TryGetValues("Content-Type", out var contentTypeValues))
        {
            throw new InvalidOperationException("Content-Type definition not found in the response message.");
        }

        // 取出首个 Content-Type 并根据 ; 切割，目的是处理携带 charset 的值
        var contentType = contentTypeValues.First()
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .First();

        // 检查当前 Content-Type 是否是受支持的类型
        if (!_contentTypeMappings.TryGetValue(contentType, out var extensionValue))
        {
            throw new NotSupportedException($"`{contentType}` is not a supported Content-Type type.");
        }

        // 设置拓展名 out 返回值
        extension = extensionValue;

        // 读取响应流
        var stream = httpResponseMessage.Content.ReadAsStream();

        return stream;
    }
}