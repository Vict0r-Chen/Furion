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
/// 类型验证器抽象基类
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
public abstract class AbstractValidator<T> : IObjectValidator<T>
    where T : class
{
    /// <summary>
    /// <see cref="ObjectValidator{T}"/>
    /// </summary>
    internal readonly ObjectValidator<T> _objectValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    public AbstractValidator()
    {
        _objectValidator = ObjectValidator<T>.Create();
    }

    /// <summary>
    /// 禁止注解（特性）验证
    /// </summary>
    private bool _suppressAnnotations = true;

    /// <summary>
    /// 禁止注解（特性）验证
    /// </summary>
    public bool SuppressAnnotations
    {
        get { return _suppressAnnotations; }
        set
        {
            _suppressAnnotations = value;
            _objectValidator.SuppressAnnotations = value;
        }
    }

    /// <summary>
    /// 创建属性验证器
    /// </summary>
    /// <typeparam name="TProperty">属性类型</typeparam>
    /// <param name="propertySelector">属性选择器</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty?>> propertySelector)
    {
        return _objectValidator.Property(propertySelector);
    }

    /// <inheritdoc />
    public IObjectValidator<T> When(Func<T, bool> condition)
    {
        return _objectValidator.When(condition);
    }

    /// <inheritdoc />
    public IObjectValidator<T> WhenContext(Func<ValidationContext, bool> condition)
    {
        return _objectValidator.WhenContext(condition);
    }

    /// <inheritdoc />
    public IObjectValidator<T> Reset()
    {
        return _objectValidator.Reset();
    }

    /// <inheritdoc />
    public bool IsValid(T instance)
    {
        return _objectValidator.IsValid(instance);
    }

    /// <inheritdoc />
    public List<ValidationResult>? GetValidationResults(T instance)
    {
        return _objectValidator.GetValidationResults(instance);
    }

    /// <summary>
    /// 执行验证
    /// </summary>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <exception cref="ValidationException"></exception>
    public void Validate(T instance)
    {
        _objectValidator.Validate(instance);
    }
}