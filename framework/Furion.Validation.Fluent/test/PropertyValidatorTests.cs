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

using System.ComponentModel.DataAnnotations;

namespace Furion.Validation.Fluent.Tests;

public class PropertyValidatorTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var propertyValidator = new PropertyValidator<PropertyModel, string?>(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var propertyValidator = new PropertyValidator<PropertyModel, string?>(new ObjectValidator<PropertyModel>(), null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.NotNull(propertyValidator._objectValidator);
        Assert.NotNull(propertyValidator._annotationValidator);
        Assert.NotNull(propertyValidator.Validators);
        Assert.Empty(propertyValidator.Validators);
        Assert.NotNull(propertyValidator.PropertyName);
        Assert.Equal("Name", propertyValidator.PropertyName);
        Assert.NotNull(propertyValidator.Property);
        Assert.NotNull(propertyValidator.Options);
        Assert.True(propertyValidator.Options.SuppressAnnotationValidation);
        Assert.Equal(ValidatorCascadeMode.Continue, propertyValidator.Options.CascadeMode);
        Assert.Null(propertyValidator.ValidationObjectResolver);
        Assert.Null(propertyValidator.Condition);
        Assert.Null(propertyValidator.SubValidator);
        Assert.Null(propertyValidator.RuleSet);
        Assert.Null(propertyValidator.DisplayName);
    }

    [Fact]
    public void ConfigureOptions_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.ConfigureOptions(null!);
        });
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void ConfigureOptions_ReturnOK(bool enable, bool result)
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        propertyValidator.ConfigureOptions(o =>
        {
            o.SuppressAnnotationValidation = enable;
            o.CascadeMode = ValidatorCascadeMode.UsingFirstSuccess;
        });

        Assert.Equal(result, propertyValidator.Options.SuppressAnnotationValidation);
        Assert.Equal(ValidatorCascadeMode.UsingFirstSuccess, propertyValidator.Options.CascadeMode);
    }

    [Fact]
    public void WithErrorMessage_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        propertyValidator.WithErrorMessage("错误信息1");
        Assert.Null(propertyValidator.Validators.LastOrDefault()?.ErrorMessage);

        propertyValidator.Age().WithErrorMessage("错误信息2");
        Assert.Equal("错误信息2", propertyValidator.Validators.LastOrDefault()?.ErrorMessage);

        propertyValidator.NotNull().WithErrorMessage("错误信息3");
        Assert.Equal("错误信息3", propertyValidator.Validators.LastOrDefault()?.ErrorMessage);

        var errorMessages = new[] { "错误信息2", "错误信息3" };
        Assert.Equal(errorMessages, propertyValidator.Validators.Select(v => v.ErrorMessage).ToArray());
    }

    [Fact]
    public void WithDisplayName_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.WithDisplayName(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            propertyValidator.WithDisplayName(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            propertyValidator.WithDisplayName("");
        });
    }

    [Fact]
    public void WithDisplayName_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        propertyValidator.WithDisplayName("Furion");

        Assert.Equal("Furion", propertyValidator.DisplayName);
    }

    [Fact]
    public void SetValidator_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.SetValidator(null!);
        });
    }

    [Fact]
    public void SetValidator_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, SubModel>(objectValidator, u => u.SubModel);

        propertyValidator.SetValidator(new SubModelValidator());

        Assert.NotNull(propertyValidator.SubValidator);
    }

    [Fact]
    public void SetValidationObjectResolver_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.SetValidationObjectResolver(null!);
        });
    }

    [Fact]
    public void SetValidationObjectResolver_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        propertyValidator.SetValidationObjectResolver(context => context.PropertyValue);

        Assert.NotNull(propertyValidator.ValidationObjectResolver);
    }

    [Fact]
    public void When_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.When(null!);
        });
    }

    [Fact]
    public void When_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.When(u => u.Name is not null);

        Assert.NotNull(propertyValidator.Condition);
    }

    [Fact]
    public void WhenContext_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.WhenContext(null!);
        });
    }

    [Fact]
    public void WhenContext_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.WhenContext(u => u.ObjectInstance is not null);

        Assert.NotNull(propertyValidator.Condition);
    }

    [Fact]
    public void Reset_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        propertyValidator.Reset();

        Assert.Null(propertyValidator.Condition);
    }

    [Fact]
    public void CanValidate_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.CanValidate(null!);
        });
    }

    [Fact]
    public void CanValidate_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        var instance = new PropertyModel
        {
            Name = "Furion"
        };

        Assert.True(propertyValidator.CanValidate(instance));

        propertyValidator.When(u => u.Name == "Furion");
        Assert.True(propertyValidator.CanValidate(instance));

        propertyValidator.When(u => u.Name != "Furion");
        Assert.False(propertyValidator.CanValidate(instance));
    }

    [Fact]
    public void IsValid_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            objectValidator.IsValid(null!);
        });
    }

    [Fact]
    public void IsValid_EmptyValidators_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        var instance = new PropertyModel
        {
            Id = 1,
            Name = "Furion"
        };

        Assert.True(objectValidator.IsValid(instance));
    }

    [Fact]
    public void IsValid_WithCondition_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id)
            .GreaterThan(1);
        propertyValidator.When(u => u.Id > 1);

        var instance = new PropertyModel
        {
            Id = 1,
            Name = "Furion"
        };

        Assert.True(propertyValidator.IsValid(instance));

        propertyValidator.Reset();

        Assert.False(propertyValidator.IsValid(instance));
    }

    [Fact]
    public void IsValid_WithAnnotationValidation_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name)
           .Equal("furion");

        var instance = new PropertyModel
        {
            Id = 0,
            Name = "furion"
        };

        Assert.True(propertyValidator.IsValid(instance));

        propertyValidator.Options.SuppressAnnotationValidation = false;

        Assert.False(propertyValidator.IsValid(instance));
    }

    [Fact]
    public void IsValid_WithSubValidator_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, SubModel>(objectValidator, u => u.SubModel)
            .NotNull();

        var instance = new PropertyModel
        {
            Id = 0,
            Name = "furion",
            SubModel = new()
            {
                Name = "furion"
            }
        };

        Assert.True(propertyValidator.IsValid(instance));

        propertyValidator.SetValidator(new SubModelValidator());
        Assert.False(propertyValidator.IsValid(instance));
    }

    [Fact]
    public void GetValidationResults_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            objectValidator.GetValidationResults(null!);
        });
    }

    [Fact]
    public void GetValidationResults_EmptyValidators_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        var instance = new PropertyModel
        {
            Id = 1,
            Name = "Furion"
        };

        Assert.Null(objectValidator.GetValidationResults(instance));
    }

    [Fact]
    public void GetValidationResults_WithCondition_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id)
            .GreaterThan(1);
        propertyValidator.When(u => u.Id > 1);

        var instance = new PropertyModel
        {
            Id = 1,
            Name = "Furion"
        };

        Assert.Null(propertyValidator.GetValidationResults(instance));

        propertyValidator.Reset();

        var validationResults = propertyValidator.GetValidationResults(instance);
        Assert.NotNull(validationResults);
        Assert.Single(validationResults);
        Assert.Equal("The field Id must be greater than '1'.", validationResults.First().ErrorMessage);
    }

    [Fact]
    public void GetValidationResults_WithAnnotationValidation_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name)
           .Equal("furion");

        var instance = new PropertyModel
        {
            Id = 0,
            Name = "furion"
        };

        Assert.Null(propertyValidator.GetValidationResults(instance));

        propertyValidator.Options.SuppressAnnotationValidation = false;

        instance.Name = "百小僧";
        var validationResults = propertyValidator.GetValidationResults(instance);
        Assert.NotNull(validationResults);
        Assert.Equal(2, validationResults.Count);

        var errorMessages = new[] { "The field Name must be a string or array type with a minimum length of '10'."
            ,"The field Name must be equal to 'furion'."};
        Assert.Equal(errorMessages, validationResults.Select(u => u.ErrorMessage).ToArray());
    }

    [Fact]
    public void GetValidationResults_WithSubValidator_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, SubModel>(objectValidator, u => u.SubModel)
            .NotNull();

        var instance = new PropertyModel
        {
            Id = 0,
            Name = "furion",
            SubModel = new()
            {
                Name = "furion"
            }
        };

        Assert.Null(propertyValidator.GetValidationResults(instance));

        propertyValidator.SetValidator(new SubModelValidator());

        var validationResults = propertyValidator.GetValidationResults(instance);
        Assert.NotNull(validationResults);
        Assert.Single(validationResults);

        var errorMessages = new[] { "The field Name cannot be equal to 'furion'." };
        Assert.Equal(errorMessages, validationResults.Select(u => u.ErrorMessage).ToArray());
    }

    [Fact]
    public void GetValidationResults_WithErrorMessage_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id)
            .GreaterThan(1)
            .WithErrorMessage("验证 {0} 失败");

        var instance = new PropertyModel
        {
            Id = 1,
            Name = "Furion"
        };

        var validationResults = propertyValidator.GetValidationResults(instance);
        Assert.NotNull(validationResults);
        Assert.Single(validationResults);
        Assert.Equal("验证 Id 失败", validationResults.First().ErrorMessage);
    }

    [Fact]
    public void Validate_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            objectValidator.Validate(null!);
        });
    }

    [Fact]
    public void Validate_EmptyValidators_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        var instance = new PropertyModel
        {
            Id = 1,
            Name = "Furion"
        };

        objectValidator.Validate(instance);
    }

    [Fact]
    public void Validate_WithCondition_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id)
            .GreaterThan(1);
        propertyValidator.When(u => u.Id > 1);

        var instance = new PropertyModel
        {
            Id = 1,
            Name = "Furion"
        };

        objectValidator.Validate(instance);

        propertyValidator.Reset();

        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            propertyValidator.Validate(instance);
        });

        Assert.NotNull(exception.InnerExceptions);
        Assert.Single(exception.InnerExceptions);
        Assert.Equal("The field Id must be greater than '1'.", exception.InnerExceptions.First().Message);
    }

    [Fact]
    public void Validate_WithAnnotationValidation_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name)
           .Equal("furion");

        var instance = new PropertyModel
        {
            Id = 0,
            Name = "furion"
        };

        objectValidator.Validate(instance);

        propertyValidator.Options.SuppressAnnotationValidation = false;

        instance.Name = "百小僧";
        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            propertyValidator.Validate(instance);
        });
        Assert.NotNull(exception.InnerExceptions);
        Assert.Equal(2, exception.InnerExceptions.Count);

        var errorMessages = new[] { "The field Name must be a string or array type with a minimum length of '10'."
            ,"The field Name must be equal to 'furion'."};
        Assert.Equal(errorMessages, exception.InnerExceptions.Select(u => u.Message).ToArray());
    }

    [Fact]
    public void Validate_WithSubValidator_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, SubModel>(objectValidator, u => u.SubModel)
            .NotNull();

        var instance = new PropertyModel
        {
            Id = 0,
            Name = "furion",
            SubModel = new()
            {
                Name = "furion"
            }
        };

        objectValidator.Validate(instance);

        propertyValidator.SetValidator(new SubModelValidator());

        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            propertyValidator.Validate(instance);
        });
        Assert.NotNull(exception.InnerExceptions);
        Assert.Single(exception.InnerExceptions);

        var errorMessages = new[] { "The field Name cannot be equal to 'furion'." };
        Assert.Equal(errorMessages, exception.InnerExceptions.Select(u => u.Message).ToArray());
    }

    [Fact]
    public void GetPropertyValue_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.GetPropertyValue(null!);
        });
    }

    [Fact]
    public void GetPropertyValue_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var instance = new PropertyModel
        {
            Id = 1,
            Name = "furion",
            SubModel = new()
            {
                Name = "furion"
            }
        };

        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        Assert.Equal(1, propertyValidator.GetPropertyValue(instance));

        var propertyValidator2 = new PropertyValidator<PropertyModel, string>(objectValidator, u => u.Name);
        Assert.Equal("furion", propertyValidator2.GetPropertyValue(instance));

        var propertyValidator3 = new PropertyValidator<PropertyModel, SubModel>(objectValidator, u => u.SubModel);
        Assert.Equal("furion", propertyValidator3.GetPropertyValue(instance)?.Name);
    }

    [Fact]
    public void ResolveValidationObject_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.ResolveValidationObject(null!, null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.ResolveValidationObject(new(), null!, null!);
        });
    }

    [Fact]
    public void ResolveValidationObject_CheckValidatorDelegator_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        var instance = new PropertyModel
        {
            Name = "furion"
        };

        var validatorDelegator = new PropertyValidator<PropertyModel, string?>.ValidatorDelegator<EqualValidator>(propertyValidator
            , instance => new[] { "furion" }
            , () => Strings.EqualValidator_Invalid);

        Assert.True(propertyValidator.ResolveValidationObject(instance, validatorDelegator, "furion") is PropertyModel);

        Assert.True(propertyValidator.ResolveValidationObject(instance, new NotEmptyValidator(), "furion") is string);
    }

    [Fact]
    public void ResolveValidationObject_WithValidationObjectResolver_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        var instance = new PropertyModel
        {
            Name = "furion"
        };

        Assert.True(propertyValidator.ResolveValidationObject(instance, new NotEmptyValidator(), "furion") is string);

        propertyValidator.SetValidationObjectResolver(context => context.ObjectInstance);

        Assert.True(propertyValidator.ResolveValidationObject(instance, new NotEmptyValidator(), "furion") is PropertyModel);
    }

    [Fact]
    public void GetDisplayName_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Equal("Name", propertyValidator.GetDisplayName());

        propertyValidator.WithDisplayName("其他名称");

        Assert.Equal("其他名称", propertyValidator.GetDisplayName());
    }

    [Fact]
    public void IsInRuleSet_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.True(propertyValidator.IsInRuleSet());
        Assert.False(propertyValidator.IsInRuleSet("furion"));
        Assert.True(propertyValidator.IsInRuleSet("*"));

        propertyValidator.RuleSet = new[] { "furion" };
        Assert.False(propertyValidator.IsInRuleSet());
        Assert.True(propertyValidator.IsInRuleSet("furion"));
        Assert.True(propertyValidator.IsInRuleSet("*"));
    }
}