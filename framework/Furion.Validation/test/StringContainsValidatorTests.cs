﻿// 麻省理工学院许可证
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

public class StringContainsValidatorTests
{
    [Fact]
    public void NewInstance_Null_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new StringContainsValidator(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var validator = new StringContainsValidator(string.Empty);
        });
    }

    [Fact]
    public void NewInstance_ReturnOK()
    {
        var validator = new StringContainsValidator("furion");
        Assert.NotNull(validator);
        Assert.Equal(StringComparison.CurrentCulture, validator.Comparison);

        var validator2 = new StringContainsValidator('c');
        Assert.NotNull(validator2);
        Assert.Equal(StringComparison.CurrentCulture, validator.Comparison);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("furion", true)]
    [InlineData("furiou", true)]
    [InlineData("furos", true)]
    [InlineData("fuion", false)]
    [InlineData("ofurion", true)]
    [InlineData("Furion", false)]
    [InlineData("fUriou", false)]
    [InlineData("fuRos", false)]
    [InlineData("ccfur", true)]
    public void IsValid(object? value, bool result)
    {
        var validator = new StringContainsValidator("fur");
        Assert.Equal(result, validator.IsValid(value));
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("furion", true)]
    [InlineData("furiou", true)]
    [InlineData("furos", true)]
    [InlineData("fuion", false)]
    [InlineData("ofurion", true)]
    [InlineData("Furion", true)]
    [InlineData("fUriou", true)]
    [InlineData("fuRos", true)]
    [InlineData("ccfur", true)]
    public void IsValid_SetComparison_OrdinalIgnoreCase(object? value, bool result)
    {
        var validator = new StringContainsValidator("fur")
        {
            Comparison = StringComparison.OrdinalIgnoreCase
        };
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void IsValid_Char()
    {
        var validator = new StringContainsValidator("f");
        Assert.True(validator.IsValid("furion"));
        Assert.False(validator.IsValid("Furion"));
        Assert.False(validator.IsValid("lurion"));
    }

    [Fact]
    public void IsValid_Char_SetComparison_OrdinalIgnoreCase()
    {
        var validator = new StringContainsValidator("f")
        {
            Comparison = StringComparison.OrdinalIgnoreCase
        };
        Assert.True(validator.IsValid("furion"));
        Assert.True(validator.IsValid("Furion"));
        Assert.False(validator.IsValid("lurion"));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new StringContainsValidator("fur");

        var failure = validator.GetValidationResult("fuion");
        Assert.NotNull(failure);
        Assert.Equal("The field is not contain the string fur.", failure.ErrorMessage);

        var failure2 = validator.GetValidationResult("fuion", new List<string> { "Value" });
        Assert.NotNull(failure2);
        Assert.Equal("The field Value is not contain the string fur.", failure2.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new StringContainsValidator("fur")
        {
            ErrorMessage = "该字符串包含fur字符串"
        };

        var failure = validator.GetValidationResult("fuion");
        Assert.NotNull(failure);

        Assert.Equal("该字符串包含fur字符串", failure.ErrorMessage);
    }
}