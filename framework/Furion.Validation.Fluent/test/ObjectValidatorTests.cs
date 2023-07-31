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

public class ObjectValidatorTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var validator = new ObjectValidator<ObjectModel>();

        Assert.NotNull(validator._propertyValidators);
        Assert.Empty(validator._propertyValidators);
        Assert.NotNull(validator._annotationValidator);
        Assert.NotNull(validator.Options);
        Assert.True(validator.Options.SuppressAnnotationValidation);
        Assert.Equal(ValidatorCascadeMode.Continue, validator.Options.CascadeMode);
        Assert.Null(validator.Condition);
        Assert.Null(validator.Items);
    }

    [Fact]
    public void Create_ReturnOK()
    {
        var validator = ObjectValidator<ObjectModel>.Create();
        Assert.NotNull(validator);

        var validator2 = ObjectValidator<ObjectModel>.Create(v =>
        {
            v.Options.SuppressAnnotationValidation = false;
        });
        Assert.NotNull(validator2);
        Assert.False(validator2.Options.SuppressAnnotationValidation);
    }

    [Fact]
    public void Property_Invalid_Parameters()
    {
        var validator = new ObjectValidator<ObjectModel>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.Property<string>(null!);
        });
    }

    [Fact]
    public void Property_ReturnOK()
    {
        var validator = new ObjectValidator<ObjectModel>();
        var propertyValidator = validator.Property(u => u.Name);

        Assert.NotNull(propertyValidator);
        Assert.Single(validator._propertyValidators);

        var propertyValidator2 = validator._propertyValidators.First() as PropertyValidator<ObjectModel, string>;
        Assert.NotNull(propertyValidator2);
        Assert.Equal("Name", propertyValidator2.PropertyName);
    }

    [Fact]
    public void Property_WithRuleSet_ReturnOK()
    {
        var validator = new ObjectValidator<ObjectModel>();
        var propertyValidator = validator.Property(u => u.Name, "furion");

        Assert.Equal("furion", propertyValidator.RuleSet?.First());

        var propertyValidator2 = validator.Property(u => u.Name, "furion,fur");
        Assert.Equal(new[] { "furion", "fur" }, propertyValidator2.RuleSet);

        var propertyValidator3 = validator.Property(u => u.Name, "furion;fur");
        Assert.Equal(new[] { "furion", "fur" }, propertyValidator3.RuleSet);
    }

    [Fact]
    public void ConfigureOptions_Invalid_Parameters()
    {
        var validator = new ObjectValidator<ObjectModel>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.ConfigureOptions(null!);
        });
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void ConfigureOptions_ReturnOK(bool enable, bool result)
    {
        var validator = new ObjectValidator<ObjectModel>();
        validator.ConfigureOptions(o =>
        {
            o.SuppressAnnotationValidation = enable;
            o.CascadeMode = ValidatorCascadeMode.UsingFirstSuccess;
        });

        Assert.Equal(result, validator.Options.SuppressAnnotationValidation);
        Assert.Equal(ValidatorCascadeMode.UsingFirstSuccess, validator.Options.CascadeMode);
        Assert.True(validator.Options.ValidateAllPropertiesForObjectAnnotationValidator);
        Assert.True(validator._annotationValidator.ValidateAllProperties);

        validator.ConfigureOptions(o =>
        {
            o.ValidateAllPropertiesForObjectAnnotationValidator = false;
        });

        Assert.False(validator.Options.ValidateAllPropertiesForObjectAnnotationValidator);
        Assert.False(validator._annotationValidator.ValidateAllProperties);
    }

    [Fact]
    public void When_Invalid_Parameters()
    {
        var validator = new ObjectValidator<ObjectModel>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.When(null!);
        });
    }

    [Fact]
    public void When_ReturnOK()
    {
        var validator = new ObjectValidator<ObjectModel>();
        validator.When(u => u.Name != null);

        Assert.NotNull(validator.Condition);
    }

    [Fact]
    public void WhenContext_Invalid_Parameters()
    {
        var validator = new ObjectValidator<ObjectModel>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.WhenContext(null!);
        });
    }

    [Fact]
    public void WhenContext_ReturnOK()
    {
        var validator = new ObjectValidator<ObjectModel>();
        validator.WhenContext(u => u.ObjectInstance != null);

        Assert.NotNull(validator.Condition);
    }

    [Fact]
    public void Reset_ReturnOK()
    {
        var validator = new ObjectValidator<ObjectModel>();
        validator.Reset();

        Assert.Null(validator.Condition);
        Assert.Null(validator.Items);
    }

    [Fact]
    public void CanValidate_Invalid_Parameters()
    {
        var validator = new ObjectValidator<ObjectModel>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.CanValidate(null!);
        });
    }

    [Fact]
    public void CanValidate_ReturnOK()
    {
        var validator = new ObjectValidator<ObjectModel>();
        var instance = new ObjectModel
        {
            Name = "Furion"
        };

        Assert.True(validator.CanValidate(instance));

        validator.When(u => u.Name == "Furion");
        Assert.True(validator.CanValidate(instance));

        validator.When(u => u.Name != "Furion");
        Assert.False(validator.CanValidate(instance));
    }

    [Fact]
    public void IsValid_Invalid_Parameters()
    {
        var validator = new ObjectValidator<ObjectModel>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.IsValid(null!);
        });
    }

    [Fact]
    public void IsValid_EmptyValidators_ReturnOK()
    {
        var validator = new ObjectValidator<ObjectModel>();
        var instance = new ObjectModel
        {
            Id = 1,
            Name = "Furion"
        };

        Assert.True(validator.IsValid(instance));
    }

    [Fact]
    public void IsValid_WithCondition_ReturnOK()
    {
        var validator = ObjectValidator<ObjectModel>.Create(v =>
        {
            v.When(u => u.Id > 1);
        });

        validator.Property(u => u.Id).GreaterThan(1);
        var instance = new ObjectModel
        {
            Id = 1,
            Name = "Furion"
        };

        Assert.True(validator.IsValid(instance));

        validator.Reset();

        Assert.False(validator.IsValid(instance));
    }

    [Fact]
    public void IsValid_WithAnnotationValidation_ReturnOK()
    {
        var validator = ObjectValidator<ObjectModel>.Create();
        validator.Property(u => u.Name).Equal("furion");

        var instance = new ObjectModel
        {
            Id = 0,
            Name = "furion"
        };

        Assert.True(validator.IsValid(instance));

        validator.Options.SuppressAnnotationValidation = false;

        Assert.False(validator.IsValid(instance));
    }

    [Fact]
    public void IsValid_WithRuleSet_ReturnOK()
    {
        var validator = ObjectValidator<ObjectModel>.Create();

        validator.Property(u => u.Id).GreaterThan(1);
        validator.Property(u => u.Id, "one").GreaterThan(2);
        validator.Property(u => u.Id, "two").GreaterThan(3);

        var instance = new ObjectModel
        {
            Id = 2
        };

        Assert.True(validator.IsValid(instance));
        Assert.False(validator.IsValid(instance, "one"));

        instance.Id = 3;
        Assert.True(validator.IsValid(instance, "one"));

        Assert.False(validator.IsValid(instance, "two"));

        instance.Id = 4;
        Assert.True(validator.IsValid(instance, "two"));

        instance.Id = 1;
        Assert.False(validator.IsValid(instance, "*"));
    }

    [Fact]
    public void GetValidationResults_Invalid_Parameters()
    {
        var validator = new ObjectValidator<ObjectModel>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.GetValidationResults(null!);
        });
    }

    [Fact]
    public void GetValidationResults_EmptyValidators_ReturnOK()
    {
        var validator = new ObjectValidator<ObjectModel>();
        var instance = new ObjectModel
        {
            Id = 1,
            Name = "Furion"
        };

        Assert.Null(validator.GetValidationResults(instance));
    }

    [Fact]
    public void GetValidationResults_WithCondition_ReturnOK()
    {
        var validator = ObjectValidator<ObjectModel>.Create(v =>
        {
            v.When(u => u.Id > 1);
        });

        validator.Property(u => u.Id).GreaterThan(1);
        var instance = new ObjectModel
        {
            Id = 1,
            Name = "Furion"
        };

        Assert.Null(validator.GetValidationResults(instance));

        validator.Reset();

        var validationResults = validator.GetValidationResults(instance);
        Assert.NotNull(validationResults);
        Assert.Single(validationResults);
        Assert.Equal("The field Id must be greater than '1'.", validationResults.First().ErrorMessage);
    }

    [Fact]
    public void GetValidationResults_WithAnnotationValidation_ReturnOK()
    {
        var validator = ObjectValidator<ObjectModel>.Create();
        validator.Property(u => u.Name).Equal("furion");

        var instance = new ObjectModel
        {
            Id = 0,
            Name = "furion"
        };

        Assert.Null(validator.GetValidationResults(instance));

        validator.Options.SuppressAnnotationValidation = false;

        instance.Name = "百小僧";
        var validationResults = validator.GetValidationResults(instance);
        Assert.NotNull(validationResults);
        Assert.Equal(3, validationResults.Count);

        var errorMessages = new[] { "The field Id must be between 1 and 10."
            , "The field Name must be a string or array type with a minimum length of '10'."
            ,"The field Name must be equal to 'furion'."};
        Assert.Equal(errorMessages, validationResults.Select(u => u.ErrorMessage).ToArray());
    }

    [Fact]
    public void GetValidationResults_WithRuleSet_ReturnOK()
    {
        var validator = ObjectValidator<ObjectModel>.Create();

        validator.Property(u => u.Id).GreaterThan(1);
        validator.Property(u => u.Id, "one").GreaterThan(2);
        validator.Property(u => u.Id, "two").GreaterThan(3);

        var instance = new ObjectModel
        {
            Id = 2
        };

        Assert.Null(validator.GetValidationResults(instance));

        var validationResult = validator.GetValidationResults(instance, "one");
        Assert.NotNull(validationResult);
        Assert.Single(validationResult);

        instance.Id = 3;
        Assert.Null(validator.GetValidationResults(instance, "one"));

        var validationResult2 = validator.GetValidationResults(instance, "two");
        Assert.NotNull(validationResult2);
        Assert.Single(validationResult2);

        instance.Id = 4;
        Assert.Null(validator.GetValidationResults(instance, "two"));

        instance.Id = 1;
        var validationResult3 = validator.GetValidationResults(instance, "*");
        Assert.NotNull(validationResult3);
        Assert.Equal(3, validationResult3.Count);
    }

    [Fact]
    public void Validate_Invalid_Parameters()
    {
        var validator = new ObjectValidator<ObjectModel>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.Validate(null!);
        });
    }

    [Fact]
    public void Validate_EmptyValidators_ReturnOK()
    {
        var validator = new ObjectValidator<ObjectModel>();
        var instance = new ObjectModel
        {
            Id = 1,
            Name = "Furion"
        };

        validator.Validate(instance);
    }

    [Fact]
    public void Validate_WithCondition_ReturnOK()
    {
        var validator = ObjectValidator<ObjectModel>.Create(v =>
        {
            v.When(u => u.Id > 1);
        });

        validator.Property(u => u.Id).GreaterThan(1);
        var instance = new ObjectModel
        {
            Id = 1,
            Name = "Furion"
        };

        validator.Validate(instance);

        validator.Reset();

        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            validator.Validate(instance);
        });

        Assert.NotNull(exception);
        Assert.Single(exception.InnerExceptions);
        Assert.Equal("The field Id must be greater than '1'.", exception.InnerExceptions.First().Message);
    }

    [Fact]
    public void Validate_WithAnnotationValidation_ReturnOK()
    {
        var validator = ObjectValidator<ObjectModel>.Create();
        validator.Property(u => u.Name).Equal("furion");

        var instance = new ObjectModel
        {
            Id = 0,
            Name = "furion"
        };

        validator.Validate(instance);

        validator.Options.SuppressAnnotationValidation = false;

        instance.Name = "百小僧";
        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            validator.Validate(instance);
        });
        Assert.NotNull(exception);
        Assert.Equal(3, exception.InnerExceptions.Count);

        var errorMessages = new[] { "The field Id must be between 1 and 10."
            , "The field Name must be a string or array type with a minimum length of '10'."
            ,"The field Name must be equal to 'furion'."};
        Assert.Equal(errorMessages, exception.InnerExceptions.Select(u => u.Message).ToArray());
    }

    [Fact]
    public void Validate_WithRuleSet_ReturnOK()
    {
        var validator = ObjectValidator<ObjectModel>.Create();

        validator.Property(u => u.Id).GreaterThan(1);
        validator.Property(u => u.Id, "one").GreaterThan(2);
        validator.Property(u => u.Id, "two").GreaterThan(3);

        var instance = new ObjectModel
        {
            Id = 2
        };

        validator.Validate(instance);

        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            validator.Validate(instance, "one");
        });
        Assert.NotNull(exception.InnerExceptions);
        Assert.Single(exception.InnerExceptions);

        instance.Id = 3;
        validator.Validate(instance, "one");

        var exception2 = Assert.Throws<AggregateValidationException>(() =>
        {
            validator.Validate(instance, "two");
        });
        Assert.NotNull(exception2.InnerExceptions);
        Assert.Single(exception2.InnerExceptions);

        instance.Id = 4;
        validator.Validate(instance, "two");

        instance.Id = 1;
        var exception3 = Assert.Throws<AggregateValidationException>(() =>
        {
            validator.Validate(instance, "*");
        });
        Assert.NotNull(exception3.InnerExceptions);
        Assert.Equal(3, exception3.InnerExceptions.Count);
    }

    [Fact]
    public void IsInRuleSet_ReturnOK()
    {
        var validator = ObjectValidator<ObjectModel>.Create();

        Assert.True(validator.IsInRuleSet());
        Assert.True(validator.IsInRuleSet("any"));
    }
}