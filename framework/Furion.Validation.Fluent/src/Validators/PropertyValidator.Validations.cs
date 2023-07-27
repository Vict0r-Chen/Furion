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

namespace Furion.Validation;

/// <summary>
/// 属性验证器
/// </summary>
public sealed partial class PropertyValidator<T, TProperty> : IObjectValidator<T>
    where T : class
{
    /// <summary>
    /// 添加必填验证器
    /// </summary>
    /// <param name="allowEmptyStrings">是否允许空字符串</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> NotNull(bool allowEmptyStrings = false)
    {
        Validators.Add(new ValueAnnotationValidator(new RequiredAttribute
        {
            AllowEmptyStrings = allowEmptyStrings
        }));

        return this;
    }

    /// <summary>
    /// 添加非空验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> NotEmpty()
    {
        Validators.Add(new NotEmptyValidator());

        return this;
    }

    /// <summary>
    /// 添加以特定字符串结尾的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> EndsWith(string value)
    {
        Validators.Add(new EndsWithValidator(value));

        return this;
    }

    /// <summary>
    /// 添加以特定字符串结尾的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> EndsWith(char value)
    {
        Validators.Add(new EndsWithValidator(value));

        return this;
    }

    /// <summary>
    /// 添加以特定字符串结尾的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> EndsWith(string value, StringComparison comparison)
    {
        Validators.Add(new EndsWithValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加以特定字符串结尾的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> EndsWith(char value, StringComparison comparison)
    {
        Validators.Add(new EndsWithValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加以特定字符串开头的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StartsWith(string value)
    {
        Validators.Add(new StartsWithValidator(value));

        return this;
    }

    /// <summary>
    /// 添加以特定字符串开头的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StartsWith(char value)
    {
        Validators.Add(new StartsWithValidator(value));

        return this;
    }

    /// <summary>
    /// 添加以特定字符串开头的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StartsWith(string value, StringComparison comparison)
    {
        Validators.Add(new StartsWithValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加以特定字符串开头的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StartsWith(char value, StringComparison comparison)
    {
        Validators.Add(new StartsWithValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加包含特定字符串的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StringContains(string value)
    {
        Validators.Add(new StringContainsValidator(value));

        return this;
    }

    /// <summary>
    /// 添加包含特定字符串的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StringContains(char value)
    {
        Validators.Add(new StringContainsValidator(value));

        return this;
    }

    /// <summary>
    /// 添加包含特定字符串的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StringContains(string value, StringComparison comparison)
    {
        Validators.Add(new StringContainsValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加包含特定字符串的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StringContains(char value, StringComparison comparison)
    {
        Validators.Add(new StringContainsValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加最大长度验证器
    /// </summary>
    /// <param name="length">长度</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> MaxLength(int length)
    {
        Validators.Add(new ValueAnnotationValidator(new MaxLengthAttribute(length)));

        return this;
    }

    /// <summary>
    /// 添加最小长度验证器
    /// </summary>
    /// <param name="length">长度</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> MinLength(int length)
    {
        Validators.Add(new ValueAnnotationValidator(new MinLengthAttribute(length)));

        return this;
    }

    /// <summary>
    /// 添加长度验证器
    /// </summary>
    /// <param name="minimumLength">最小长度</param>
    /// <param name="maximumLength">最大长度</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Length(int minimumLength, int maximumLength)
    {
        Validators.Add(new ValueAnnotationValidator(new LengthAttribute(minimumLength, maximumLength)));

        return this;
    }

    /// <summary>
    /// 添加字符串长度验证器
    /// </summary>
    /// <param name="maximumLength">最大长度</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StringLength(int maximumLength)
    {
        Validators.Add(new ValueAnnotationValidator(new StringLengthAttribute(maximumLength)));

        return this;
    }

    /// <summary>
    /// 添加字符串长度验证器
    /// </summary>
    /// <param name="minimumLength">最小长度</param>
    /// <param name="maximumLength">最大长度</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StringLength(int minimumLength, int maximumLength)
    {
        Validators.Add(new ValueAnnotationValidator(new StringLengthAttribute(maximumLength)
        {
            MinimumLength = minimumLength
        }));

        return this;
    }

    /// <summary>
    /// 添加范围验证器
    /// </summary>
    /// <param name="minimum">最小值</param>
    /// <param name="maximum">最大值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Range(int minimum, int maximum)
    {
        Validators.Add(new ValueAnnotationValidator(new RangeAttribute(minimum, maximum)));

        return this;
    }

    /// <summary>
    /// 添加范围验证器
    /// </summary>
    /// <param name="minimum">最小值</param>
    /// <param name="maximum">最大值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Range(double minimum, double maximum)
    {
        Validators.Add(new ValueAnnotationValidator(new RangeAttribute(minimum, maximum)));

        return this;
    }

    /// <summary>
    /// 添加正则表达式验证器
    /// </summary>
    /// <param name="pattern">正则表达式</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> RegularExpression(string pattern)
    {
        Validators.Add(new ValueAnnotationValidator(new RegularExpressionAttribute(pattern)));

        return this;
    }

    /// <summary>
    /// 添加正则表达式验证器
    /// </summary>
    /// <param name="pattern">正则表达式</param>
    /// <param name="matchTimeoutInMilliseconds">匹配超时毫秒数</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> RegularExpression(string pattern, int matchTimeoutInMilliseconds)
    {
        Validators.Add(new ValueAnnotationValidator(new RegularExpressionAttribute(pattern)
        {
            MatchTimeoutInMilliseconds = matchTimeoutInMilliseconds
        }));

        return this;
    }

    /// <summary>
    /// 添加邮箱地址验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> EmailAddress()
    {
        Validators.Add(new ValueAnnotationValidator(new EmailAddressAttribute()));

        return this;
    }

    /// <summary>
    /// 添加用户名验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> UserName()
    {
        Validators.Add(new UserNameValidator());

        return this;
    }

    /// <summary>
    /// 添加密码验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Password()
    {
        Validators.Add(new PasswordValidator());

        return this;
    }

    /// <summary>
    /// 添加强密码验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StrongPassword()
    {
        Validators.Add(new StrongPasswordValidator());

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThanOrEqualTo(int value)
    {
        Validators.Add(new GreaterThanOrEqualToValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThanOrEqualTo(double value)
    {
        Validators.Add(new GreaterThanOrEqualToValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThanOrEqualTo(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new GreaterThanOrEqualToValidator(() => predicate(instance)).IsValid(GetPropertyValue(instance))
            , Strings.GreaterThanOrEqualToValidator_Invalid
            , instance => new[] { GetPropertyValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThan(int value)
    {
        Validators.Add(new GreaterThanValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThan(double value)
    {
        Validators.Add(new GreaterThanValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThan(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new GreaterThanValidator(() => predicate(instance)).IsValid(GetPropertyValue(instance))
            , Strings.GreaterThanValidator_Invalid
            , instance => new[] { GetPropertyValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThanOrEqualTo(int value)
    {
        Validators.Add(new LessThanOrEqualToValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThanOrEqualTo(double value)
    {
        Validators.Add(new LessThanOrEqualToValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThanOrEqualTo(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new LessThanOrEqualToValidator(() => predicate(instance)).IsValid(GetPropertyValue(instance))
            , Strings.LessThanOrEqualToValidator_Invalid
            , instance => new[] { GetPropertyValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThan(int value)
    {
        Validators.Add(new LessThanValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThan(double value)
    {
        Validators.Add(new LessThanValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThan(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new LessThanValidator(() => predicate(instance)).IsValid(GetPropertyValue(instance))
            , Strings.LessThanValidator_Invalid
            , instance => new[] { GetPropertyValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加不相等验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> NotEqual(object? value)
    {
        Validators.Add(new NotEqualValidator(value));

        return this;
    }

    /// <summary>
    /// 添加不相等验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> NotEqual(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new NotEqualValidator(() => predicate(instance)).IsValid(GetPropertyValue(instance))
            , Strings.NotEqualValidator_Invalid
            , instance => new[] { GetPropertyValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加相等验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Equal(object? value)
    {
        Validators.Add(new EqualValidator(value));

        return this;
    }

    /// <summary>
    /// 添加相等验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Equal(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new EqualValidator(() => predicate(instance)).IsValid(GetPropertyValue(instance))
            , Strings.EqualValidator_Invalid
            , instance => new[] { GetPropertyValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加委托对象验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Predicate(Func<T, bool> predicate)
    {
        Validators.Add(new PredicateValidator<T>(predicate));

        return this;
    }

    /// <summary>
    /// 添加组合验证器
    /// </summary>
    /// <param name="validators">验证器集合</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Composite(params ValidatorBase[] validators)
    {
        Validators.Add(new CompositeValidator(validators));

        return this;
    }

    /// <summary>
    /// 添加组合验证器
    /// </summary>
    /// <param name="validators">验证器集合</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Composite(IList<ValidatorBase> validators)
    {
        Validators.Add(new CompositeValidator(validators));

        return this;
    }

    /// <summary>
    /// 添加注解（特性）验证器
    /// </summary>
    /// <param name="validationAttributes">验证特性集合</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> AddAttributes(params ValidationAttribute[] validationAttributes)
    {
        Validators.Add(new ValueAnnotationValidator(validationAttributes));

        return this;
    }

    /// <summary>
    /// 添加注解（特性）验证器
    /// </summary>
    /// <param name="validationAttributes">验证特性集合</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> AddAttributes(IList<ValidationAttribute> validationAttributes)
    {
        Validators.Add(new ValueAnnotationValidator(validationAttributes));

        return this;
    }

    /// <summary>
    /// 添加验证器
    /// </summary>
    /// <param name="validator"><see cref="ValidatorBase"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> AddValidator(ValidatorBase validator)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validator, nameof(validator));

        Validators.Add(validator);

        return this;
    }

    /// <summary>
    /// 添加验证器
    /// </summary>
    /// <param name="validator"><see cref="ValidatorBase"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> AddValidators(params ValidatorBase[] validator)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validator, nameof(validator));

        Validators.AddRange(validator);

        return this;
    }

    /// <summary>
    /// 添加验证器
    /// </summary>
    /// <param name="validator"><see cref="ValidatorBase"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> AddValidators(IEnumerable<ValidatorBase> validator)
    {
        return AddValidators(validator?.ToArray()!);
    }

    /// <summary>
    /// 添加自定义验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <param name="defaultErrorMessage">默认错误消息</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Custom(Func<T, bool> predicate, string? defaultErrorMessage = null)
    {
        Validators.Add(new CustomValidator(predicate, defaultErrorMessage));

        return this;
    }

    /// <summary>
    /// 添加对象注解（特性）验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> ObjectAnnotation()
    {
        Validators.Add(new ObjectAnnotationValidator());

        return this;
    }

    /// <summary>
    /// 设置类型验证器
    /// </summary>
    /// <param name="objectValidator"><see cref="IObjectValidator{T}"/></param>
    /// <returns></returns>
    public PropertyValidator<T, TProperty> SetValidator(IObjectValidator<TProperty> objectValidator)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(objectValidator, nameof(objectValidator));

        Validator = objectValidator;

        return this;
    }
}