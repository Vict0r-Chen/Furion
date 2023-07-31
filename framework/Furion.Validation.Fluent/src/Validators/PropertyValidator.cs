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

            return Validator!.GetValidationResults(PropertyValue, GetDisplayName(name));
        }

        /// <inheritdoc />
        public override string FormatErrorMessage(string name, T instance)
        {
            // 初始化
            Initialize(instance);

            return Validator!.FormatErrorMessage(GetDisplayName(name), PropertyValue);
        }

        /// <inheritdoc />
        public override void Validate(T instance, string name)
        {
            // 初始化
            Initialize(instance);

            Validator!.Validate(PropertyValue, GetDisplayName(name));
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

        /// <summary>
        /// 获取属性显示名称
        /// </summary>
        /// <param name="name">显示名称</param>
        /// <returns><see cref="string"/></returns>
        internal string GetDisplayName(string name)
        {
            return name ?? _propertyValidator.GetDisplayName();
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
        // 空检查
        ArgumentNullException.ThrowIfNull(objectValidator);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        _objectValidator = objectValidator;
        PropertyName = propertyExpression.GetPropertyName();

        Validators = new();
        _annotationValidator = new(propertyExpression);

        Options = new();
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
    public ValidatorOptions Options { get; init; }

    /// <summary>
    /// 验证对象解析器
    /// </summary>
    internal Func<T, ValidatorBase, object?, object?>? ValidationObjectResolver { get; private set; }

    /// <summary>
    /// 执行验证的符合条件表达式
    /// </summary>
    internal Func<ValidationContext, bool>? ConditionExpression { get; private set; }

    /// <summary>
    /// 子属性验证器
    /// </summary>
    internal IObjectValidator<TProperty>? SubValidator { get; private set; }

    /// <summary>
    /// 规则集
    /// </summary>
    public string[]? RuleSet { get; internal set; }

    /// <summary>
    /// 属性别名
    /// </summary>
    public string? DisplayName { get; private set; }

    /// <summary>
    /// 配置验证器选项
    /// </summary>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> ConfigureOptions(Action<ValidatorOptions> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        // 调用自定义配置委托
        configure?.Invoke(Options);

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
    /// 设置属性别名
    /// </summary>
    /// <param name="displayName">属性别名</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> WithDisplayName(string displayName)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

        DisplayName = displayName;

        return this;
    }

    /// <summary>
    /// 设置子属性验证器
    /// </summary>
    /// <param name="subValidator"><see cref="IObjectValidator{T}"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public PropertyValidator<T, TProperty> SetValidator(IObjectValidator<TProperty> subValidator)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(subValidator);

        SubValidator = subValidator;

        return this;
    }

    /// <summary>
    /// 设置验证对象解析器
    /// </summary>
    /// <param name="predicate">自定义委托</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> SetValidationObjectResolver(Func<T, ValidatorBase, object?, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate);

        ValidationObjectResolver = predicate;

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
    /// <param name="ruleSet">规则集</param>
    /// <returns><see cref="bool"/></returns>
    internal bool CanValidate(object instance, string? ruleSet = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 检查规则集
        if (!IsInRuleSet(ruleSet))
        {
            return false;
        }

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
    public bool IsValid(object instance, string? ruleSet = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 检查是否可以执行验证程序
        if (!CanValidate(instance, ruleSet))
        {
            return true;
        }

        // 检查是否启用注解（特性）验证
        var isValid = Options.SuppressAnnotationValidation || _annotationValidator.IsValid(instance);

        // 获取属性值
        var propertyValue = GetPropertyValue(instance);

        // 检查是否设置了子属性验证器
        if (SubValidator is null)
        {
            return isValid && Validators
                .All(validator => validator.IsValid(ResolveValidationObject(instance, validator, propertyValue)));
        }

        // 空检查（子属性验证器 T 类型不能为 null）
        if (propertyValue is null)
        {
            return isValid;
        }

        return isValid && SubValidator.IsValid(propertyValue);
    }

    /// <inheritdoc />
    public List<ValidationResult>? GetValidationResults(object instance, string? ruleSet = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 检查是否可以执行验证程序
        if (!CanValidate(instance, ruleSet))
        {
            return null;
        }

        // 初始化验证结果集合
        var validationResults = new List<ValidationResult>();

        // 检查是否启用注解（特性）验证
        if (!Options.SuppressAnnotationValidation)
        {
            validationResults.AddRange(_annotationValidator
                .GetValidationResults(instance, GetDisplayName()) ?? Enumerable.Empty<ValidationResult>());
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
                    .GetValidationResults(propertyValue, ruleSet) ?? Enumerable.Empty<ValidationResult>());
            }
        }
        else
        {
            // 获取所有验证器验证结果集合
            validationResults.AddRange(Validators
                .SelectMany(validator => validator.GetValidationResults(
                    ResolveValidationObject(instance, validator, propertyValue), GetDisplayName()) ?? Enumerable.Empty<ValidationResult>(), Options.CascadeMode));
        }

        return validationResults.Count == 0 ? null : validationResults;
    }

    /// <inheritdoc />
    public void Validate(object instance, string? ruleSet = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 获取验证结果
        var validationResults = GetValidationResults(instance, ruleSet);

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

    /// <inheritdoc />
    public bool IsInRuleSet(string? ruleSet = null)
    {
        // 如果未设置规则集且传入的规则集为 null 则通过
        if (ruleSet is null && RuleSet is null)
        {
            return true;
        }

        // 如果设置规则集为 * 则通过
        if (ruleSet == "*")
        {
            return true;
        }

        // 如果设置了规则集且传入的规则集在其中则通过
        if (RuleSet is not null && RuleSet.Contains(ruleSet))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="instance">对象实例</param>
    /// <returns><see cref="object"/></returns>
    internal TProperty? GetPropertyValue(object instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        return (TProperty?)_annotationValidator.GetPropertyValue((T)instance, PropertyName);
    }

    /// <summary>
    /// 解析验证对象
    /// </summary>
    /// <param name="instance">对象实例</param>
    /// <param name="validator"><see cref="ValidatorBase"/></param>
    /// <param name="propertyValue">属性值</param>
    /// <returns><see cref="object"/></returns>
    internal object? ResolveValidationObject(object instance, ValidatorBase validator, TProperty? propertyValue)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(validator);

        // 检查是否是验证器委托器类型
        var validatorType = validator.GetType();
        if (validatorType.IsGenericType
            && validatorType.GetGenericTypeDefinition() == typeof(ValidatorDelegator<>))
        {
            return instance;
        }

        // 检查是否设置了验证对象解析器
        return ValidationObjectResolver is not null
            ? ValidationObjectResolver((T)instance, validator, propertyValue)
            : propertyValue;
    }

    /// <summary>
    /// 获取属性显示名称
    /// </summary>
    /// <returns><see cref="string"/></returns>
    internal string GetDisplayName()
    {
        return DisplayName ?? PropertyName;
    }
}