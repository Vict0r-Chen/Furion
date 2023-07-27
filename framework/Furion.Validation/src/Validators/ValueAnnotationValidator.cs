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

namespace Furion.Validation;

/// <summary>
/// 单个值注解（特性）验证器
/// </summary>
public class ValueAnnotationValidator : ValidatorBase
{
    /// <summary>
    /// <inheritdoc cref="ValueAnnotationValidator"/>
    /// </summary>
    /// <param name="validationAttributes">验证特性集合</param>
    public ValueAnnotationValidator(params ValidationAttribute[] validationAttributes)
        : this((validationAttributes ?? throw new ArgumentNullException(nameof(validationAttributes))).ToList())
    {
    }

    /// <summary>
    /// <inheritdoc cref="ValueAnnotationValidator"/>
    /// </summary>
    /// <param name="validationAttributes">验证特性集合</param>
    public ValueAnnotationValidator(IList<ValidationAttribute> validationAttributes)
        : base()
    {
        // 检查验证特性集合合法性
        EnsureLegalData(validationAttributes);

        Attributes = validationAttributes;
    }

    /// <summary>
    /// 验证特性集合
    /// </summary>
    public IList<ValidationAttribute> Attributes { get; init; }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        return TryValidate(value, null!, out _);
    }

    /// <inheritdoc />
    public override List<ValidationResult>? GetValidationResults(object? value, string name)
    {
        // 检查单个值注解（特性）合法性
        if (TryValidate(value, name, out var validationResults))
        {
            return null;
        }

        // 检查是否配置了自定义错误消息
        if (ErrorMessage is not null)
        {
            validationResults.Insert(0, new ValidationResult(FormatErrorMessage(name, value), new[] { name }));
        }

        return validationResults;
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name, object? value = null)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name ?? "Value");
    }

    /// <summary>
    /// 检查单个值注解（特性）合法性
    /// </summary>
    /// <param name="value">对象值</param>
    /// <param name="name">显示名称</param>
    /// <param name="validationResults"><see cref="List{T}"/></param>
    /// <returns><see cref="bool"/></returns>
    internal bool TryValidate(object? value, string name, out List<ValidationResult> validationResults)
    {
        // 检查验证特性集合合法性
        EnsureLegalData(Attributes);

        // 如果定义了 [Required] 特性则优先验证
        var requiredAttribute = Attributes.OfType<RequiredAttribute>().FirstOrDefault();
        if (requiredAttribute is not null && value is null)
        {
            validationResults = new()
            {
                new (requiredAttribute.FormatErrorMessage(name),new[]{ name })
            };

            return false;
        }

        // 空检查
        ArgumentNullException.ThrowIfNull(value);

        // 初始化验证上下文
        var validationContext = new ValidationContext(value)
        {
            MemberName = name ?? "Value"
        };
        validationResults = new();

        // 调用 Validator.TryValidateValue 静态方法验证
        return Validator.TryValidateValue(value, validationContext, validationResults, Attributes);
    }

    /// <summary>
    /// 检查验证特性集合合法性
    /// </summary>
    /// <param name="validationAttributes">验证特性集合</param>
    /// <exception cref="ArgumentException"></exception>
    internal static void EnsureLegalData(IList<ValidationAttribute> validationAttributes)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validationAttributes);

        // 子项空检查
        if (validationAttributes.Any(attribute => attribute is null))
        {
            throw new ArgumentException("The validation attribute collection contains a null value.", nameof(validationAttributes));
        }
    }
}