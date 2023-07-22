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

namespace Furion.Validation.Tests;

public class PhoneNumberValidatorTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData(13800138000, true)]
    [InlineData(16616682256, true)]
    [InlineData(18924931226, true)]
    [InlineData("13800138000", true)]
    [InlineData("16616682256", true)]
    [InlineData("18924931226", true)]
    [InlineData("+8613800138000", true)]
    [InlineData("+8616616682256", true)]
    [InlineData("+8618924931226", true)]
    [InlineData("008613800138000", true)]
    [InlineData("008616616682256", true)]
    [InlineData("008618924931226", true)]
    [InlineData(12345678900, false)]
    [InlineData("12345678900", false)]
    [InlineData("8618924931226", false)]
    [InlineData(10086, false)]
    [InlineData("10086", false)]
    [InlineData("0760-88808874", false)]
    public void IsValid(object? value, bool result)
    {
        var validator = new PhoneNumberValidator();
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new PhoneNumberValidator();

        var failure = validator.GetValidationResult(10086, "Value");
        Assert.NotNull(failure);
        Assert.Equal("The field Value is not a valid phone number format.", failure.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new PhoneNumberValidator
        {
            ErrorMessage = "不是一个有效的手机号码格式"
        };

        var failure = validator.GetValidationResult(10086, null!);
        Assert.NotNull(failure);

        Assert.Equal("不是一个有效的手机号码格式", failure.ErrorMessage);
    }
}