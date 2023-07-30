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
/// 对象验证器
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
public sealed class ObjectValidator<T> : IObjectValidator<T>
    where T : class
{
    /// <summary>
    /// 属性验证器集合
    /// </summary>
    internal readonly List<IObjectValidator<T>> _propertyValidators;

    /// <inheritdoc cref="ObjectAnnotationValidator" />
    internal readonly ObjectAnnotationValidator _annotationValidator;

    /// <summary>
    /// <inheritdoc cref="ObjectValidator{T}"/>
    /// </summary>
    public ObjectValidator()
    {
        Options = new();

        _propertyValidators = new();
        _annotationValidator = new()
        {
            ValidateAllProperties = Options.ValidateAllPropertiesForAnnotationValidation
        };
    }

    /// <inheritdoc />
    public ValidatorOptions Options { get; init; }

    /// <summary>
    /// 执行验证的符合条件表达式
    /// </summary>
    internal Func<ValidationContext, bool>? ConditionExpression { get; private set; }

    /// <summary>
    /// 附加属性
    /// </summary>
    internal IDictionary<object, object?>? Items { get; private set; }

    /// <summary>
    /// 初始化对象验证器
    /// </summary>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="ObjectValidator{T}"/></returns>
    public static ObjectValidator<T> Create(Action<ObjectValidator<T>>? configure = null)
    {
        // 初始化对象验证器
        var validator = new ObjectValidator<T>();

        // 调用自定义配置委托
        configure?.Invoke(validator);

        return validator;
    }

    /// <summary>
    /// 配置验证器选项
    /// </summary>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="ObjectValidator{T}"/></returns>
    public ObjectValidator<T> ConfigureOptions(Action<ValidatorOptions> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        // 调用自定义配置委托
        configure?.Invoke(Options);

        // 同步注解（特性）验证器配置
        _annotationValidator.ValidateAllProperties = Options.ValidateAllPropertiesForAnnotationValidation;

        return this;
    }

    /// <summary>
    /// 初始化属性验证器
    /// </summary>
    /// <typeparam name="TProperty">属性类型</typeparam>
    /// <param name="propertyExpression">属性选择器</param>
    /// <param name="ruleSet">规则集</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty?>> propertyExpression, string? ruleSet = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(propertyExpression);

        // 初始化属性验证器
        var validator = new PropertyValidator<T, TProperty>(this, propertyExpression)
        {
            RuleSet = ruleSet?.Split(Helpers._ruleSetSeparator, StringSplitOptions.RemoveEmptyEntries)
        };

        // 将属性验证器添加到集合中
        _propertyValidators.Add(validator);

        return validator;
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
        Items = null;

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
        var validationContext = new ValidationContext(instance, new Dictionary<object, object?>());
        Items = validationContext.Items;

        // 调用条件表达式并返回
        return ConditionExpression(validationContext);
    }

    /// <inheritdoc />
    public bool IsValid(T instance, string? ruleSet = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 检查是否可以执行验证程序
        if (!CanValidate(instance))
        {
            return true;
        }

        // 检查是否启用注解（特性）验证，同时调用属性验证器集合进行验证
        return (Options.SuppressAnnotationValidation || _annotationValidator.IsValid(instance))
            && _propertyValidators.Where(v => v.IsInRuleSet(ruleSet))
                .All(validator => validator.IsValid(instance, ruleSet));
    }

    /// <inheritdoc />
    public List<ValidationResult>? GetValidationResults(T instance, string? ruleSet = null)
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
        if (!Options.SuppressAnnotationValidation)
        {
            validationResults.AddRange(_annotationValidator
                .GetValidationResults(instance, null!) ?? Enumerable.Empty<ValidationResult>());
        }

        // 获取属性验证器集合所有验证结果
        validationResults.AddRange(_propertyValidators
            .Where(v => v.IsInRuleSet(ruleSet))
            .SelectMany(validator => validator.GetValidationResults(instance, ruleSet) ?? Enumerable.Empty<ValidationResult>()));

        return validationResults.Count == 0 ? null : validationResults;
    }

    /// <inheritdoc />
    public void Validate(T instance, string? ruleSet = null)
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
        return true;
    }
}