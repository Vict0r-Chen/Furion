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
/// 错误码解析器
/// </summary>
internal sealed class ErrorCodeParser
{
    /// <summary>
    /// 延迟初始化 <see cref="ErrorCodeParser"/> 实例并保证线程安全
    /// </summary>
    private static readonly Lazy<ErrorCodeParser> _instance = new(() => new());

    /// <summary>
    /// 错误码消息集合
    /// </summary>
    internal readonly ConcurrentDictionary<string, string> _errorCodeMessages;

    /// <summary>
    /// <inheritdoc cref="ErrorCodeParser" />
    /// </summary>
    private ErrorCodeParser()
    {
        _errorCodeMessages = new(StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc cref="ErrorCodeParser" />
    internal static ErrorCodeParser Instance => _instance.Value;

    /// <summary>
    /// 解析错误码并返回错误消息
    /// </summary>
    /// <param name="errorCode">错误码</param>
    /// <param name="args">格式化参数</param>
    /// <returns><see cref="string"/></returns>
    internal static string Parse(object? errorCode, params object?[] args)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(errorCode);

        // 解析错误消息
        var errorMessage = errorCode switch
        {
            // 检查错误码是否是字符串类型
            string stringErrorCode => stringErrorCode,
            // 检查错误码是否是枚举类型
            var enumErrorCode when enumErrorCode.GetType().IsEnum => Instance.ParseEnum(enumErrorCode),
            // 缺省值
            _ => errorCode?.ToString() ?? string.Empty
        };

        // 格式化错误消息并返回
        return string.Format(CultureInfo.CurrentCulture, errorMessage, args);
    }

    /// <summary>
    /// 解析枚举类型错误码并返回错误消息
    /// </summary>
    /// <param name="errorCode">错误码</param>
    /// <returns><see cref="string"/></returns>
    internal string ParseEnum(object errorCode)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(errorCode);

        // 获取枚举类型
        var enumType = errorCode.GetType();

        // 获取枚举名称
        var enumName = Enum.GetName(enumType, errorCode);

        // 空检查
        ArgumentNullException.ThrowIfNull(enumName);

        // 查找或创建错误消息
        return _errorCodeMessages.GetOrAdd($"{enumType}.{enumName}", _ => errorCode.GetEnumDescription());
    }
}