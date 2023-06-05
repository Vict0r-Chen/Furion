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
    /// 已扫描的程序集
    /// </summary>
    private readonly HashSet<Assembly> _assemblies = new();

    /// <summary>
    /// 禁用指定派生类型作为服务注册
    /// </summary>
    private readonly HashSet<Type> _suppressDerivedTypes = new()
    {
        typeof(IDisposable), typeof(IAsyncDisposable),
        typeof(IDependency), typeof(IEnumerator),
        typeof(IEnumerable), typeof(ICollection),
        typeof(IDictionary), typeof(IComparable),
        typeof(object), typeof(DynamicObject)
    };

    /// <summary>
    /// 禁用程序集扫描
    /// </summary>
    public bool SuppressAssemblyScanning { get; set; }

    /// <summary>
    /// 禁用非公开类型
    /// </summary>
    public bool SuppressNotPublicType { get; set; }

    /// <summary>
    /// 服务描述器模型过滤配置
    /// </summary>
    /// <remarks>可过滤是否将服务描述器添加到 <see cref="IServiceCollection"/> 中</remarks>
    public Func<ServiceDescriptorModel, bool>? FilterConfigure { get; set; }

    /// <summary>
    /// 追加程序集扫描
    /// </summary>
    /// <param name="assemblies">可变数量程序集</param>
    public void AddAssemblies(params Assembly[] assemblies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(assemblies);

        Array.ForEach(assemblies, ass => _assemblies.Add(ass));
    }

    /// <summary>
    /// 构建模块
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    internal void Build(IServiceCollection services)
    {
        // 扫描程序集创建服务描述器集合
        var serviceDescriptors = CreateServiceDescriptors();

        // 空检查
        if (!serviceDescriptors.Any())
        {
            // 清空集合
            ClearAll();

            return;
        }

        // 将服务描述器进行排序
        var sortedOfServiceDescriptors = serviceDescriptors.OrderBy(s => s.Descriptor.ServiceType.Name)
                                                                                              .ThenBy(s => s.Order);

        // 日志事件记录
        DependencyInjectionEventSource.Log.BuildStarted();

        // 将服务描述器添加到 IServiceCollection 中
        foreach (var serviceDescriptorModel in sortedOfServiceDescriptors)
        {
            AddToServiceCollection(services, serviceDescriptorModel);
        }

        // 清空集合
        ClearAll();
    }

    /// <summary>
    /// 清空集合
    /// </summary>
    private void ClearAll()
    {
        // 清空集合
        _assemblies.Clear();
        _suppressDerivedTypes.Clear();
    }

    /// <summary>
    /// 扫描程序集创建服务描述器集合
    /// </summary>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    private IEnumerable<ServiceDescriptorModel> CreateServiceDescriptors()
    {
        // 检查是否禁用程序集扫描
        if (SuppressAssemblyScanning)
        {
            return Array.Empty<ServiceDescriptorModel>();
        }

        var serviceDescriptors = new List<ServiceDescriptorModel>();

        // 遍历所有程序集创建服务描述器集合
        foreach (var assembly in _assemblies)
        {
            // 查找所有实现 IDependency 的类型
            var exportedTypes = assembly.GetTypes()
                                                       .Where(t => (!SuppressNotPublicType ? (t.IsPublic || t.IsNotPublic) : t.IsPublic)
                                                                             && t.IsInstantiatedTypeWithAssignableFrom(typeof(IDependency)));

            // 空检查
            if (!exportedTypes.Any())
            {
                continue;
            }

            // 遍历所有实现类型创建服务描述器模型
            foreach (var exportedType in exportedTypes)
            {
                // 获取 [ServiceInjection] 特性
                var serviceInjectionAttribute = exportedType.GetCustomAttributeIfIsDefined<ServiceInjectionAttribute>(true) ?? new();

                // 判断是否配置了 Ignore
                if (serviceInjectionAttribute is { Ignore: true })
                {
                    continue;
                }

                // 获取服务类型集合
                var serviceTypes = GetServiceTypes(exportedType
                    , serviceInjectionAttribute
                    , out var implementationType
                    , out var lifetimeDependencyType);

                // 获取服务生存期
                var serviceLifetime = GetServiceLifetime(lifetimeDependencyType);

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

                    // 调用服务描述器模型过滤配置
                    if (FilterConfigure is null || FilterConfigure.Invoke(serviceDescriptorModel))
                    {
                        serviceDescriptors.Add(serviceDescriptorModel);
                    }
                }
            }
        }

        return serviceDescriptors;
    }

    /// <summary>
    /// 根据 <see cref="IDependency"/> 派生类型获取对应的服务生存期
    /// </summary>
    /// <param name="lifetimeDependencyType"><see cref="IDependency"/> 派生类型</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static ServiceLifetime GetServiceLifetime(Type lifetimeDependencyType)
    {
        // 返回服务生存期枚举对象
        return lifetimeDependencyType switch
        {
            // 瞬时
            var value when value == typeof(ITransientDependency) => ServiceLifetime.Transient,
            // 范围
            var value when value == typeof(IScopedDependency) => ServiceLifetime.Scoped,
            // 单例
            var value when value == typeof(ISingletonDependency) => ServiceLifetime.Singleton,
            // 不受支持服务生存期
            _ => throw new ArgumentOutOfRangeException(nameof(lifetimeDependencyType), "Not supported service lifetime interface.")
        };
    }

    /// <summary>
    /// 将服务描述器添加到 <see cref="IServiceCollection"/> 中
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="serviceDescriptorModel">服务描述器模型</param>
    private static void AddToServiceCollection(IServiceCollection services, ServiceDescriptorModel serviceDescriptorModel)
    {
        var serviceDescriptor = serviceDescriptorModel.Descriptor;

        // services.Add
        if (serviceDescriptorModel.Addition is ServiceAddition.Add or ServiceAddition.Default)
        {
            services.Add(serviceDescriptor);
        }
        // services.TryAdd
        else if (serviceDescriptorModel.Addition is ServiceAddition.TryAdd)
        {
            services.TryAdd(serviceDescriptor);
        }
        // services.TryAddEnumerable
        else if (serviceDescriptorModel.Addition is ServiceAddition.TryAddEnumerable)
        {
            services.TryAddEnumerable(serviceDescriptor);
        }
        // services.Replace
        else if (serviceDescriptorModel.Addition is ServiceAddition.Replace)
        {
            services.Replace(serviceDescriptor);
        }
        else { }
    }

    /// <summary>
    /// 获取类型的服务类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <param name="serviceInjectionAttribute"><see cref="ServiceInjectionAttribute"/></param>
    /// <param name="implementationType">实现类型</param>
    /// <param name="lifetimeDependencyType">服务生存期类型</param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    private IEnumerable<Type> GetServiceTypes(Type type
        , ServiceInjectionAttribute serviceInjectionAttribute
        , out Type implementationType
        , out Type lifetimeDependencyType)
    {
        // 获取类型定义
        implementationType = type.IsGenericType
                               ? type.GetGenericTypeDefinition()
                               : type;

        // 获取类型实现的所有接口
        var allInterfaces = type.GetInterfaces();

        // 解析服务生存期类型
        var dependencyType = typeof(IDependency);
        lifetimeDependencyType = allInterfaces.Single(i => i != dependencyType
                                                                          && dependencyType.IsAssignableFrom(i));

        // 获取基类类型
        var baseType = !serviceInjectionAttribute.IncludingBase
                                || type.BaseType is null
                                || type.BaseType == typeof(object)
                                || type.BaseType.IsNotPublic
                                || (type.IsGenericType && !type.BaseType.IsGenericType)
                            ? null
                            : type.BaseType;

        // 过滤无效服务类型
        var suppressDerivedTypes = _suppressDerivedTypes.Concat(serviceInjectionAttribute.SuppressDerivedTypes ?? Array.Empty<Type>());
        var filteredOfServiceTypes = allInterfaces.Concat(new[] { baseType })
                                                                 .Where(t => t is not null
                                                                                        && !suppressDerivedTypes.Contains(t)
                                                                                        && !dependencyType.IsAssignableFrom(t))
                                                                 .Select(t => t!);

        // 获取类型定义参数
        var typeDefinitionParameters = type.GetTypeInfo().GenericTypeParameters;

        // 获取服务类型集合
        var serviceTypes = !type.IsGenericType
                                             ? filteredOfServiceTypes
                                             : filteredOfServiceTypes.Where(i => i.IsGenericType
                                                                                           && i.GenericTypeArguments.Length == typeDefinitionParameters.Length
                                                                                           && i.GenericTypeArguments.SequenceEqual(typeDefinitionParameters))
                                                                     .Select(i => i.GetGenericTypeDefinition());

        // 判断是否将自身作为服务类型
        if (!serviceInjectionAttribute.IncludingSelf
            && serviceTypes.Any())
        {
            return serviceTypes;
        }

        return serviceTypes.Concat(new[] { implementationType });
    }
}