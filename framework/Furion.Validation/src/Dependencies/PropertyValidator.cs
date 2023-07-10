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

namespace Furion.Validation;

/// <summary>
/// 属性验证器
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
public sealed class PropertyValidator<T> : IValidator<T>
    where T : class
{
    /// <summary>
    /// 验证器集合
    /// </summary>
    internal readonly Validator<T> _validator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="validator"><see cref="Validator{T}"/></param>
    /// <param name="propertyExpression">属性选择器</param>
    internal PropertyValidator(Validator<T> validator, Expression<Func<T, object?>> propertyExpression)
    {
        Validators = new();
        PropertyName = propertyExpression.GetPropertyName();

        // 将当前属性验证器添加到类型验证器集合中
        _validator = validator;
        _validator.AddPropertyValidator(this);
    }

    /// <summary>
    /// 验证器集合
    /// </summary>
    public List<ValidatorBase> Validators { get; init; }

    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropertyName { get; init; }

    /// <summary>
    /// 错误消息访问器
    /// </summary>
    internal Func<T, string>? ErrorMessageAccessor { get; private set; }

    /// <summary>
    /// 验证对象访问器
    /// </summary>
    internal Func<ValidatorBase, T, object?, object?>? ValidationObjectAccessor { get; private set; }

    /// <summary>
    /// 验证条件
    /// </summary>
    internal Func<T, bool>? Condition { get; private set; }

    /// <summary>
    /// 设置错误消息
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    public void WithErrorMessage(string errorMessage)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(errorMessage, nameof(errorMessage));

        ErrorMessageAccessor = (_) => errorMessage;
    }

    /// <summary>
    /// 设置错误消息
    /// </summary>
    /// <param name="errorMessageAccessor">错误消息访问器</param>
    public void WithErrorMessage(Func<T, string> errorMessageAccessor)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(errorMessageAccessor, nameof(errorMessageAccessor));

        ErrorMessageAccessor = errorMessageAccessor;
    }

    /// <summary>
    /// 添加必填验证器
    /// </summary>
    /// <param name="allowEmptyStrings">是否允许空字符串</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> NotNull(bool allowEmptyStrings = false)
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
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> NotEmpty()
    {
        Validators.Add(new NotEmptyValidator());

        return this;
    }

    /// <summary>
    /// 添加以特定字符串结尾的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> EndsWith(string value)
    {
        Validators.Add(new EndsWithValidator(value));

        return this;
    }

    /// <summary>
    /// 添加以特定字符串结尾的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> EndsWith(char value)
    {
        Validators.Add(new EndsWithValidator(value));

        return this;
    }

    /// <summary>
    /// 添加以特定字符串结尾的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> EndsWith(string value, StringComparison comparison)
    {
        Validators.Add(new EndsWithValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加以特定字符串结尾的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> EndsWith(char value, StringComparison comparison)
    {
        Validators.Add(new EndsWithValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加以特定字符串开头的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> StartsWith(string value)
    {
        Validators.Add(new StartsWithValidator(value));

        return this;
    }

    /// <summary>
    /// 添加以特定字符串开头的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> StartsWith(char value)
    {
        Validators.Add(new StartsWithValidator(value));

        return this;
    }

    /// <summary>
    /// 添加以特定字符串开头的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> StartsWith(string value, StringComparison comparison)
    {
        Validators.Add(new StartsWithValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加以特定字符串开头的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> StartsWith(char value, StringComparison comparison)
    {
        Validators.Add(new StartsWithValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加包含特定字符串的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> StringContains(string value)
    {
        Validators.Add(new StringContainsValidator(value));

        return this;
    }

    /// <summary>
    /// 添加包含特定字符串的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> StringContains(char value)
    {
        Validators.Add(new StringContainsValidator(value));

        return this;
    }

    /// <summary>
    /// 添加包含特定字符串的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> StringContains(string value, StringComparison comparison)
    {
        Validators.Add(new StringContainsValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加包含特定字符串的验证器
    /// </summary>
    /// <param name="value">检索的值</param>
    /// <param name="comparison"><see cref="StringComparison"/></param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> StringContains(char value, StringComparison comparison)
    {
        Validators.Add(new StringContainsValidator(value) { Comparison = comparison });

        return this;
    }

    /// <summary>
    /// 添加最大长度验证器
    /// </summary>
    /// <param name="length">长度</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> MaxLength(int length)
    {
        Validators.Add(new ValueAnnotationValidator(new MaxLengthAttribute(length)));

        return this;
    }

    /// <summary>
    /// 添加最小长度验证器
    /// </summary>
    /// <param name="length">长度</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> MinLength(int length)
    {
        Validators.Add(new ValueAnnotationValidator(new MinLengthAttribute(length)));

        return this;
    }

    /// <summary>
    /// 添加长度验证器
    /// </summary>
    /// <param name="minimumLength">最小长度</param>
    /// <param name="maximumLength">最大长度</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> Length(int minimumLength, int maximumLength)
    {
        Validators.Add(new ValueAnnotationValidator(new LengthAttribute(minimumLength, maximumLength)));

        return this;
    }

    /// <summary>
    /// 添加字符串长度验证器
    /// </summary>
    /// <param name="maximumLength">最大长度</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> StringLength(int maximumLength)
    {
        Validators.Add(new ValueAnnotationValidator(new StringLengthAttribute(maximumLength)));

        return this;
    }

    /// <summary>
    /// 添加字符串长度验证器
    /// </summary>
    /// <param name="minimumLength">最小长度</param>
    /// <param name="maximumLength">最大长度</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> StringLength(int minimumLength, int maximumLength)
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
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> Range(int minimum, int maximum)
    {
        Validators.Add(new ValueAnnotationValidator(new RangeAttribute(minimum, maximum)));

        return this;
    }

    /// <summary>
    /// 添加范围验证器
    /// </summary>
    /// <param name="minimum">最小值</param>
    /// <param name="maximum">最大值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> Range(double minimum, double maximum)
    {
        Validators.Add(new ValueAnnotationValidator(new RangeAttribute(minimum, maximum)));

        return this;
    }

    /// <summary>
    /// 添加正则表达式验证器
    /// </summary>
    /// <param name="pattern">正则表达式</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> RegularExpression(string pattern)
    {
        Validators.Add(new ValueAnnotationValidator(new RegularExpressionAttribute(pattern)));

        return this;
    }

    /// <summary>
    /// 添加正则表达式验证器
    /// </summary>
    /// <param name="pattern">正则表达式</param>
    /// <param name="matchTimeoutInMilliseconds">匹配超时毫秒数</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> RegularExpression(string pattern, int matchTimeoutInMilliseconds)
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
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> EmailAddress()
    {
        Validators.Add(new ValueAnnotationValidator(new EmailAddressAttribute()));

        return this;
    }

    /// <summary>
    /// 添加用户名验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> UserName()
    {
        Validators.Add(new UserNameValidator());

        return this;
    }

    /// <summary>
    /// 添加密码验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> Password()
    {
        Validators.Add(new PasswordValidator());

        return this;
    }

    /// <summary>
    /// 添加强密码验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> StrongPassword()
    {
        Validators.Add(new StrongPasswordValidator());

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> GreaterThanOrEqualTo(int value)
    {
        Validators.Add(new GreaterThanOrEqualToValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> GreaterThanOrEqualTo(double value)
    {
        Validators.Add(new GreaterThanOrEqualToValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> GreaterThanOrEqualTo(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new GreaterThanOrEqualToValidator(predicate(instance)).IsValid(GetValue(instance))
            , Strings.GreaterThanOrEqualToValidator_Invalid
            , instance => new[] { GetValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> GreaterThan(int value)
    {
        Validators.Add(new GreaterThanValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> GreaterThan(double value)
    {
        Validators.Add(new GreaterThanValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> GreaterThan(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new GreaterThanValidator(predicate(instance)).IsValid(GetValue(instance))
            , Strings.GreaterThanValidator_Invalid
            , instance => new[] { GetValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> LessThanOrEqualTo(int value)
    {
        Validators.Add(new LessThanOrEqualToValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> LessThanOrEqualTo(double value)
    {
        Validators.Add(new LessThanOrEqualToValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于等于验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> LessThanOrEqualTo(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new LessThanOrEqualToValidator(predicate(instance)).IsValid(GetValue(instance))
            , Strings.LessThanOrEqualToValidator_Invalid
            , instance => new[] { GetValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> LessThan(int value)
    {
        Validators.Add(new LessThanValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> LessThan(double value)
    {
        Validators.Add(new LessThanValidator(value));

        return this;
    }

    /// <summary>
    /// 添加大于验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> LessThan(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new LessThanValidator(predicate(instance)).IsValid(GetValue(instance))
            , Strings.LessThanValidator_Invalid
            , instance => new[] { GetValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加不相等验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> NotEqual(object? value)
    {
        Validators.Add(new NotEqualValidator(value));

        return this;
    }

    /// <summary>
    /// 添加不相等验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> NotEqual(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new NotEqualValidator(predicate(instance)).IsValid(GetValue(instance))
            , Strings.NotEqualValidator_Invalid
            , instance => new[] { GetValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加相等验证器
    /// </summary>
    /// <param name="value">比较的值</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> Equal(object? value)
    {
        Validators.Add(new EqualValidator(value));

        return this;
    }

    /// <summary>
    /// 添加相等验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> Equal(Func<T, object?> predicate)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        Validators.Add(new CustomValidator(instance => new EqualValidator(predicate(instance)).IsValid(GetValue(instance))
            , Strings.EqualValidator_Invalid
            , instance => new[] { GetValue(instance)?.ToString() }));

        return this;
    }

    /// <summary>
    /// 添加委托对象验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> Predicate(Func<object?, bool> predicate)
    {
        Validators.Add(new PredicateValidator(predicate));

        return this;
    }

    /// <summary>
    /// 添加组合验证器
    /// </summary>
    /// <param name="validators">验证器集合</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> Composite(params ValidatorBase[] validators)
    {
        Validators.Add(new CompositeValidator(validators));

        return this;
    }

    /// <summary>
    /// 添加组合验证器
    /// </summary>
    /// <param name="validators">验证器集合</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> Composite(IList<ValidatorBase> validators)
    {
        Validators.Add(new CompositeValidator(validators));

        return this;
    }

    /// <summary>
    /// 添加注解（特性）验证器
    /// </summary>
    /// <param name="validationAttributes">验证特性集合</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> SetAnnotations(params ValidationAttribute[] validationAttributes)
    {
        Validators.Add(new ValueAnnotationValidator(validationAttributes));

        return this;
    }

    /// <summary>
    /// 添加注解（特性）验证器
    /// </summary>
    /// <param name="validationAttributes">验证特性集合</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> SetAnnotations(IList<ValidationAttribute> validationAttributes)
    {
        Validators.Add(new ValueAnnotationValidator(validationAttributes));

        return this;
    }

    /// <summary>
    /// 添加验证器
    /// </summary>
    /// <param name="validator"><see cref="ValidatorBase"/></param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> SetValidator(ValidatorBase validator)
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
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> SetValidators(params ValidatorBase[] validator)
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
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> SetValidators(IEnumerable<ValidatorBase> validator)
    {
        return SetValidators(validator?.ToArray()!);
    }

    /// <summary>
    /// 创建注解（特性）验证器
    /// </summary>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> Annotate()
    {
        Validators.Add(new ObjectAnnotationValidator());

        return this;
    }

    /// <summary>
    /// 添加自定义验证器
    /// </summary>
    /// <param name="predicate">委托对象</param>
    /// <param name="defaultErrorMessage">默认错误消息</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> Custom(Func<T, bool> predicate, string? defaultErrorMessage = default)
    {
        Validators.Add(new CustomValidator(predicate, defaultErrorMessage));

        return this;
    }

    /// <summary>
    /// 添加自定义验证器
    /// </summary>
    /// <param name="condition">条件委托</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> When(Func<T, bool> condition)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));

        Condition = condition;

        return this;
    }

    /// <summary>
    /// 设置验证对象访问器
    /// </summary>
    /// <param name="validationObjectAccessor">验证对象访问器</param>
    /// <returns><see cref="PropertyValidator{T}"/></returns>
    public PropertyValidator<T> SetValidationObjectAccessor(Func<ValidatorBase, T, object?, object?> validationObjectAccessor)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validationObjectAccessor, nameof(validationObjectAccessor));

        ValidationObjectAccessor = validationObjectAccessor;

        return this;
    }

    /// <summary>
    /// 检查是否可以执行验证程序
    /// </summary>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <returns><see cref="bool"/></returns>
    internal bool CanValidate(T instance)
    {
        if (Condition is null)
        {
            return true;
        }

        return Condition(instance);
    }

    /// <summary>
    /// 检查值有效性
    /// </summary>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <returns><see cref="bool"/></returns>
    public bool IsValid(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        // 检查是否可以执行验证程序
        if (!CanValidate(instance))
        {
            return true;
        }

        // 获取属性值
        var propertyValue = GetValue(instance);

        return Validators.All(validator => validator.IsValid(GetValidationObject(validator, instance, propertyValue)));
    }

    /// <summary>
    /// 获取验证结果
    /// </summary>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <returns><see cref="ValidationResult"/> 集合</returns>
    public List<ValidationResult>? GetValidationResults(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        // 检查是否可以执行验证程序
        if (!CanValidate(instance))
        {
            return null;
        }

        // 获取属性值
        var propertyValue = GetValue(instance);

        // 获取所有验证器验证结果集合
        var validatorResults = Validators.SelectMany(validator => validator.GetValidationResults(GetValidationObject(validator, instance, propertyValue), PropertyName) ?? Enumerable.Empty<ValidationResult>())
                                                            .ToList();
        if (validatorResults.Count == 0)
        {
            return null;
        }

        // 添加自定义错误消息
        if (ErrorMessageAccessor is not null)
        {
            validatorResults.Insert(0, new ValidationResult(ErrorMessageAccessor(instance), new[] { PropertyName }));
        }

        return validatorResults;
    }

    /// <inheritdoc />
    public void Validate(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        // 检查是否可以执行验证程序
        if (!CanValidate(instance))
        {
            return;
        }

        // 获取验证结果
        var validationResults = GetValidationResults(instance);

        if (validationResults is null)
        {
            return;
        }

        // 创建组合异常
        var validationExceptions = validationResults.Select(validationResult => new ValidationException(validationResult, null, instance));

        // 抛出组合验证异常
        throw new AggregateValidationException(validationExceptions);
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <returns><see cref="object"/></returns>
    internal object? GetValue(T instance)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        // 根据属性名称查找属性对象
        var propertyInfo = instance.GetType().GetProperty(PropertyName);

        // 空检查
        ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

        // 返回属性值
        return propertyInfo.GetValue(instance);
    }

    /// <summary>
    /// 获取验证对象
    /// </summary>
    /// <param name="validator"><see cref="ValidatorBase"/></param>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <param name="propertyValue">属性值</param>
    /// <returns><see cref="object"/></returns>
    internal object? GetValidationObject(ValidatorBase validator, T instance, object? propertyValue)
    {
        // 如果是自定义验证器则返回对象实例
        if (validator.IsSameAs(typeof(CustomValidator)))
        {
            return instance;
        }

        // 检查是否设置了验证对象访问器
        if (ValidationObjectAccessor is not null)
        {
            return ValidationObjectAccessor(validator, instance, propertyValue);
        }

        return propertyValue;
    }

    /// <summary>
    /// 自定义验证器
    /// </summary>
    internal sealed class CustomValidator : ValidatorBase
    {
        /// <summary>
        /// 格式化参数访问器
        /// </summary>
        internal readonly Func<T, string?[]>? _formatArgsAccessor;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="predicate">委托对象</param>
        /// <param name="defaultErrorMessage">默认错误消息</param>
        /// <param name="formatArgsAccessor">格式化参数访问器</param>
        internal CustomValidator(Func<T, bool> predicate, string? defaultErrorMessage = default, Func<T, string?[]>? formatArgsAccessor = default)
            : base(defaultErrorMessage)
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            Predicate = predicate;
            ErrorMessage = defaultErrorMessage;
            _formatArgsAccessor = formatArgsAccessor;
        }

        /// <summary>
        /// 委托对象
        /// </summary>
        internal Func<T, bool> Predicate { get; init; }

        /// <inheritdoc />
        public override bool IsValid(object? instance)
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));

            return Predicate((T)instance);
        }

        /// <inheritdoc />
        public override string FormatErrorMessage(string name, object? instance = default)
        {
            var args = new List<string?> { name };

            if (_formatArgsAccessor is not null)
            {
                // 空检查
                ArgumentNullException.ThrowIfNull(instance, nameof(instance));

                args.AddRange(_formatArgsAccessor((T)instance));
            }

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, args.ToArray());
        }
    }
}