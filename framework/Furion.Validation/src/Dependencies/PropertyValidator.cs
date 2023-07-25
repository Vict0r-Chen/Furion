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
/// 属性验证器
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
/// <typeparam name="TProperty">属性类型</typeparam>
public sealed partial class PropertyValidator<T, TProperty> : IObjectValidator<T>
    where T : class
{
    /// <summary>
    /// <see cref="ObjectValidator{T}"/>
    /// </summary>
    internal readonly ObjectValidator<T> _objectValidator;

    /// <summary>
    /// <see cref="PropertyAnnotationValidator{T}"/>
    /// </summary>
    internal readonly PropertyAnnotationValidator _propertyAnnotationValidator;

    /// <summary>
    /// <inheritdoc cref="PropertyValidator{T, TProperty}"/>
    /// </summary>
    /// <param name="objectValidator"><see cref="ObjectValidator{T}"/></param>
    /// <param name="propertyExpression">属性选择器</param>
    internal PropertyValidator(ObjectValidator<T> objectValidator, Expression<Func<T, TProperty?>> propertyExpression)
    {
        Validators = new();
        PropertyName = propertyExpression.GetPropertyName();

        // 将当前属性验证器添加到类型验证器集合中
        _objectValidator = objectValidator;
        _objectValidator.AddPropertyValidator(this);

        _propertyAnnotationValidator = new(PropertyName);
    }

    /// <summary>
    /// 验证器集合
    /// </summary>
    public List<ValidatorBase> Validators { get; }

    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropertyName { get; }

    /// <inheritdoc />
    public bool SuppressAnnotations { get; set; } = true;

    /// <summary>
    /// 错误消息访问器
    /// </summary>
    internal Func<T, string>? ErrorMessageAccessor { get; private set; }

    /// <summary>
    /// 验证对象访问器
    /// </summary>
    internal Func<ValidatorBase, T, object?, object?>? ValidationObjectAccessor { get; private set; }

    /// <summary>
    /// 验证条件
    /// </summary>
    internal Func<ValidationContext, bool>? Condition { get; private set; }

    /// <summary>
    /// 类型验证器
    /// </summary>
    internal IObjectValidator<TProperty>? Validator { get; private set; }

    /// <summary>
    /// 启用/禁用注解（特性）验证器
    /// </summary>
    /// <param name="enable">是否启用</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> WithAnnotations(bool enable = true)
    {
        SuppressAnnotations = !enable;

        return this;
    }

    /// <summary>
    /// 设置错误消息
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> WithErrorMessage(string errorMessage)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(errorMessage);

        ErrorMessageAccessor = (_) => errorMessage;

        return this;
    }

    /// <summary>
    /// 设置错误消息
    /// </summary>
    /// <param name="errorMessageAccessor">错误消息访问器</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> WithErrorMessage(Func<T, string> errorMessageAccessor)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(errorMessageAccessor);

        ErrorMessageAccessor = errorMessageAccessor;

        return this;
    }

    /// <summary>
    /// 设置验证对象访问器
    /// </summary>
    /// <param name="validationObjectAccessor">验证对象访问器</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> SetValidationObjectAccessor(Func<ValidatorBase, T, object?, object?> validationObjectAccessor)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validationObjectAccessor);

        ValidationObjectAccessor = validationObjectAccessor;

        return this;
    }

    /// <inheritdoc />
    public IObjectValidator<T> When(Func<T, bool> condition)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(condition);

        return WhenContext(context => condition((T)context.ObjectInstance));
    }

    /// <inheritdoc />
    public IObjectValidator<T> WhenContext(Func<ValidationContext, bool> condition)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(condition);

        Condition = condition;

        return this;
    }

    /// <inheritdoc />
    public IObjectValidator<T> Reset()
    {
        Condition = null;

        return this;
    }

    /// <summary>
    /// 检查是否可以执行验证程序
    /// </summary>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <returns><see cref="bool"/></returns>
    internal bool CanValidate(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        if (Condition is null)
        {
            return true;
        }

        return Condition(new ValidationContext(instance, _objectValidator.Items));
    }

    /// <inheritdoc />
    public bool IsValid(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 检查是否可以执行验证程序
        if (!CanValidate(instance))
        {
            return true;
        }

        // 处理属性注解（特性）验证器
        var isValid = true;
        if (!SuppressAnnotations)
        {
            isValid = _propertyAnnotationValidator.IsValid(instance);
        }

        // 获取属性值
        var propertyValue = GetPropertyValue(instance);

        // 处理设置类型验证器
        if (Validator is null)
        {
            return isValid && Validators.All(validator =>
                validator.IsValid(GetValidationObject(validator, instance, propertyValue)));
        }

        if (propertyValue is null)
        {
            return isValid;
        }

        return isValid && Validator.IsValid(propertyValue);
    }

    /// <inheritdoc />
    public List<ValidationResult>? GetValidationResults(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 检查是否可以执行验证程序
        if (!CanValidate(instance))
        {
            return null;
        }

        var validationResults = new List<ValidationResult>();

        // 处理属性注解（特性）验证器
        if (!SuppressAnnotations)
        {
            validationResults.AddRange(_propertyAnnotationValidator.GetValidationResults(instance, null!) ?? Enumerable.Empty<ValidationResult>());
        }

        // 获取属性值
        var propertyValue = GetPropertyValue(instance);

        // 处理设置类型验证器
        if (Validator is not null)
        {
            if (propertyValue is not null)
            {
                validationResults.AddRange(Validator.GetValidationResults(propertyValue) ?? Enumerable.Empty<ValidationResult>());
            }
        }
        else
        {
            // 获取所有验证器验证结果集合
            validationResults.AddRange(Validators.SelectMany(validator => validator.GetValidationResults(GetValidationObject(validator, instance, propertyValue), PropertyName) ?? Enumerable.Empty<ValidationResult>()));
        }

        if (validationResults.Count == 0)
        {
            return null;
        }

        // 添加自定义错误消息
        if (ErrorMessageAccessor is not null)
        {
            validationResults.Insert(0, new ValidationResult(ErrorMessageAccessor(instance), new[] { PropertyName }));
        }

        return validationResults;
    }

    /// <inheritdoc />
    public void Validate(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 获取验证结果
        var validationResults = GetValidationResults(instance);
        if (validationResults is null)
        {
            return;
        }

        // 创建组合异常
        var validationExceptions = validationResults.Select(validationResult => new ValidationException(validationResult, null, instance));

        // 抛出组合验证异常
        throw new AggregateValidationException(validationExceptions);
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <returns><see cref="object"/></returns>
    internal TProperty? GetPropertyValue(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 返回属性值
        return (TProperty?)GetPropertyInfo().GetValue(instance);
    }

    /// <summary>
    /// 获取属性信息
    /// </summary>
    /// <returns><see cref="PropertyInfo"/></returns>
    internal PropertyInfo GetPropertyInfo()
    {
        // 根据属性名称查找属性对象
        var propertyInfo = typeof(T).GetProperty(PropertyName);

        // 空检查
        ArgumentNullException.ThrowIfNull(propertyInfo);

        return propertyInfo;
    }

    /// <summary>
    /// 获取验证对象
    /// </summary>
    /// <param name="validator"><see cref="ValidatorBase"/></param>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <param name="propertyValue">属性值</param>
    /// <returns><see cref="object"/></returns>
    internal object? GetValidationObject(ValidatorBase validator, T instance, TProperty? propertyValue)
    {
        // 如果是自定义验证器则返回对象实例
        if (validator.IsSameAs(typeof(CustomValidator)))
        {
            return instance;
        }

        // 检查是否设置了验证对象访问器
        return ValidationObjectAccessor is not null
            ? ValidationObjectAccessor(validator, instance, propertyValue)
            : propertyValue;
    }

    /// <summary>
    /// 自定义验证器
    /// </summary>
    internal sealed class CustomValidator : ValidatorBase
    {
        /// <summary>
        /// 格式化参数访问器
        /// </summary>
        internal readonly Func<T, string?[]>? _formatArgsAccessor;

        /// <summary>
        /// <inheritdoc cref="CustomValidator"/>
        /// </summary>
        /// <param name="predicate">委托对象</param>
        /// <param name="defaultErrorMessage">默认错误消息</param>
        /// <param name="formatArgsAccessor">格式化参数访问器</param>
        internal CustomValidator(Func<T, bool> predicate
            , string? defaultErrorMessage = default
            , Func<T, string?[]>? formatArgsAccessor = default)
            : base(defaultErrorMessage)
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(predicate);

            Predicate = predicate;
            ErrorMessage = defaultErrorMessage;
            _formatArgsAccessor = formatArgsAccessor;
        }

        /// <summary>
        /// 委托对象
        /// </summary>
        internal Func<T, bool> Predicate { get; }

        /// <inheritdoc />
        public override bool IsValid(object? instance)
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(instance);

            return Predicate((T)instance);
        }

        /// <inheritdoc />
        public override string FormatErrorMessage(string name, object? instance = default)
        {
            var args = new List<string?> { name };

            if (_formatArgsAccessor is null)
            {
                return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, args.ToArray());
            }

            // 空检查
            ArgumentNullException.ThrowIfNull(instance);

            args.AddRange(_formatArgsAccessor((T)instance));

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, args.ToArray());
        }
    }
}