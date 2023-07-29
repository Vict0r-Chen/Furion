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
    /// 验证器委托器
    /// </summary>
    /// <typeparam name="TValidator"><see cref="ValidatorBase"/></typeparam>
    internal sealed class ValidatorDelegator<TValidator> : ValidatorBase<T>
        where TValidator : ValidatorBase
    {
        /// <inheritdoc cref="PropertyValidator{T, TProperty}" />
        internal readonly PropertyValidator<T, TProperty> _propertyValidator;

        /// <summary>
        /// 构造函数参数访问器
        /// </summary>
        internal readonly Func<T, object?[]?> _constructorParametersAccessor;

        /// <summary>
        /// <inheritdoc cref="ValidatorDelegator{TValidator}" />
        /// </summary>
        /// <param name="propertyValidator"><see cref="PropertyValidator{T, TProperty}"/></param>
        /// <param name="constructorParametersAccessor">构造函数参数访问器</param>
        /// <param name="errorMessageResourceAccessor">错误消息资源访问器</param>
        public ValidatorDelegator(PropertyValidator<T, TProperty> propertyValidator
            , Func<T, object?[]?> constructorParametersAccessor
            , Func<string> errorMessageResourceAccessor)
            : base(errorMessageResourceAccessor)
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(propertyValidator);
            ArgumentNullException.ThrowIfNull(constructorParametersAccessor);
            ArgumentNullException.ThrowIfNull(errorMessageResourceAccessor);

            _propertyValidator = propertyValidator;
            _constructorParametersAccessor = constructorParametersAccessor;
        }

        /// <inheritdoc cref="ValidatorBase" />
        internal TValidator? Validator { get; private set; }

        /// <summary>
        /// 属性值
        /// </summary>
        internal object? PropertyValue { get; private set; }

        /// <summary>
        /// 自定义配置委托
        /// </summary>
        internal Action<TValidator>? ValidatorConfigure { get; private set; }

        /// <inheritdoc />
        public override bool IsValid(T instance)
        {
            // 初始化
            Initialize(instance);

            return Validator!.IsValid(PropertyValue);
        }

        /// <inheritdoc />
        public override List<ValidationResult>? GetValidationResults(T instance, string name)
        {
            // 初始化
            Initialize(instance);

            return Validator!.GetValidationResults(PropertyValue, name ?? _propertyValidator.PropertyName);
        }

        /// <inheritdoc />
        public override string FormatErrorMessage(string name, T instance)
        {
            // 初始化
            Initialize(instance);

            return Validator!.FormatErrorMessage(name ?? _propertyValidator.PropertyName, PropertyValue);
        }

        /// <inheritdoc />
        public override void Validate(T instance, string name)
        {
            // 初始化
            Initialize(instance);

            Validator!.Validate(PropertyValue, name ?? _propertyValidator.PropertyName);
        }

        /// <summary>
        /// 配置验证器
        /// </summary>
        /// <param name="configure">自定义配置委托</param>
        public void Configure(Action<TValidator> configure)
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(configure);

            ValidatorConfigure = configure;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="instance">对象实例</param>
        internal void Initialize(T instance)
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(instance);

            // 创建验证器
            Validator ??= Activator.CreateInstance(typeof(TValidator), _constructorParametersAccessor(instance)) as TValidator;

            // 空检查
            ArgumentNullException.ThrowIfNull(Validator);

            // 设置错误消息
            Validator.ErrorMessage = ErrorMessage;

            // 调用自定义配置委托
            ValidatorConfigure?.Invoke(Validator);

            // 初始化属性值
            PropertyValue = _propertyValidator.GetPropertyValue(instance);
        }
    }

    /// <inheritdoc cref="ObjectValidator{T}" />
    internal readonly ObjectValidator<T> _objectValidator;

    /// <inheritdoc cref="PropertyAnnotationValidator{T, TProperty}" />
    internal readonly PropertyAnnotationValidator<T, TProperty> _annotationValidator;

    /// <summary>
    /// <inheritdoc cref="PropertyValidator{T, TProperty}"/>
    /// </summary>
    /// <param name="objectValidator"><see cref="ObjectValidator{T}"/></param>
    /// <param name="propertyExpression">属性选择器</param>
    internal PropertyValidator(ObjectValidator<T> objectValidator
        , Expression<Func<T, TProperty?>> propertyExpression)
    {
        _objectValidator = objectValidator;
        PropertyName = propertyExpression.GetPropertyName();

        Validators = new();
        _annotationValidator = new(propertyExpression);
    }

    /// <summary>
    /// 验证器集合
    /// </summary>
    public List<ValidatorBase> Validators { get; init; }

    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropertyName { get; init; }

    /// <inheritdoc />
    public bool SuppressAnnotationValidation { get; set; } = true;

    /// <summary>
    /// 验证对象访问器
    /// </summary>
    internal Func<T, ValidatorBase, object?, object?>? ValidationObjectAccessor { get; private set; }

    /// <summary>
    /// 执行验证的符合条件表达式
    /// </summary>
    internal Func<ValidationContext, bool>? ConditionExpression { get; private set; }

    /// <summary>
    /// 子属性验证器
    /// </summary>
    internal IObjectValidator<TProperty>? SubValidator { get; private set; }

    /// <summary>
    /// 启用/禁用注解（特性）验证
    /// </summary>
    /// <param name="enable">是否启用</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> WithAnnotationValidation(bool enable = true)
    {
        SuppressAnnotationValidation = !enable;

        return this;
    }

    /// <summary>
    /// 设置错误消息
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> WithErrorMessage(string? errorMessage)
    {
        Validators.LastOrDefault()?.WithErrorMessage(errorMessage);

        return this;
    }

    /// <summary>
    /// 设置验证对象访问器
    /// </summary>
    /// <param name="validationObjectAccessor">验证对象访问器</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> ConfigureValidationObject(Func<T, ValidatorBase, object?, object?> validationObjectAccessor)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validationObjectAccessor);

        ValidationObjectAccessor = validationObjectAccessor;

        return this;
    }

    /// <inheritdoc />
    public IObjectValidator<T> When(Func<T, bool> conditionExpression)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(conditionExpression);

        // 配置执行验证的符合条件表达式
        return WhenContext(context => conditionExpression((T)context.ObjectInstance));
    }

    /// <inheritdoc />
    public IObjectValidator<T> WhenContext(Func<ValidationContext, bool> conditionExpression)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(conditionExpression);

        ConditionExpression = conditionExpression;

        return this;
    }

    /// <inheritdoc />
    public IObjectValidator<T> Reset()
    {
        ConditionExpression = null;

        return this;
    }

    /// <summary>
    /// 检查是否可以执行验证程序
    /// </summary>
    /// <param name="instance">对象实例</param>
    /// <returns><see cref="bool"/></returns>
    internal bool CanValidate(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 检查是否设置了条件表达式
        if (ConditionExpression is null)
        {
            return true;
        }

        // 初始化验证上下文
        var validationContext = new ValidationContext(instance, _objectValidator.Items);

        // 调用条件表达式并返回
        return ConditionExpression(validationContext);
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

        // 检查是否启用注解（特性）验证
        var isValid = SuppressAnnotationValidation || _annotationValidator.IsValid(instance);

        // 获取属性值
        var propertyValue = GetPropertyValue(instance);

        // 检查是否设置了子属性验证器
        if (SubValidator is null)
        {
            return isValid && Validators
                .All(validator => validator.IsValid(GetValidationObject(instance, validator, propertyValue)));
        }

        // 空检查（子属性验证器 T 类型不能为 null）
        if (propertyValue is null)
        {
            return isValid;
        }

        return isValid && SubValidator.IsValid(propertyValue);
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

        // 初始化验证结果集合
        var validationResults = new List<ValidationResult>();

        // 检查是否启用注解（特性）验证
        if (!SuppressAnnotationValidation)
        {
            validationResults.AddRange(_annotationValidator
                .GetValidationResults(instance, PropertyName) ?? Enumerable.Empty<ValidationResult>());
        }

        // 获取属性值
        var propertyValue = GetPropertyValue(instance);

        // 检查是否设置了子属性验证器
        if (SubValidator is not null)
        {
            // 空检查（子属性验证器 T 类型不能为 null）
            if (propertyValue is not null)
            {
                validationResults.AddRange(SubValidator
                    .GetValidationResults(propertyValue) ?? Enumerable.Empty<ValidationResult>());
            }
        }
        else
        {
            // 获取所有验证器验证结果集合
            validationResults.AddRange(Validators
                .SelectMany(validator => validator.GetValidationResults(
                    GetValidationObject(instance, validator, propertyValue), PropertyName) ?? Enumerable.Empty<ValidationResult>()));
        }

        return validationResults.Count == 0 ? null : validationResults;
    }

    /// <inheritdoc />
    public void Validate(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 获取验证结果
        var validationResults = GetValidationResults(instance);

        // 空检查
        if (validationResults is null)
        {
            return;
        }

        // 初始化组合验证异常
        var validationExceptions = validationResults
            .Select(validationResult => new ValidationException(validationResult, null, instance));

        // 抛出组合验证异常
        throw new AggregateValidationException(validationExceptions);
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="instance">对象实例</param>
    /// <returns><see cref="object"/></returns>
    internal TProperty? GetPropertyValue(T instance)
    {
        return (TProperty?)_annotationValidator.GetPropertyValue(instance, PropertyName);
    }

    /// <summary>
    /// 获取验证对象
    /// </summary>
    /// <param name="instance">对象实例</param>
    /// <param name="validator"><see cref="ValidatorBase"/></param>
    /// <param name="propertyValue">属性值</param>
    /// <returns><see cref="object"/></returns>
    internal object? GetValidationObject(T instance, ValidatorBase validator, TProperty? propertyValue)
    {
        // 检查是否是验证器委托器类型
        var validatorType = validator.GetType();
        if (validatorType.IsGenericType
            && validatorType.GetGenericTypeDefinition() == typeof(ValidatorDelegator<>))
        {
            return instance;
        }

        // 检查是否设置了验证对象访问器
        return ValidationObjectAccessor is not null
            ? ValidationObjectAccessor(instance, validator, propertyValue)
            : propertyValue;
    }
}