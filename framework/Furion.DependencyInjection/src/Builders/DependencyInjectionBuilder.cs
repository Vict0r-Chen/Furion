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
    /// 已标记注册的服务描述器集合
    /// </summary>
    private readonly IList<ServiceDescriptorModel> _serviceDescriptors;

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
    /// 构造函数
    /// </summary>
    public DependencyInjectionBuilder()
    {
        _serviceDescriptors = new List<ServiceDescriptorModel>();
    }

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

        // 检查是否禁用程序集扫描
        if (SuppressAssemblyScanning)
        {
            return;
        }

        // 遍历程序集并创建待注册的服务
        foreach (var assembly in assemblies)
        {
            // 避免重复扫描
            if (!_assemblies.Add(assembly))
            {
                continue;
            }

            // 查找所有实现 IDependency 的类型
            var implementationTypes = assembly.GetTypes()
                                                             .Where(t => (!SuppressNotPublicType ? (t.IsPublic || t.IsNotPublic) : t.IsPublic)
                                                                                   && t.IsInstantiatedTypeWithAssignableFrom(typeof(IDependency)));

            // 遍历所有实现类型并创建服务描述器模型
            foreach (var implementationType in implementationTypes)
            {
                // 获取 [ServiceInjection] 特性
                var serviceInjectionAttribute = implementationType.GetCustomAttributeIfIsDefined<ServiceInjectionAttribute>(true) ?? new();

                // 判断是否配置了 Ignore
                if (serviceInjectionAttribute is { Ignore: true })
                {
                    continue;
                }
            }
        }
    }

    /// <summary>
    /// 构建模块
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    internal void Build(IServiceCollection services)
    {
        // 添加默认启动程序集
        AddAssemblies(Assembly.GetEntryAssembly()!);

        // 将服务描述器进行排序
        var sortedOfServiceDescriptions = _serviceDescriptors.OrderBy(s => s.Descriptor.ServiceType.Name)
                                                                                                .ThenBy(s => s.Order);
        if (!sortedOfServiceDescriptions.Any())
        {
            return;
        }

        // 将服务描述器添加到 IServiceCollection 中
        foreach (var serviceDescriptorModel in sortedOfServiceDescriptions)
        {
            AddToServiceCollection(services, serviceDescriptorModel);
        }
    }

    /// <summary>
    /// 添加服务描述器
    /// </summary>
    /// <param name="serviceDescriptor"><see cref="ServiceDescriptorModel"/></param>
    private void AddServiceDescriptor(ServiceDescriptorModel serviceDescriptor)
    {
        // 调用服务描述器模型过滤配置
        if (FilterConfigure is null || FilterConfigure.Invoke(serviceDescriptor))
        {
            _serviceDescriptors.Add(serviceDescriptor);
        }
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
            // 暂时
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
}