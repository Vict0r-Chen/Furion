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

namespace System.ComponentModel.DataAnnotations;

/// <summary>
/// 比较验证器验证特性
/// </summary>
/// <typeparam name="TComparableValidator"><see cref="ComparableValidator"/></typeparam>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public abstract class ComparableAttribute<TComparableValidator> : ValidationAttribute
    where TComparableValidator : ComparableValidator
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <param name="errorMessageAccessor">错误消息资源访问器</param>
    public ComparableAttribute(object? value, Func<string> errorMessageAccessor)
        : base(errorMessageAccessor)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        Value = value;
    }

    /// <summary>
    /// 比较的值
    /// </summary>
    public object Value { get; }

    /// <inheritdoc />
    public override sealed bool IsValid(object? value)
    {
        // 创建比较验证器实例
        var comparableValidator = Activator.CreateInstance(typeof(TComparableValidator), new object[] { Value }) as ComparableValidator;

        // 空检查
        ArgumentNullException.ThrowIfNull(comparableValidator, nameof(comparableValidator));

        return comparableValidator.IsValid(value);
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Value);
    }
}