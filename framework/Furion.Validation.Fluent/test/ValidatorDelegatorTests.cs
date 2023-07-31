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

#pragma warning disable

namespace Furion.Validation.Fluent.Tests;

public class ValidatorDelegatorTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var validatorDelegator = new PropertyValidator<PropertyModel, object>
                .ValidatorDelegator<AgeValidator>(null!, null!, null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var validatorDelegator = new PropertyValidator<PropertyModel, object>
                .ValidatorDelegator<NotEqualValidator>(new(new ObjectValidator<PropertyModel>(), u => u.Name), u => new[] { "furion" }, () => Strings.NotEqualValidator_Invalid);

        Assert.NotNull(validatorDelegator);
        Assert.NotNull(validatorDelegator._propertyValidator);
        Assert.NotNull(validatorDelegator._constructorParametersAccessor);
        Assert.Null(validatorDelegator.Validator);
        Assert.Null(validatorDelegator.PropertyValue);
        Assert.Null(validatorDelegator.ValidatorConfigure);
    }

    [Fact]
    public void Configure_Invalid_Parameters()
    {
        var validatorDelegator = new PropertyValidator<PropertyModel, string?>
                .ValidatorDelegator<NotEqualValidator>(new(new ObjectValidator<PropertyModel>(), u => u.Name), u => new[] { "furion" }, () => Strings.NotEqualValidator_Invalid);

        Assert.Throws<ArgumentNullException>(() =>
        {
            validatorDelegator.Configure(null);
        });
    }

    [Fact]
    public void Configure_ReturnOK()
    {
        var validatorDelegator = new PropertyValidator<PropertyModel, string?>
                .ValidatorDelegator<NotEqualValidator>(new(new ObjectValidator<PropertyModel>(), u => u.Name), u => new[] { "furion" }, () => Strings.NotEqualValidator_Invalid);

        validatorDelegator.Configure(v =>
        {
            v.CompareValue = "Furion";
        });

        Assert.NotNull(validatorDelegator.ValidatorConfigure);
    }

    [Fact]
    public void IsValid_ReturnOK()
    {
        var validatorDelegator = new PropertyValidator<PropertyModel, string?>
               .ValidatorDelegator<NotEqualValidator>(new(new ObjectValidator<PropertyModel>(), u => u.Name), u => new[] { "furion" }, () => Strings.NotEqualValidator_Invalid);

        var instance = new PropertyModel
        {
            Name = "furion"
        };

        Assert.False(validatorDelegator.IsValid(instance));
    }

    [Fact]
    public void GetValidationResults_ReturnOK()
    {
        var validatorDelegator = new PropertyValidator<PropertyModel, string?>
               .ValidatorDelegator<NotEqualValidator>(new(new ObjectValidator<PropertyModel>(), u => u.Name), u => new[] { "furion" }, () => Strings.NotEqualValidator_Invalid);

        var instance = new PropertyModel
        {
            Name = "furion"
        };

        var validationResults = validatorDelegator.GetValidationResults(instance, null!);
        Assert.NotNull(validationResults);
        Assert.Single(validationResults);
        Assert.Equal("Name", validationResults.First().MemberNames.First());
        Assert.Equal("The field Name cannot be equal to 'furion'.", validationResults.First().ErrorMessage);
    }

    [Fact]
    public void GetValidationResults_WithErrorMessage_ReturnOK()
    {
        var validatorDelegator = new PropertyValidator<PropertyModel, string?>
               .ValidatorDelegator<NotEqualValidator>(new(new ObjectValidator<PropertyModel>(), u => u.Name), u => new[] { "furion" }, () => Strings.NotEqualValidator_Invalid)
        {
            ErrorMessage = "数据 {0} 验证失败"
        };

        var instance = new PropertyModel
        {
            Name = "furion"
        };

        var validationResults = validatorDelegator.GetValidationResults(instance, null!);
        Assert.NotNull(validationResults);
        Assert.Equal("数据 Name 验证失败", validationResults.First().ErrorMessage);
    }

    [Fact]
    public void FormatErrorMessage_ReturnOK()
    {
        var validatorDelegator = new PropertyValidator<PropertyModel, string?>
               .ValidatorDelegator<NotEqualValidator>(new(new ObjectValidator<PropertyModel>(), u => u.Name), u => new[] { "furion" }, () => Strings.NotEqualValidator_Invalid);

        var instance = new PropertyModel
        {
            Name = "furion"
        };

        Assert.Equal("The field Name cannot be equal to 'furion'.", validatorDelegator.FormatErrorMessage(null!, instance));
    }

    [Fact]
    public void Validate_ReturnOK()
    {
        var validatorDelegator = new PropertyValidator<PropertyModel, string?>
               .ValidatorDelegator<NotEqualValidator>(new(new ObjectValidator<PropertyModel>(), u => u.Name), u => new[] { "furion" }, () => Strings.NotEqualValidator_Invalid);

        var instance = new PropertyModel
        {
            Name = "furion"
        };

        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            validatorDelegator.Validate(instance, null!);
        });

        Assert.Single(exception.InnerExceptions);
        Assert.Equal("The field Name cannot be equal to 'furion'.", exception.InnerExceptions.First().Message);
    }

    [Fact]
    public void Initialize_Invalid_Parameters()
    {
        var validatorDelegator = new PropertyValidator<PropertyModel, object>
                .ValidatorDelegator<NotEqualValidator>(new(new ObjectValidator<PropertyModel>(), u => u.Name), u => new[] { "furion" }, () => Strings.NotEqualValidator_Invalid);

        Assert.Throws<ArgumentNullException>(() =>
        {
            validatorDelegator.Initialize(null!);
        });
    }

    [Fact]
    public void Initialize_ReturnOK()
    {
        var validatorDelegator = new PropertyValidator<PropertyModel, string?>
                .ValidatorDelegator<NotEqualValidator>(new(new ObjectValidator<PropertyModel>(), u => u.Name), u => new[] { "furion" }, () => Strings.NotEqualValidator_Invalid);

        var instance = new PropertyModel
        {
            Name = "Furion"
        };
        validatorDelegator.Initialize(instance);

        Assert.NotNull(validatorDelegator.Validator);
        Assert.NotNull(validatorDelegator.PropertyValue);
        Assert.Equal("Furion", validatorDelegator.PropertyValue);
        Assert.Equal(validatorDelegator.ErrorMessage, validatorDelegator.Validator.ErrorMessage);

        instance.Name = "百小僧";
        validatorDelegator.WithErrorMessage("自定义错误消息");
        validatorDelegator.Configure(v =>
        {
            v.CompareValue = "Fur";
        });

        validatorDelegator.Initialize(instance);
        Assert.NotNull(validatorDelegator.Validator);
        Assert.NotNull(validatorDelegator.PropertyValue);
        Assert.Equal("百小僧", validatorDelegator.PropertyValue);
        Assert.Equal(validatorDelegator.ErrorMessage, validatorDelegator.Validator.ErrorMessage);
        Assert.Equal("自定义错误消息", validatorDelegator.Validator.ErrorMessage);
        Assert.Equal("Fur", validatorDelegator.Validator.CompareValue);
    }

    [Fact]
    public void GetDisplayName_ReturnOK()
    {
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(new ObjectValidator<PropertyModel>(), u => u.Name);
        var validatorDelegator = new PropertyValidator<PropertyModel, string?>
               .ValidatorDelegator<NotEqualValidator>(propertyValidator, u => new[] { "furion" }, () => Strings.NotEqualValidator_Invalid);

        Assert.Equal("Name", validatorDelegator.GetDisplayName(null!));

        propertyValidator.WithDisplayName("其他名称");

        Assert.Equal("其他名称", propertyValidator.GetDisplayName());
    }
}