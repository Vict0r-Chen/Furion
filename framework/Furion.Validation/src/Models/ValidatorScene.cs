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
/// 支持场景值的 <see cref="Validator"/>
/// </summary>
public static class ValidatorScene
{
    /// <inheritdoc cref="Validator.TryValidateObject(object, ValidationContext, ICollection{ValidationResult}?)" />
    public static bool TryValidateObject(object instance, ValidationContext validationContext, ICollection<ValidationResult>? validationResults, string? scene = default)
    {
        return Validator.TryValidateObject(instance, ValidationContextWithScene(validationContext, scene), validationResults);
    }

    /// <inheritdoc cref="Validator.TryValidateObject(object, ValidationContext, ICollection{ValidationResult}?, bool)" />
    public static bool TryValidateObject(object instance, ValidationContext validationContext, ICollection<ValidationResult>? validationResults, bool validateAllProperties, string? scene = default)
    {
        return Validator.TryValidateObject(instance, ValidationContextWithScene(validationContext, scene), validationResults, validateAllProperties);
    }

    /// <inheritdoc cref="Validator.TryValidateProperty(object?, ValidationContext, ICollection{ValidationResult}?)" />
    public static bool TryValidateProperty(object? value, ValidationContext validationContext, ICollection<ValidationResult>? validationResults, string? scene = default)
    {
        return Validator.TryValidateProperty(value, ValidationContextWithScene(validationContext, scene), validationResults);
    }

    /// <inheritdoc cref="Validator.TryValidateValue(object, ValidationContext, ICollection{ValidationResult}?, IEnumerable{ValidationAttribute})" />
    public static bool TryValidateValue(object value, ValidationContext validationContext, ICollection<ValidationResult>? validationResults, IEnumerable<ValidationAttribute> validationAttributes, string? scene = default)
    {
        return Validator.TryValidateValue(value, ValidationContextWithScene(validationContext, scene), validationResults, validationAttributes);
    }

    /// <inheritdoc cref="Validator.ValidateObject(object, ValidationContext)" />
    public static void ValidateObject(object instance, ValidationContext validationContext, string? scene = default)
    {
        Validator.ValidateObject(instance, ValidationContextWithScene(validationContext, scene));
    }

    /// <inheritdoc cref="Validator.ValidateObject(object, ValidationContext, bool)" />
    public static void ValidateObject(object instance, ValidationContext validationContext, bool validateAllProperties, string? scene = default)
    {
        Validator.ValidateObject(instance, ValidationContextWithScene(validationContext, scene), validateAllProperties);
    }

    /// <inheritdoc cref="Validator.ValidateProperty(object?, ValidationContext)" />
    public static void ValidateProperty(object? value, ValidationContext validationContext, string? scene = default)
    {
        Validator.ValidateProperty(value, ValidationContextWithScene(validationContext, scene));
    }

    /// <inheritdoc cref="Validator.ValidateValue(object, ValidationContext, IEnumerable{ValidationAttribute})" />
    public static void ValidateValue(object value, ValidationContext validationContext, IEnumerable<ValidationAttribute> validationAttributes, string? scene = default)
    {
        Validator.ValidateValue(value, ValidationContextWithScene(validationContext, scene), validationAttributes);
    }

    /// <summary>
    /// 支持场景值的 <see cref="ValidationContext"/>
    /// </summary>
    /// <param name="validationContext"><see cref="ValidationContext"/></param>
    /// <param name="scene">场景值</param>
    /// <returns><see cref="ValidationContext"/></returns>
    internal static ValidationContext ValidationContextWithScene(ValidationContext validationContext, string? scene = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validationContext, nameof(validationContext));

        // 空检查
        if (string.IsNullOrWhiteSpace(scene))
        {
            return validationContext;
        }

        validationContext.Items[SCENE_KEY] = scene;
        return validationContext;
    }

    /// <summary>
    /// 场景值 KEY
    /// </summary>
    internal const string SCENE_KEY = "Scene";
}