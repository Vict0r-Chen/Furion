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
/// 文件配置解析器
/// </summary>
internal sealed partial class FileConfigurationParser
{
    /// <summary>
    /// 文件拓展名解析器
    /// </summary>
    internal readonly IDictionary<string, Func<Stream, IDictionary<string, string?>>> _parsers;

    /// <summary>
    /// 文件拓展名配置源
    /// </summary>
    internal readonly IDictionary<string, Type> _sourceTypes;

    /// <summary>
    /// JSON 文件解析器
    /// </summary>
    internal static Func<Stream, IDictionary<string, string?>>? _jsonParser;

    /// <summary>
    /// 构造函数
    /// </summary>
    internal FileConfigurationParser()
    {
        _parsers = new Dictionary<string, Func<Stream, IDictionary<string, string?>>>(StringComparer.OrdinalIgnoreCase)
        {
            {".json", ResolveJsonParser() },
            /*
             * {".xml", stream => XmlStreamConfigurationProvider.Read(stream, XmlDocumentDecryptor.Instance) },
             * {".ini", IniStreamConfigurationProvider.Read }
             */
        };

        _sourceTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            {".json", typeof(JsonConfigurationSource) },
            /*
             * {".xml", typeof(XmlConfigurationSource) },
             * {".ini", typeof(IniConfigurationSource) }
             */
        };
    }

    /// <summary>
    /// 添加文件拓展名解析器
    /// </summary>
    /// <param name="extension">文件拓展名</param>
    /// <param name="parser"><see cref="Func{T, TResult}"/></param>
    internal void AddParser(string extension, Func<Stream, IDictionary<string, string?>> parser)
    {
        // 检查文件拓展名有效性
        EnsureLegalExtension(extension);

        // 空检查
        ArgumentNullException.ThrowIfNull(parser);

        // 添加或更新解析器
        _parsers[extension] = parser;
    }

    /// <summary>
    /// 添加文件拓展名配置源
    /// </summary>
    /// <param name="extension">文件拓展名</param>
    /// <param name="sourceType">实现 <see cref="FileConfigurationSource"/> 的类型</param>
    /// <exception cref="InvalidOperationException"></exception>
    internal void AddSource(string extension, Type sourceType)
    {
        // 检查文件拓展名有效性
        EnsureLegalExtension(extension);

        // 空检查
        ArgumentNullException.ThrowIfNull(sourceType);

        // 检查是否派生自 FileConfigurationSource
        var baseType = typeof(FileConfigurationSource);
        if (!baseType.IsAssignableFrom(sourceType))
        {
            throw new ArgumentException($"The `{sourceType.Name}` type is not assignable from `{baseType.Name}`.", nameof(sourceType));
        }

        // 添加或更新配置源
        _sourceTypes[extension] = sourceType;
    }

    /// <summary>
    /// 创建文件拓展名配置源对象
    /// </summary>
    /// <param name="extension">文件拓展名</param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="FileConfigurationSource"/></returns>
    /// <exception cref="ArgumentException"></exception>
    internal FileConfigurationSource CreateSourceInstance(string extension, Action<FileConfigurationSource> configure)
    {
        // 检查文件拓展名有效性
        EnsureLegalExtension(extension);

        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        // 查找该拓展名配置源
        if (!_sourceTypes.TryGetValue(extension, out var sourceType))
        {
            throw new ArgumentException($"Configuration source for file with the extension `{extension}` not found.", nameof(extension));
        }

        // 创建文件配置源对象
        var fileConfigurationSource = Activator.CreateInstance(sourceType) as FileConfigurationSource;

        // 空检查
        ArgumentNullException.ThrowIfNull(fileConfigurationSource);

        // 调用自定义配置委托
        configure(fileConfigurationSource);

        // 初始化配置文件提供器
        fileConfigurationSource.ResolveFileProvider();

        return fileConfigurationSource;
    }

    /// <summary>
    /// 解析指定文件拓展名内容并返回字典集合
    /// </summary>
    /// <param name="extension">文件拓展名</param>
    /// <param name="stream"><see cref="Stream"/></param>
    /// <returns><see cref="IDictionary{TKey, TValue}"/></returns>
    /// <exception cref="ArgumentException"></exception>
    internal IDictionary<string, string?> Parse(string extension, Stream stream)
    {
        // 检查文件拓展名有效性
        EnsureLegalExtension(extension);

        // 空检查
        ArgumentNullException.ThrowIfNull(stream);

        // 查找该拓展名解析器
        if (!_parsers.TryGetValue(extension, out var parser))
        {
            throw new ArgumentException($"Configuration parser for file with the extension `{extension}` not found.", nameof(extension));
        }

        // 调用解析器解析
        var result = parser(stream);

        return result;
    }

    /// <summary>
    /// 检查文件拓展名有效性
    /// </summary>
    /// <param name="extension">文件拓展名</param>
    /// <exception cref="ArgumentException"></exception>
    internal static void EnsureLegalExtension(string extension)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);

        // 检查文件拓展名有效性
        if (FileExtensionRegex().IsMatch(extension))
        {
            return;
        }

        throw new ArgumentException($"The `{extension}` is not a valid file extension.", nameof(extension));
    }

    /// <summary>
    /// 解析 JSON 文件解析器
    /// </summary>
    /// <returns><see cref="Func{T, TResult}"/></returns>
    internal static Func<Stream, IDictionary<string, string?>> ResolveJsonParser()
    {
        // 检查 JSON 解析器是否已初始化
        if (_jsonParser is not null)
        {
            return _jsonParser;
        }

        // 获取 JsonConfigurationProvider 所在程序集
        var assembly = typeof(JsonConfigurationProvider).Assembly;

        // 查找 JsonConfigurationFileParser.Parse 静态方法
        var parseStaticMethodInfo = assembly.GetType($"{assembly.GetName().Name}.JsonConfigurationFileParser")
            ?.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, new[] { typeof(Stream) });

        // 空检查
        ArgumentNullException.ThrowIfNull(parseStaticMethodInfo);

        // 将静态方法转换为 Func 委托
        var @delegate = parseStaticMethodInfo.CreateDelegate<Func<Stream, IDictionary<string, string?>>>();
        _jsonParser = @delegate;

        return @delegate;
    }

    /// <summary>
    /// 文件拓展名正则表达式
    /// </summary>
    /// <returns><see cref="Regex"/></returns>
    [GeneratedRegex("^\\.[a-zA-Z0-9]+$")]
    private static partial Regex FileExtensionRegex();
}