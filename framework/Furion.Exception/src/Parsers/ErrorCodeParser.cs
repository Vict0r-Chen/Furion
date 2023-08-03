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
    /// 错误码消息集合
    /// </summary>
    internal readonly ConcurrentDictionary<string, string> _errorCodeMessages;

    /// <summary>
    /// <inheritdoc cref="ErrorCodeParser" />
    /// </summary>
    internal ErrorCodeParser()
    {
        _errorCodeMessages = new(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 解析错误码并返回错误消息
    /// </summary>
    /// <param name="errorCode">错误码</param>
    /// <returns><see cref="string"/></returns>
    internal string Parse(object? errorCode)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(errorCode);

        // 检查错误码是否是字符串类型
        if (errorCode is string stringErrorCode)
        {
            return stringErrorCode;
        }

        // 获取错误码类型
        var errorCodeType = errorCode.GetType();

        // 检查错误码是否是枚举类型
        if (errorCodeType.IsEnum)
        {
            return ParseEnum(errorCode);
        }

        return errorCode.ToString()!;
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

        // 返回或
        return _errorCodeMessages.GetOrAdd($"{enumType}.{enumName}", _ => errorCode.GetEnumDescription());
    }
}