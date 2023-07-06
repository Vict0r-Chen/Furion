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
    /// 默认错误消息
    /// </summary>
    protected string ErrorMessageString
    {
        get
        {
            return _errorMessageResourceAccessor();
        }
    }

    /// <summary>
    /// 获取验证结果
    /// </summary>
    /// <param name="value">验证的值</param>
    /// <param name="memberNames">成员名称集合</param>
    /// <returns><see cref="List{T}"/></returns>
    public virtual List<ValidationResult>? GetValidationResults(object? value, IEnumerable<string>? memberNames = null)
    {
        if (IsValid(value))
        {
            return null;
        }

        return new List<ValidationResult> {
            new ValidationResult(FormatErrorMessage(memberNames), memberNames)
        };
    }

    /// <summary>
    /// 获取验证结果
    /// </summary>
    /// <param name="value">验证的值</param>
    /// <param name="memberNames">成员名称集合</param>
    /// <returns><see cref="ValidationResult"/></returns>
    public virtual ValidationResult? GetValidationResult(object? value, IEnumerable<string>? memberNames = null)
    {
        return GetValidationResults(value, memberNames)?.FirstOrDefault();
    }

    /// <summary>
    /// 设置错误消息
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    /// <returns><see cref="ValidatorBase"/></returns>
    internal virtual ValidatorBase WithErrorMessage(string errorMessage)
    {
        ErrorMessage = errorMessage;
        return this;
    }

    /// <summary>
    /// 格式化错误消息
    /// </summary>
    /// <param name="memberNames">成员名称集合</param>
    /// <returns><see cref="string"/></returns>
    protected virtual string FormatErrorMessage(IEnumerable<string>? memberNames = null)
    {
        // 获取错误消息
        var errorMessage = ErrorMessage ?? ErrorMessageString;

        // 检查是否设置成员名称，用于替换错误消息中的占位符
        if (memberNames is null)
        {
            return string.Format(CultureInfo.CurrentCulture, PlaceholderRegex().Replace(errorMessage, string.Empty));
        }

        return string.Format(CultureInfo.CurrentCulture, errorMessage, memberNames.ToArray());
    }

    /// <summary>
    /// 执行验证
    /// </summary>
    /// <param name="value">验证的值</param>
    /// <param name="memberNames">成员名称集合</param>
    /// <exception cref="ValidationException"></exception>
    public virtual void Validate(object? value, IEnumerable<string>? memberNames = null)
    {
        var validationResult = GetValidationResult(value, memberNames);
        if (validationResult is null)
        {
            return;
        }

        throw new ValidationException(validationResult, null, value);
    }

    /// <summary>
    /// 检查值有效性
    /// </summary>
    /// <param name="value">验证的值</param>
    /// <returns><see cref="bool"/></returns>
    public abstract bool IsValid(object? value);

    /// <summary>
    /// 占位符正则表达式
    /// </summary>
    /// <returns><see cref="System.Text.RegularExpressions.Regex"/></returns>
    [GeneratedRegex(@"\{\d+\}\s*")]
    internal static partial Regex PlaceholderRegex();
}