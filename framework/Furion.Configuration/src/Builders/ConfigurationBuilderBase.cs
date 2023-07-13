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
/// 配置构建器抽象基类
/// </summary>
public abstract class ConfigurationBuilderBase
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
    /// 构造函数
    /// </summary>
    internal ConfigurationBuilderBase()
    {
        _parsers = new Dictionary<string, Func<Stream, IDictionary<string, string?>>>(StringComparer.OrdinalIgnoreCase);
        _sourceTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 添加文件拓展名解析器
    /// </summary>
    /// <param name="extension">文件拓展名</param>
    /// <param name="parser"><see cref="Func{T, TResult}"/></param>
    public void AddParser(string extension, Func<Stream, IDictionary<string, string?>> parser)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);
        ArgumentNullException.ThrowIfNull(parser);

        _parsers[extension] = parser;
    }

    /// <summary>
    /// 添加文件拓展名配置源
    /// </summary>
    /// <param name="extension">文件拓展名</param>
    /// <param name="sourceType">实现 <see cref="FileConfigurationSource"/> 的类型</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddSource(string extension, Type sourceType)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);
        ArgumentNullException.ThrowIfNull(sourceType);

        _sourceTypes[extension] = sourceType;
    }

    /// <summary>
    /// 初始化 <see cref="FileConfigurationParser"/>
    /// </summary>
    /// <param name="fileConfigurationParser"><see cref="FileConfigurationParser"/></param>
    internal void Initialize(FileConfigurationParser fileConfigurationParser)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(fileConfigurationParser);

        // 添加文件拓展名解析器
        foreach (var (extension, parser) in _parsers)
        {
            fileConfigurationParser.AddParser(extension, parser);
        }

        // 添加文件拓展名配置源
        foreach (var (extension, sourceType) in _sourceTypes)
        {
            fileConfigurationParser.AddSource(extension, sourceType);
        }

        // 释放对象
        _parsers.Clear();
        _sourceTypes.Clear();
    }
}