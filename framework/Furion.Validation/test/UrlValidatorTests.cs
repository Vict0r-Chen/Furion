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

public class UrlValidatorTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("http://furion.net", true)]
    [InlineData("https://furion.net", true)]
    [InlineData("ftp://furion.net", true)]
    [InlineData("www.furion.net", true)]
    [InlineData("http://furion.net:8080", true)]
    [InlineData("https://114.115.288.19:8080", true)]
    [InlineData("http://先知.com", true)]
    [InlineData("ws://localhost:8089", false)]
    [InlineData("https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=furion&fenlei=256&oq=%25E7%25AB%25AF%25E5%258F%25A3%25E5%258F%25B7%25E6%259C%2580%25E5%25A4%25A7%25E5%2580%25BC&rsv_pq=a8294f190000ba27&rsv_t=216feJgh%2BnAm%2BJD6GyY8hEZBbtAjafeIdqYu3IRyZH%2BuIOkCPabQCFWjdkE&rqlang=cn&rsv_enter=1&rsv_dl=tb&rsv_btype=t&inputT=1452&rsv_sug3=24&rsv_sug1=14&rsv_sug7=100&rsv_sug2=0&rsv_sug4=1452&rsv_sug=1", true)]
    public void IsValid(object? value, bool result)
    {
        var validator = new UrlValidator();
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new UrlValidator();

        var failure = validator.GetValidationResult("ws://localhost:8089");
        Assert.NotNull(failure);
        Assert.Equal("The field is not a valid url format.", failure.ErrorMessage);

        var failure2 = validator.GetValidationResult("ws://localhost:8089", new List<string> { "Value" });
        Assert.NotNull(failure2);
        Assert.Equal("The field Value is not a valid url format.", failure2.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new UrlValidator
        {
            ErrorMessage = "不是一个有效的 URL 格式"
        };

        var failure = validator.GetValidationResult("ws://localhost:8089");
        Assert.NotNull(failure);

        Assert.Equal("不是一个有效的 URL 格式", failure.ErrorMessage);
    }
}