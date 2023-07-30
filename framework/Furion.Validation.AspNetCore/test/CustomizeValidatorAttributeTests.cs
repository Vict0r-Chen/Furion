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

#pragma warning disable

namespace Furion.Validation.AspNetCore.Tests;

public class CustomizeValidatorAttributeTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var customizeValidatorAttribute = new CustomizeValidatorAttribute<FluentModelValidator4>();
        });
        Assert.Equal("`Furion.Validation.AspNetCore.Tests.FluentModelValidator4` type is not assignable from `Furion.Validation.AbstractValidator`1[T]`.", exception.Message);

        var exception2 = Assert.Throws<InvalidOperationException>(() =>
        {
            var customizeValidatorAttribute = new CustomizeValidatorAttribute<FluentModelValidator3>();
        });
        Assert.Equal($"`Furion.Validation.AspNetCore.Tests.FluentModelValidator3` type must be able to be instantiated.", exception2.Message);
    }

    [Fact]
    public void New_ReturnOK()
    {
        var customizeValidatorAttribute = new CustomizeValidatorAttribute();
        var customizeValidatorAttribute2 = new CustomizeValidatorAttribute<FluentModelValidator>();

        Assert.NotNull(customizeValidatorAttribute);
        Assert.NotNull(customizeValidatorAttribute2);

        Assert.Equal(ValidatorCascadeMode.Continue, customizeValidatorAttribute2.CascadeMode);
        Assert.True(customizeValidatorAttribute2.SuppressAnnotationValidation);
        Assert.True(customizeValidatorAttribute2.ValidateAllPropertiesForObjectAnnotationValidator);
        Assert.Null(customizeValidatorAttribute2.RuleSet);
    }

    [Fact]
    public void IsValid_Invalid_Parameters()
    {
        var customizeValidatorAttribute = new CustomizeValidatorAttribute();

        Assert.Throws<ArgumentNullException>(() =>
        {
            customizeValidatorAttribute.IsValid(new FluentModel(), null!);
        });
    }

    [Fact]
    public void IsValid_ReturnOK()
    {
        var customizeValidatorAttribute = new CustomizeValidatorAttribute();
        var model = new FluentModel();

        var validationResult = customizeValidatorAttribute.IsValid(model, _ => new FluentModelValidator());

        Assert.NotNull(validationResult);
        Assert.Equal("The field Id must be greater than '1'.", validationResult.ErrorMessage);
    }

    [Fact]
    public void IsValid_WithErrorMessage_ReturnOK()
    {
        var customizeValidatorAttribute = new CustomizeValidatorAttribute
        {
            ErrorMessage = "自定义错误消息"
        };
        var model = new FluentModel();

        var validationResult = customizeValidatorAttribute.IsValid(model, _ => new FluentModelValidator());

        Assert.NotNull(validationResult);
        Assert.Equal("自定义错误消息", validationResult.ErrorMessage);
    }

    [Fact]
    public void IsValid_WithRuleSet_ReturnOK()
    {
        var customizeValidatorAttribute = new CustomizeValidatorAttribute
        {
            RuleSet = "furion"
        };
        var model = new FluentModel();

        var validationResult = customizeValidatorAttribute.IsValid(model, _ => new FluentModelValidator());

        Assert.NotNull(validationResult);
        Assert.Equal("The Name field is required.", validationResult.ErrorMessage);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(long.MaxValue, false)]
    [InlineData(int.MaxValue, false)]
    [InlineData(double.MaxValue, false)]
    [InlineData(float.MaxValue, false)]
    [InlineData(short.MaxValue, false)]
    [InlineData(byte.MaxValue, false)]
    [InlineData(1.03, false)]
    [InlineData('f', false)]
    [InlineData("furion", false)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(new[] { "furion" }, false)]
    public void CanValidate_ReturnOK(object? value, bool result)
    {
        Assert.Equal(result, CustomizeValidatorAttribute.CanValidate(value));
    }

    [Fact]
    public void CanValidate_Class_ReturnOK()
    {
        Assert.False(CustomizeValidatorAttribute.CanValidate(new List<string>()));
        Assert.False(CustomizeValidatorAttribute.CanValidate(ValidatorCascadeMode.Continue));
        Assert.False(CustomizeValidatorAttribute.CanValidate(decimal.MaxValue));
        Assert.True(CustomizeValidatorAttribute.CanValidate(new FluentModel()));
    }

    [Fact]
    public void ConfigureOptions_Invalid_Parameters()
    {
        var customizeValidatorAttribute = new CustomizeValidatorAttribute();

        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            customizeValidatorAttribute.ConfigureOptions(null!);
        });
    }

    [Fact]
    public void ConfigureOptions_ReturnOK()
    {
        var customizeValidatorAttribute = new CustomizeValidatorAttribute()
        {
            CascadeMode = ValidatorCascadeMode.UsingFirstSuccess,
            SuppressAnnotationValidation = false,
            ValidateAllPropertiesForObjectAnnotationValidator = false
        };

        var validator = new FluentModelValidator();
        customizeValidatorAttribute.ConfigureOptions(validator);

        Assert.Equal(ValidatorCascadeMode.UsingFirstSuccess, validator.Options.CascadeMode);
        Assert.False(validator.Options.SuppressAnnotationValidation);
        Assert.False(validator.Options.ValidateAllPropertiesForObjectAnnotationValidator);
    }

    [Theory]
    [InlineData("/Customize/TestParameter", typeof(FluentModel))]
    [InlineData("/Customize/TestClass", typeof(FluentModel2))]
    public async Task Integration_ReturnOK(string url, Type type)
    {
        var port = Helpers.GetIdlePort();
        var urls = new[] { "--urls", $"http://localhost:{port}" };
        var builder = WebApplication.CreateBuilder(urls);

        builder.Services.AddFluentValidation(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });
        builder.Services.AddControllers()
            .AddApplicationPart(GetType().Assembly);

        await using var app = builder.Build();

        app.MapControllers();

        await app.StartAsync();

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(nameof(CustomizeValidatorAttributeTests));

        var httpContent = new StringContent(JsonSerializer.Serialize(Activator.CreateInstance(type))
            , new MediaTypeHeaderValue("application/json"));
        var httpResponseMessage = await httpClient.PostAsync($"http://localhost:{port}" + url, httpContent);

        Assert.NotNull(httpResponseMessage);
        Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
    }
}