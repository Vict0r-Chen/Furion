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

public class ChineseNameValidatorTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var validator = new ChineseNameValidator();

        Assert.NotNull(validator);
        Assert.Null(validator.ErrorMessage);
        Assert.NotNull(validator._errorMessageResourceAccessor);
        Assert.Equal("The field {0} is not a valid Chinese name.", validator._errorMessageResourceAccessor());
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("秦", false)]
    [InlineData("张三", true)]
    [InlineData("李四", true)]
    [InlineData("王五", true)]
    [InlineData("葛二蛋", true)]
    [InlineData("易烊千玺", true)]
    [InlineData("百小僧", true)]
    [InlineData("凯文·杜兰特", true)]
    [InlineData("德克·维尔纳·诺维茨基", true)]
    [InlineData("蒙奇·D·路飞", false)]
    [InlineData("罗罗诺亚·索隆", true)]
    [InlineData("陈老6", false)]
    public void IsValid_ReturnOK(object? value, bool result)
    {
        var validator = new ChineseNameValidator();

        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void GetValidationResults_ReturnOK()
    {
        var validator = new ChineseNameValidator();

        var validationResultsOfSucceed = validator.GetValidationResults("百小僧", "data");
        Assert.Null(validationResultsOfSucceed);

        var validationResultsOfFailure = validator.GetValidationResults("蒙奇·D·路飞", "data");
        Assert.NotNull(validationResultsOfFailure);
        Assert.Single(validationResultsOfFailure);
        Assert.Equal("data", validationResultsOfFailure.First().MemberNames.First());
        Assert.Equal("The field data is not a valid Chinese name.", validationResultsOfFailure.First().ErrorMessage);
    }

    [Fact]
    public void GetValidationResults_WithErrorMessage_ReturnOK()
    {
        var validator = new ChineseNameValidator
        {
            ErrorMessage = "数据 {0} 验证失败"
        };

        var validationResultsOfFailure = validator.GetValidationResults("蒙奇·D·路飞", "data");
        Assert.NotNull(validationResultsOfFailure);
        Assert.Equal("数据 data 验证失败", validationResultsOfFailure.First().ErrorMessage);
    }

    [Fact]
    public void FormatErrorMessage_ReturnOK()
    {
        var validator = new ChineseNameValidator();

        Assert.Equal("The field data is not a valid Chinese name.", validator.FormatErrorMessage("data"));
    }

    [Fact]
    public void Validate_ReturnOK()
    {
        var validator = new ChineseNameValidator();

        validator.Validate("百小僧", "data");
    }

    [Fact]
    public void Validate_Failure()
    {
        var validator = new ChineseNameValidator();

        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            validator.Validate("蒙奇·D·路飞", "data");
        });

        Assert.Single(exception.InnerExceptions);
        Assert.Equal("The field data is not a valid Chinese name.", exception.InnerExceptions.First().Message);
    }

    [Theory]
    [InlineData(typeof(ChineseNameValidator), true)]
    [InlineData(typeof(ValidatorBase), false)]
    public void IsSameAs_ReturnOK(Type type, bool result)
    {
        var validator = new ChineseNameValidator();

        Assert.Equal(result, validator.IsSameAs(type));
    }
}