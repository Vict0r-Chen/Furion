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
    internal readonly IDictionary<string, string> _contentTypeExtensions;

    internal readonly FileConfigurationParser _fileConfigurationParser;

    /// <summary>
    /// 构造函数
    /// </summary>
    public RemotedConfigurationParser()
    {
        _contentTypeExtensions = new Dictionary<string, string>(StringComparer.Ordinal)
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
    /// 解析内容
    /// </summary>
    /// <param name="remotedConfigurationModel"></param>
    /// <returns></returns>
    internal Dictionary<string, string?> Parse(RemotedConfigurationModel remotedConfigurationModel)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(remotedConfigurationModel);

        // 发送请求并读取响应流
        using var stream = Send(remotedConfigurationModel, out var extension);

        var keyValues = _fileConfigurationParser.Parse(extension, stream);

        if (keyValues is null || keyValues.Count == 0)
        {
            return new();
        }

        var data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        // 将 程序集名称:键 添加到集合中
        foreach (var (key, value) in keyValues)
        {
            if (string.IsNullOrWhiteSpace(remotedConfigurationModel.Prefix))
            {
                data[key] = value;
                continue;
            }

            data[$"{remotedConfigurationModel.Prefix}:{key}"] = value;
        }

        // 清空集合
        keyValues.Clear();

        // 输出调试事件
        Debugging.File("The remoted `{0}` with prefix `{1}` has been successfully added to the configuration.", remotedConfigurationModel.RequestUri, remotedConfigurationModel.Prefix);

        return data;
    }

    /// <summary>
    /// 发送请求并读取响应流
    /// </summary>
    /// <param name="remotedConfigurationModel"><see cref="RemotedConfigurationModel"/></param>
    /// <param name="extension">文件拓展名</param>
    /// <returns><see cref="Stream"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    internal Stream Send(RemotedConfigurationModel remotedConfigurationModel, out string extension)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(remotedConfigurationModel);

        // 创建 HttpClient 实例
        using var httpClient = new HttpClient();

        // 配置 GET 请求禁止缓存头
        if (remotedConfigurationModel.HttpMethod == HttpMethod.Get)
        {
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };
        }

        // 设置超时时间
        httpClient.Timeout = remotedConfigurationModel.Timeout;

        // 创建请求消息
        var httpRequestMessage = new HttpRequestMessage(remotedConfigurationModel.HttpMethod, remotedConfigurationModel.RequestUri);

        // 请求 Url 地址
        var httpResponseMessage = httpClient.Send(httpRequestMessage);

        // 确保请求成功
        httpResponseMessage.EnsureSuccessStatusCode();

        // 查找响应 Content-Type
        if (!httpResponseMessage.Content.Headers.TryGetValues("Content-Type", out var contentTypes))
        {
            throw new InvalidOperationException("Content-Type definition not found in the response message.");
        }

        // 判断 Content-Type 是否受支持
        var contentType = contentTypes.First()
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .First();
        if (!_contentTypeExtensions.TryGetValue(contentType, out var value))
        {
            throw new NotSupportedException($"`{contentType}` is not a supported Content-Type type.");
        }

        // 读取请求响应流
        var stream = httpResponseMessage.Content.ReadAsStream();

        extension = value;
        return stream;
    }
}