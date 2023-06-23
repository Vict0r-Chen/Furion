﻿// 麻省理工学院许可证
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
/// 配置文件解析器
/// </summary>
internal sealed class ConfigurationFileParser
{
    /// <summary>
    /// 文件解析器集合
    /// </summary>
    internal readonly Dictionary<string, Func<Stream, IDictionary<string, string?>>> _fileParsers;

    /// <summary>
    /// 构造函数
    /// </summary>
    internal ConfigurationFileParser()
    {
        _fileParsers = new(StringComparer.OrdinalIgnoreCase)
        {
            {".json", CreateJsonFileParser() },
            {".xml",input => XmlStreamConfigurationProvider.Read(input, XmlDocumentDecryptor.Instance) },
            {".ini", IniStreamConfigurationProvider.Read }
        };
    }

    /// <summary>
    /// 解析配置文件流
    /// </summary>
    /// <param name="resourceName">资源名称</param>
    /// <param name="input"><see cref="Stream"/></param>
    /// <returns><see cref="IDictionary{TKey, TValue}"/></returns>
    internal IDictionary<string, string?>? Parse(string resourceName, Stream input)
    {
        var extension = Path.GetExtension(resourceName);
        if (_fileParsers.TryGetValue(extension, out var parser))
        {
            return parser(input);
        }

        return null;
    }

    /// <summary>
    /// 创建 JSON 文件解析器
    /// </summary>
    /// <returns><see cref="Func{T, TResult}"/></returns>
    internal Func<Stream, IDictionary<string, string?>> CreateJsonFileParser()
    {
        var assembly = typeof(JsonConfigurationProvider).Assembly;

        // 获取 JsonConfigurationFileParser.Parse 静态方法
        var parseMethod = assembly.GetType($"{assembly.GetName().Name}.JsonConfigurationFileParser")
                                           ?.GetMethod(nameof(Parse), BindingFlags.Public | BindingFlags.Static, new[] { typeof(Stream) });

        // 空检查
        ArgumentNullException.ThrowIfNull(parseMethod, nameof(parseMethod));

        // 创建 JSON 文件解析器
        return (Func<Stream, IDictionary<string, string?>>)parseMethod.CreateDelegate(typeof(Func<Stream, IDictionary<string, string?>>));
    }
}