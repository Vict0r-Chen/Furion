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

namespace Furion.Validation.Fluent.Tests;

public class AbstractValidatorTests
{
    [Fact]
    public void New_Default()
    {
        var validator = new TestModelValidator();

        Assert.NotNull(validator);
        Assert.NotNull(validator._objectValidator);
        Assert.Null(validator._ruleSet);
        Assert.True(validator.SuppressAnnotationValidation);
        Assert.Equal(ValidatorCascadeMode.Continue, validator.CascadeMode);
        Assert.Equal(2, validator._objectValidator._propertyValidators.Count);
    }

    [Fact]
    public void RuleFor_ReturnOK()
    {
        var validator = new TestModelValidator();
        validator.RuleFor(u => u.Id).Range(10, 100);

        Assert.Equal(3, validator._objectValidator._propertyValidators.Count);
    }

    [Fact]
    public void RuleFor_WithRuleSet_ReturnOK()
    {
        var validator = new TestModelValidator();
        validator.RuleFor(u => u.Id, "furion").Range(10, 100);

        Assert.Equal(3, validator._objectValidator._propertyValidators.Count);

        var propertyValidator = validator._objectValidator._propertyValidators.Last() as PropertyValidator<TestModel, int>;
        Assert.NotNull(propertyValidator);
        Assert.Equal("furion", propertyValidator.RuleSet?.First());
    }

    [Fact]
    public void RuleSet_Invalid_Parameters()
    {
        var validator = new TestModelValidator();

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.RuleSet(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            validator.RuleSet(string.Empty, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            validator.RuleSet("", null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            validator.RuleSet("furion", null!);
        });
    }

    [Fact]
    public void RuleSet_ReturnOK()
    {
        var validator = new TestModelValidator();

        validator.RuleSet("furion", () =>
        {
            validator.RuleFor(u => u.Id).GreaterThan(1);
        });

        Assert.Null(validator._ruleSet);

        var propertyValidator = validator._objectValidator._propertyValidators.Last() as PropertyValidator<TestModel, int>;
        Assert.NotNull(propertyValidator);
        Assert.Equal("furion", propertyValidator.RuleSet?.First());
    }

    [Fact]
    public void When_ReturnOK()
    {
        var validator = new TestModelValidator();
        validator.When(u => u.Id > 0);

        Assert.NotNull(validator._objectValidator.ConditionExpression);
    }

    [Fact]
    public void WhenContext_ReturnOK()
    {
        var validator = new TestModelValidator();
        validator.WhenContext(u => true);

        Assert.NotNull(validator._objectValidator.ConditionExpression);
    }

    [Fact]
    public void Reset_ReturnOK()
    {
        var validator = new TestModelValidator();
        validator.When(u => u.Id > 0);

        validator.Reset();

        Assert.Null(validator._objectValidator.ConditionExpression);
        Assert.Null(validator._objectValidator.Items);
    }

    [Fact]
    public void IsValid_ReturnOK()
    {
        var validator = new TestModelValidator();

        Assert.True(validator.IsValid(new TestModel
        {
            Id = 2,
            Name = "百小僧"
        }));
    }

    [Fact]
    public void GetValidationResults_ReturnOK()
    {
        var validator = new TestModelValidator();

        var validationResults = validator.GetValidationResults(new TestModel
        {
            Id = 1,
            Name = "furion"
        });

        Assert.NotNull(validationResults);
        Assert.Equal(2, validationResults.Count);
    }

    [Fact]
    public void Validate_ReturnOK()
    {
        var validator = new TestModelValidator();

        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            validator.Validate(new TestModel
            {
                Id = 1,
                Name = "furion"
            });
        });

        Assert.NotNull(exception.InnerExceptions);
        Assert.Equal(2, exception.InnerExceptions.Count);
    }

    [Fact]
    public void IsInRuleSet_ReturnOK()
    {
        var validator = new TestModelValidator();

        Assert.True(validator.IsInRuleSet());
        Assert.True(validator.IsInRuleSet("any"));
    }
}