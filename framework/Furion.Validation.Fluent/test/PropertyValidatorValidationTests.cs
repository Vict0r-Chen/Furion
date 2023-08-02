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

namespace Furion.Validation.Fluent.Tests;

#pragma warning disable

public class PropertyValidatorValidationTests
{
    [Fact]
    public void Age_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Age();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as AgeValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void BankCardNumber_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.BankCardNumber();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as BankCardNumberValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void ChineseName_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.ChineseName();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ChineseNameValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void Chinese_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Chinese();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ChineseValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void ColorValue_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.ColorValue();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ColorValueValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void ColorValue_WithFullModel_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.ColorValue(false);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ColorValueValidator;
        Assert.NotNull(validator);
        Assert.False(validator.FullMode);
    }

    [Fact]
    public void Composite_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Composite(new AgeValidator());

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as CompositeValidator;
        Assert.NotNull(validator);
        Assert.Single(validator.Validators);
    }

    [Fact]
    public void Composite_ListParameters_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Composite(new List<ValidatorBase> { new AgeValidator() });

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as CompositeValidator;
        Assert.NotNull(validator);
        Assert.Single(validator.Validators);
    }

    [Fact]
    public void Composite_WithValidatorCascadeMode_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Composite(new[] { new AgeValidator() }, ValidatorCascadeMode.UsingFirstSuccess);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as CompositeValidator;
        Assert.NotNull(validator);
        Assert.Single(validator.Validators);
        Assert.Equal(ValidatorCascadeMode.UsingFirstSuccess, validator.CascadeMode);
    }

    [Fact]
    public void Composite_ListParameters_WithValidatorCascadeMode_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Composite(new List<ValidatorBase> { new AgeValidator() }, ValidatorCascadeMode.UsingFirstSuccess);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as CompositeValidator;
        Assert.NotNull(validator);
        Assert.Single(validator.Validators);
        Assert.Equal(ValidatorCascadeMode.UsingFirstSuccess, validator.CascadeMode);
    }

    [Fact]
    public void Domain_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Domain();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as DomainValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void EndsWith_CharParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.EndsWith('f');

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as EndsWithValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
    }

    [Fact]
    public void EndsWith_StringParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.EndsWith("f");

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as EndsWithValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
    }

    [Fact]
    public void EndsWith_CharParameter_WithStringComparison_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.EndsWith('f', StringComparison.OrdinalIgnoreCase);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as EndsWithValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
        Assert.Equal(StringComparison.OrdinalIgnoreCase, validator.Comparison);
    }

    [Fact]
    public void EndsWith_StringParameter_WithStringComparison_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.EndsWith("f", StringComparison.OrdinalIgnoreCase);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as EndsWithValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
        Assert.Equal(StringComparison.OrdinalIgnoreCase, validator.Comparison);
    }

    [Fact]
    public void Equal_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Equal("furion");

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as EqualValidator;
        Assert.NotNull(validator);
        Assert.Equal("furion", validator.CompareValue);
    }

    [Fact]
    public void Equal_FuncParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Equal(u => "furion");

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PropertyValidator<PropertyModel, string?>.ValidatorDelegator<EqualValidator>;
        Assert.NotNull(validator);
        Assert.Equal(Strings.EqualValidator_Invalid, validator._errorMessageResourceAccessor());
    }

    [Fact]
    public void GreaterThanOrEqualTo_IntParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.GreaterThanOrEqualTo(1);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as GreaterThanOrEqualToValidator;
        Assert.NotNull(validator);
        Assert.Equal(1, validator.CompareValue);
    }

    [Fact]
    public void GreaterThanOrEqualTo_DoubleParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.GreaterThanOrEqualTo(1.3);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as GreaterThanOrEqualToValidator;
        Assert.NotNull(validator);
        Assert.Equal(1.3, validator.CompareValue);
    }

    [Fact]
    public void GreaterThanOrEqualTo_ObjectParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.GreaterThanOrEqualTo(1.03m);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as GreaterThanOrEqualToValidator;
        Assert.NotNull(validator);
        Assert.Equal(1.03m, validator.CompareValue);
    }

    [Fact]
    public void GreaterThanOrEqualTo_FuncParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.GreaterThanOrEqualTo(u => 1);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PropertyValidator<PropertyModel, int>.ValidatorDelegator<GreaterThanOrEqualToValidator>;
        Assert.NotNull(validator);
        Assert.Equal(Strings.GreaterThanOrEqualToValidator_Invalid, validator._errorMessageResourceAccessor());
    }

    [Fact]
    public void GreaterThan_IntParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.GreaterThan(1);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as GreaterThanValidator;
        Assert.NotNull(validator);
        Assert.Equal(1, validator.CompareValue);
    }

    [Fact]
    public void GreaterThan_DoubleParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.GreaterThan(1.3);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as GreaterThanValidator;
        Assert.NotNull(validator);
        Assert.Equal(1.3, validator.CompareValue);
    }

    [Fact]
    public void GreaterThan_ObjectParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.GreaterThan(1.03m);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as GreaterThanValidator;
        Assert.NotNull(validator);
        Assert.Equal(1.03m, validator.CompareValue);
    }

    [Fact]
    public void GreaterThan_FuncParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.GreaterThan(u => 1);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PropertyValidator<PropertyModel, int>.ValidatorDelegator<GreaterThanValidator>;
        Assert.NotNull(validator);
        Assert.Equal(Strings.GreaterThanValidator_Invalid, validator._errorMessageResourceAccessor());
    }

    [Fact]
    public void IdCardNumber_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.IdCardNumber();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as IdCardNumberValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void LessThanOrEqualTo_IntParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.LessThanOrEqualTo(1);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as LessThanOrEqualToValidator;
        Assert.NotNull(validator);
        Assert.Equal(1, validator.CompareValue);
    }

    [Fact]
    public void LessThanOrEqualTo_DoubleParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.LessThanOrEqualTo(1.3);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as LessThanOrEqualToValidator;
        Assert.NotNull(validator);
        Assert.Equal(1.3, validator.CompareValue);
    }

    [Fact]
    public void LessThanOrEqualTo_ObjectParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.LessThanOrEqualTo(1.03m);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as LessThanOrEqualToValidator;
        Assert.NotNull(validator);
        Assert.Equal(1.03m, validator.CompareValue);
    }

    [Fact]
    public void LessThanOrEqualTo_FuncParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.LessThanOrEqualTo(u => 1);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PropertyValidator<PropertyModel, int>.ValidatorDelegator<LessThanOrEqualToValidator>;
        Assert.NotNull(validator);
        Assert.Equal(Strings.LessThanOrEqualToValidator_Invalid, validator._errorMessageResourceAccessor());
    }

    [Fact]
    public void LessThan_IntParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.LessThan(1);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as LessThanValidator;
        Assert.NotNull(validator);
        Assert.Equal(1, validator.CompareValue);
    }

    [Fact]
    public void LessThan_DoubleParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.LessThan(1.3);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as LessThanValidator;
        Assert.NotNull(validator);
        Assert.Equal(1.3, validator.CompareValue);
    }

    [Fact]
    public void LessThan_ObjectParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.LessThan(1.03m);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as LessThanValidator;
        Assert.NotNull(validator);
        Assert.Equal(1.03m, validator.CompareValue);
    }

    [Fact]
    public void LessThan_FuncParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.LessThan(u => 1);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PropertyValidator<PropertyModel, int>.ValidatorDelegator<LessThanValidator>;
        Assert.NotNull(validator);
        Assert.Equal(Strings.LessThanValidator_Invalid, validator._errorMessageResourceAccessor());
    }

    [Fact]
    public void NotEmpty_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.NotEmpty();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as NotEmptyValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void NotEqual_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.NotEqual("furion");

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as NotEqualValidator;
        Assert.NotNull(validator);
        Assert.Equal("furion", validator.CompareValue);
    }

    [Fact]
    public void NotEqual_FuncParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.NotEqual(u => "furion");

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PropertyValidator<PropertyModel, string?>.ValidatorDelegator<NotEqualValidator>;
        Assert.NotNull(validator);
        Assert.Equal(Strings.NotEqualValidator_Invalid, validator._errorMessageResourceAccessor());
    }

    [Fact]
    public void Password_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Password();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PasswordValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void PhoneNumber_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.PhoneNumber();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PhoneNumberValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void PostalCode_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.PostalCode();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PostalCodeValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void Predicate_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.Predicate(u => u.Id > 1);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PropertyValidator<PropertyModel, int>.ValidatorDelegator<PredicateValidator<PropertyModel>>;
        Assert.NotNull(validator);
        Assert.Equal(Strings.ValidatorBase_Invalid, validator._errorMessageResourceAccessor());
    }

    [Fact]
    public void Single_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Single();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as SingleValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void StartsWith_CharParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.StartsWith('f');

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as StartsWithValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
    }

    [Fact]
    public void StartsWith_StringParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.StartsWith("f");

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as StartsWithValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
    }

    [Fact]
    public void StartsWith_CharParameter_WithStringComparison_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.StartsWith('f', StringComparison.OrdinalIgnoreCase);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as StartsWithValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
        Assert.Equal(StringComparison.OrdinalIgnoreCase, validator.Comparison);
    }

    [Fact]
    public void StartsWith_StringParameter_WithStringComparison_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.StartsWith("f", StringComparison.OrdinalIgnoreCase);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as StartsWithValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
        Assert.Equal(StringComparison.OrdinalIgnoreCase, validator.Comparison);
    }

    [Fact]
    public void StringContains_CharParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.StringContains('f');

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as StringContainsValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
    }

    [Fact]
    public void StringContains_StringParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.StringContains("f");

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as StringContainsValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
    }

    [Fact]
    public void StringContains_CharParameter_WithStringComparison_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.StringContains('f', StringComparison.OrdinalIgnoreCase);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as StringContainsValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
        Assert.Equal(StringComparison.OrdinalIgnoreCase, validator.Comparison);
    }

    [Fact]
    public void StringContains_StringParameter_WithStringComparison_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.StringContains("f", StringComparison.OrdinalIgnoreCase);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as StringContainsValidator;
        Assert.NotNull(validator);
        Assert.Equal("f", validator.SearchValue);
        Assert.Equal(StringComparison.OrdinalIgnoreCase, validator.Comparison);
    }

    [Fact]
    public void StrongPassword_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.StrongPassword();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as StrongPasswordValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void Telephone_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Telephone();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as TelephoneValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void UserName_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.UserName();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as UserNameValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void NotNull_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.NotNull();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as RequiredAttribute;
        Assert.NotNull(attribute);
    }

    [Fact]
    public void NotNull_WithAllowEmptyStrings_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.NotNull(true);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as RequiredAttribute;
        Assert.NotNull(attribute);
        Assert.True(attribute.AllowEmptyStrings);
    }

    [Fact]
    public void MaxLength_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.MaxLength(10);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as MaxLengthAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(10, attribute.Length);
    }

    [Fact]
    public void MinLength_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.MinLength(10);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as MinLengthAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(10, attribute.Length);
    }

    [Fact]
    public void Length_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Length(10, 100);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as LengthAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(10, attribute.MinimumLength);
        Assert.Equal(100, attribute.MaximumLength);
    }

    [Fact]
    public void StringLength_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.StringLength(10);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as StringLengthAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(10, attribute.MaximumLength);
    }

    [Fact]
    public void StringLength_WithMaxAndMin_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.StringLength(10, 100);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as StringLengthAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(10, attribute.MinimumLength);
        Assert.Equal(100, attribute.MaximumLength);
    }

    [Fact]
    public void Range_IntParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Range(10, 100);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as RangeAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(10, attribute.Minimum);
        Assert.Equal(100, attribute.Maximum);
    }

    [Fact]
    public void Range_DoubleParameter_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.Range(1.3, 10.1);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as RangeAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(1.3, attribute.Minimum);
        Assert.Equal(10.1, attribute.Maximum);
    }

    [Fact]
    public void RegularExpression_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.RegularExpression(".+");

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as RegularExpressionAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(".+", attribute.Pattern);
    }

    [Fact]
    public void RegularExpression_WithMatchTimeoutInMilliseconds_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.RegularExpression(".+", 100);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as RegularExpressionAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(".+", attribute.Pattern);
        Assert.Equal(100, attribute.MatchTimeoutInMilliseconds);
    }

    [Fact]
    public void EmailAddress_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);
        propertyValidator.EmailAddress();

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as EmailAddressAttribute;
        Assert.NotNull(attribute);
    }

    [Fact]
    public void Add_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.Add(null!);
        });
    }

    [Fact]
    public void Add_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        propertyValidator.Add(new AgeValidator());

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as AgeValidator;
        Assert.NotNull(validator);
    }

    [Fact]
    public void AddAttribute_Invalid_Parameters()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        Assert.Throws<ArgumentNullException>(() =>
        {
            propertyValidator.AddAttribute(null!);
        });
    }

    [Fact]
    public void AddAttribute_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        propertyValidator.AddAttribute(new AgeAttribute());

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as ValueAnnotationValidator;
        Assert.NotNull(validator);

        var attribute = validator.Attributes.First() as AgeAttribute;
        Assert.NotNull(attribute);
    }

    [Fact]
    public void Custom_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, int>(objectValidator, u => u.Id);
        propertyValidator.Custom(u => u.Id > 1);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PropertyValidator<PropertyModel, int>.ValidatorDelegator<PredicateValidator<PropertyModel>>;
        Assert.NotNull(validator);
        Assert.Equal(Strings.ValidatorBase_Invalid, validator._errorMessageResourceAccessor());
    }

    [Fact]
    public void AddDelegator_ReturnOK()
    {
        var objectValidator = new ObjectValidator<PropertyModel>();
        var propertyValidator = new PropertyValidator<PropertyModel, string?>(objectValidator, u => u.Name);

        propertyValidator.AddDelegator<EqualValidator>(instance => new[] { "furion" }
            , () => Strings.EqualValidator_Invalid);

        Assert.Single(propertyValidator.Validators);

        var validator = propertyValidator.Validators.ElementAt(0) as PropertyValidator<PropertyModel, string?>.ValidatorDelegator<EqualValidator>;
        Assert.NotNull(validator);
        Assert.Equal(Strings.EqualValidator_Invalid, validator._errorMessageResourceAccessor());
    }
}