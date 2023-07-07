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

public class CompositeValidatorTests
{
    [Fact]
    public void NewInstance_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new CompositeValidator(null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            var validator = new CompositeValidator(new AgeValidator(), null!, new ChineseNameValidator());
        });
        Assert.Equal("The validator collection contains a null value. (Parameter 'validators')", exception.Message);

        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new CompositeValidator((List<ValidatorBase>)null!);
        });
    }

    [Fact]
    public void NewInstance_ReturnOK()
    {
        var validator = new CompositeValidator();
        Assert.NotNull(validator);
        Assert.Equal(ValidatorRelationship.Default, validator.Relationship);

        var validator2 = new CompositeValidator(new CompositeValidator());
        Assert.NotNull(validator2);

        var validator3 = new CompositeValidator(new AgeValidator(), new ChineseNameValidator());
        Assert.NotNull(validator3);

        var validator4 = new CompositeValidator(new CompositeValidator(new AgeValidator(), new ChineseNameValidator()), new AgeValidator(), new CompositeValidator(new ChineseNameValidator()));
        Assert.NotNull(validator4);
    }

    [Fact]
    public void IsValid_Default()
    {
        var validator = new CompositeValidator();
        Assert.True(validator.IsValid(null));
        Assert.True(validator.IsValid(1));
        Assert.True(validator.IsValid(""));
    }

    [Fact]
    public void IsValid_ValidatorRelationship_And()
    {
        var validator = new CompositeValidator(new AgeValidator(), new CustomValidator(v => { return v is int; }));
        Assert.False(validator.IsValid(130));
        Assert.True(validator.IsValid(100));
    }

    [Fact]
    public void IsValid_ValidatorRelationship_Or()
    {
        var validator = new CompositeValidator(new AgeValidator(), new CustomValidator(v => { return v is int; }))
        {
            Relationship = ValidatorRelationship.Or
        };
        Assert.True(validator.IsValid(130));
        Assert.True(validator.IsValid(100));
        Assert.False(validator.IsValid(123.456m));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new CompositeValidator(new AgeValidator(), new CustomValidator(v => { return v is int; }));

        var failure = validator.GetValidationResult(130);
        Assert.NotNull(failure);
        Assert.Equal("The field is not a valid age format.", failure.ErrorMessage);

        var failure2 = validator.GetValidationResult(130, new List<string> { "Value" });
        Assert.NotNull(failure2);
        Assert.Equal("The field Value is not a valid age format.", failure2.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new CompositeValidator(new AgeValidator(), new CustomValidator(v => { return v is int; }))
        {
            ErrorMessage = "不是一个有效的组合格式"
        };

        var failure = validator.GetValidationResult(130);
        Assert.NotNull(failure);

        Assert.Equal("不是一个有效的组合格式", failure.ErrorMessage);
    }

    [Fact]
    public void GetValidationResults()
    {
        var validator = new CompositeValidator(new AgeValidator(), new CustomValidator(v => { return v is int; }));
        var validationResults = validator.GetValidationResults(123.456m);

        Assert.NotNull(validationResults);
        Assert.Equal(2, validationResults.Count);

        Assert.Equal("The field is not a valid age format.", validationResults.ElementAt(0).ErrorMessage);
        Assert.Equal("The field is invalid.", validationResults.ElementAt(1).ErrorMessage);
    }

    [Fact]
    public void GetValidationResults_With_MemberNames()
    {
        var validator = new CompositeValidator(new AgeValidator(), new CustomValidator(v => { return v is int; }));
        var validationResults = validator.GetValidationResults(123.456m, new List<string> { "Value" });

        Assert.NotNull(validationResults);
        Assert.Equal(2, validationResults.Count);

        Assert.Equal("The field Value is not a valid age format.", validationResults.ElementAt(0).ErrorMessage);
        Assert.Equal("The field Value is invalid.", validationResults.ElementAt(1).ErrorMessage);
    }
}