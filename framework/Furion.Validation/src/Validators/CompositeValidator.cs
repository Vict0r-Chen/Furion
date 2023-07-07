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
/// 组合验证器
/// </summary>
public partial class CompositeValidator : ValidatorBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="validators">验证器集合</param>
    public CompositeValidator(params ValidatorBase[] validators)
        : this(validators?.ToList()!)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="validators">验证器集合</param>
    public CompositeValidator(IList<ValidatorBase> validators)
        : base(() => Strings.CompositeValidator_Invalid)
    {
        // 合法数据检查
        EnsureLegalData(validators);

        ValidatorCollection = validators;
    }

    /// <summary>
    /// 验证器集合
    /// </summary>
    public IList<ValidatorBase> ValidatorCollection { get; init; }

    /// <inheritdoc cref="ValidatorRelationship" />
    public ValidatorRelationship Relationship { get; set; }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        // 合法数据检查
        EnsureLegalData(ValidatorCollection);

        return Relationship switch
        {
            ValidatorRelationship.Default or ValidatorRelationship.And => ValidatorCollection.All(validator => validator.IsValid(value)),
            ValidatorRelationship.Or => ValidatorCollection.Any(validator => validator.IsValid(value)),
            _ => false,
        };
    }

    /// <inheritdoc />
    public override List<ValidationResult>? GetValidationResults(object? value, IEnumerable<string>? memberNames = null)
    {
        var validationResults = new List<ValidationResult>();

        // 处理验证器关系为 And 或者为 Or 且全部验证失败的情况
        if (Relationship is ValidatorRelationship.Default or ValidatorRelationship.And
            || (Relationship is ValidatorRelationship.Or
                && !ValidatorCollection.All(validator => validator.IsValid(value))))
        {
            validationResults.AddRange(ValidatorCollection
                .SelectMany(validator => validator.GetValidationResults(value, memberNames) ?? Enumerable.Empty<ValidationResult>()));
        }

        // 处理自定义错误消息情况
        if (ErrorMessage is not null && validationResults.Count > 0)
        {
            validationResults.Insert(0, new ValidationResult(FormatErrorMessage(memberNames)));
        }

        return validationResults.Count == 0
            ? null
            : validationResults;
    }

    /// <summary>
    /// 合法数据检查
    /// </summary>
    /// <param name="validators">验证器集合</param>
    /// <exception cref="InvalidOperationException"></exception>
    internal static void EnsureLegalData(IList<ValidatorBase> validators)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validators, nameof(validators));

        // 检查集合中是否存在 null 值
        if (validators.Any(validator => validator is null))
        {
            throw new ArgumentException("The validator collection contains a null value.", nameof(validators));
        }
    }
}