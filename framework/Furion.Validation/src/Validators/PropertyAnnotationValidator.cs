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
/// 属性注解（特性）验证器
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
/// <typeparam name="TProperty">属性类型</typeparam>
public class PropertyAnnotationValidator<T, TProperty> : PropertyAnnotationValidator<T>
    where T : class
{
    /// <summary>
    /// 属性选择器
    /// </summary>
    internal Expression<Func<T, TProperty?>> _propertyExpression;

    /// <summary>
    /// <inheritdoc cref="PropertyAnnotationValidator{T, TProperty}"/>
    /// </summary>
    /// <param name="propertyExpression">属性选择器</param>
    public PropertyAnnotationValidator(Expression<Func<T, TProperty?>> propertyExpression)
        : base(Convert(propertyExpression))
    {
        _propertyExpression = propertyExpression;
    }

    /// <inheritdoc cref="PropertyAnnotationValidator{T}.PropertyExpression"/>
    public new Expression<Func<T, TProperty?>> PropertyExpression
    {
        get
        {
            return _propertyExpression;
        }
        set
        {
            // 转换表达式
            base.PropertyExpression = Convert(value);
            _propertyExpression = value;
        }
    }

    /// <summary>
    /// 转换表达式
    /// </summary>
    /// <param name="propertyExpression">属性选择器</param>
    /// <returns><see cref="Expression{TDelegate}"/></returns>
    internal static Expression<Func<T, object?>> Convert(Expression<Func<T, TProperty?>> propertyExpression)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(propertyExpression);

        // 创建 Expression<Func<T, object?>> 表达式
        return Expression.Lambda<Func<T, object?>>(Expression.Convert(propertyExpression.Body, typeof(object))
            , propertyExpression.Parameters[0]);
    }
}

/// <summary>
/// 属性注解（特性）验证器
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
public class PropertyAnnotationValidator<T> : ValidatorBase<T>
    where T : class
{
    /// <summary>
    /// <inheritdoc cref="PropertyAnnotationValidator{T}"/>
    /// </summary>
    /// <param name="propertyExpression">属性选择器</param>
    public PropertyAnnotationValidator(Expression<Func<T, object?>> propertyExpression)
        : base()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(propertyExpression);

        PropertyExpression = propertyExpression;
    }

    /// <summary>
    /// 属性选择器
    /// </summary>
    public Expression<Func<T, object?>> PropertyExpression { get; set; }

    /// <summary>
    /// 属性名称
    /// </summary>
    internal string PropertyName
    {
        get
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(PropertyExpression);

            // 解析表达式属性名称
            return PropertyExpression.GetPropertyName();
        }
    }

    /// <summary>
    /// 属性值访问器
    /// </summary>
    internal Func<T, object?>? Getter { get; private set; }

    /// <inheritdoc />
    public override bool IsValid(T value)
    {
        return TryValidate(value, out _);
    }

    /// <inheritdoc />
    public override List<ValidationResult>? GetValidationResults(T value, string name)
    {
        // 检查属性注解（特性）合法性
        if (TryValidate(value, out var validationResults))
        {
            return null;
        }

        // 检查是否配置了自定义错误消息
        if (ErrorMessage is not null)
        {
            validationResults.Insert(0, new ValidationResult(FormatErrorMessage(name, value), new[] { name }));
        }

        return validationResults;
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name, T value)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name ?? PropertyName);
    }

    /// <summary>
    /// 检查属性注解（特性）合法性
    /// </summary>
    /// <param name="instance">对象实例</param>
    /// <param name="validationResults"><see cref="List{T}"/></param>
    /// <returns><see cref="bool"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal bool TryValidate(T instance, out List<ValidationResult> validationResults)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 获取属性名称
        var propertyName = PropertyName;

        // 初始化验证上下文
        var validationContext = new ValidationContext(instance)
        {
            MemberName = propertyName
        };
        validationResults = new();

        // 调用 Validator.TryValidateProperty 静态方法验证
        return Validator.TryValidateProperty(GetPropertyValue(instance, propertyName), validationContext, validationResults);
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="propertyName">属性名称</param>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <returns><see cref="object"/></returns>
    internal object? GetPropertyValue(T instance, string propertyName)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        // 空检查
        if (Getter is null)
        {
            // 创建 t 表达式
            var paramExpression = Expression.Parameter(typeof(T));

            // 创建 t.Property 表达式
            var lambdaExpression = Expression.Lambda(Expression.Property(paramExpression, propertyName)
                , paramExpression);

            // 创建 t => t.Property 表达式
            Getter = (Func<T, object?>)lambdaExpression.Compile();
        }

        return Getter(instance);
    }
}