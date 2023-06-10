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

using System.Xml.Linq;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 组件化模块拓展
/// </summary>
public static class ComponentServiceCollectionExtensions
{
    /// <summary>
    /// 添加组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="ComponentBase"/></typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="configure">自定义构建器配置</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponent<TComponent>(this IServiceCollection services, IConfiguration configuration, Action<ComponentBuilder>? configure = null)
        where TComponent : ComponentBase, new()
    {
        return services.AddComponent(typeof(TComponent), configuration, configure);
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="ComponentBase"/></typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="componentBuilder"><see cref="ComponentBuilder"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponent<TComponent>(this IServiceCollection services, IConfiguration configuration, ComponentBuilder componentBuilder)
        where TComponent : ComponentBase, new()
    {
        return services.AddComponent(typeof(TComponent), configuration, componentBuilder);
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="configure">自定义构建器配置</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponent(this IServiceCollection services, Type componentType, IConfiguration configuration, Action<ComponentBuilder>? configure = null)
    {
        // 生成组件依赖字典
        var dependencies = ComponentBase.GenerateDependencyMap(componentType);

        return services.AddComponent(dependencies, configuration, configure);
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="componentBuilder"><see cref="ComponentBuilder"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponent(this IServiceCollection services, Type componentType, IConfiguration configuration, ComponentBuilder componentBuilder)
    {
        // 生成组件依赖字典
        var dependencies = ComponentBase.GenerateDependencyMap(componentType);

        return services.AddComponent(dependencies, configuration, componentBuilder);
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="dependencies">组件依赖字典</param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="configure">自定义构建器配置</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponent(this IServiceCollection services, Dictionary<Type, Type[]> dependencies, IConfiguration configuration, Action<ComponentBuilder>? configure = null)
    {
        // 创建组件模块构建器
        var componentBuilder = new ComponentBuilder();

        // 调用自定义配置
        configure?.Invoke(componentBuilder);

        return services.AddComponent(dependencies, configuration, componentBuilder);
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="dependencies">组件依赖字典</param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="componentBuilder"><see cref="ComponentBuilder"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponent(this IServiceCollection services, Dictionary<Type, Type[]> dependencies, IConfiguration configuration, ComponentBuilder componentBuilder)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentBuilder, nameof(componentBuilder));

        // 构建组件模块
        componentBuilder.Build(services);

        // 添加组件配置选项
        services.TryAddSingleton(new ComponentOptions());

        // 获取环境对象
        var environment = services.GetHostEnvironment();

        // 创建上下文
        var serviceContext = new ServiceContext(services, configuration);

        // 生成组件依赖拓扑图
        var topologicalMap = ComponentBase.GenerateTopologicalMap(dependencies);

        // 获取组件化配置选项
        var componentOptions = services.GetComponentOptions();

        // 组件对象集合
        var components = new List<ComponentBase>();

        // 依次初始化组件实例
        foreach (var node in topologicalMap)
        {
            var component = Activator.CreateInstance(node) as ComponentBase;
            ArgumentNullException.ThrowIfNull(component, nameof(component));

            component.Options = componentOptions;
            components.Add(component);
        }

        // 打印组件依赖链
        Debug.WriteLine(string.Join(" ← ", components.Select(c => c.GetType().Name)));

        // 调用前置配置服务
        components.ForEach(component => component.PreConfigureServices(serviceContext));

        // 调用配置服务
        components.ForEach(component => component.ConfigureServices(serviceContext));

        return services;
    }

    /// <summary>
    /// 获取环境对象
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IHostEnvironment"/></returns>
    internal static IHostEnvironment? GetHostEnvironment(this IServiceCollection services)
    {
        // 查找 Web 主机环境是否配置
        var webHostEnvironment = services.FirstOrDefault(s => s.ServiceType.FullName == "Microsoft.AspNetCore.Hosting.IWebHostEnvironment")
                                                ?.ImplementationInstance as IHostEnvironment;

        // 如果没配置则查找泛型主机环境是否配置
        var hostEnvironment = webHostEnvironment ?? services.FirstOrDefault(s => s.ServiceType == typeof(IHostEnvironment))
                                               ?.ImplementationInstance as IHostEnvironment;

        return hostEnvironment;
    }

    /// <summary>
    /// 获取单例服务对象
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <returns><typeparamref name="T"/></returns>
    public static T GetSingletonInstance<T>(this IServiceCollection services, T? instance = null)
        where T : class
    {
        // 如果组件配置选项不存在则添加
        if (!services.Any(s => s.ServiceType == typeof(T)))
        {
            services.TryAddSingleton(instance ?? Activator.CreateInstance<T>());
        }

        // 获取组件配置选项
        var singletonInstance = services.First(s => s.ServiceType == typeof(T)).ImplementationInstance as T;

        // 空检查
        ArgumentNullException.ThrowIfNull(singletonInstance, nameof(singletonInstance));

        return singletonInstance;
    }

    /// <summary>
    /// 获取组件配置选项
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="ComponentOptions"/></returns>
    internal static ComponentOptions GetComponentOptions(this IServiceCollection services)
    {
        return services.GetSingletonInstance<ComponentOptions>();
    }
}