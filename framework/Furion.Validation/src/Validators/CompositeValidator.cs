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
/// 组合验证器
/// </summary>
public class CompositeValidator : ValidatorBase
{
    /// <summary>
    /// <inheritdoc cref="CompositeValidator"/>
    /// </summary>
    /// <param name="validators">验证器集合</param>
    public CompositeValidator(params ValidatorBase[] validators)
        : this((validators ?? throw new ArgumentNullException(nameof(validators))).ToList())
    {
    }

    /// <summary>
    /// <inheritdoc cref="CompositeValidator"/>
    /// </summary>
    /// <param name="validators">验证器集合</param>
    public CompositeValidator(IList<ValidatorBase> validators)
        : base()
    {
        // 检查验证器集合合法性
        EnsureLegalData(validators);

        Validators = validators;
    }

    /// <summary>
    /// 验证器集合
    /// </summary>
    public IList<ValidatorBase> Validators { get; init; }

    /// <inheritdoc cref="ValidatorCascadeMode" />
    public ValidatorCascadeMode CascadeMode { get; set; }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        // 检查验证器集合合法性
        EnsureLegalData(Validators);

        // 空集合检查
        if (Validators.Count == 0)
        {
            return true;
        }

        return CascadeMode switch
        {
            ValidatorCascadeMode.Continue or ValidatorCascadeMode.StopOnFirstFailure => Validators.All(validator => validator.IsValid(value)),
            ValidatorCascadeMode.UsingFirstSuccess => Validators.Any(validator => validator.IsValid(value)),
            _ => false,
        };
    }

    /// <inheritdoc />
    public override List<ValidationResult>? GetValidationResults(object? value, string name)
    {
        // 检查值是否有效
        if (IsValid(value))
        {
            return null;
        }

        // 初始化验证结果集合
        var validationResults = Validators.SelectMany(validator => validator.GetValidationResults(value, name) ?? Enumerable.Empty<ValidationResult>(), CascadeMode)
            .ToList();

        // 检查是否配置了自定义错误消息
        if (ErrorMessage is not null)
        {
            validationResults.Insert(0, new(FormatErrorMessage(name, value), new[] { name }));
        }

        return validationResults;
    }

    /// <summary>
    /// 检查验证器集合合法性
    /// </summary>
    /// <param name="validators">验证器集合</param>
    /// <exception cref="ArgumentException"></exception>
    internal static void EnsureLegalData(IList<ValidatorBase> validators)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validators);

        // 子项空检查
        if (validators.Any(validator => validator is null))
        {
            throw new ArgumentException("The validator collection contains a null value.", nameof(validators));
        }
    }
}