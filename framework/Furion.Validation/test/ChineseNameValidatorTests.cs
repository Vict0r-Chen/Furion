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

public class ChineseNameValidatorTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("秦", false)]
    [InlineData("张三", true)]
    [InlineData("李四", true)]
    [InlineData("王五", true)]
    [InlineData("葛二蛋", true)]
    [InlineData("易烊千玺", true)]
    [InlineData("百小僧", true)]
    [InlineData("凯文·杜兰特", true)]
    [InlineData("德克·维尔纳·诺维茨基", true)]
    [InlineData("蒙奇·D·路飞", false)]
    [InlineData("罗罗诺亚·索隆", true)]
    [InlineData("陈老6", false)]
    public void IsValid(object? value, bool result)
    {
        var validator = new ChineseNameValidator();
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new ChineseNameValidator();

        var failure = validator.GetValidationResult("蒙奇·D·路飞", "Value");
        Assert.NotNull(failure);
        Assert.Equal("The field Value is not a valid Chinese name.", failure.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new ChineseNameValidator
        {
            ErrorMessage = "不是一个有效的中文姓名"
        };

        var failure = validator.GetValidationResult("蒙奇·D·路飞", null!);
        Assert.NotNull(failure);

        Assert.Equal("不是一个有效的中文姓名", failure.ErrorMessage);
    }
}