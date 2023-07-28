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
    /// 添加年龄（0-120）验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Age()
    {
        Validators.Add(new AgeValidator());

        return this;
    }

    /// <summary>
    /// 添加银行卡号验证器
    /// </summary>
    /// <remarks>
    /// <see href="https://pay.weixin.qq.com/wiki/doc/api/xiaowei.php?chapter=22_1">银行卡号对照表</see>
    /// </remarks>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> BankCardNumber()
    {
        Validators.Add(new BankCardNumberValidator());

        return this;
    }

    /// <summary>
    /// 添加中文姓名验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> ChineseName()
    {
        Validators.Add(new ChineseNameValidator());

        return this;
    }

    /// <summary>
    /// 添加中文/汉字验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Chinese()
    {
        Validators.Add(new ChineseValidator());

        return this;
    }

    /// <summary>
    /// 添加颜色值验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> ColorValue()
    {
        Validators.Add(new ColorValueValidator());

        return this;
    }

    /// <summary>
    /// 添加颜色值验证器
    /// </summary>
    /// <param name="fullMode">完整模式</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> ColorValue(bool fullMode)
    {
        Validators.Add(new ColorValueValidator
        {
            FullMode = fullMode
        });

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
    /// 添加组合验证器
    /// </summary>
    /// <param name="validators">验证器集合</param>
    /// <param name="relationship"><see cref="ValidatorRelationship"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Composite(ValidatorBase[] validators, ValidatorRelationship relationship)
    {
        Validators.Add(new CompositeValidator(validators)
        {
            Relationship = relationship
        });

        return this;
    }

    /// <summary>
    /// 添加组合验证器
    /// </summary>
    /// <param name="validators">验证器集合</param>
    /// <param name="relationship"><see cref="ValidatorRelationship"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Composite(IList<ValidatorBase> validators, ValidatorRelationship relationship)
    {
        Validators.Add(new CompositeValidator(validators)
        {
            Relationship = relationship
        });

        return this;
    }

    /// <summary>
    /// 添加域名验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Domain()
    {
        Validators.Add(new DomainValidator());

        return this;
    }

    /// <summary>
    /// 添加以特定字符/字符串结尾的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> EndsWith(char searchValue)
    {
        Validators.Add(new EndsWithValidator(searchValue));

        return this;
    }

    /// <summary>
    /// 添加以特定字符/字符串结尾的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> EndsWith(string searchValue)
    {
        Validators.Add(new EndsWithValidator(searchValue));

        return this;
    }

    /// <summary>
    /// 添加以特定字符/字符串结尾的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> EndsWith(char searchValue, StringComparison comparison)
    {
        Validators.Add(new EndsWithValidator(searchValue)
        {
            Comparison = comparison
        });

        return this;
    }

    /// <summary>
    /// 添加以特定字符/字符串结尾的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> EndsWith(string searchValue, StringComparison comparison)
    {
        Validators.Add(new EndsWithValidator(searchValue)
        {
            Comparison = comparison
        });

        return this;
    }

    /// <summary>
    /// 添加相等验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Equal(object? compareValue)
    {
        Validators.Add(new EqualValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加相等验证器
    /// </summary>
    /// <param name="predicate">自定义委托</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Equal(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate);

        // 初始化验证器委托器
        var validatorDelegator = new ValidatorDelegator<EqualValidator>(this
            , instance => new[] { predicate(instance) }
            , () => Strings.EqualValidator_Invalid);

        Validators.Add(validatorDelegator);

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThanOrEqualTo(int compareValue)
    {
        Validators.Add(new GreaterThanOrEqualToValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThanOrEqualTo(double compareValue)
    {
        Validators.Add(new GreaterThanOrEqualToValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThanOrEqualTo(object compareValue)
    {
        Validators.Add(new GreaterThanOrEqualToValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="predicate">自定义委托</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThanOrEqualTo(Func<T, object> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate);

        // 初始化验证器委托器
        var validatorDelegator = new ValidatorDelegator<GreaterThanOrEqualToValidator>(this
            , instance => new[] { predicate(instance) }
            , () => Strings.GreaterThanOrEqualToValidator_Invalid);

        Validators.Add(validatorDelegator);

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThan(int compareValue)
    {
        Validators.Add(new GreaterThanValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThan(double compareValue)
    {
        Validators.Add(new GreaterThanValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThan(object compareValue)
    {
        Validators.Add(new GreaterThanValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="predicate">自定义委托</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> GreaterThan(Func<T, object> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate);

        // 初始化验证器委托器
        var validatorDelegator = new ValidatorDelegator<GreaterThanValidator>(this
            , instance => new[] { predicate(instance) }
            , () => Strings.GreaterThanValidator_Invalid);

        Validators.Add(validatorDelegator);

        return this;
    }

    /// <summary>
    /// 添加身份证号验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> IdCardNumber()
    {
        Validators.Add(new IdCardNumberValidator());

        return this;
    }

    /// <summary>
    /// 添加小于等于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThanOrEqualTo(int compareValue)
    {
        Validators.Add(new LessThanOrEqualToValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加小于等于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThanOrEqualTo(double compareValue)
    {
        Validators.Add(new LessThanOrEqualToValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加小于等于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThanOrEqualTo(object compareValue)
    {
        Validators.Add(new LessThanOrEqualToValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加小于等于验证器
    /// </summary>
    /// <param name="predicate">自定义委托</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThanOrEqualTo(Func<T, object> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate);

        // 初始化验证器委托器
        var validatorDelegator = new ValidatorDelegator<LessThanOrEqualToValidator>(this
            , instance => new[] { predicate(instance) }
            , () => Strings.LessThanOrEqualToValidator_Invalid);

        Validators.Add(validatorDelegator);

        return this;
    }

    /// <summary>
    /// 添加小于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThan(int compareValue)
    {
        Validators.Add(new LessThanValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加小于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThan(double compareValue)
    {
        Validators.Add(new LessThanValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加小于验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThan(object compareValue)
    {
        Validators.Add(new LessThanValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加小于验证器
    /// </summary>
    /// <param name="predicate">自定义委托</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> LessThan(Func<T, object> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate);

        // 初始化验证器委托器
        var validatorDelegator = new ValidatorDelegator<LessThanValidator>(this
            , instance => new[] { predicate(instance) }
            , () => Strings.LessThanValidator_Invalid);

        Validators.Add(validatorDelegator);

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
    /// 添加不相等验证器
    /// </summary>
    /// <param name="compareValue">比较的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> NotEqual(object? compareValue)
    {
        Validators.Add(new NotEqualValidator(compareValue));

        return this;
    }

    /// <summary>
    /// 添加不相等验证器
    /// </summary>
    /// <param name="predicate">自定义委托</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> NotEqual(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate);

        // 初始化验证器委托器
        var validatorDelegator = new ValidatorDelegator<NotEqualValidator>(this
            , instance => new[] { predicate(instance) }
            , () => Strings.NotEqualValidator_Invalid);

        Validators.Add(validatorDelegator);

        return this;
    }

    /// <summary>
    /// 添加密码验证器
    /// </summary>
    /// <remarks>
    /// 密码长度为 6-18 位，包含至少一个字母和一个数字
    /// </remarks>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Password()
    {
        Validators.Add(new PasswordValidator());

        return this;
    }

    /// <summary>
    /// 添加手机号验证器
    /// </summary>
    public PropertyValidator<T, TProperty> PhoneNumber()
    {
        Validators.Add(new PhoneNumberValidator());

        return this;
    }

    /// <summary>
    /// 添加邮政编码验证器
    /// </summary>
    public PropertyValidator<T, TProperty> PostalCode()
    {
        Validators.Add(new PostalCodeValidator());

        return this;
    }

    /// <summary>
    /// 添加自定义委托验证器
    /// </summary>
    /// <param name="predicate">自定义委托</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Predicate(Func<T, bool> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate);

        // 初始化验证器委托器
        var validatorDelegator = new ValidatorDelegator<PredicateValidator<T>>(this
            , instance => new object?[] { predicate }
            , () => Strings.ValidatorBase_Invalid);

        Validators.Add(validatorDelegator);

        return this;
    }

    /// <summary>
    /// 添加单项验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Single()
    {
        Validators.Add(new SingleValidator());

        return this;
    }

    /// <summary>
    /// 添加以特定字符/字符串开头的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StartsWith(char searchValue)
    {
        Validators.Add(new StartsWithValidator(searchValue));

        return this;
    }

    /// <summary>
    /// 添加以特定字符/字符串开头的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StartsWith(string searchValue)
    {
        Validators.Add(new StartsWithValidator(searchValue));

        return this;
    }

    /// <summary>
    /// 添加以特定字符/字符串开头的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StartsWith(char searchValue, StringComparison comparison)
    {
        Validators.Add(new StartsWithValidator(searchValue)
        {
            Comparison = comparison
        });

        return this;
    }

    /// <summary>
    /// 添加以特定字符/字符串开头的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StartsWith(string searchValue, StringComparison comparison)
    {
        Validators.Add(new StartsWithValidator(searchValue)
        {
            Comparison = comparison
        });

        return this;
    }

    /// <summary>
    /// 添加包含特定字符/字符串的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StringContains(char searchValue)
    {
        Validators.Add(new StringContainsValidator(searchValue));

        return this;
    }

    /// <summary>
    /// 添加包含特定字符/字符串的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StringContains(string searchValue)
    {
        Validators.Add(new StringContainsValidator(searchValue));

        return this;
    }

    /// <summary>
    /// 添加包含特定字符/字符串的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StringContains(char searchValue, StringComparison comparison)
    {
        Validators.Add(new StringContainsValidator(searchValue)
        {
            Comparison = comparison
        });

        return this;
    }

    /// <summary>
    /// 添加包含特定字符/字符串的验证器
    /// </summary>
    /// <param name="searchValue">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StringContains(string searchValue, StringComparison comparison)
    {
        Validators.Add(new StringContainsValidator(searchValue)
        {
            Comparison = comparison
        });

        return this;
    }

    /// <summary>
    /// 添加强类型密码验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> StrongPassword()
    {
        Validators.Add(new StrongPasswordValidator());

        return this;
    }

    /// <summary>
    /// 添加座机（电话）验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Telephone()
    {
        Validators.Add(new TelephoneValidator());

        return this;
    }

    /// <summary>
    /// 添加用户名验证器
    /// </summary>
    /// <remarks>
    /// 长度 4-16 位，仅支持字母，数字，下划线，减号组合
    /// </remarks>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> UserName()
    {
        Validators.Add(new UserNameValidator());

        return this;
    }

    /// <summary>
    /// 添加必填验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> NotNull()
    {
        Validators.Add(new ValueAnnotationValidator(new RequiredAttribute()));

        return this;
    }

    /// <summary>
    /// 添加必填验证器
    /// </summary>
    /// <param name="allowEmptyStrings">是否允许空字符串</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> NotNull(bool allowEmptyStrings)
    {
        Validators.Add(new ValueAnnotationValidator(new RequiredAttribute
        {
            AllowEmptyStrings = allowEmptyStrings
        }));

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
    /// 设置子属性验证器
    /// </summary>
    /// <param name="subValidator"><see cref="IObjectValidator{T}"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> SetValidator(IObjectValidator<TProperty> subValidator)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(subValidator);

        SubValidator = subValidator;

        return this;
    }

    /// <summary>
    /// 添加验证器
    /// </summary>
    /// <param name="validator"><see cref="ValidatorBase"/></param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Add(ValidatorBase validator)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validator);

        Validators.Add(validator);

        return this;
    }

    /// <summary>
    /// 添加自定义委托验证器
    /// </summary>
    /// <param name="predicate">自定义委托</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> Custom(Func<T, bool> predicate)
    {
        return Predicate(predicate);
    }

    /// <summary>
    /// 添加注解（特性）验证器
    /// </summary>
    /// <param name="validationAttribute">验证特性</param>
    /// <returns><see cref="PropertyValidator{T, TProperty}"/></returns>
    public PropertyValidator<T, TProperty> AddAttribute(ValidationAttribute validationAttribute)
    {
        Validators.Add(new ValueAnnotationValidator(validationAttribute));

        return this;
    }
}