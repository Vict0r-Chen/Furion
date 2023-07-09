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
/// 验证器
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class Validator<T>
    where T : class
{
    /// <summary>
    /// 属性验证器
    /// </summary>
    internal readonly List<PropertyValidator<T>> _propertyValidators;

    /// <summary>
    /// 构造函数
    /// </summary>
    internal Validator()
    {
        _propertyValidators = new();
    }

    /// <summary>
    /// 创建规则
    /// </summary>
    /// <param name="propertySelector"></param>
    /// <returns></returns>
    public PropertyValidator<T> For(Expression<Func<T, object?>> propertySelector)
    {
        return new PropertyValidator<T>(this, propertySelector);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <returns></returns>
    public Validator<T> Create()
    {
        return this;
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <returns></returns>
    public static Validator<T> Create(Action<Validator<T>> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        var validator = new Validator<T>();
        predicate(validator);

        return validator;
    }

    /// <summary>
    /// 检查值有效性
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public bool IsValid(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        return _propertyValidators.All(validator => validator.IsValid(instance));
    }

    /// <summary>
    /// 获取验证结果
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public List<ValidationResult>? GetValidationResults(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        var validatorResults = _propertyValidators.SelectMany(validator => validator.GetValidationResults(instance) ?? Enumerable.Empty<ValidationResult>())
                                                                     .ToList();

        if (validatorResults.Count == 0)
        {
            return null;
        }

        return validatorResults;
    }

    /// <summary>
    /// 获取验证结果
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public ValidationResult? GetValidationResult(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        return GetValidationResults(instance)?.FirstOrDefault();
    }

    /// <summary>
    /// 执行验证
    /// </summary>
    /// <param name="instance"></param>
    public void Validate(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        // 获取验证结果
        var validationResult = GetValidationResult(instance);
        if (validationResult is null)
        {
            return;
        }

        // 抛出验证异常
        throw new ValidationException(validationResult, null, instance);
    }

    internal void AddProperty(PropertyValidator<T> propertyValidator)
    {
        _propertyValidators.Add(propertyValidator);
    }
}