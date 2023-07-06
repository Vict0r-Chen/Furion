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
/// 数字/货币金额验证器
/// </summary>
public partial class CurrencyAmountValidator : ValidatorBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public CurrencyAmountValidator()
        : base(() => Strings.CurrencyAmountValidator_Invalid)
    {
    }

    /// <summary>
    /// 允许负数
    /// </summary>
    public bool AllowNegative { get; set; }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is decimal decimalValue)
        {
            if (!AllowNegative && decimalValue < 0)
            {
                return false;
            }

            // 检查小数点后面位数是否小于等于 2
            return decimal.GetBits(decimalValue)[3] >> 16 == 0;
        }

        var stringValue = value.ToString()!;

        return (AllowNegative
                ? Regex()
                : PositiveRegex()).IsMatch(stringValue)
                && decimal.TryParse(stringValue, out _);
    }

    /// <summary>
    /// 数字/货币金额正则表达式（只支持正数）
    /// </summary>
    /// <returns><see cref="System.Text.RegularExpressions.Regex"/></returns>
    [GeneratedRegex(@"(?:^[1-9]([0-9]+)?(?:\.[0-9]{1,2})?$)|(?:^(?:0)$)|(?:^[0-9]\.[0-9](?:[0-9])?$)")]
    internal static partial Regex PositiveRegex();

    /// <summary>
    /// 数字/货币金额正则表达式（支持正负数）
    /// </summary>
    /// <returns><see cref="System.Text.RegularExpressions.Regex"/></returns>
    [GeneratedRegex(@"^-?\d+(,\d{3})*(\.\d{1,2})?$")]
    internal static partial Regex Regex();
}