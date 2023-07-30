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

public class ObjectAnnotationValidatorTests
{
    [Fact]
    public void New_Default()
    {
        var validator = new ObjectAnnotationValidator();

        Assert.NotNull(validator);
        Assert.True(validator.ValidateAllProperties);
        Assert.Null(validator.ErrorMessage);
        Assert.NotNull(validator._errorMessageResourceAccessor);
        Assert.Equal("The field {0} is invalid.", validator._errorMessageResourceAccessor());
    }

    [Fact]
    public void IsValid_ReturnOK()
    {
        var validator = new ObjectAnnotationValidator();

        Assert.True(validator.IsValid(null));

        var objectModel = new ObjectModel
        {
            Id = 1,
            Name = "Furion",
            Email = "monksoul@outlook.com"
        };
        Assert.True(validator.IsValid(objectModel));

        var objectModel2 = new ObjectModel();
        Assert.False(validator.IsValid(objectModel2));
    }

    [Fact]
    public void GetValidationResults_ReturnOK()
    {
        var validator = new ObjectAnnotationValidator();
        var objectModel = new ObjectModel
        {
            Id = 1,
            Name = "Furion",
            Email = "monksoul@outlook.com"
        };

        var validationResultsOfSucceed = validator.GetValidationResults(objectModel, "data");
        Assert.Null(validationResultsOfSucceed);

        var objectModel2 = new ObjectModel();
        var validationResultsOfFailure = validator.GetValidationResults(objectModel2, "data");
        Assert.NotNull(validationResultsOfFailure);
        Assert.Equal(2, validationResultsOfFailure.Count);
        Assert.Equal("Id", validationResultsOfFailure.ElementAt(0).MemberNames.ElementAt(0));
        Assert.Equal("Name", validationResultsOfFailure.ElementAt(1).MemberNames.ElementAt(0));
        Assert.Equal("The field Id must be between 1 and 2147483647.", validationResultsOfFailure.ElementAt(0).ErrorMessage);
        Assert.Equal("The Name field is required.", validationResultsOfFailure.ElementAt(1).ErrorMessage);
    }

    [Fact]
    public void GetValidationResults_WithErrorMessage_ReturnOK()
    {
        var validator = new ObjectAnnotationValidator
        {
            ErrorMessage = "数据 {0} 验证失败"
        };

        var objectModel2 = new ObjectModel();
        var validationResultsOfFailure = validator.GetValidationResults(objectModel2, "data");
        Assert.NotNull(validationResultsOfFailure);
        Assert.Equal("数据 data 验证失败", validationResultsOfFailure.First().ErrorMessage);
    }

    [Fact]
    public void FormatErrorMessage_ReturnOK()
    {
        var validator = new ObjectAnnotationValidator();

        Assert.Equal("The field data is invalid.", validator.FormatErrorMessage("data"));
    }

    [Fact]
    public void Validate_ReturnOK()
    {
        var validator = new ObjectAnnotationValidator();

        var objectModel = new ObjectModel
        {
            Id = 1,
            Name = "Furion",
            Email = "monksoul@outlook.com"
        };
        validator.Validate(objectModel, "data");
    }

    [Fact]
    public void Validate_Failure()
    {
        var validator = new ObjectAnnotationValidator();

        var objectModel2 = new ObjectModel();
        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            validator.Validate(objectModel2, "data");
        });

        Assert.Equal(2, exception.InnerExceptions.Count);
        Assert.Equal("The field Id must be between 1 and 2147483647.", exception.InnerExceptions.ElementAt(0).Message);
        Assert.Equal("The Name field is required.", exception.InnerExceptions.ElementAt(1).Message);
    }

    [Theory]
    [InlineData(typeof(ObjectAnnotationValidator), true)]
    [InlineData(typeof(ValidatorBase), false)]
    public void IsSameAs_ReturnOK(Type type, bool result)
    {
        var validator = new ObjectAnnotationValidator();

        Assert.Equal(result, validator.IsSameAs(type));
    }

    [Fact]
    public void TryValidate_Invalid_Parameters()
    {
        var validator = new ObjectAnnotationValidator();

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.TryValidate(null!, out _);
        });
    }

    [Fact]
    public void TryValidate_ReturnOK()
    {
        var objectModel = new ObjectModel
        {
            Id = 1,
            Name = "Furion",
            Email = "monksoul@outlook.com"
        };

        var validator = new ObjectAnnotationValidator();

        var result = validator.TryValidate(objectModel, out var validationResults);

        Assert.True(result);
        Assert.NotNull(validationResults);
        Assert.Empty(validationResults);

        var objectModel2 = new ObjectModel();

        var result2 = validator.TryValidate(objectModel2, out var validationResults2);

        Assert.False(result2);
        Assert.NotNull(validationResults2);
        Assert.Equal(2, validationResults2.Count);
        Assert.Equal("Id", validationResults2.ElementAt(0).MemberNames.ElementAt(0));
        Assert.Equal("Name", validationResults2.ElementAt(1).MemberNames.ElementAt(0));
        Assert.Equal("The field Id must be between 1 and 2147483647.", validationResults2.ElementAt(0).ErrorMessage);
        Assert.Equal("The Name field is required.", validationResults2.ElementAt(1).ErrorMessage);
    }

    [Fact]
    public void TryValidate_WithValidateAllProperties_ReturnOK()
    {
        var validator = new ObjectAnnotationValidator
        {
            ValidateAllProperties = false
        };

        var objectModel = new ObjectModel();

        var result = validator.TryValidate(objectModel, out var validationResults);

        Assert.False(result);
        Assert.NotNull(validationResults);
        Assert.Single(validationResults);
        Assert.Equal("The Name field is required.", validationResults.ElementAt(0).ErrorMessage);
    }
}