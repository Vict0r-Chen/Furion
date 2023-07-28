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
/// 链式验证器扫描器
/// </summary>
internal sealed class FluentValidatorScanner
{
    /// <inheritdoc cref="IServiceCollection"/>
    internal readonly IServiceCollection _services;

    /// <inheritdoc cref="FluentValidationBuilder"/>
    internal readonly FluentValidationBuilder _fluentValidationBuilder;

    /// <summary>
    /// <inheritdoc cref="FluentValidatorScanner"/>
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="fluentValidationBuilder"><see cref="FluentValidationBuilder"/></param>
    internal FluentValidatorScanner(IServiceCollection services
        , FluentValidationBuilder fluentValidationBuilder)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(fluentValidationBuilder);

        _services = services;
        _fluentValidationBuilder = fluentValidationBuilder;

        // 初始化
        Initialize();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    internal void Initialize()
    {
        // 默认添加启动程序集作为扫描
        _fluentValidationBuilder.AddAssemblies(Assembly.GetEntryAssembly()!);
    }

    /// <summary>
    /// 扫描并添加服务
    /// </summary>
    internal void ScanToAddServices()
    {
        // 检查是否禁用程序集扫描
        if (_fluentValidationBuilder.SuppressAssemblyScanning)
        {
            // 输出调试事件
            Debugging.Warn("Type scanning has been disabled.");

            return;
        }

        // 获取验证器类型集合
        var validatorTypes = GetValidatorTypes();

        // 逐条添加服务
        foreach (var validatorType in validatorTypes)
        {
            AddService(validatorType);
        }
    }

    /// <summary>
    /// 获取验证器类型集合
    /// </summary>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal IEnumerable<Type> GetValidatorTypes()
    {
        // 扫描所有程序集类型
        var validatorTypes = _fluentValidationBuilder._assemblies
            .SelectMany(assembly => assembly.GetTypes(_fluentValidationBuilder.SuppressNonPublicType))
            .Where(type => type.BaseType is not null && typeof(AbstractValidator<>).IsDefinitionEqual(type.BaseType) && type.IsInstantiable())
            .Where(type => _fluentValidationBuilder._typeFilterConfigure is null || _fluentValidationBuilder._typeFilterConfigure.Invoke(type))
            .Concat(_fluentValidationBuilder._validatorTypes)
            .Distinct();

        return validatorTypes;
    }

    /// <summary>
    /// 添加服务
    /// </summary>
    /// <param name="validatorType"><see cref="ValidatorBase"/></param>
    internal void AddService(Type validatorType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validatorType);

        // 获取验证器模型类型
        var baseType = validatorType.BaseType!;
        var modelType = baseType.GenericTypeArguments[0];

        // 添加服务
        _services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IObjectValidator<>).MakeGenericType(modelType), validatorType));
        _services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IObjectValidator), validatorType));
    }
}