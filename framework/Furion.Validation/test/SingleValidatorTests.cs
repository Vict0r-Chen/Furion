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

public class SingleValidatorTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("furion", false)]
    [InlineData("f", true)]
    [InlineData('f', true)]
    public void IsValid(object? value, bool result)
    {
        var validator = new SingleValidator();
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void IsValid_Empty_Collection()
    {
        var validator = new SingleValidator();

        var emptyArray = Array.Empty<string>();
        Assert.False(validator.IsValid(emptyArray));

        var emptyList = new List<string>();
        Assert.False(validator.IsValid(emptyList));

        Assert.False(validator.IsValid(string.Empty));

        var emptyDictionary = new Dictionary<string, string>();
        Assert.False(validator.IsValid(emptyDictionary));

        IEnumerable<string> emptyEnumerable = new List<string>();
        Assert.False(validator.IsValid(emptyEnumerable));

        var emptyArrayList = new ArrayList();
        Assert.False(validator.IsValid(emptyArrayList));

        var emptyHashSet = new HashSet<string>();
        Assert.False(validator.IsValid(emptyHashSet));
    }

    [Fact]
    public void IsValid_Single_Collection()
    {
        var validator = new SingleValidator();

        var notEmptyArray = new[] { "furion" };
        Assert.True(validator.IsValid(notEmptyArray));

        var notEmptyList = new List<string> { "Furion" };
        Assert.True(validator.IsValid(notEmptyList));

        var notEmptyDictionary = new Dictionary<string, string>() { { "Furion", "百小僧" } };
        Assert.True(validator.IsValid(notEmptyDictionary));

        IEnumerable<string> notEmptyEnumerable = new List<string> { "Furion" };
        Assert.True(validator.IsValid(notEmptyEnumerable));

        var notEmptyArrayList = new ArrayList
        {
            "furion"
        };

        Assert.True(validator.IsValid(notEmptyArrayList));

        var notEmptyHashSet = new HashSet<string> { "Furion" };
        Assert.True(validator.IsValid(notEmptyHashSet));
    }

    [Fact]
    public void IsValid_Multiple_Collection()
    {
        var validator = new SingleValidator();

        var notEmptyArray = new[] { "furion", "fur" };
        Assert.False(validator.IsValid(notEmptyArray));

        var notEmptyList = new List<string> { "Furion", "fur" };
        Assert.False(validator.IsValid(notEmptyList));

        var notEmptyDictionary = new Dictionary<string, string>() { { "Furion", "百小僧" }, { "fur", "百小僧" } };
        Assert.False(validator.IsValid(notEmptyDictionary));

        IEnumerable<string> notEmptyEnumerable = new List<string> { "Furion", "fur" };
        Assert.False(validator.IsValid(notEmptyEnumerable));

        var notEmptyArrayList = new ArrayList
        {
            "furion", "fur"
        };

        Assert.False(validator.IsValid(notEmptyArrayList));

        var notEmptyHashSet = new HashSet<string> { "Furion", "fur" };
        Assert.False(validator.IsValid(notEmptyHashSet));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new SingleValidator();

        var failure = validator.GetValidationResult("furion", "Value");
        Assert.NotNull(failure);
        Assert.Equal("The field Value only allows for a single value.", failure.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new SingleValidator
        {
            ErrorMessage = "只允许单个值"
        };

        var failure = validator.GetValidationResult("furion", null!);
        Assert.NotNull(failure);

        Assert.Equal("只允许单个值", failure.ErrorMessage);
    }
}