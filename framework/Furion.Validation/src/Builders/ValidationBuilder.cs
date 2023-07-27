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
    /// 待注册的验证器服务集合
    /// </summary>
    internal readonly Dictionary<Type, Type> _validators;

    /// <summary>
    /// <inheritdoc cref="ValidationBuilder"/>
    /// </summary>
    public ValidationBuilder()
    {
        _validators = new();
    }

    /// <summary>
    /// 添加验证器
    /// </summary>
    /// <typeparam name="TValidator"><see cref="IObjectValidator{T}"/></typeparam>
    /// <returns><see cref="ValidationBuilder"/></returns>
    public ValidationBuilder AddValidator<TValidator>()
        where TValidator : class, IObjectValidator
    {
        return AddValidator(typeof(TValidator));
    }

    /// <summary>
    /// 添加验证器
    /// </summary>
    /// <param name="validatorType"><see cref="IObjectValidator{T}"/></param>
    /// <returns><see cref="ValidationBuilder"/></returns>
    public ValidationBuilder AddValidator(Type validatorType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validatorType, nameof(validatorType));

        // 是否继承 AbstractValidator<> 泛型类型
        var baseType = validatorType.BaseType;
        if (!(baseType is not null && typeof(AbstractValidator<>).IsDefinitionEqual(baseType)))
        {
            throw new ArgumentException($"`{validatorType.Name}` validator type is not assignable from `AbstractValidator<>`.");
        }

        // 类型必须可以实例化
        if (!validatorType.IsInstantiable())
        {
            throw new InvalidOperationException($"`{validatorType.Name}` validator type must be able to be instantiated.");
        }

        // 获取验证器模型类型
        var modelType = baseType.GenericTypeArguments.First();

        // 添加待注册的验证器服务
        _validators.Add(typeof(IObjectValidator<>).MakeGenericType(modelType), validatorType);
        _validators.Add(typeof(IObjectValidator), validatorType);

        return this;
    }

    /// <summary>
    /// 构建模块服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    internal void Build(IServiceCollection services)
    {
        // 遍历集合将验证器服务添加到 IServiceCollection 中
        foreach (var (serviceType, implementType) in _validators)
        {
            services.AddTransient(serviceType, implementType);
        }

        // 释放对象
        Release();
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    internal void Release()
    {
        _validators.Clear();
    }
}