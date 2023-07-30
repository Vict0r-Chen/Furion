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
/// 链式验证器特性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = false)]
public class CustomizeValidatorAttribute : ValidationAttribute
{
    /// <inheritdoc cref="ValidatorCascadeMode" />
    public ValidatorCascadeMode CascadeMode { get; set; }

    /// <inheritdoc cref="ValidatorOptions.SuppressAnnotationValidation" />
    public bool SuppressAnnotationValidation { get; set; } = true;

    /// <inheritdoc cref="ValidatorOptions.ValidateAllPropertiesForObjectAnnotationValidator" />
    public bool ValidateAllPropertiesForObjectAnnotationValidator { get; set; } = true;

    /// <summary>
    /// 规则集
    /// </summary>
    public string? RuleSet { get; set; }

    /// <inheritdoc />
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // 检查对象合法性
        return IsValid(value, modelType =>
        {
            // 解析验证器服务
            return validationContext.GetService(typeof(IObjectValidator<>)
                .MakeGenericType(modelType)) as IObjectValidator;
        });
    }

    /// <summary>
    /// 检查对象合法性
    /// </summary>
    /// <param name="value">对象值</param>
    /// <param name="createValidatorFactory">验证器创建工厂</param>
    /// <returns><see cref="ValidationResult"/></returns>
    internal ValidationResult? IsValid(object? value, Func<Type, IObjectValidator?> createValidatorFactory)
    {
        // 检查是否可以执行验证程序
        if (!CanValidate(value))
        {
            return ValidationResult.Success;
        }

        // 空检查
        ArgumentNullException.ThrowIfNull(createValidatorFactory);

        // 解析验证器服务
        var objectValidator = createValidatorFactory(value!.GetType());

        // 空检查
        if (objectValidator is null)
        {
            // 输出调试事件
            Debugging.Warn("Validator service of type {0} not found.", value!.GetType());

            return ValidationResult.Success;
        }

        // 配置验证器选项
        ConfigureOptions(objectValidator);

        // 获取验证结果
        var validationResults = objectValidator.GetValidationResults(value!, RuleSet);

        // 如果验证失败则返回首条验证结果
        if (validationResults is not null)
        {
            var validationResult = validationResults.First();

            // 检查是否配置了错误消息
            if (ErrorMessage is not null)
            {
                validationResult.ErrorMessage = ErrorMessage;
            }

            // 本地化 TODO!

            return validationResult;
        }

        return ValidationResult.Success;
    }

    /// <summary>
    /// 检查是否可以执行验证程序
    /// </summary>
    /// <param name="value">对象值</param>
    /// <returns><see cref="bool"/></returns>
    internal static bool CanValidate(object? value)
    {
        // 检查对象值是否是 null、string 或 IEnumerable 类型，如果是则跳过
        if (value is null || value is string || value is IEnumerable)
        {
            return false;
        }

        // 检查对象类型是否是基元类型、数组类型或非引用类型，如果是则跳过
        var valueType = value.GetType();
        if (valueType.IsPrimitive
            || valueType.IsArray
            || !valueType.IsClass)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 配置验证器选项
    /// </summary>
    /// <param name="objectValidator"><see cref="IObjectValidator"/></param>
    internal void ConfigureOptions(IObjectValidator objectValidator)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(objectValidator);

        // 获取验证器选项
        var validatorOptions = objectValidator.Options;

        // 同步验证器选项属性
        validatorOptions.SuppressAnnotationValidation = SuppressAnnotationValidation;
        validatorOptions.CascadeMode = CascadeMode;
        validatorOptions.ValidateAllPropertiesForObjectAnnotationValidator = ValidateAllPropertiesForObjectAnnotationValidator;
    }
}

/// <summary>
/// 链式验证器特性
/// </summary>
/// <typeparam name="TValidator"><see cref="AbstractValidator{T}"/></typeparam>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = false)]
public sealed class CustomizeValidatorAttribute<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TValidator> : CustomizeValidatorAttribute
    where TValidator : class, IObjectValidator
{
    /// <summary>
    /// <inheritdoc cref="CustomizeValidatorAttribute{TValidator}" />
    /// </summary>
    public CustomizeValidatorAttribute()
    {
        // 检查验证器类型合法性
        Helpers.EnsureLegalValidatorType(typeof(TValidator));
    }

    /// <inheritdoc />
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // 检查对象合法性
        return IsValid(value, modelType =>
        {
            // 获取验证器模型类型
            var validatorType = typeof(TValidator);

            // 检查验证器基类 AbstractValidator<T> 的泛型类型是否和验证模型类型一致
            if (modelType != validatorType.BaseType!.GenericTypeArguments[0])
            {
                throw new InvalidOperationException($"Unable to set Validator `validatorType` to Type `{modelType}`.");
            }

            // 初始化验证器服务
            return ActivatorUtilities.CreateInstance(validationContext, validatorType) as IObjectValidator;
        });
    }
}