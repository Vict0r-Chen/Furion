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
/// 颜色值验证器
/// </summary>
public partial class ColorValueValidator : ValidatorBase
{
    /// <summary>
    /// <inheritdoc cref="ColorValueValidator"/>
    /// </summary>
    /// <param name="fullMode">全面模式</param>
    public ColorValueValidator(bool fullMode = true)
        : base(() => Strings.ColorValueValidator_Invalid)
    {
        FullMode = fullMode;
    }

    /// <summary>
    /// 全面模式
    /// </summary>
    public bool FullMode { get; set; } = true;

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is string text)
        {
            return (FullMode
                ? Regex()
                : StandardRegex()).IsMatch(text);
        }

        return false;
    }

    /// <summary>
    /// 颜色值正则表达式（全面模式）
    /// </summary>
    /// <returns><see cref="System.Text.RegularExpressions.Regex"/></returns>
    [GeneratedRegex(@"^(?:#(?:[0-9a-fA-F]{3}){1,2}|rgba?\((?:\s*\d+\%?\s*,){2}\s*(?:\d+\%?\s*(?:,\s*[0-9.]+\s*)?)?\)|hsla?\((?:\s*\d+\%?\s*,){2}\s*(?:\d+\%?\s*(?:,\s*[0-9.]+\s*)?)?\)|hwb\((?:\s*\d+\%?\s*,){2}\s*(?:\d+\%?\s*)?\)|lch\((?:\s*\d+\%?\s*,){2}\s*(?:\d+\%?\s*)?\)|oklch\((?:\s*\d+\%?\s*,){2}\s*(?:\d+\%?\s*)?\)|lab\((?:\s*[-+]?\d+\%?\s*,){2}\s*[-+]?\d+\%?\s*\)|oklab\((?:\s*[-+]?\d+\%?\s*,){2}\s*[-+]?\d+\%?\s*\))$", RegexOptions.IgnoreCase)]
    internal static partial Regex Regex();

    /// <summary>
    /// 颜色值正则表达式（标准模式）
    /// </summary>
    /// <remarks>只支持十六进制、RGB、RGBA</remarks>
    /// <returns><see cref="System.Text.RegularExpressions.Regex"/></returns>
    [GeneratedRegex(@"^(?:#(?:[0-9a-fA-F]{3}){1,2}|rgba?\((?:\s*(?:\d+%?)\s*,){2}\s*(?:\d+%?)\s*(?:,\s*(?:\d+(?:\.\d+)?|\.\d+))?\))$", RegexOptions.IgnoreCase)]
    internal static partial Regex StandardRegex();
}