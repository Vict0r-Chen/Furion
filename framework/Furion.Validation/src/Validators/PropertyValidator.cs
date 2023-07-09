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
/// 属性验证器
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class PropertyValidator<T>
    where T : class
{
    internal sealed class CustomValidator : ValidatorBase
    {
        internal CustomValidator(Func<T, bool> predicate)
            : base()
        {
            Predicate = predicate;
        }

        internal Func<T, bool> Predicate { get; init; }

        public override bool IsValid(object? instance)
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));

            return Predicate((T)instance);
        }
    }

    internal readonly Validator<T> _validator;

    internal PropertyValidator(Validator<T> validator, Expression<Func<T, object?>> propertyExpression)
    {
        Validators = new();
        PropertyName = propertyExpression.GetPropertyName();
        _validator = validator;

        _validator.AddProperty(this);
    }

    internal List<ValidatorBase> Validators { get; init; }

    internal string PropertyName { get; init; }

    internal Func<T, string>? ErrorMessageAccessor { get; private set; }

    /// <summary>
    /// 创建规则
    /// </summary>
    /// <param name="propertySelector"></param>
    /// <returns></returns>
    public PropertyValidator<T> ThenFor(Expression<Func<T, object?>> propertySelector)
    {
        return new PropertyValidator<T>(_validator, propertySelector);
    }

    /// <summary>
    /// 设置消息
    /// </summary>
    /// <param name="errorMessage"></param>
    public void WithErrorMessage(string errorMessage)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(errorMessage, nameof(errorMessage));

        ErrorMessageAccessor = (T instance) => errorMessage;
    }

    /// <summary>
    /// 设置消息
    /// </summary>
    /// <param name="errorMessageAccessor"></param>
    public void WithErrorMessage(Func<T, string> errorMessageAccessor)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(errorMessageAccessor, nameof(errorMessageAccessor));

        ErrorMessageAccessor = errorMessageAccessor;
    }

    /// <summary>
    /// 非空
    /// </summary>
    /// <returns></returns>
    public PropertyValidator<T> NotNull(bool allowEmptyStrings = false)
    {
        Validators.Add(new ValueAnnotationValidator(new RequiredAttribute
        {
            AllowEmptyStrings = allowEmptyStrings
        }));

        return this;
    }

    /// <summary>
    /// 非空
    /// </summary>
    /// <returns></returns>
    public PropertyValidator<T> NotEmpty()
    {
        Validators.Add(new NotEmptyValidator());

        return this;
    }

    /// <summary>
    /// 自定义
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public PropertyValidator<T> Custom(Func<T, bool> predicate)
    {
        Validators.Add(new CustomValidator(predicate));
        return this;
    }

    /// <summary>
    /// 组合验证器
    /// </summary>
    /// <param name="validators"></param>
    /// <returns></returns>
    public PropertyValidator<T> Composite(params ValidatorBase[] validators)
    {
        Validators.Add(new CompositeValidator(validators));
        return this;
    }

    /// <summary>
    /// 组合验证器
    /// </summary>
    /// <param name="validators"></param>
    /// <returns></returns>
    public PropertyValidator<T> Composite(IList<ValidatorBase> validators)
    {
        Validators.Add(new CompositeValidator(validators));
        return this;
    }

    /// <summary>
    /// 设置验证器
    /// </summary>
    /// <param name="validator"></param>
    /// <returns></returns>
    public PropertyValidator<T> SetValidator(ValidatorBase validator)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validator, nameof(validator));

        Validators.Add(validator);
        return this;
    }

    internal bool IsValid(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        var value = GetValue(instance);

        return Validators.All(validator => validator.IsValid(validator.IsSameAs(typeof(CustomValidator)) ? instance : value));
    }

    internal List<ValidationResult>? GetValidationResults(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        var value = GetValue(instance);

        var validatorResults = Validators.SelectMany(validator => validator.GetValidationResults(validator.IsSameAs(typeof(CustomValidator)) ? instance : value, PropertyName) ?? Enumerable.Empty<ValidationResult>())
                                                            .ToList();
        if (validatorResults.Count == 0)
        {
            return null;
        }

        if (ErrorMessageAccessor != null)
        {
            validatorResults.Insert(0, new ValidationResult(ErrorMessageAccessor(instance), new[] { PropertyName }));
        }

        return validatorResults;
    }

    internal object? GetValue(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        // 根据属性名称查找属性对象
        var propertyInfo = instance.GetType().GetProperty(PropertyName);
        ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

        // 获取属性值
        var propertyValue = propertyInfo.GetValue(instance);
        return propertyValue;
    }
}