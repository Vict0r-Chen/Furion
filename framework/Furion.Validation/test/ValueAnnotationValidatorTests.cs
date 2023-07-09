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

public class ValueAnnotationValidatorTests
{
    [Fact]
    public void NewInstance_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new ValueAnnotationValidator(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new ValueAnnotationValidator((ValidationAttribute[])null!);
        });

        var validator = new ValueAnnotationValidator();
    }

    [Fact]
    public void NewInstance_OK()
    {
        var validator = new ValueAnnotationValidator(new RequiredAttribute());
        Assert.NotNull(validator);
        Assert.Single(validator.Attributes);
    }

    [Fact]
    public void IsValid_Failure()
    {
        Assert.False(new ValueAnnotationValidator(new RequiredAttribute()).IsValid(null));
        Assert.False(new ValueAnnotationValidator(new RequiredAttribute(), new MinLengthAttribute(7)).IsValid("furion"));
        Assert.False(new ValueAnnotationValidator(new AgeAttribute()).IsValid(130));
    }

    [Fact]
    public void IsValid_OK()
    {
        Assert.True(new ValueAnnotationValidator(new RequiredAttribute()).IsValid("furion"));
        Assert.True(new ValueAnnotationValidator(new RequiredAttribute(), new MinLengthAttribute(6)).IsValid("furion"));
        Assert.True(new ValueAnnotationValidator(new AgeAttribute()).IsValid(31));
    }

    [Fact]
    public void IsValid_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new ValueAnnotationValidator().IsValid(null);
        });
    }

    [Fact]
    public void GetValidationResults_Failure()
    {
        var validator = new ValueAnnotationValidator(new RequiredAttribute());

        var validationResults = validator.GetValidationResults(null!);
        Assert.NotNull(validationResults);

        validator.Attributes.Add(new MinLengthAttribute(3));
        var validationResults2 = validator.GetValidationResults("fu");
        Assert.NotNull(validationResults2);
    }

    [Fact]
    public void GetValidationResults_OK()
    {
        var validator = new ValueAnnotationValidator(new RequiredAttribute());

        var validationResults = validator.GetValidationResults("furion");
        Assert.Null(validationResults);

        validator.Attributes.Add(new MinLengthAttribute(3));
        var validationResults2 = validator.GetValidationResults("furion");
        Assert.Null(validationResults2);
    }
}