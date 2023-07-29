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
/// 链式数据验证构建器
/// </summary>
public sealed class FluentValidationBuilder
{
    /// <summary>
    /// 待扫描的程序集集合
    /// </summary>
    internal readonly HashSet<Assembly> _assemblies;

    /// <summary>
    /// 待注册的验证器类型集合
    /// </summary>
    internal readonly HashSet<Type> _validatorTypes;

    /// <summary>
    /// 类型扫描过滤器
    /// </summary>
    internal Func<Type, bool>? _typeFilterConfigure;

    /// <summary>
    /// <inheritdoc cref="FluentValidationBuilder"/>
    /// </summary>
    public FluentValidationBuilder()
    {
        _assemblies = new();
        _validatorTypes = new();
    }

    /// <summary>
    /// 禁用程序集扫描
    /// </summary>
    public bool SuppressAssemblyScanning { get; set; }

    /// <summary>
    /// 禁用非公开类型
    /// </summary>
    public bool SuppressNonPublicType { get; set; }

    /// <summary>
    /// 添加类型扫描过滤器
    /// </summary>
    /// <param name="configure">自定义配置委托</param>
    public void AddTypeFilter(Func<Type, bool> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        _typeFilterConfigure = configure;
    }

    /// <summary>
    /// 添加程序集
    /// </summary>
    /// <param name="assemblies"><see cref="Assembly"/>[]</param>
    /// <returns><see cref="FluentValidationBuilder"/></returns>
    public FluentValidationBuilder AddAssemblies(params Assembly[] assemblies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(assemblies);

        Array.ForEach(assemblies, assembly =>
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(assembly);

            _assemblies.Add(assembly);
        });

        return this;
    }

    /// <summary>
    /// 添加程序集
    /// </summary>
    /// <param name="assemblies"><see cref="IEnumerable{T}"/></param>
    /// <returns><see cref="FluentValidationBuilder"/></returns>
    public FluentValidationBuilder AddAssemblies(IEnumerable<Assembly> assemblies)
    {
        return AddAssemblies(assemblies.ToArray());
    }

    /// <summary>
    /// 添加验证器
    /// </summary>
    /// <typeparam name="TValidator"><see cref="AbstractValidator{T}"/></typeparam>
    /// <returns><see cref="FluentValidationBuilder"/></returns>
    public FluentValidationBuilder AddValidator<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TValidator>()
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
    public FluentValidationBuilder AddValidator([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type validatorType)
    {
        // 检查验证器类型合法性
        Helpers.EnsureLegalValidatorType(validatorType);

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
        // 空检查
        ArgumentNullException.ThrowIfNull(services);

        // 初始化链式验证器扫描器并执行扫描
        new FluentValidatorScanner(services, this)
            .ScanToAddServices();
    }
}