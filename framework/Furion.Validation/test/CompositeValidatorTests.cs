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

public class CompositeValidatorTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new CompositeValidator((List<ValidatorBase>)null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new CompositeValidator((List<ValidatorBase>)null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            var validator = new CompositeValidator(new ColorValueValidator(), null!, new ChineseValidator());
        });
        Assert.Equal("The validator collection contains a null value. (Parameter 'validators')", exception.Message);
    }

    [Fact]
    public void New_ReturnOK()
    {
        var validator = new CompositeValidator();

        Assert.NotNull(validator);
        Assert.Equal(ValidatorCascadeMode.Continue, validator.CascadeMode);
        Assert.NotNull(validator.Validators);
        Assert.Empty(validator.Validators);
        Assert.Null(validator.ErrorMessage);
        Assert.NotNull(validator._errorMessageResourceAccessor);
        Assert.Equal("The field {0} is invalid.", validator._errorMessageResourceAccessor());
    }

    [Fact]
    public void IsValid_Invalid_Parameters()
    {
        var validator = new CompositeValidator();
        validator.Validators.Add(null!);

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            validator.IsValid(null);
        });
        Assert.Equal("The validator collection contains a null value. (Parameter 'validators')", exception.Message);
    }

    [Fact]
    public void IsValid_EmptyValidators_ReturnOK()
    {
        var validator = new CompositeValidator();
        Assert.True(validator.IsValid(null));
        Assert.True(validator.IsValid(string.Empty));

        validator.CascadeMode = ValidatorCascadeMode.UsingFirstSuccess;
        Assert.True(validator.IsValid(null));
        Assert.True(validator.IsValid(string.Empty));
    }

    [Fact]
    public void IsValid_ReturnOK()
    {
        var validator = new CompositeValidator(new ChineseNameValidator(), new ChineseValidator());
        Assert.True(validator.IsValid(null));
        Assert.False(validator.IsValid(string.Empty));
        Assert.False(validator.IsValid("赢"));
        Assert.True(validator.IsValid("赢家"));

        validator.CascadeMode = ValidatorCascadeMode.UsingFirstSuccess;
        Assert.True(validator.IsValid(null));
        Assert.False(validator.IsValid(string.Empty));
        Assert.True(validator.IsValid("赢"));
        Assert.True(validator.IsValid("赢家"));

        validator.CascadeMode = ValidatorCascadeMode.StopOnFirstFailure;
        Assert.True(validator.IsValid(null));
        Assert.False(validator.IsValid(string.Empty));
        Assert.False(validator.IsValid("赢"));
        Assert.True(validator.IsValid("赢家"));
    }

    [Fact]
    public void GetValidationResults_Invalid_Parameters()
    {
        var validator = new CompositeValidator();
        validator.Validators.Add(null!);

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            var validationResults = validator.GetValidationResults(null, "data");
        });
        Assert.Equal("The validator collection contains a null value. (Parameter 'validators')", exception.Message);
    }

    [Fact]
    public void GetValidationResults_EmptyValidators_ReturnOK()
    {
        var validator = new CompositeValidator();
        Assert.Null(validator.GetValidationResults(null, "data"));
        Assert.Null(validator.GetValidationResults(string.Empty, "data"));

        validator.CascadeMode = ValidatorCascadeMode.UsingFirstSuccess;
        Assert.Null(validator.GetValidationResults(null, "data"));
        Assert.Null(validator.GetValidationResults(string.Empty, "data"));
    }

    [Fact]
    public void GetValidationResults_ReturnOK()
    {
        var validator = new CompositeValidator(new ChineseNameValidator(), new ChineseValidator());

        // Continue =====
        var validationResultsOfSucceed = validator.GetValidationResults("百小僧", "data");
        Assert.Null(validationResultsOfSucceed);

        var validationResultsOfFailure = validator.GetValidationResults("赢", "data");
        Assert.NotNull(validationResultsOfFailure);
        Assert.Single(validationResultsOfFailure);
        Assert.Equal("data", validationResultsOfFailure.First().MemberNames.First());
        Assert.Equal("The field data is not a valid Chinese name.", validationResultsOfFailure.First().ErrorMessage);

        // UsingFirstSuccess =====
        validator.CascadeMode = ValidatorCascadeMode.UsingFirstSuccess;
        var validationResultsOfSucceed2 = validator.GetValidationResults("赢", "data");
        Assert.Null(validationResultsOfSucceed2);

        var validationResultsOfFailure2 = validator.GetValidationResults("蒙奇·D·路飞", "data");
        Assert.NotNull(validationResultsOfFailure2);
        Assert.Equal(2, validationResultsOfFailure2.Count);
        Assert.Equal("data", validationResultsOfFailure2.First().MemberNames.First());
        Assert.Equal("The field data is not a valid Chinese name.", validationResultsOfFailure2.First().ErrorMessage);
        Assert.Equal("The field data is not a valid Chinese.", validationResultsOfFailure2.Last().ErrorMessage);

        // StopOnFirstFailure
        validator.CascadeMode = ValidatorCascadeMode.StopOnFirstFailure;
        var validationResultsOfFailure3 = validator.GetValidationResults("蒙奇·D·路飞", "data");
        Assert.NotNull(validationResultsOfFailure3);
        Assert.Single(validationResultsOfFailure3);
        Assert.Equal("The field data is not a valid Chinese name.", validationResultsOfFailure3.First().ErrorMessage);
    }

    [Fact]
    public void GetValidationResults_WithErrorMessage_ReturnOK()
    {
        var validator = new CompositeValidator(new ChineseNameValidator(), new ChineseValidator())
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
        var validator = new CompositeValidator();

        Assert.Equal("The field data is invalid.", validator.FormatErrorMessage("data"));
    }

    [Fact]
    public void Validate_Invalid_Parameters()
    {
        var validator = new CompositeValidator();
        validator.Validators.Add(null!);

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            validator.Validate(null, "data");
        });
        Assert.Equal("The validator collection contains a null value. (Parameter 'validators')", exception.Message);
    }

    [Fact]
    public void Validate_ReturnOK()
    {
        var validator = new CompositeValidator(new ChineseNameValidator(), new ChineseValidator());

        validator.Validate("百小僧", "data");
    }

    [Fact]
    public void Validate_Failure()
    {
        var validator = new CompositeValidator(new ChineseNameValidator(), new ChineseValidator());

        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            validator.Validate("蒙奇·D·路飞", "data");
        });

        Assert.Equal(2, exception.InnerExceptions.Count);
        Assert.Equal("The field data is not a valid Chinese name.", exception.InnerExceptions.First().Message);
    }

    [Theory]
    [InlineData(typeof(CompositeValidator), true)]
    [InlineData(typeof(ValidatorBase), false)]
    public void IsSameAs_ReturnOK(Type type, bool result)
    {
        var validator = new CompositeValidator();

        Assert.Equal(result, validator.IsSameAs(type));
    }

    [Fact]
    public void EnsureLegalData_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            CompositeValidator.EnsureLegalData(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            CompositeValidator.EnsureLegalData(null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            CompositeValidator.EnsureLegalData(new List<ValidatorBase> { new ColorValueValidator(), null!, new ChineseValidator() });
        });
        Assert.Equal("The validator collection contains a null value. (Parameter 'validators')", exception.Message);
    }
}