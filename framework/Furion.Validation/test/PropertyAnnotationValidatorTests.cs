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

public class PropertyAnnotationValidatorTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new PropertyAnnotationValidator<PropertyModel>(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new PropertyAnnotationValidator<PropertyModel, int>(null!);
        });
    }

    [Fact]
    public void New_Default()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);

        Assert.NotNull(validator);
        Assert.NotNull(validator.PropertyExpression);
        Assert.Equal("Name", validator.PropertyName);
        Assert.Null(validator.ErrorMessage);
        Assert.NotNull(validator._errorMessageResourceAccessor);
        Assert.Equal("The field {0} is invalid.", validator._errorMessageResourceAccessor());

        var validator2 = new PropertyAnnotationValidator<PropertyModel, int>(u => u.Id);
        Assert.NotNull(validator2._propertyExpression);
        Assert.NotNull(validator2.PropertyExpression);
        Assert.Equal("Id", validator2.PropertyName);
        Assert.NotNull(((PropertyAnnotationValidator<PropertyModel>)validator2).PropertyExpression);
    }

    [Fact]
    public void Convert_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            PropertyAnnotationValidator<PropertyModel, string>.Convert(null!);
        });
    }

    [Fact]
    public void Convert_ReturnOK()
    {
        var expression = PropertyAnnotationValidator<PropertyModel, string>.Convert(u => u.Name);
        Assert.NotNull(expression);
    }

    [Fact]
    public void IsValid_Invalid_Parameters()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);
        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.IsValid(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.PropertyExpression = null!;
            validator.IsValid(new PropertyModel { Name = "furion" });
        });
    }

    [Fact]
    public void IsValid_ReturnOK()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);
        Assert.True(validator.IsValid(new PropertyModel { Name = "furion" }));

        Assert.False(validator.IsValid(new PropertyModel { Name = null }));
        Assert.False(validator.IsValid(new PropertyModel { Name = "fu" }));
    }

    [Fact]
    public void GetValidationResults_Invalid_Parameters()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);
        Assert.Throws<ArgumentNullException>(() =>
        {
            var validationResults = validator.GetValidationResults(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.PropertyExpression = null!;
            var validationResults = validator.GetValidationResults(new PropertyModel { Name = "furion" }, null!);
        });
    }

    [Fact]
    public void GetValidationResults_ReturnOK()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);

        var validationResultsOfSucceed = validator.GetValidationResults(new PropertyModel { Name = "furion" }, null!);
        Assert.Null(validationResultsOfSucceed);

        var validationResultsOfFailure = validator.GetValidationResults(new PropertyModel { Name = null }, null!);
        Assert.NotNull(validationResultsOfFailure);
        Assert.Single(validationResultsOfFailure);
        Assert.Equal("Name", validationResultsOfFailure.ElementAt(0).MemberNames.First());
        Assert.Equal("The Name field is required.", validationResultsOfFailure.ElementAt(0).ErrorMessage);
    }

    [Fact]
    public void GetValidationResults_WithErrorMessage_ReturnOK()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name)
        {
            ErrorMessage = "数据 {0} 验证失败"
        };

        var validationResultsOfFailure = validator.GetValidationResults(new PropertyModel { Name = null }, null!);
        Assert.NotNull(validationResultsOfFailure);
        Assert.Equal("数据 Name 验证失败", validationResultsOfFailure.First().ErrorMessage);
    }

    [Fact]
    public void FormatErrorMessage_ReturnOK()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);

        Assert.Equal("The field Name is invalid.", validator.FormatErrorMessage(null!, new PropertyModel()));
    }

    [Fact]
    public void Validate_Invalid_Parameters()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);
        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.Validate(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.PropertyExpression = null!;
            validator.Validate(new PropertyModel { Name = "furion" }, null!);
        });
    }

    [Fact]
    public void Validate_ReturnOK()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);

        validator.Validate(new PropertyModel { Name = "furion" }, null!);
    }

    [Fact]
    public void Validate_Failure()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);

        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            validator.Validate(new PropertyModel { Name = null }, null!);
        });

        Assert.Single(exception.InnerExceptions);
        Assert.Equal("The Name field is required.", exception.InnerExceptions.First().Message);
    }

    [Theory]
    [InlineData(typeof(PropertyAnnotationValidator<PropertyModel>), true)]
    [InlineData(typeof(ValidatorBase), false)]
    public void IsSameAs_ReturnOK(Type type, bool result)
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);

        Assert.Equal(result, validator.IsSameAs(type));
    }

    [Fact]
    public void TryValidate_Invalid_Parameters()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);
        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.TryValidate(null!, out _);
        });
    }

    [Fact]
    public void TryValidate_ReturnOK()
    {
        var validator = new PropertyAnnotationValidator<PropertyModel>(u => u.Name);

        var result = validator.TryValidate(new PropertyModel { Name = "furion" }, out var validationResults);
        Assert.True(result);
        Assert.NotNull(validationResults);
        Assert.Empty(validationResults);

        var result2 = validator.TryValidate(new PropertyModel { Name = null }, out var validationResults2);
        Assert.False(result2);
        Assert.NotNull(validationResults2);
        Assert.Single(validationResults2);
        Assert.Equal("Name", validationResults2.ElementAt(0).MemberNames.First());
        Assert.Equal("The Name field is required.", validationResults2.ElementAt(0).ErrorMessage);
    }
}