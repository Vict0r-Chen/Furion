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

public class EmailValidatorTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("support@furion.net", true)]
    [InlineData("8020222@qq.net", true)]
    [InlineData("monksoul@outlook.com", true)]
    [InlineData("monksoul@163.com", true)]
    [InlineData("dotnetchina@126.com", true)]
    [InlineData("dotnetchina@hotmail.com", true)]
    [InlineData("汉字@hotmail.com", true)]
    [InlineData("qq@hotmail", false)]
    [InlineData("support@@furion.net", false)]
    [InlineData("support@中文.com", false)]
    [InlineData("support", false)]
    public void IsValid(object? value, bool result)
    {
        var validator = new EmailValidator();
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new EmailValidator();

        var failure = validator.GetValidationResult("support@@furion.net");
        Assert.NotNull(failure);
        Assert.Equal("The field is not a valid email format.", failure.ErrorMessage);

        var failure2 = validator.GetValidationResult("support@@furion.net", new List<string> { "Value" });
        Assert.NotNull(failure2);
        Assert.Equal("The field Value is not a valid email format.", failure2.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new EmailValidator
        {
            ErrorMessage = "不是一个有效的电子邮箱格式"
        };

        var failure = validator.GetValidationResult("support@@furion.net");
        Assert.NotNull(failure);

        Assert.Equal("不是一个有效的电子邮箱格式", failure.ErrorMessage);
    }
}