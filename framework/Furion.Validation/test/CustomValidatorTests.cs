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

public class CustomValidatorTests
{
    [Fact]
    public void NewInstance_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new CustomValidator(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var validator = new CustomValidator<string>(null!);
        });
    }

    [Fact]
    public void NewInstance_ReturnOK()
    {
        var validator = new CustomValidator(o =>
        {
            return true;
        });
        Assert.NotNull(validator);
        Assert.NotNull(validator.Predicate);

        var validator2 = new CustomValidator<string>(o =>
        {
            return true;
        });

        Assert.NotNull(validator);
        Assert.NotNull(validator2.Predicate);
    }

    [Fact]
    public void IsValid()
    {
        var validator = new CustomValidator<string>(str =>
        {
            return str != null && str == "Furion";
        });

        Assert.False(validator.IsValid(null));
        Assert.True(validator.IsValid("Furion"));
        Assert.False(validator.IsValid("百小僧"));

        var validator2 = new CustomValidator(str =>
        {
            return str != null && str.ToString() == "Furion";
        });

        Assert.False(validator2.IsValid(null));
        Assert.True(validator2.IsValid("Furion"));
        Assert.False(validator2.IsValid("百小僧"));
    }

    [Fact]
    public void IsValid_Predicate_Null_Throw()
    {
        var validator = new CustomValidator<string>(str =>
        {
            return str != null && str == "Furion";
        })
        {
            Predicate = null!
        };

        Assert.Throws<ArgumentNullException>(() =>
        {
            Assert.True(validator.IsValid(null));
            Assert.True(validator.IsValid("Furion"));
            Assert.True(validator.IsValid("百小僧"));
        });
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new CustomValidator<string>(str =>
        {
            return str != null && str == "Furion";
        });

        var failure = validator.GetValidationResult("百小僧", "Value");
        Assert.NotNull(failure);
        Assert.Equal("The field Value is invalid.", failure.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new CustomValidator<string>(str =>
        {
            return str != null && str == "Furion";
        })
        {
            ErrorMessage = "不是一个有效的格式"
        };

        var failure = validator.GetValidationResult("百小僧", null!);
        Assert.NotNull(failure);

        Assert.Equal("不是一个有效的格式", failure.ErrorMessage);
    }
}