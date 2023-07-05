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

public class ChineseProvinceValidatorTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("浙江", true)]
    [InlineData("上海", true)]
    [InlineData("北京", true)]
    [InlineData("天津", true)]
    [InlineData("重庆", true)]
    [InlineData("黑龙江", true)]
    [InlineData("吉林", true)]
    [InlineData("辽宁", true)]
    [InlineData("内蒙古", true)]
    [InlineData("河北", true)]
    [InlineData("新疆", true)]
    [InlineData("甘肃", true)]
    [InlineData("青海", true)]
    [InlineData("陕西", true)]
    [InlineData("宁夏", true)]
    [InlineData("河南", true)]
    [InlineData("山东", true)]
    [InlineData("山西", true)]
    [InlineData("安徽", true)]
    [InlineData("湖北", true)]
    [InlineData("湖南", true)]
    [InlineData("江苏", true)]
    [InlineData("四川", true)]
    [InlineData("贵州", true)]
    [InlineData("云南", true)]
    [InlineData("广西", true)]
    [InlineData("西藏", true)]
    [InlineData("江西", true)]
    [InlineData("广东", true)]
    [InlineData("福建", true)]
    [InlineData("台湾", true)]
    [InlineData("海南", true)]
    [InlineData("香港", true)]
    [InlineData("澳门", true)]
    [InlineData("湛江", false)]
    [InlineData("武汉", false)]
    [InlineData("南宁", false)]
    public void IsValid(object? value, bool result)
    {
        var validator = new ChineseProvinceValidator();
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new ChineseProvinceValidator();

        var failure = validator.GetValidationResult("武汉");
        Assert.NotNull(failure);
        Assert.Equal("The field is not a valid chinese province.", failure.ErrorMessage);

        var failure2 = validator.GetValidationResult("武汉", new List<string> { "Province" });
        Assert.NotNull(failure2);
        Assert.Equal("The field Province is not a valid chinese province.", failure2.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new ChineseProvinceValidator
        {
            ErrorMessage = "不是一个有效的中国省份"
        };

        var failure = validator.GetValidationResult("武汉");
        Assert.NotNull(failure);

        Assert.Equal("不是一个有效的中国省份", failure.ErrorMessage);
    }
}