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
    public void NewInstance_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new PropertyAnnotationValidator(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var validator = new PropertyAnnotationValidator(string.Empty);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new PropertyAnnotationValidator<ObjectModel>(null!);
        });
    }

    [Fact]
    public void NewInstance_OK()
    {
        var model = new ObjectModel();
        var validator = new PropertyAnnotationValidator("Name");
        Assert.NotNull(validator);
        Assert.Equal("Name", validator.PropertyName);

        var validator2 = new PropertyAnnotationValidator<ObjectModel>(u => u.Name);
        Assert.NotNull(validator2);
        Assert.Equal("Name", validator2.PropertyName);
    }

    [Fact]
    public void IsValid_Failure()
    {
        var model = new ObjectModel
        {
            Id = 0,
            Name = "fu",
            Age = 130
        };
        Assert.False(new PropertyAnnotationValidator<ObjectModel>(u => u.Id).IsValid(model));
        Assert.False(new PropertyAnnotationValidator<ObjectModel>(u => u.Name).IsValid(model));
        Assert.False(new PropertyAnnotationValidator<ObjectModel>(u => u.Age).IsValid(model));
    }

    [Fact]
    public void IsValid_OK()
    {
        var model = new ObjectModel
        {
            Id = 1,
            Name = "furion",
            Age = 31
        };
        Assert.True(new PropertyAnnotationValidator<ObjectModel>(u => u.Id).IsValid(model));
        Assert.True(new PropertyAnnotationValidator<ObjectModel>(u => u.Name).IsValid(model));
        Assert.True(new PropertyAnnotationValidator<ObjectModel>(u => u.Age).IsValid(model));
        Assert.True(new PropertyAnnotationValidator<ObjectModel>(u => u.Address).IsValid(model));
    }

    [Fact]
    public void IsValid_Throw()
    {
        var model = new ObjectModel
        {
            Id = 1,
            Name = "furion",
            Age = 31
        };

        var validator = new PropertyAnnotationValidator<ObjectModel>(u => u.Id)
        {
            PropertyName = "Name1"
        };

        Assert.Throws<ArgumentNullException>(() => validator.IsValid(model));
    }

    [Fact]
    public void GetValidationResults_Failure()
    {
        var model = new ObjectModel
        {
            Id = 0,
            Name = "fu",
            Age = 130
        };
        var validator = new PropertyAnnotationValidator<ObjectModel>(u => u.Name);

        var validationResults = validator.GetValidationResults(model, null!);
        Assert.NotNull(validationResults);
    }

    [Fact]
    public void GetValidationResults_OK()
    {
        var model = new ObjectModel
        {
            Id = 1,
            Name = "furion",
            Age = 31
        };

        var validator = new PropertyAnnotationValidator<ObjectModel>(u => u.Name);

        var validationResults = validator.GetValidationResults(model, null!);
        Assert.Null(validationResults);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var model = new ObjectModel
        {
            Id = 0,
            Name = "fu",
            Age = 130
        };

        var validator = new PropertyAnnotationValidator<ObjectModel>(u => u.Name)
        {
            ErrorMessage = "自定义验证失败消息"
        };
        var validationResults = validator.GetValidationResults(model, null!);
        Assert.NotNull(validationResults);
        Assert.True(validationResults.Count > 1);

        Assert.Equal("自定义验证失败消息", validationResults.First().ErrorMessage);
    }
}