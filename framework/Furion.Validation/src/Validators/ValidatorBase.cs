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

namespace Furion.Validation;

/// <summary>
/// 验证器抽象基类
/// </summary>
public abstract partial class ValidatorBase
{
    /// <summary>
    /// 错误消息资源访问器
    /// </summary>
    internal readonly Func<string> _errorMessageResourceAccessor;

    /// <summary>
    /// 构造函数
    /// </summary>
    protected ValidatorBase()
        : this(() => Strings.ValidatorBase_Invalid)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="errorMessageAccessor">错误消息资源访问器</param>
    protected ValidatorBase(Func<string> errorMessageAccessor)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(errorMessageAccessor, nameof(errorMessageAccessor));

        _errorMessageResourceAccessor = errorMessageAccessor;
    }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    protected string ErrorMessageString
    {
        get
        {
            return _errorMessageResourceAccessor();
        }
    }

    /// <summary>
    /// 获取验证结果集合
    /// </summary>
    /// <param name="value">待验证的值</param>
    /// <returns><see cref="ValidationResult"/> 集合</returns>
    public virtual List<ValidationResult>? GetValidationResults(object? value)
    {
        if (IsValid(value))
        {
            return null;
        }

        return new List<ValidationResult> {
            new ValidationResult(FormatErrorMessage())
        };
    }

    /// <summary>
    /// 获取单个验证结果
    /// </summary>
    /// <param name="value">待验证的值</param>
    /// <returns><see cref="ValidationResult"/> 集合</returns>
    public virtual ValidationResult? GetValidationResult(object? value)
    {
        return GetValidationResults(value)?.FirstOrDefault();
    }

    /// <summary>
    /// 格式化错误消息
    /// </summary>
    /// <returns><see cref="string"/></returns>
    protected virtual string FormatErrorMessage()
    {
        var errorMessage = PlaceholderRegex().Replace(ErrorMessage ?? ErrorMessageString, string.Empty);
        return string.Format(CultureInfo.CurrentCulture, errorMessage);
    }

    /// <summary>
    /// 验证值有效性
    /// </summary>
    /// <param name="value">待验证的值</param>
    /// <exception cref="ValidationException"></exception>
    public void Validate(object? value)
    {
        if (IsValid(value))
        {
            return;
        }

        throw new ValidationException(GetValidationResult(value)?.ErrorMessage ?? ErrorMessage ?? ErrorMessageString);
    }

    /// <summary>
    /// 验证值有效性
    /// </summary>
    /// <param name="value">待验证的值</param>
    /// <returns><see cref="bool"/></returns>
    public abstract bool IsValid(object? value);

    /// <summary>
    /// 移除占位符正则表达式
    /// </summary>
    /// <returns><see cref="System.Text.RegularExpressions.Regex"/></returns>
    [GeneratedRegex(@"\{\d+\}\s*")]
    internal static partial Regex PlaceholderRegex();
}