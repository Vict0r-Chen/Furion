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
/// 长度验证器
/// </summary>
public partial class LengthValidator : ValidatorBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="minimumLength">最小长度</param>
    /// <param name="maximumLength">最大长度</param>
    public LengthValidator(int minimumLength, int maximumLength)
        : base(() => Strings.LengthValidator_Invalid)
    {
        // 合法数据检查
        EnsureLegalData(minimumLength, maximumLength);

        MinimumLength = minimumLength;
        MaximumLength = maximumLength;
    }

    /// <summary>
    /// 最小长度
    /// </summary>
    public int MinimumLength { get; set; }

    /// <summary>
    /// 最大长度
    /// </summary>
    public int MaximumLength { get; set; }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        // 合法数据检查
        EnsureLegalData(MinimumLength, MaximumLength);

        if (value is null)
        {
            return true;
        }

        if (value.TryGetCount(out var count)
            && count >= MinimumLength
            && count <= MaximumLength)
        {
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    protected override string[] GetDefaultMemberNames()
    {
        return new string[] { MinimumLength.ToString(), MaximumLength.ToString() };
    }

    /// <summary>
    /// 合法数据检查
    /// </summary>
    /// <param name="minimumLength">最小长度</param>
    /// <param name="maximumLength">最大长度</param>
    /// <exception cref="InvalidOperationException"></exception>
    internal static void EnsureLegalData(int minimumLength, int maximumLength)
    {
        if (minimumLength < 0)
        {
            throw new InvalidOperationException("The minimum length cannot be less than 0.");
        }

        if (maximumLength < minimumLength)
        {
            throw new InvalidOperationException("The maximum length cannot be less than the minimum length.");
        }
    }
}