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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Furion.DependencyInjection;

/// <summary>
/// 依赖注入服务构建器
/// </summary>
public sealed partial class DependencyInjectionBuilder
{
    /// <summary>
    /// 已添加的服务模型集合
    /// </summary>
    private HashSet<ServiceModel>? _serviceModels = new();

    /// <summary>
    /// 已添加扫描的程序集集合
    /// </summary>
    private HashSet<Assembly?>? _assemblies = new();

    /// <summary>
    /// 已排除的非服务接口集合
    /// </summary>
    private HashSet<Type>? _excludeInterfaces = new()
    {
        typeof(IDisposable), typeof(IAsyncDisposable),
        typeof(ILifetimeDependency),
        typeof(IEnumerator)
    };

    /// <summary>
    /// 禁用程序集扫描
    /// </summary>
    /// <remarks>默认 false：不禁用</remarks>
    public bool SuppressAssemblyScanning { get; set; } = false;

    /// <summary>
    /// 服务模型筛选器
    /// </summary>
    public Func<ServiceModel, bool>? FilterConfigure { get; set; }

    /// <summary>
    /// 扫描程序集并添加到服务模型集合中
    /// </summary>
    /// <param name="assemblies">程序集</param>
    public void AddAssemblies(params Assembly?[] assemblies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(assemblies);

        // 遍历程序集并创建服务模型集合
        foreach (var assembly in assemblies)
        {
            if (assembly is null || _assemblies?.Add(assembly) == false) continue;

            // 查找所有类型（非接口、非静态类、非抽象类、非值类型或枚举）且实现 ILifetimeDependency 接口
            var implementationTypes = assembly.GetTypes()
                                                             .Where(t => !t.IsAbstract
                                                                                   && !t.IsStatic()
                                                                                   && t.IsClass
                                                                                   && CheckIsAssignableFromILifetimeDependency(t));

            // 遍历所有实现类型并创建服务模型
            foreach (var implementationType in implementationTypes)
            {
                // 获取 [ServiceInjection] 特性
                var serviceInjectionAttribute = implementationType.GetCustomAttribute<ServiceInjectionAttribute>(true) ?? new();

                // 配置 Ignore 属性则跳过
                if (serviceInjectionAttribute is { Ignore: true }) continue;

                // 获取所有匹配的服务类型
                var (typeDefinition, inheritTypes, lifetimeDependencyType) = implementationType.GetMatchInheritTypes(
                    _excludeInterfaces?.Concat(serviceInjectionAttribute.ExcludeServiceTypes ?? Array.Empty<Type>()));

                // 获取服务生存期
                var serviceLifetime = GetServiceLifetime(lifetimeDependencyType);

                // 创建服务模型
                foreach (var serviceType in inheritTypes)
                {
                    var serviceModel = new ServiceModel(serviceType
                        , typeDefinition
                        , serviceLifetime
                        , serviceInjectionAttribute.ServiceRegister);

                    // 调用服务模型过滤委托
                    if (FilterConfigure is null || FilterConfigure(serviceModel)) _serviceModels?.Add(serviceModel);
                }

                // 注册自身
                if (serviceInjectionAttribute is { IncludingSelf: true })
                {
                    var serviceModel = new ServiceModel(typeDefinition
                        , typeDefinition
                        , serviceLifetime);

                    // 调用服务模型过滤委托
                    if (FilterConfigure is null || FilterConfigure(serviceModel)) _serviceModels?.Add(serviceModel);
                }
            }
        }
    }

    /// <summary>
    /// 构建器构建
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> - 服务描述器集合</param>
    internal void Build(IServiceCollection services)
    {
        // 添加默认启动程序集扫描
        if (!SuppressAssemblyScanning)
        {
            AddAssemblies(Assembly.GetEntryAssembly());
        }

        // 过滤已经注册的服务模型
        var serviceModels = _serviceModels?.Where(m => m.CanRegister(services)) ?? Array.Empty<ServiceModel>();

        // 写入构建开始诊断日志
        _diagnosticSource.WriteIsEnabled("BuildStart", new
        {
            serviceModels,
            _assemblies
        });

        // 遍历所有服务模型并注册服务
        foreach (var serviceModel in serviceModels)
        {
            // 解析服务描述器
            var serviceDescriptor = serviceModel.ServiceDescriptor;

            // TryAddEnumerable
            if (serviceModel.ServiceRegister is ServiceRegister.Default or ServiceRegister.TryAddEnumerable)
            {
                services.TryAddEnumerable(serviceDescriptor);
            }
            // Add
            else if (serviceModel.ServiceRegister == ServiceRegister.Add)
            {
                services.Add(serviceDescriptor);
            }
            // TryAdd
            else if (serviceModel.ServiceRegister == ServiceRegister.TryAdd)
            {
                services.TryAdd(serviceDescriptor);
            }
            else { }
        }

        // 回收（释放）集合对象
        Recycling();
    }

    /// <summary>
    /// 根据 <see cref="ILifetimeDependency"/> 派生类型获取服务生存期
    /// </summary>
    /// <param name="lifetimeDependency">服务生存期依赖服务接口类型</param>
    /// <returns><see cref="ServiceLifetime"/> 服务生存期</returns>
    /// <exception cref="InvalidOperationException">不受支持的服务生存期接口类型</exception>
    private static ServiceLifetime GetServiceLifetime(Type? lifetimeDependency)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(lifetimeDependency);

        // 返回服务生存期枚举对象
        return lifetimeDependency switch
        {
            // 暂时
            var value when value == typeof(ITransientDependency) => ServiceLifetime.Transient,
            // 范围
            var value when value == typeof(IScopedDependency) => ServiceLifetime.Scoped,
            // 单例
            var value when value == typeof(ISingletonDependency) => ServiceLifetime.Singleton,
            // 不受支持服务生存期
            _ => throw new InvalidOperationException("Not supported service lifetime interface.")
        };
    }

    /// <summary>
    /// 回收（释放）集合对象
    /// </summary>
    private void Recycling()
    {
        _serviceModels?.Clear();
        _assemblies?.Clear();
        _excludeInterfaces?.Clear();

        _serviceModels = null;
        _assemblies = null;
        _excludeInterfaces = null;
    }

    /// <summary>
    /// 查找实现 <see cref="ILifetimeDependency"/> 服务生存期的接口
    /// </summary>
    internal static readonly Func<Type, bool> CheckIsAssignableFromILifetimeDependency = i => i != typeof(ILifetimeDependency) && typeof(ILifetimeDependency).IsAssignableFrom(i);
}