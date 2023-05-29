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
    /// 待注册的服务模型集合
    /// </summary>
    private List<ServiceModel>? _serviceModels = new();

    /// <summary>
    /// 待扫描的程序集
    /// </summary>
    /// <remarks>默认添加启动程序集</remarks>
    private HashSet<Assembly?>? _assemblies = new() { Assembly.GetEntryAssembly() };

    /// <summary>
    /// 待排除的扫描接口
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
    /// 添加待扫描的程序集
    /// </summary>
    /// <param name="assemblies"></param>
    /// <returns><see cref="DependencyInjectionBuilder"/> - 依赖注入服务构建器</returns>
    public DependencyInjectionBuilder AddAssembliesToBeScanned(params Assembly[] assemblies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(assemblies);

        // 批量添加
        Array.ForEach(assemblies, assembly => _assemblies?.Add(assembly));

        return this;
    }

    /// <summary>
    /// 构建器构建
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> - 服务描述器集合</param>
    internal void Build(IServiceCollection services)
    {
        // 写入构建开始诊断日志
        _diagnosticSource.WriteIsEnabled("BuildStart", new
        {
            _serviceModels,
            _assemblies
        });

        // 判断是否禁用程序集扫描
        if (!SuppressAssemblyScanning)
        {
            ScanningAssemblyAndRegisteringServices();
        }

        // 批量注册服务
        if (_serviceModels is not null)
        {
            foreach (var serviceModel in _serviceModels)
            {
                // 解析服务描述器
                var serviceDescriptor = serviceModel.ServiceDescriptor;

                if (serviceModel.ServiceRegister is ServiceRegister.Default or ServiceRegister.TryAddEnumerable)
                {
                    services.TryAddEnumerable(serviceDescriptor);
                }
                else if (serviceModel.ServiceRegister == ServiceRegister.Add)
                {
                    services.Add(serviceDescriptor);
                }
                else if (serviceModel.ServiceRegister == ServiceRegister.TryAdd)
                {
                    services.TryAdd(serviceDescriptor);
                }
                else { }
            }
        }

        // 回收（释放）集合对象
        Recycling();
    }

    /// <summary>
    /// 扫描程序集并登录待注册的服务
    /// </summary>
    private void ScanningAssemblyAndRegisteringServices()
    {
        if (_assemblies is null) return;

        // 声明服务接口生存期判断委托
        var lifetimeAction = (Type i) => typeof(ILifetimeDependency).IsAssignableFrom(i);

        foreach (var assembly in _assemblies)
        {
            if (assembly is null) continue;

            // 查找所有类（非接口、非静态类、非抽象类、非值类型或枚举）且实现 ILifetimeDependency 接口
            var serviceTypes = assembly.GetTypes()
                                                      .Where(t => !t.IsAbstract
                                                                            && !t.IsStatic()
                                                                            && t.IsClass
                                                                            && lifetimeAction(t));

            // 遍历类型并创建 ServiceDescriptor 服务描述器类型
            if (serviceTypes is null) continue;
            foreach (var serviceType in serviceTypes)
            {
                // 获取 [ServiceInjection] 特性
                var serviceInjectionAttribute = serviceType.GetCustomAttribute<ServiceInjectionAttribute>(true) ?? new ServiceInjectionAttribute();

                // 跳过配置了 Ignore 属性
                if (serviceInjectionAttribute is { Ignore: true }) continue;

                // 查找所有排除特定接口的接口集合
                var interfaces = serviceType.GetInterfaces().Where(i => _excludeInterfaces?.Contains(i) == false);
                if (interfaces is null) continue;

                // 获取注册服务生存器类型
                var lifetimeDependency = interfaces.Single(lifetimeAction);

                // 获取服务注册生存期
                var lifetime = GetServiceLifetime(lifetimeDependency);

                // 如果类型只有一个接口，则直接注册类型
                if (interfaces.LongCount() == 1)
                {
                    _serviceModels?.Add(new ServiceModel
                    {
                        ServiceDescriptor = ServiceDescriptor.Describe(serviceType, serviceType, lifetime),
                        ServiceRegister = serviceInjectionAttribute.ServiceRegister
                    });
                }
                else
                {
                    // 将每一个接口进行注册
                    foreach (var interType in interfaces)
                    {
                        if (lifetimeAction(interType)) continue;

                        _serviceModels?.Add(new ServiceModel
                        {
                            ServiceDescriptor = ServiceDescriptor.Describe(interType, serviceType, lifetime),
                            ServiceRegister = serviceInjectionAttribute.ServiceRegister
                        });
                    }
                }

                // 注册基类类型，如果积累存在那么必须是公开类型
                var baseType = serviceType.BaseType;
                if (baseType is null || baseType.IsNotPublic) continue;

                _serviceModels?.Add(new ServiceModel
                {
                    ServiceDescriptor = ServiceDescriptor.Describe(baseType, serviceType, lifetime),
                    ServiceRegister = serviceInjectionAttribute.ServiceRegister
                });
            }
        }
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

        if (lifetimeDependency == typeof(ITransientDependency))
        {
            return ServiceLifetime.Transient;
        }
        else if (lifetimeDependency == typeof(IScopedDependency))
        {
            return ServiceLifetime.Scoped;
        }
        else if (lifetimeDependency == typeof(ISingletonDependency))
        {
            return ServiceLifetime.Singleton;
        }
        else
        {
            throw new InvalidOperationException("Not supported service lifetime interface.");
        }
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
}