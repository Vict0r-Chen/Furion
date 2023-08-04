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

namespace Furion.Exception;

/// <summary>
/// 异常编码解析器
/// </summary>
internal sealed class ExceptionCodeParser
{
    /// <summary>
    /// 延迟初始化 <see cref="ExceptionCodeParser"/> 实例并保证线程安全
    /// </summary>
    internal static readonly Lazy<ExceptionCodeParser> _instance = new(() => new());

    /// <summary>
    /// 异常编码信息缓存集合
    /// </summary>
    internal readonly ConcurrentDictionary<string, string> _codeMessagesCache;

    /// <summary>
    /// <inheritdoc cref="ExceptionCodeParser" />
    /// </summary>
    private ExceptionCodeParser()
    {
        _codeMessagesCache = new(StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc cref="ExceptionCodeParser" />
    internal static ExceptionCodeParser Instance => _instance.Value;

    /// <summary>
    /// 解析异常编码并返回异常信息
    /// </summary>
    /// <param name="code">异常编码</param>
    /// <param name="args">格式化参数</param>
    /// <returns><see cref="string"/></returns>
    internal static string Parse(object? code, params object?[] args)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(code);

        // 解析异常信息
        var message = code switch
        {
            // 检查异常编码是否是字符串类型
            string stringCode => stringCode,
            // 检查异常编码是否是枚举类型
            var enumCode when enumCode.GetType().IsEnum => Instance.ParseEnum(enumCode),
            // 其他类型
            _ => code?.ToString() ?? string.Empty
        };

        // 格式化异常信息并返回
        return string.Format(CultureInfo.CurrentCulture, message, args);
    }

    /// <summary>
    /// 解析枚举类型异常编码并返回异常信息
    /// </summary>
    /// <param name="code">异常编码</param>
    /// <returns><see cref="string"/></returns>
    internal string ParseEnum(object code)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(code);

        // 获取枚举类型
        var enumType = code.GetType();

        // 获取枚举名称
        var enumName = Enum.GetName(enumType, code);

        // 空检查
        ArgumentNullException.ThrowIfNull(enumName);

        // 查找或创建异常信息
        return _codeMessagesCache.GetOrAdd($"{enumType}.{enumName}", _ => code.GetEnumDescription());
    }
}