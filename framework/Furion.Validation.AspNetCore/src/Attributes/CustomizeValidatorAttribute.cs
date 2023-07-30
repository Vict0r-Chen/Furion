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

using Microsoft.Extensions.DependencyInjection;

namespace Furion.Validation;

/// <summary>
/// 验证器特性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = false)]
public class CustomizeValidatorAttribute : ValidationAttribute
{
    /// <summary>
    /// <inheritdoc cref="CustomizeValidatorAttribute"/>
    /// </summary>
    public CustomizeValidatorAttribute()
    {
    }

    /// <inheritdoc cref="ValidatorCascadeMode" />
    public ValidatorCascadeMode CascadeMode { get; set; }

    /// <summary>
    /// 禁用注解（特性）验证
    /// </summary>
    public bool SuppressAnnotationValidation { get; set; } = true;

    /// <summary>
    /// 为对象注解（特性）配置是否验证所有属性
    /// </summary>
    public bool ValidateAllPropertiesForObjectAnnotationValidator { get; set; } = true;

    /// <summary>
    /// 规则集
    /// </summary>
    public string? RuleSet { get; set; }

    /// <inheritdoc />
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (!CanValidate(value))
        {
            return ValidationResult.Success;
        }

        var valueType = value!.GetType();

        var objectValidator = validationContext.GetService(typeof(IObjectValidator<>)
            .MakeGenericType(valueType)) as IObjectValidator;

        if (objectValidator is not null)
        {
            ConfigureOptions(objectValidator);

            var validationResults = objectValidator.GetValidationResults(value, RuleSet);
            if (validationResults is not null)
            {
                return validationResults.First();
            }
        }

        return ValidationResult.Success;
    }

    /// <summary>
    /// 能否被验证
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected bool CanValidate(object? value)
    {
        // 检查服务是否注册（流畅服务）
        if (value is null)
        {
            return false;
        }

        var valueType = value.GetType();

        if (value is string
            || valueType.IsPrimitive
            || valueType.IsArray
            || !valueType.IsClass
            || typeof(IEnumerable).IsAssignableFrom(valueType))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 配置验证特性
    /// </summary>
    /// <param name="objectValidator"></param>
    protected void ConfigureOptions(IObjectValidator objectValidator)
    {
        var validatorOptions = objectValidator.Options;
        validatorOptions.SuppressAnnotationValidation = SuppressAnnotationValidation;
        validatorOptions.CascadeMode = CascadeMode;
        validatorOptions.ValidateAllPropertiesForObjectAnnotationValidator = ValidateAllPropertiesForObjectAnnotationValidator;
    }
}

/// <summary>
/// 验证器特性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = false)]
public sealed class CustomizeValidatorAttribute<TValidator> : CustomizeValidatorAttribute
    where TValidator : class, IObjectValidator
{
    /// <summary>
    /// <inheritdoc cref="CustomizeValidatorAttribute"/>
    /// </summary>
    public CustomizeValidatorAttribute()
    {
        // 检查验证器类型合法性
        Helpers.EnsureLegalValidatorType(typeof(TValidator));
    }

    /// <inheritdoc />
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (!CanValidate(value))
        {
            return ValidationResult.Success;
        }

        var valueType = value!.GetType();

        var validatorType = typeof(TValidator);
        var modelType = validatorType.BaseType!.GenericTypeArguments[0];

        if (modelType != valueType)
        {
            throw new InvalidOperationException("验证器模型对不上");
        }

        var objectValidator = ActivatorUtilities.CreateInstance(validationContext, typeof(TValidator)) as IObjectValidator;

        if (objectValidator is not null)
        {
            ConfigureOptions(objectValidator);

            var validationResults = objectValidator.GetValidationResults(value, RuleSet);
            if (validationResults is not null)
            {
                return validationResults.First();
            }
        }

        return ValidationResult.Success;
    }
}