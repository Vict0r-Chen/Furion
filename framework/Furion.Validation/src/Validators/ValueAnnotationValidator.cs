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
/// 单个值注解（特性）验证器
/// </summary>
public partial class ValueAnnotationValidator : ValidatorBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="validationAttributes">验证特性集合</param>
    public ValueAnnotationValidator(params ValidationAttribute[] validationAttributes)
        : this((IEnumerable<ValidationAttribute>)validationAttributes)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="validationAttributes">验证特性集合</param>
    public ValueAnnotationValidator(IEnumerable<ValidationAttribute> validationAttributes)
        : base()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validationAttributes, nameof(validationAttributes));

        Attributes = validationAttributes.ToList();
    }

    /// <summary>
    /// 验证特性集合
    /// </summary>
    public IList<ValidationAttribute> Attributes { get; init; }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        return TryValidate(value, out _, null!);
    }

    /// <inheritdoc />
    public override List<ValidationResult>? GetValidationResults(object? value, string name)
    {
        if (!TryValidate(value, out var validationResults, name))
        {
            // 处理自定义错误消息
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                validationResults.Insert(0, new ValidationResult(FormatErrorMessage(name)));
            }

            return validationResults;
        }

        return null;
    }

    /// <summary>
    /// 验证逻辑
    /// </summary>
    /// <param name="value">验证的值</param>
    /// <param name="validationResults"><see cref="ValidationResult"/> 集合</param>
    /// <param name="name">显示名称</param>
    /// <returns><see cref="bool"/></returns>
    internal bool TryValidate(object? value, out List<ValidationResult> validationResults, string name)
    {
        var displayName = name ?? value?.GetType().Name ?? "Value";

        // 如果定义了 [Required] 特性则优先验证
        var requiredAttribute = Attributes.OfType<RequiredAttribute>().FirstOrDefault();
        if (requiredAttribute is not null
            && value is null)
        {
            // 格式化错误消息
            var errorMessage = requiredAttribute.FormatErrorMessage(displayName);

            validationResults = new List<ValidationResult>()
            {
                new ValidationResult(errorMessage)
            };

            return false;
        }

        // 空检查
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        // 调用 Validator 静态类验证
        var validationContext = new ValidationContext(value)
        {
            MemberName = displayName
        };
        validationResults = new List<ValidationResult>();

        return Validator.TryValidateValue(value, validationContext, validationResults, Attributes);
    }
}