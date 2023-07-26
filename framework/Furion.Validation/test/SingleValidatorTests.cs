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

public class SingleValidatorTests
{
    [Fact]
    public void New_Default()
    {
        var validator = new SingleValidator();

        Assert.NotNull(validator);
        Assert.Null(validator.ErrorMessage);
        Assert.NotNull(validator._errorMessageResourceAccessor);
        Assert.Equal("The field {0} only allows a single item.", validator._errorMessageResourceAccessor());
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData('a', true)]
    [InlineData("赢", true)]
    [InlineData("", false)]
    public void IsValid_ReturnOK(object? value, bool result)
    {
        var validator = new SingleValidator();

        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void IsValid_Collection_ReturnOK()
    {
        var validator = new SingleValidator();

        // Empty ===
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

        // Single ===

        var singleArray = new[] { "furion" };
        Assert.True(validator.IsValid(singleArray));

        var singleList = new List<string> { "Furion" };
        Assert.True(validator.IsValid(singleList));

        var singleDictionary = new Dictionary<string, string>() { { "Furion", "百小僧" } };
        Assert.True(validator.IsValid(singleDictionary));

        IEnumerable<string> singleEnumerable = new List<string> { "Furion" };
        Assert.True(validator.IsValid(singleEnumerable));

        var singleArrayList = new ArrayList
        {
            "furion"
        };

        Assert.True(validator.IsValid(singleArrayList));

        var singleHashSet = new HashSet<string> { "Furion" };
        Assert.True(validator.IsValid(singleHashSet));

        // Not Single ===

        var notSingleArray = new[] { "furion", "百小僧" };
        Assert.False(validator.IsValid(notSingleArray));

        var notSingleList = new List<string> { "Furion", "百小僧" };
        Assert.False(validator.IsValid(notSingleList));

        var notSingleDictionary = new Dictionary<string, string>() { { "Furion", "百小僧" }, { "Fur", "MonkSoul" } };
        Assert.False(validator.IsValid(notSingleDictionary));

        IEnumerable<string> notSingleEnumerable = new List<string> { "Furion", "百小僧" };
        Assert.False(validator.IsValid(notSingleEnumerable));

        var notSingleArrayList = new ArrayList
        {
            "furion", "百小僧"
        };

        Assert.False(validator.IsValid(notSingleArrayList));

        var notSingleHashSet = new HashSet<string> { "Furion", "百小僧" };
        Assert.False(validator.IsValid(notSingleHashSet));
    }

    [Fact]
    public void GetValidationResults_ReturnOK()
    {
        var validator = new SingleValidator();

        var validationResultsOfSucceed = validator.GetValidationResults("赢", "data");
        Assert.Null(validationResultsOfSucceed);

        var validationResultsOfFailure = validator.GetValidationResults(string.Empty, "data");
        Assert.NotNull(validationResultsOfFailure);
        Assert.Single(validationResultsOfFailure);
        Assert.Equal("data", validationResultsOfFailure.First().MemberNames.First());
        Assert.Equal("The field data only allows a single item.", validationResultsOfFailure.First().ErrorMessage);
    }

    [Fact]
    public void GetValidationResults_WithErrorMessage_ReturnOK()
    {
        var validator = new SingleValidator
        {
            ErrorMessage = "数据 {0} 验证失败"
        };

        var validationResultsOfFailure = validator.GetValidationResults(string.Empty, "data");
        Assert.NotNull(validationResultsOfFailure);
        Assert.Equal("数据 data 验证失败", validationResultsOfFailure.First().ErrorMessage);
    }

    [Fact]
    public void FormatErrorMessage_ReturnOK()
    {
        var validator = new SingleValidator();

        Assert.Equal("The field data only allows a single item.", validator.FormatErrorMessage("data"));
    }

    [Fact]
    public void Validate_ReturnOK()
    {
        var validator = new SingleValidator();

        validator.Validate("赢", "data");
    }

    [Fact]
    public void Validate_Failure()
    {
        var validator = new SingleValidator();

        var exception = Assert.Throws<AggregateValidationException>(() =>
        {
            validator.Validate(string.Empty, "data");
        });

        Assert.Single(exception.InnerExceptions);
        Assert.Equal("The field data only allows a single item.", exception.InnerExceptions.First().Message);
    }

    [Theory]
    [InlineData(typeof(SingleValidator), true)]
    [InlineData(typeof(ValidatorBase), false)]
    public void IsSameAs_ReturnOK(Type type, bool result)
    {
        var validator = new SingleValidator();

        Assert.Equal(result, validator.IsSameAs(type));
    }
}