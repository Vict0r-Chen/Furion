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
        IsFileConfiguration = provider is FileConfigurationProvider;

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
    /// 是否是文件配置
    /// </summary>
    public bool IsFileConfiguration { get; init; }

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
            // 创建 Data 属性值访问器并获取其值
            var dataGetter = providerType.CreatePropertyGetter(dataProperty);
            Data = dataGetter(Provider) as IDictionary<string, string?>;
        }

        // 检查是否定义了 Source 属性
        var sourceProperty = providerType.GetProperty(nameof(Source), _bindingAttr);
        if (sourceProperty is not null)
        {
            // 创建 Source 属性值访问器并获取其值
            var sourceGetter = providerType.CreatePropertyGetter(sourceProperty);
            Source = sourceGetter(Provider) as IConfigurationSource;
        }
    }

    /// <summary>
    /// 转换为 JSON 字符串
    /// </summary>
    /// <param name="jsonSerializerOptions"><see cref="JsonSerializerOptions"/></param>
    /// <returns><see cref="string"/></returns>
    public string ConvertToJson(JsonSerializerOptions? jsonSerializerOptions = default)
    {
        // 空检查
        if (Data is null)
        {
            return "null";
        }

        // 空集合检查
        if (Data.Count == 0)
        {
            return "{}";
        }

        // 创建一个嵌套的数据字典对象，用于保存层级结构的键值对
        var nestedDictionary = new Dictionary<string, object?>();

        // 遍历数据字典每一项
        foreach (var entry in Data)
        {
            // 将键按照 ":" 进行分割，以获取层级结构的键数组
            var keys = entry.Key.Split(':');

            // 获取当前键对应的值
            var value = entry.Value;

            // 创建一个指向嵌套数据字典的引用
            var nestedDict = nestedDictionary;

            // 根据层级结构遍历键数组
            for (var i = 0; i < keys.Length - 1; i++)
            {
                // 获取当前键
                var currentKey = keys[i];

                // 如果当前键不存在于嵌套数据字典，则添加一个新的嵌套数据字典
                if (!nestedDict!.ContainsKey(currentKey))
                {
                    nestedDict[currentKey] = new Dictionary<string, object?>();
                }

                // 将嵌套数据字典的引用指向当前键对应的值，进入下一层级
                nestedDict = (Dictionary<string, object?>)nestedDict[currentKey]!;
            }

            // 将最后一级键和对应的值添加到嵌套数据字典中
            nestedDict![keys[^1]] = value;
        }

        // 使用 JsonSerializer 对嵌套数据字典进行序列化，并返回 JSON 字符串
        return JsonSerializer.Serialize(nestedDictionary, jsonSerializerOptions);
    }
}