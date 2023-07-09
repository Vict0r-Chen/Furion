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
    protected string ErrorMessageString => ErrorMessage ?? _errorMessageResourceAccessor();

    /// <summary>
    /// 自定义错误消息
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 检查值有效性
    /// </summary>
    /// <param name="value">验证的值</param>
    /// <returns><see cref="bool"/></returns>
    public abstract bool IsValid(object? value);

    /// <summary>
    /// 获取验证结果
    /// </summary>
    /// <param name="value">验证的值</param>
    /// <param name="name">显示名称</param>
    /// <returns><see cref="List{T}"/></returns>
    public virtual List<ValidationResult>? GetValidationResults(object? value, string name)
    {
        // 检查值有效性
        if (IsValid(value))
        {
            return null;
        }

        // 返回默认验证结果
        return new List<ValidationResult>
        {
            new ValidationResult(FormatErrorMessage(name),new[] { name })
        };
    }

    /// <summary>
    /// 获取验证结果
    /// </summary>
    /// <param name="value">验证的值</param>
    /// <param name="name">显示名称</param>
    /// <returns><see cref="ValidationResult"/></returns>
    public virtual ValidationResult? GetValidationResult(object? value, string name)
    {
        return GetValidationResults(value, name)?.FirstOrDefault();
    }

    /// <summary>
    /// 格式化错误消息
    /// </summary>
    /// <param name="name">显示名称</param>
    /// <returns><see cref="string"/></returns>
    public virtual string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
    }

    /// <summary>
    /// 执行验证
    /// </summary>
    /// <param name="value">验证的值</param>
    /// <param name="name">显示名称</param>
    /// <exception cref="ValidationException"></exception>
    public void Validate(object? value, string name)
    {
        // 获取验证结果
        var validationResult = GetValidationResult(value, name);
        if (validationResult is null)
        {
            return;
        }

        // 抛出验证异常
        throw new ValidationException(validationResult, null, value);
    }

    /// <summary>
    /// 验证器类型是否一致
    /// </summary>
    /// <param name="validatorType">验证器类型</param>
    /// <returns><see cref="bool"/></returns>
    internal bool IsSameAs(Type validatorType)
    {
        return GetType() == validatorType;
    }
}