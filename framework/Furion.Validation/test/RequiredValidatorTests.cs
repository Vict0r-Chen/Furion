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

public class RequiredValidatorTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("ok", true)]
    [InlineData(1, true)]
    [InlineData(true, true)]
    [InlineData(123.46d, true)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    public void IsValid(object? value, bool result)
    {
        var validator = new RequiredValidator();
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void IsValid_Any()
    {
        var validator = new RequiredValidator();
        Assert.False(validator.IsValid(string.Empty));
        Assert.True(validator.IsValid(new Dictionary<string, string>()));
        Assert.True(validator.IsValid(Array.Empty<string>()));
        Assert.True(validator.IsValid(Enumerable.Empty<string>()));
        Assert.True(validator.IsValid(new List<string>()));
    }

    [Fact]
    public void IsValid_AllowEmptyStrings()
    {
        var validator = new RequiredValidator();
        Assert.False(validator.IsValid(string.Empty));

        validator.AllowEmptyStrings = true;
        Assert.True(validator.IsValid(string.Empty));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new RequiredValidator();

        var failure = validator.GetValidationResult(null);
        Assert.NotNull(failure);
        Assert.Equal("The field is required.", failure.ErrorMessage);

        var failure2 = validator.GetValidationResult(null, new List<string> { "Value" });
        Assert.NotNull(failure2);
        Assert.Equal("The Value field is required.", failure2.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new RequiredValidator
        {
            ErrorMessage = "字段为必填项"
        };

        var failure = validator.GetValidationResult(null);
        Assert.NotNull(failure);

        Assert.Equal("字段为必填项", failure.ErrorMessage);
    }
}