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
/// 嵌入资源配置解析器
/// </summary>
internal sealed class ManifestResourceConfigurationParser
{
    /// <inheritdoc cref="FileConfigurationParser" />
    internal readonly FileConfigurationParser _fileConfigurationParser;

    /// <summary>
    /// <inheritdoc cref="ManifestResourceConfigurationParser"/>
    /// </summary>
    /// <param name="fileConfigurationParser"><see cref="FileConfigurationParser"/></param>
    internal ManifestResourceConfigurationParser(FileConfigurationParser fileConfigurationParser)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(fileConfigurationParser);

        _fileConfigurationParser = fileConfigurationParser;
    }

    /// <summary>
    /// 解析嵌入资源并返回配置集合
    /// </summary>
    /// <param name="manifestResourceConfigurationModel"><see cref="ManifestResourceConfigurationModel"/></param>
    /// <returns><see cref="IDictionary{TKey, TValue}"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal IDictionary<string, string?> ParseResource(ManifestResourceConfigurationModel manifestResourceConfigurationModel)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(manifestResourceConfigurationModel);

        // 读取嵌入资源并返回文件流
        using var stream = manifestResourceConfigurationModel.Assembly.GetManifestResourceStream(manifestResourceConfigurationModel.ResourceName);

        // 空检查
        if (stream is null)
        {
            return new Dictionary<string, string?>();
        }

        // 限制最大的文件流大小为 10M
        if (stream.Length > 1024 * 1024 * 10)
        {
            throw new InvalidOperationException("The size of the embedded resource file content cannot exceed 10485760 (10M) bytes.");
        }

        // 调用文件配置解析器对象进行解析
        var keyValues = _fileConfigurationParser.Parse(manifestResourceConfigurationModel.Extension, stream);

        // 调试事件消息
        var debugMessage = "The embed resource `{0}` has been successfully loaded into the configuration.";

        // 检查是否设置了前缀
        if (!string.IsNullOrWhiteSpace(manifestResourceConfigurationModel.Prefix))
        {
            // 遍历字典集合并包装 Key
            keyValues = keyValues.ToDictionary(u => $"{manifestResourceConfigurationModel.Prefix.TrimEnd(':')}:{u.Key}"
                , u => u.Value
                , StringComparer.OrdinalIgnoreCase);

            debugMessage += " (Prefix is `{1}`)";
        }

        // 输出调试事件
        Debugging.File(debugMessage, manifestResourceConfigurationModel.ResourceName, manifestResourceConfigurationModel.Prefix?.TrimEnd(':'));

        return keyValues;
    }
}