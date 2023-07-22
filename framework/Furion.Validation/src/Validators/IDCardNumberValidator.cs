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
/// 身份证号验证器
/// </summary>
public partial class IDCardNumberValidator : ValidatorBase
{
    /// <summary>
    /// <inheritdoc cref="IDCardNumberValidator"/>
    /// </summary>
    public IDCardNumberValidator()
        : base(() => Strings.IDCardNumberValidator_invalid)
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is string text)
        {
            return Regex().IsMatch(text);
        }

        if (value.GetType().IsInteger())
        {
            return Regex().IsMatch(value.ToString()!);
        }

        return false;
    }

    /// <summary>
    /// 身份证号正则表达式
    /// </summary>
    /// <returns><see cref="System.Text.RegularExpressions.Regex"/></returns>
    [GeneratedRegex(@"^\d{6}((((((19|20)\d{2})(0[13-9]|1[012])(0[1-9]|[12]\d|30))|(((19|20)\d{2})(0[13578]|1[02])31)|((19|20)\d{2})02(0[1-9]|1\d|2[0-8])|((((19|20)([13579][26]|[2468][048]|0[48]))|(2000))0229))\d{3})|((((\d{2})(0[13-9]|1[012])(0[1-9]|[12]\d|30))|((\d{2})(0[13578]|1[02])31)|((\d{2})02(0[1-9]|1\d|2[0-8]))|(([13579][26]|[2468][048]|0[048])0229))\d{2}))(\d|X|x)$")]
    internal static partial Regex Regex();
}