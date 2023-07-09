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

public class ObjectAnnotationValidatorTests
{
    [Fact]
    public void IsValid_Failure()
    {
        var validator = new ObjectAnnotationValidator();
        var model = new ObjectModel
        {
            Id = 0,
            Name = "fu",
            Age = 130
        };
        Assert.False(validator.IsValid(model));
    }

    [Fact]
    public void IsValid_OK()
    {
        var validator = new ObjectAnnotationValidator();
        Assert.True(validator.IsValid(null));

        var model = new ObjectModel
        {
            Id = 1,
            Name = "furion",
            Age = 31
        };
        Assert.True(validator.IsValid(model));
        Assert.True(validator.IsValid("furion"));
    }

    [Fact]
    public void GetValidationResults_Failure()
    {
        var validator = new ObjectAnnotationValidator();
        var model = new ObjectModel
        {
            Id = 0,
            Name = "fu",
            Age = 130
        };

        var validationResults = validator.GetValidationResults(model, null!);
        Assert.NotNull(validationResults);
        Assert.Equal(3, validationResults.Count);
    }

    [Fact]
    public void GetValidationResults_OK()
    {
        var validator = new ObjectAnnotationValidator();
        var model = new ObjectModel
        {
            Id = 1,
            Name = "furion",
            Age = 31
        };

        var validationResults = validator.GetValidationResults(model, null!);
        Assert.Null(validationResults);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new ObjectAnnotationValidator
        {
            ErrorMessage = "自定义验证失败消息"
        };
        var model = new ObjectModel
        {
            Id = 0,
            Name = "fu",
            Age = 130
        };
        var failure = validator.GetValidationResult(model, null!);
        Assert.NotNull(failure);
        Assert.Equal("自定义验证失败消息", failure.ErrorMessage);

        var validationResults = validator.GetValidationResults(model, null!);
        Assert.NotNull(validationResults);
        Assert.True(validationResults.Count > 1);
        Assert.Equal("自定义验证失败消息", validationResults.First().ErrorMessage);
    }
}