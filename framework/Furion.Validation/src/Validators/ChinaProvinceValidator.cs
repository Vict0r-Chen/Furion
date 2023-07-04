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
/// 中国省份验证器
/// </summary>
public partial class ChinaProvinceValidator : ValidatorBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public ChinaProvinceValidator()
        : base()
    {
    }

    /// <inheritdoc />
    protected override bool Validate(object? value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is string text)
        {
            return ChinaProvinceRegex().IsMatch(text);
        }

        return false;
    }

    /// <summary>
    /// 中国省份正则表达式
    /// </summary>
    /// <returns><see cref="Regex"/></returns>
    [GeneratedRegex(@"^浙江|上海|北京|天津|重庆|黑龙江|吉林|辽宁|内蒙古|河北|新疆|甘肃|青海|陕西|宁夏|河南|山东|山西|安徽|湖北|湖南|江苏|四川|贵州|云南|广西|西藏|江西|广东|福建|台湾|海南|香港|澳门$")]
    internal static partial Regex ChinaProvinceRegex();
}