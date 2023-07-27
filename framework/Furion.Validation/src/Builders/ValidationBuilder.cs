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
/// 数据验证构建器
/// </summary>
public sealed class ValidationBuilder
{
    /// <summary>
    /// 待注册的验证器类型集合
    /// </summary>
    internal readonly HashSet<Type> _validatorTypes;

    /// <summary>
    /// <inheritdoc cref="ValidationBuilder"/>
    /// </summary>
    public ValidationBuilder()
    {
        _validatorTypes = new();
    }

    /// <summary>
    /// 添加验证器
    /// </summary>
    /// <typeparam name="TValidator"><see cref="AbstractValidator{T}"/></typeparam>
    /// <returns><see cref="ValidationBuilder"/></returns>
    public ValidationBuilder AddValidator<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TValidator>()
        where TValidator : class, IObjectValidator
    {
        return AddValidator(typeof(TValidator));
    }

    /// <summary>
    /// 添加验证器
    /// </summary>
    /// <param name="validatorType"><see cref="AbstractValidator{T}"/></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ValidationBuilder AddValidator([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type validatorType)
    {
        // 检查类型合法性
        EnsureLegalValidatorType(validatorType);

        // 将验证器类型添加到集合中
        _validatorTypes.Add(validatorType);

        return this;
    }

    /// <summary>
    /// 构建模块服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    internal void Build(IServiceCollection services)
    {
        // 逐条添加服务
        foreach (var validatorType in _validatorTypes)
        {
            // 获取验证器模型类型
            var baseType = validatorType.BaseType!;
            var modelType = baseType.GenericTypeArguments[0];

            // 添加服务
            services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IObjectValidator<>).MakeGenericType(modelType), validatorType));
            services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IObjectValidator), validatorType));
        }
    }

    /// <summary>
    /// 检查类型合法性
    /// </summary>
    /// <param name="validatorType"><see cref="AbstractValidator{T}"/></param>
    /// <exception cref="InvalidOperationException"></exception>
    internal static void EnsureLegalValidatorType(Type validatorType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validatorType);

        // 检查类型是否派生自 AbstractValidator<> 类型
        var baseType = validatorType.BaseType;
        if (!(baseType is not null
            && typeof(AbstractValidator<>).IsDefinitionEqual(baseType)))
        {
            throw new InvalidOperationException($"`{validatorType}` type is not assignable from `{typeof(AbstractValidator<>)}`.");
        }

        // 检查类型是否可以实例化
        if (!validatorType.IsInstantiable())
        {
            throw new InvalidOperationException($"`{validatorType}` type must be able to be instantiated.");
        }
    }
}