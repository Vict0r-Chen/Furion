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
/// 类型验证器
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
public sealed class Validator<T> : IValidator<T>
    where T : class
{
    /// <summary>
    /// 属性验证器集合
    /// </summary>
    internal readonly List<IValidator<T>> _propertyValidators;

    /// <summary>
    /// <see cref="ObjectAnnotationValidator"/>
    /// </summary>
    internal readonly ObjectAnnotationValidator _objectAnnotationValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    public Validator()
    {
        _propertyValidators = new();
        _objectAnnotationValidator = new();
    }

    /// <inheritdoc />
    public bool SuppressAnnotations { get; set; } = true;

    /// <summary>
    /// 验证条件
    /// </summary>
    internal Func<ValidationContext, bool>? Condition { get; private set; }

    /// <summary>
    /// 附加属性
    /// </summary>
    internal IDictionary<object, object?>? Items { get; private set; }

    /// <summary>
    /// 创建类型验证器
    /// </summary>
    /// <returns><see cref="Validator{T}"/></returns>
    public static Validator<T> Create()
    {
        return new Validator<T>();
    }

    /// <summary>
    /// 创建类型验证器
    /// </summary>
    /// <param name="predicate">配置委托</param>
    /// <returns><see cref="Validator{T}"/></returns>
    public static Validator<T> Create(Action<Validator<T>> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        // 创建类型验证器实例
        var validator = Create();
        predicate(validator);

        return validator;
    }

    /// <summary>
    /// 创建属性验证器
    /// </summary>
    /// <typeparam name="TProperty">属性类型</typeparam>
    /// <param name="propertySelector">属性选择器</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty?>> propertySelector)
    {
        return new PropertyValidator<T, TProperty>(this, propertySelector);
    }

    /// <summary>
    /// 启用/禁用注解（特性）验证器
    /// </summary>
    /// <param name="enable">是否启用</param>
    /// <returns><see cref="Validate(T)"/></returns>
    public Validator<T> WithAnnotations(bool enable = true)
    {
        SuppressAnnotations = !enable;

        return this;
    }

    /// <inheritdoc />
    public IValidator<T> When(Func<T, bool> condition)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));

        return WhenContext(context => condition((T)context.ObjectInstance));
    }

    /// <inheritdoc />
    public IValidator<T> WhenContext(Func<ValidationContext, bool> condition)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));

        Condition = condition;

        return this;
    }

    /// <inheritdoc />
    public IValidator<T> Reset()
    {
        Condition = null;
        Items?.Clear();

        return this;
    }

    /// <summary>
    /// 检查是否可以执行验证程序
    /// </summary>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <returns><see cref="bool"/></returns>
    internal bool CanValidate(T instance)
    {
        if (Condition is null)
        {
            return true;
        }

        // 执行条件配置
        var validationContext = new ValidationContext(instance, new Dictionary<object, object?>());
        var result = Condition(validationContext);

        // 同步附加属性
        Items = validationContext.Items;

        return result;
    }

    /// <inheritdoc />
    public bool IsValid(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        // 检查是否可以执行验证程序
        if (!CanValidate(instance))
        {
            return true;
        }

        // 处理对象注解（特性）验证器
        var isValid = true;
        if (!SuppressAnnotations)
        {
            isValid = _objectAnnotationValidator.IsValid(instance);
        }

        return isValid && _propertyValidators.All(validator => validator.IsValid(instance));
    }

    /// <inheritdoc />
    public List<ValidationResult>? GetValidationResults(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        // 检查是否可以执行验证程序
        if (!CanValidate(instance))
        {
            return null;
        }

        var validationResults = new List<ValidationResult>();

        // 处理对象注解（特性）验证器
        if (!SuppressAnnotations)
        {
            validationResults.AddRange(_objectAnnotationValidator.GetValidationResults(instance, null!) ?? Enumerable.Empty<ValidationResult>());
        }

        // 获取所有验证器验证结果集合
        validationResults.AddRange(_propertyValidators.SelectMany(validator => validator.GetValidationResults(instance) ?? Enumerable.Empty<ValidationResult>()));

        if (validationResults.Count == 0)
        {
            return null;
        }

        return validationResults;
    }

    /// <inheritdoc />
    public void Validate(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        // 检查是否可以执行验证程序
        if (!CanValidate(instance))
        {
            return;
        }

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
    /// 添加属性验证器
    /// </summary>
    /// <typeparam name="TProperty">属性类型</typeparam>
    /// <param name="propertyValidator"><see cref="PropertyValidator{T, TProperty}" /></param>
    internal void AddPropertyValidator<TProperty>(PropertyValidator<T, TProperty> propertyValidator)
    {
        _propertyValidators.Add(propertyValidator);
    }
}