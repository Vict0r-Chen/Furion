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
/// 包含特定字符/字符串的验证器
/// </summary>
public class StringContainsValidator : ValidatorBase
{
    /// <summary>
    /// <inheritdoc cref="StringContainsValidator"/>
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    public StringContainsValidator(char searchValue)
        : this(searchValue.ToString)
    {
    }

    /// <summary>
    /// <inheritdoc cref="StringContainsValidator"/>
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    public StringContainsValidator(string searchValue)
        : base(() => searchValue)
    {
    }

    /// <summary>
    /// <inheritdoc cref="GreaterThanOrEqualToValidator"/>
    /// </summary>
    /// <param name="searchValueAccessor">检索的值访问器</param>
    public StringContainsValidator(Func<string?> searchValueAccessor)
        : base(() => Strings.StringContainsValidator_Invalid)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(searchValueAccessor);

        SearchValue = searchValueAccessor();
    }

    /// <summary>
    /// 检索的值
    /// </summary>
    public string? SearchValue { get; set; }

    /// <inheritdoc cref="StringComparison"/>
    public StringComparison Comparison { get; set; }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrEmpty(SearchValue);

        return value switch
        {
            null => false,
            string text => text.Contains(SearchValue, Comparison),
            _ => false
        };
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name, object? value = default)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, SearchValue);
    }
}