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
/// 属性注解（特性）验证器
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
public partial class PropertyAnnotationValidator<T> : PropertyAnnotationValidator
    where T : class
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="propertyExpression">属性表达式</param>
    public PropertyAnnotationValidator(Expression<Func<T, object?>> propertyExpression)
        : base(propertyExpression?.GetPropertyName()!)
    {
    }
}

/// <summary>
/// 属性注解（特性）验证器
/// </summary>
public partial class PropertyAnnotationValidator : ValidatorBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="propertyName">属性名称</param>
    public PropertyAnnotationValidator(string propertyName)
        : base()
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName, nameof(propertyName));

        PropertyName = propertyName;
    }

    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropertyName { get; set; }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        return TryValidate(value, out _);
    }

    /// <inheritdoc />
    public override List<ValidationResult>? GetValidationResults(object? value, string name)
    {
        if (!TryValidate(value, out var validationResults))
        {
            // 处理自定义错误消息
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                validationResults.Insert(0, new ValidationResult(FormatErrorMessage(name), new[] { name }));
            }

            return validationResults;
        }

        return null;
    }

    /// <summary>
    /// 验证逻辑
    /// </summary>
    /// <param name="value">验证的值</param>
    /// <param name="validationResults"><see cref="ValidationResult"/> 集合</param>
    /// <returns><see cref="bool"/></returns>
    internal bool TryValidate(object? value, out List<ValidationResult> validationResults)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        ArgumentException.ThrowIfNullOrWhiteSpace(PropertyName, nameof(PropertyName));

        // 根据属性名称查找属性对象
        var propertyInfo = value.GetType().GetProperty(PropertyName);

        // 空检查
        ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

        // 获取属性值
        var propertyValue = propertyInfo.GetValue(value);

        // 调用 Validator 静态类验证
        var validationContext = new ValidationContext(value)
        {
            MemberName = PropertyName
        };
        validationResults = new List<ValidationResult>();

        return Validator.TryValidateProperty(propertyValue, validationContext, validationResults);
    }
}