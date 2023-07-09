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
/// <typeparam name="TInstance">对象类型</typeparam>
public partial class PropertyAnnotationValidator<TInstance> : PropertyAnnotationValidator
    where TInstance : class
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="instance">对象实例</param>
    /// <param name="propertyExpression">属性表达式</param>
    public PropertyAnnotationValidator(TInstance instance, Expression<Func<TInstance, object?>> propertyExpression)
        : base(instance, propertyExpression?.GetPropertyName()!)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(propertyExpression, nameof(propertyExpression));
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
    /// <param name="instance">对象实例</param>
    /// <param name="propertyName">属性名称</param>
    public PropertyAnnotationValidator(object instance, string propertyName)
        : base()
    {
        // 合法数据检查
        EnsureLegalData(instance, propertyName);

        Instance = instance;
        PropertyName = propertyName;
    }

    /// <summary>
    /// 对象实例
    /// </summary>
    public object Instance { get; set; }

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
    public override List<ValidationResult>? GetValidationResults(object? value, IEnumerable<string>? memberNames = null)
    {
        if (!TryValidate(value, out var validationResults, memberNames))
        {
            return validationResults;
        }

        return null;
    }

    /// <summary>
    /// 验证逻辑
    /// </summary>
    /// <param name="value">验证的值</param>
    /// <param name="validationResults"><see cref="ValidationResult"/> 集合</param>
    /// <param name="memberNames">成员名称集合</param>
    /// <returns><see cref="bool"/></returns>
    internal bool TryValidate(object? value, out List<ValidationResult> validationResults, IEnumerable<string>? memberNames = null)
    {
        // 调用 Validator 静态类验证
        var validationContext = new ValidationContext(Instance)
        {
            MemberName = memberNames?.FirstOrDefault() ?? PropertyName
        };
        validationResults = new List<ValidationResult>();

        return Validator.TryValidateProperty(value, validationContext, validationResults);
    }

    /// <summary>
    /// 合法数据检查
    /// </summary>
    /// <param name="instance">对象实例</param>
    /// <param name="propertyName">属性名称</param>
    /// <exception cref="ArgumentException"></exception>
    internal static void EnsureLegalData(object instance, string propertyName)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName, nameof(propertyName));

        // 属性定义检查
        if (!instance.GetType().GetProperties()
            .Any(p => p.Name == propertyName))
        {
            throw new ArgumentException($"The definition of the `{propertyName}` attribute cannot be found in Type {instance.GetType().Name}.", nameof(propertyName));
        }
    }
}