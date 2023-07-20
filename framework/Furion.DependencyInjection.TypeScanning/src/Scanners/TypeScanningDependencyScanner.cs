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

namespace Furion.DependencyInjection;

/// <summary>
/// 类型扫描依赖关系扫描器
/// </summary>
internal sealed class TypeScanningDependencyScanner
{
    /// <inheritdoc cref="IServiceCollection"/>
    internal readonly IServiceCollection _services;

    /// <inheritdoc cref="TypeScanningDependencyBuilder"/>
    internal readonly TypeScanningDependencyBuilder _typeScanningDependencyBuilder;

    /// <summary>
    /// 依赖关系接口类型
    /// </summary>
    internal readonly Type _dependencyType;

    /// <summary>
    /// <inheritdoc cref="TypeScanningDependencyScanner"/>
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="typeScanningDependencyBuilder"><see cref="TypeScanningDependencyBuilder"/></param>
    internal TypeScanningDependencyScanner(IServiceCollection services
        , TypeScanningDependencyBuilder typeScanningDependencyBuilder)
    {
        _services = services;
        _typeScanningDependencyBuilder = typeScanningDependencyBuilder;
        _dependencyType = typeof(IDependency);
    }

    /// <summary>
    /// 扫描并添加服务
    /// </summary>
    internal void ScanToAddServices()
    {
        // 扫描程序集并创建服务描述器模型集合
        var serviceDescriptors = CreateModels();

        // 遍历集合将服务描述器添加到 IServiceCollection 中
        foreach (var serviceDescriptorModel in serviceDescriptors)
        {
            AddingToServices(_services, serviceDescriptorModel);
        }
    }

    /// <summary>
    /// 创建类型扫描依赖关系模型集合
    /// </summary>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal IEnumerable<TypeScanningDependencyModel> CreateModels()
    {
        // 是否禁用程序集扫描
        if (_typeScanningDependencyBuilder.SuppressAssemblyScanning)
        {
            // 输出事件消息
            Debugging.Warn("Dependency Injection module assembly scanning has been disabled.");

            return Enumerable.Empty<TypeScanningDependencyModel>();
        }

        // 扫描程序集查找所有符合规则的类型
        var effectiveTypes = _typeScanningDependencyBuilder._assemblies.SelectMany(assembly => assembly.GetTypes(_typeScanningDependencyBuilder.SuppressNonPublicType))
            .Where(t => _dependencyType.IsAssignableFrom(t) && t.IsInstantiable());

        // 空检查
        if (!effectiveTypes.Any())
        {
            return Enumerable.Empty<TypeScanningDependencyModel>();
        }

        // 返回扫描后的服务描述器集合
        return effectiveTypes.SelectMany(CreateServiceDescriptors)
            .OrderBy(s => s.Descriptor.ServiceType.Name)
            .ThenBy(s => s.Order);
    }

    /// <summary>
    /// 根据类型创建服务描述器模型集合
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="List{T}"/></returns>
    internal IEnumerable<TypeScanningDependencyModel> CreateServiceDescriptors(Type type)
    {
        // 获取 [dependency] 特性
        var dependencyAttribute = type.GetDefinedCustomAttributeOrNew<DependencyAttribute>(true);

        // 是否配置了 Ignore 属性
        if (dependencyAttribute is { Ignore: true })
        {
            yield break;
        }

        // 是否重复定义 [ExposeServices] 特性
        if (!_typeScanningDependencyBuilder.ValidateExposeService
            && type.IsMultipleSameDefined(typeof(ExposeServicesAttribute), true))
        {
            yield break;
        }

        // 获取 [ExposeServices] 特性
        var exposeServicesAttribute = type.GetDefinedCustomAttributeOrNew<ExposeServicesAttribute>(true);

        // 获取有效的服务类型集合
        var serviceTypes = GetEffectiveServiceTypes(type
            , exposeServicesAttribute.ServiceTypes
            , out var dependencyType);

        // 获取服务生存期
        var serviceLifetime = GetServiceLifetime(dependencyType, _typeScanningDependencyBuilder.ValidateLifetime);
        if (serviceLifetime is null)
        {
            serviceTypes.Clear();
            yield break;
        }

        // 是否包含基类
        var baseType = type.BaseType;
        if (baseType is not null
            && baseType != typeof(object)
            && dependencyAttribute is { IncludeBase: true }
            && type.IsTypeCompatibilityTo(baseType))
        {
            serviceTypes.Add(!type.IsGenericType
                ? baseType
                : baseType.GetGenericTypeDefinition());
        }

        // 获取服务实现类型
        var implementationType = !type.IsGenericType
            ? type
            : type.GetGenericTypeDefinition();

        // 是否包含自身
        if (dependencyAttribute is { IncludeSelf: true }
            || serviceTypes.Count == 0)
        {
            serviceTypes.Add(implementationType);
        }

        // 遍历服务类型并创建服务描述器模型
        foreach (var serviceType in serviceTypes)
        {
            // 创建服务描述器模型
            var serviceDescriptorModel = new TypeScanningDependencyModel(serviceType
                , implementationType
                , serviceLifetime.Value
                , dependencyAttribute.Registration)
            {
                Order = dependencyAttribute.Order
            };

            // 调用服务描述器过滤器
            if (_typeScanningDependencyBuilder._filterConfigure is null || _typeScanningDependencyBuilder._filterConfigure.Invoke(serviceDescriptorModel))
            {
                yield return serviceDescriptorModel;
            }
        }

        serviceTypes.Clear();
    }

    /// <summary>
    /// 获取有效的服务类型集合
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="limitTypes"><see cref="Type"/>[]</param>
    /// <param name="dependencyType"><see cref="Type"/></param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal List<Type> GetEffectiveServiceTypes(Type type
        , Type[]? limitTypes
        , out Type? dependencyType)
    {
        // 获取类型实现的所有接口
        var interfaces = type.GetInterfaces();

        // 获取类型实现的服务生存期类型
        dependencyType = interfaces.LastOrDefault(i => i.IsAlienAssignableTo(_dependencyType));

        // 查找不在黑名单且在限制类型集合中的服务类型集合
        var serviceTypes = interfaces.Where(t => (_typeScanningDependencyBuilder._blacklistServiceTypes.IsNullOrEmpty() || !_typeScanningDependencyBuilder._blacklistServiceTypes.Any(s => s.IsEqualTypeDefinition(t)))
                                                                    && (limitTypes.IsNullOrEmpty() || limitTypes!.Any(s => s.IsEqualTypeDefinition(t)))
                                                                    && !_dependencyType.IsAssignableFrom(t)
                                                                    && type.IsTypeCompatibilityTo(t))
                                              .Select(t => !type.IsGenericType
                                                                     ? t :
                                                                     t.GetGenericTypeDefinition())
                                              .ToList();

        return serviceTypes;
    }

    /// <summary>
    /// 根据依赖类型获取对应的 <see cref="ServiceLifetime"/>
    /// </summary>
    /// <param name="dependencyType"><see cref="Type"/></param>
    /// <param name="validateLifetime"><see cref="ValidateLifetime"/></param>
    /// <returns><see cref="ServiceLifetime"/></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal static ServiceLifetime? GetServiceLifetime(Type? dependencyType, bool validateLifetime = true)
    {
        return dependencyType switch
        {
            // Transient
            var value when value == typeof(ITransientDependency) => ServiceLifetime.Transient,
            // Scoped
            var value when value == typeof(IScopedDependency) => ServiceLifetime.Scoped,
            // Singleton
            var value when value == typeof(ISingletonDependency) => ServiceLifetime.Singleton,
            _ => throw new NotSupportedException()
        };
    }

    /// <summary>
    /// 将服务描述器添加到 <see cref="IServiceCollection"/> 中
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="serviceDescriptorModel"><see cref="TypeScanningDependencyModel"/></param>
    internal static void AddingToServices(IServiceCollection services, TypeScanningDependencyModel serviceDescriptorModel)
    {
        // 获取服务描述器
        var serviceDescriptor = serviceDescriptorModel.Descriptor;

        // 穷举服务注册方式
        switch (serviceDescriptorModel.Registration)
        {
            // Add
            case RegistrationType.Add:
                services.Add(serviceDescriptor);
                break;
            // TryAdd
            case RegistrationType.TryAdd:
                services.TryAdd(serviceDescriptor);
                break;
            // TryAddEnumerable
            case RegistrationType.TryAddEnumerable:
                services.TryAddEnumerable(serviceDescriptor);
                break;
            // Replace
            case RegistrationType.Replace:
                services.Replace(serviceDescriptor);
                break;

            default: throw new NotSupportedException();
        }
    }
}