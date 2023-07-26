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
/// 比较验证器抽象基类
/// </summary>
public abstract class ComparableValidator : ValidatorBase
{
    /// <summary>
    /// <inheritdoc cref="ComparableValidator"/>
    /// </summary>
    /// <param name="compareValueAccessor">比较的值访问器</param>
    /// <param name="errorMessageAccessor">错误消息资源访问器</param>
    protected ComparableValidator(Func<object?> compareValueAccessor, Func<string> errorMessageAccessor)
        : base(errorMessageAccessor)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(compareValueAccessor);

        CompareValue = compareValueAccessor();
    }

    /// <summary>
    /// 比较的值
    /// </summary>
    public object? CompareValue { get; set; }

    /// <inheritdoc />
    public override sealed bool IsValid(object? value)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(CompareValue);

        return value switch
        {
            null => true,
            IComparable val when CompareValue is IComparable cmpVal && val.GetType() == cmpVal.GetType() => IsValid(val, cmpVal),
            _ => false
        };
    }

    /// <summary>
    /// 检查值合法性
    /// </summary>
    /// <param name="value">对象值</param>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="bool"/></returns>
    protected abstract bool IsValid(IComparable value, IComparable compareValue);

    /// <inheritdoc />
    public override string FormatErrorMessage(string name, object? value = default)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, CompareValue);
    }
}