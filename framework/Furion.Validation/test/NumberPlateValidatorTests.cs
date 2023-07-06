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

namespace Furion.Validation.Tests;

public class NumberPlateValidatorTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("京A00599", true)]
    [InlineData("黑D23908", true)]
    [InlineData("京AD92035", true)]
    [InlineData("甘G23459F", true)]
    [InlineData("京AA92035", true)]
    [InlineData("京A12345D", true)]
    [InlineData("粤T88888", true)]
    [InlineData("粤B好几个8", false)]
    [InlineData("宁AD1234555555", false)]
    [InlineData("浙苏H6F681", false)]
    public void IsValid(object? value, bool result)
    {
        var validator = new NumberPlateValidator();
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new NumberPlateValidator();

        var failure = validator.GetValidationResult("粤B好几个8");
        Assert.NotNull(failure);
        Assert.Equal("The field is not a valid number plate format.", failure.ErrorMessage);

        var failure2 = validator.GetValidationResult("粤B好几个8", new List<string> { "Value" });
        Assert.NotNull(failure2);
        Assert.Equal("The field Value is not a valid number plate format.", failure2.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new NumberPlateValidator
        {
            ErrorMessage = "不是一个有效的车牌号格式"
        };

        var failure = validator.GetValidationResult("粤B好几个8");
        Assert.NotNull(failure);

        Assert.Equal("不是一个有效的车牌号格式", failure.ErrorMessage);
    }
}