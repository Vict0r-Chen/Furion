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
    /// <inheritdoc cref="PropertyAnnotationValidator{T, TProperty}"/>
    /// </summary>
    /// <param name="propertyExpression">属性选择器</param>
    public PropertyAnnotationValidator(Expression<Func<T, TProperty?>> propertyExpression)
        : base(Convert(propertyExpression))
    {
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
    /// 反射搜索成员方式
    /// </summary>
    internal const BindingFlags _bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    /// <summary>
    /// <inheritdoc cref="PropertyAnnotationValidator{T}"/>
    /// </summary>
    /// <param name="propertyExpression">属性选择器</param>
    public PropertyAnnotationValidator(Expression<Func<T, object?>> propertyExpression)
        : base()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(propertyExpression);

        PropertyName = propertyExpression.GetPropertyName();
        Property = typeof(T).GetProperty(PropertyName, _bindingAttr)!;
        DisplayName = Property.GetCustomAttribute<DisplayNameAttribute>(false)?.DisplayName;
    }

    /// <summary>
    /// 属性名称
    /// </summary>
    internal string PropertyName { get; init; }

    /// <inheritdoc cref="Property"/>
    internal PropertyInfo Property { get; init; }

    /// <summary>
    /// 属性别名
    /// </summary>
    internal string? DisplayName { get; init; }

    /// <summary>
    /// 属性值访问器
    /// </summary>
    internal Func<object, object?>? Getter { get; private set; }

    /// <inheritdoc />
    public override bool IsValid(T value)
    {
        return TryValidate(value, null!, out _);
    }

    /// <inheritdoc />
    public override List<ValidationResult>? GetValidationResults(T value, string name)
    {
        // 检查属性注解（特性）合法性
        if (TryValidate(value, name, out var validationResults))
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
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, GetDisplayName(name));
    }

    /// <summary>
    /// 检查属性注解（特性）合法性
    /// </summary>
    /// <param name="instance">对象实例</param>
    /// <param name="name">显示名称</param>
    /// <param name="validationResults"><see cref="List{T}"/></param>
    /// <returns><see cref="bool"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal bool TryValidate(T instance, string name, out List<ValidationResult> validationResults)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 初始化验证上下文
        var validationContext = new ValidationContext(instance)
        {
            MemberName = GetDisplayName(name)
        };
        validationResults = new();

        // 调用 Validator.TryValidateProperty 静态方法验证
        return Validator.TryValidateProperty(GetPropertyValue(instance), validationContext, validationResults);
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="instance">对象实例</param>
    /// <returns><see cref="object"/></returns>
    internal object? GetPropertyValue(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        // 创建属性值访问器
        Getter ??= typeof(T).CreatePropertyGetter(Property);

        return Getter(instance);
    }

    /// <summary>
    /// 获取显示名称
    /// </summary>
    /// <param name="name">显示名称</param>
    /// <returns><see cref="string"/></returns>
    internal string GetDisplayName(string name)
    {
        return name ?? DisplayName ?? PropertyName;
    }
}