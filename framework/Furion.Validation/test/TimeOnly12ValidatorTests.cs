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

public class TimeOnly12ValidatorTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("12:00:00", true)]
    [InlineData("03:00:00", true)]
    [InlineData("3:00:00", true)]
    [InlineData("04:10:59", true)]
    [InlineData("03:02:33", true)]
    [InlineData("03:02:03", true)]
    [InlineData("00:00:00", false)]
    [InlineData("12:60:60", false)]
    [InlineData("03:60:00", false)]
    [InlineData("3:00:60", false)]
    [InlineData("23:00:00", false)]
    [InlineData("13:02:33", false)]
    [InlineData("18:02:03", false)]
    [InlineData("03:2:33", false)]
    [InlineData("03:02:3", false)]
    public void IsValid(object? value, bool result)
    {
        var validator = new TimeOnly12Validator();
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new TimeOnly12Validator();

        var failure = validator.GetValidationResult("23:00:00");
        Assert.NotNull(failure);
        Assert.Equal("The field is not a valid half day clock time format.", failure.ErrorMessage);

        var failure2 = validator.GetValidationResult("23:00:00", new List<string> { "Time" });
        Assert.NotNull(failure2);
        Assert.Equal("The field Time is not a valid half day clock time format.", failure2.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new TimeOnly12Validator
        {
            ErrorMessage = "不是一个有效的12小时制时间"
        };

        var failure = validator.GetValidationResult("23:00:00");
        Assert.NotNull(failure);

        Assert.Equal("不是一个有效的12小时制时间", failure.ErrorMessage);
    }
}