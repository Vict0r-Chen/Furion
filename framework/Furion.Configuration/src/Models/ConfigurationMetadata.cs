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
/// 配置元数据
/// </summary>
public sealed class ConfigurationMetadata
{
    /// <summary>
    /// 反射搜索成员方式
    /// </summary>
    internal const BindingFlags _bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    /// <summary>
    /// <inheritdoc cref="ConfigurationMetadata"/>
    /// </summary>
    /// <param name="provider"><see cref="IConfigurationProvider"/></param>
    public ConfigurationMetadata(IConfigurationProvider provider)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(provider);

        Provider = provider;

        // 初始化属性
        InitializeProperties();
    }

    /// <inheritdoc cref="IConfigurationProvider" />
    public IConfigurationProvider Provider { get; init; }

    /// <summary>
    /// 数据字典
    /// </summary>
    public IDictionary<string, string?>? Data { get; private set; }

    /// <inheritdoc cref="IConfigurationSource" />
    public IConfigurationSource? Source { get; private set; }

    /// <summary>
    /// 子配置元数据集合
    /// </summary>
    public IEnumerable<ConfigurationMetadata>? Metadata { get; private set; }

    /// <summary>
    /// 初始化属性
    /// </summary>
    internal void InitializeProperties()
    {
        // 处理 ChainedConfigurationProvider 配置提供器
        if (Provider is ChainedConfigurationProvider chainedConfigurationProvider)
        {
            // 获取配置根对象
            var configurationRoot = (IConfigurationRoot)chainedConfigurationProvider.Configuration;

            // 生成配置元数据集合
            Metadata = configurationRoot.Providers.Select(provider => new ConfigurationMetadata(provider));

            // 初始化数据字典
            var data = new Dictionary<string, string?>();

            // 合并所有配置提供器数据字典
            foreach (var providerData in Metadata.Select(m => m.Data))
            {
                // 空检查
                if (providerData is null)
                {
                    continue;
                }

                data.AddOrUpdate(providerData);
            }

            Data = data;

            return;
        }

        // 获取配置提供器类型
        var providerType = Provider.GetType();

        // 检查是否定义了 Data 属性
        var dataProperty = providerType.GetProperty(nameof(Data), _bindingAttr);
        if (dataProperty is not null)
        {
            // 获取 Data 属性值访问器
            var dataGetter = providerType.CreatePropertyGetter(dataProperty);

            // 解析 Data 属性值并设置
            Data = dataGetter(Provider) as IDictionary<string, string?>;
        }

        // 检查是否定义了 Source 属性
        var sourceProperty = providerType.GetProperty(nameof(Source), _bindingAttr);
        if (sourceProperty is not null)
        {
            // 获取 Source 属性值访问器
            var sourceGetter = providerType.CreatePropertyGetter(sourceProperty);

            // 解析 Source 属性值并设置
            Source = sourceGetter(Provider) as IConfigurationSource;
        }
    }

    /// <summary>
    /// 转换为 JSON 字符串
    /// </summary>
    /// <param name="jsonWriterOptions"><see cref="JsonWriterOptions"/></param>
    /// <returns><see cref="string"/></returns>
    public string ConvertToJson(JsonWriterOptions jsonWriterOptions = default)
    {
        // 创建一个内存流，用于存储生成的 JSON 数据
        using var stream = new MemoryStream();

        // 创建一个 Utf8JsonWriter 对象来写入 JSON 数据到内存流中
        using (var jsonWriter = new Utf8JsonWriter(stream, jsonWriterOptions))
        {
            // 生成 JSON 数据
            BuildJson(Data, jsonWriter);
        }

        // 将内存流中的数据转换为字符串并返回
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    /// <summary>
    /// 生成 JSON 数据
    /// </summary>
    /// <param name="data">数据字典</param>
    /// <param name="jsonWriter"><see cref="Utf8JsonWriter"/></param>
    internal static void BuildJson(IDictionary<string, string?>? data, Utf8JsonWriter jsonWriter)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(jsonWriter);

        // 空检查
        if (data is null)
        {
            jsonWriter.WriteNullValue();
            return;
        }

        // 空集合检查
        if (data.Count == 0)
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteEndObject();
            return;
        }

        // 写入 JSON 对象的起始括号
        jsonWriter.WriteStartObject();

        // 遍历配置的每个子节点
        foreach (var key in data.Keys)
        {
            // 将节点 Key 进行 : 分割
            var children = key.Split(':');

            // 如果子节点有子节点，说明是一个嵌套的配置节点
            if (children.Length > 1)
            {
                // 写入嵌套节点的名称
                jsonWriter.WritePropertyName(children[0]);

                // 递归调用生成嵌套节点的 JSON 数据
                BuildJson(new Dictionary<string, string?>
                {
                    { string.Join(':', children.Skip(1)), data[key] }
                }, jsonWriter);
            }
            else
            {
                // 如果子节点没有子节点，说明是一个键值对
                jsonWriter.WriteString(key, data[key]);
            }
        }

        // 写入 JSON 对象的结束括号
        jsonWriter.WriteEndObject();
    }
}