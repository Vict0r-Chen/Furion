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

public class GreaterThanValidatorTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData(10, false)]
    [InlineData(9, false)]
    [InlineData(11, true)]
    public void IsValid_Int(object? value, bool result)
    {
        var validator = new GreaterThanValidator(10);
        Assert.Equal(result, validator.IsValid(value));
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData(10.0, false)]
    [InlineData(10.1, true)]
    [InlineData(11.0, true)]
    [InlineData(9.0d, false)]
    public void IsValid_Double(object? value, bool result)
    {
        var validator = new GreaterThanValidator(10.0);
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new GreaterThanValidator(10);

        var failure = validator.GetValidationResult(9, "Value");
        Assert.NotNull(failure);
        Assert.Equal("The field Value must be greater than 10.", failure.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new GreaterThanValidator(10)
        {
            ErrorMessage = "必须小于等于"
        };

        var failure = validator.GetValidationResult(9, null!);
        Assert.NotNull(failure);

        Assert.Equal("必须小于等于", failure.ErrorMessage);
    }
}