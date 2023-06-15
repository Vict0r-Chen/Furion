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
/// 依赖注入模块构建器
/// </summary>
public sealed class DependencyInjectionBuilder
{
    /// <summary>
    /// 程序集扫描类型依赖接口
    /// </summary>
    internal readonly Type _dependencyType;

    /// <summary>
    /// 待扫描的程序集集合
    /// </summary>
    internal readonly HashSet<Assembly> _assemblies;

    /// <summary>
    /// 服务类型黑名单
    /// </summary>
    /// <remarks>禁止已配置的服务类型作为服务注册</remarks>
    internal readonly HashSet<Type> _serviceTypeBlacklist;

    /// <summary>
    /// 服务描述器过滤器
    /// </summary>
    internal Func<ServiceDescriptorModel, bool>? _filterConfigure;

    /// <summary>
    /// 构造函数
    /// </summary>
    public DependencyInjectionBuilder()
    {
        _dependencyType = typeof(IDependency);

        // 初始化并添加默认启动程序集
        _assemblies = new();
        AddAssemblies(Assembly.GetEntryAssembly()!);

        // 初始化服务类型黑名单
        _serviceTypeBlacklist = new()
        {
            typeof(IDisposable), typeof(IAsyncDisposable),
            typeof(IDependency), typeof(IEnumerator),
            typeof(IEnumerable), typeof(ICollection),
            typeof(IDictionary), typeof(IComparable),
            typeof(object), typeof(DynamicObject)
        };
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
    /// 添加服务描述器过滤器
    /// </summary>
    /// <param name="configure"><see cref="Func{T, TResult}"/></param>
    public void AddFilter(Func<ServiceDescriptorModel, bool> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));

        _filterConfigure = configure;
    }

    /// <summary>
    /// 添加待扫描的程序集
    /// </summary>
    /// <param name="assemblies"><see cref="Assembly"/>[]</param>
    /// <returns><see cref="DependencyInjectionBuilder"/></returns>
    public DependencyInjectionBuilder AddAssemblies(params Assembly[] assemblies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(assemblies, nameof(assemblies));

        Array.ForEach(assemblies, assembly =>
        {
            if (!_assemblies.Add(assembly))
            {
                return;
            }

            // 输出事件消息
            Debugging.Info("{0} program assembly has been added successfully.", assembly);
        });

        return this;
    }

    /// <summary>
    /// 构建模块
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    internal void Build(IServiceCollection services)
    {
        // 是否禁用程序集扫描
        if (SuppressAssemblyScanning)
        {
            return;
        }

        // 扫描程序集创建服务描述器模型集合
        var serviceDescriptors = CreateServiceDescriptors();

        // 空检查
        if (!serviceDescriptors.Any())
        {
            return;
        }

        // 将服务描述器添加到 IServiceCollection 中
        foreach (var serviceDescriptorModel in serviceDescriptors)
        {
            AddingToServices(services, serviceDescriptorModel);
        }
    }

    /// <summary>
    /// 扫描程序集创建服务描述器模型集合
    /// </summary>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal IEnumerable<ServiceDescriptorModel> CreateServiceDescriptors()
    {
        // 查找所有符合规则的类型
        var effectiveTypes = _assemblies.SelectMany(assembly =>
        {
            return assembly.GetTypes(SuppressNonPublicType)
                           .Where(t => t.IsAlienAssignableTo(_dependencyType)
                                                 && t.IsInstantiable());
        });

        // 空检查
        if (!effectiveTypes.Any())
        {
            return Array.Empty<ServiceDescriptorModel>();
        }

        // 创建服务描述器模型集合
        var serviceDescriptors = new List<ServiceDescriptorModel>();

        // 遍历符合规则的类型并创建服务描述器模型
        foreach (var type in effectiveTypes)
        {
            // 获取 [ServiceInjection] 特性
            var serviceInjectionAttribute = type.GetDefinedCustomAttributeOrNew<ServiceInjectionAttribute>(true);

            // 是否配置了 Ignore 属性
            if (serviceInjectionAttribute is { Ignore: true })
            {
                continue;
            }

            // 获取 [ExposeServices] 特性
            var exposeServicesAttribute = type.GetDefinedCustomAttributeOrNew<ExposeServicesAttribute>(true);

            // 获取类型兼容的且已配置导出的服务类型集合
            var serviceTypes = GetCompatibilityServiceTypes(type, out var dependencyType)
                                            .Where(t => exposeServicesAttribute.ServiceTypes
                                                                  .Any(s => s.IsEqualTypeDefinition(t)))
                                            .ToList();

            // 是否包含基类
            var baseType = type.BaseType;
            if (baseType is not null
                && serviceInjectionAttribute is { IncludeBase: true }
                && type.IsTypeCompatibilityTo(baseType))
            {
                serviceTypes.Add(!type.IsGenericType
                    ? baseType
                    : baseType.GetGenericTypeDefinition());
            }

            // 获取实现类型
            var implementationType = !type.IsGenericType
                ? type
                : type.GetGenericTypeDefinition();

            // 是否包含自身
            if (serviceInjectionAttribute is { IncludeSelf: true }
                || serviceTypes.Count == 0)
            {
                serviceTypes.Add(implementationType);
            }

            // 获取服务生存期
            var serviceLifetime = GetServiceLifetime(dependencyType);

            // 遍历服务类型并创建服务描述器模型添加到集合中
            foreach (var serviceType in serviceTypes)
            {
                // 创建服务描述器模型
                var serviceDescriptorModel = new ServiceDescriptorModel(serviceType
                    , implementationType
                    , serviceLifetime
                    , serviceInjectionAttribute.Addition)
                {
                    Order = serviceInjectionAttribute.Order
                };

                // 调用服务描述器过滤器
                if (_filterConfigure is null || _filterConfigure.Invoke(serviceDescriptorModel))
                {
                    serviceDescriptors.Add(serviceDescriptorModel);
                }
            }
        }

        // 对服务描述器模型集合进行排序
        return serviceDescriptors.OrderBy(s => s.Descriptor.ServiceType.Name)
                                 .ThenBy(s => s.Order)
                                 .ToList();
    }

    /// <summary>
    /// 获取类型兼容的服务类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="dependencyType"><see cref="Type"/></param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal IEnumerable<Type> GetCompatibilityServiceTypes(Type type, out Type dependencyType)
    {
        // 获取类型实现的所有接口
        var allInterfaces = type.GetInterfaces();

        // 获取类型实现的服务生存期类型
        dependencyType = allInterfaces.Last(i => i.IsAlienAssignableTo(_dependencyType));

        // 过滤不兼容的服务类型
        var serviceTypes = allInterfaces.Where(t => !_dependencyType.IsAssignableFrom(t)
                                                                             && type.IsTypeCompatibilityTo(t))
                                                       .Select(t => !type.IsGenericType
                                                                              ? t :
                                                                              t.GetGenericTypeDefinition());

        return serviceTypes;
    }

    /// <summary>
    /// 根据依赖类型获取对应的 <see cref="ServiceLifetime"/>
    /// </summary>
    /// <param name="dependencyType"><see cref="Type"/></param>
    /// <returns><see cref="ServiceLifetime"/></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal static ServiceLifetime GetServiceLifetime(Type dependencyType)
    {
        return dependencyType switch
        {
            // Transient
            var value when value == typeof(ITransientDependency) => ServiceLifetime.Transient,
            // Scoped
            var value when value == typeof(IScopedDependency) => ServiceLifetime.Scoped,
            // Singleton
            var value when value == typeof(ISingletonDependency) => ServiceLifetime.Singleton,
            // 无效类型
            _ => throw new ArgumentOutOfRangeException(nameof(dependencyType), $"{dependencyType} type is not a valid service lifetime type.")
        };
    }

    /// <summary>
    /// 将服务描述器添加到 <see cref="IServiceCollection"/> 中
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="serviceDescriptorModel"><see cref="ServiceDescriptorModel"/></param>
    internal static void AddingToServices(IServiceCollection services, ServiceDescriptorModel serviceDescriptorModel)
    {
        var serviceDescriptor = serviceDescriptorModel.Descriptor;

        // Add
        if (serviceDescriptorModel.Addition is ServiceAddition.Add or ServiceAddition.Default)
        {
            services.Add(serviceDescriptor);
        }
        // TryAdd
        else if (serviceDescriptorModel.Addition is ServiceAddition.TryAdd)
        {
            services.TryAdd(serviceDescriptor);
        }
        // TryAddEnumerable
        else if (serviceDescriptorModel.Addition is ServiceAddition.TryAddEnumerable)
        {
            services.TryAddEnumerable(serviceDescriptor);
        }
        // Replace
        else if (serviceDescriptorModel.Addition is ServiceAddition.Replace)
        {
            services.Replace(serviceDescriptor);
        }
        // 无效操作
        else
        {
            throw new InvalidOperationException($"{serviceDescriptorModel.Addition}.");
        }
    }
}