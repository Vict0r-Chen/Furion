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

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 组件模块 <see cref="IServiceCollection"/> 拓展类
/// </summary>
public static class ComponentServiceCollectionExtensions
{
    /// <summary>
    /// 添加组件模块服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponentCore(this IServiceCollection services, Action<ComponentBuilder>? configure = null)
    {
        // 初始化组件模块构建器
        var componentBuilder = new ComponentBuilder();

        // 调用自定义配置委托
        configure?.Invoke(componentBuilder);

        return services.AddComponentCore(componentBuilder);
    }

    /// <summary>
    /// 添加组件模块服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="componentBuilder"><see cref="ComponentBuilder"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponentCore(this IServiceCollection services, ComponentBuilder componentBuilder)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentBuilder, nameof(componentBuilder));

        // 添加核心模块选项服务
        services.AddCoreOptions();

        // 构建模块服务
        componentBuilder.Build(services);

        return services;
    }

    /// <summary>
    /// 添加服务组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="ComponentBase"/></typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponent<TComponent>(this IServiceCollection services, IConfiguration configuration, Action<ComponentBuilderBase>? configure = null)
        where TComponent : ComponentBase
    {
        return services.AddComponent(typeof(TComponent), configuration, configure);
    }

    /// <summary>
    /// 添加服务组件
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponent(this IServiceCollection services, Type componentType, IConfiguration configuration, Action<ComponentBuilderBase>? configure = null)
    {
        // 创建组件依赖关系集合
        var dependencies = ComponentBase.CreateDependencies(componentType);

        return services.AddComponent(dependencies, configuration, configure);
    }

    /// <summary>
    /// 添加服务组件
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="dependencies">组件依赖关系集合</param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponent(this IServiceCollection services, Dictionary<Type, Type[]> dependencies, IConfiguration configuration, Action<ComponentBuilderBase>? configure = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(dependencies, nameof(dependencies));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        // 空检查
        if (dependencies.Count == 0)
        {
            return services;
        }

        // 创建组件拓扑排序集合
        var topologicalSets = ComponentBase.CreateTopological(dependencies);

        // 创建组件模块构建器同时调用自定义配置委托
        var componentBuilder = new ComponentBuilderBase();
        configure?.Invoke(componentBuilder);
        componentBuilder.Build(services);

        // 创建组件上下文
        var componentContext = new ServiceComponentContext(services, configuration);

        // 创建组件依赖关系对象集合
        var components = CreateComponents(topologicalSets, componentContext);

        // 调用配置服务
        components.ForEach(component =>
        {
            // 输出调试事件
            Debugging.Trace("`{0}.{1}` method has been called.", component.GetType(), nameof(ComponentBase.ConfigureServices));

            component.ConfigureServices(componentContext);
        });

        components.Clear();

        return services;
    }

    /// <summary>
    /// 创建组件依赖关系对象集合
    /// </summary>
    /// <param name="topologicalSets">组件拓扑排序集合</param>
    /// <param name="componentContext"><see cref="ServiceComponentContext"/></param>
    /// <returns><see cref="List{T}"/></returns>
    internal static List<ComponentBase> CreateComponents(List<Type> topologicalSets, ServiceComponentContext componentContext)
    {
        // 组件依赖关系对象集合
        var components = new List<ComponentBase>();

        // 获取组件模块配置选项
        var componentOptions = componentContext.Options;

        // 从尾部依次初始化组件实例
        for (var i = topologicalSets.Count - 1; i >= 0; i--)
        {
            var componentType = topologicalSets[i];

            // 组件重复调用检测
            var recordName = componentType.FullName + " (Type 'ComponentBase')";
            if (componentOptions.SuppressDuplicateCall)
            {
                if (componentOptions.CallRecords.Any(t => t == recordName))
                {
                    // 输出调试事件
                    Debugging.Warn("`{0}` component has been prevented from duplicate invocation.", componentType.Name);
                    continue;
                }

                componentOptions.CallRecords.Add(recordName);
            }

            // 创建组件实例
            var component = ComponentBase.CreateInstance(componentType, componentOptions);
            components.Insert(0, component);

            // 调用前置配置服务
            component.PreConfigureServices(componentContext);

            // 输出调试事件
            Debugging.Trace("`{0}.{1}` method has been called.", component.GetType(), nameof(ComponentBase.PreConfigureServices));
        }

        return components;
    }

    /// <summary>
    /// 获取环境对象
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IHostEnvironment"/></returns>
    internal static IHostEnvironment? GetHostEnvironment(this IServiceCollection services)
    {
        // 查找 Web 主机环境是否配置
        var hostEnvironment = (services.FirstOrDefault(s => s.ServiceType.FullName == IWEBHOSTENVIRONMENT_TYPE_FULLNAME)?.ImplementationInstance
                                                ?? services.FirstOrDefault(s => s.ServiceType == typeof(IHostEnvironment))?.ImplementationInstance) as IHostEnvironment;
        return hostEnvironment;
    }

    /// <summary>
    /// 获取组件模块配置选项
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="ComponentOptions"/></returns>
    internal static ComponentOptions GetComponentOptions(this IServiceCollection services)
    {
        return services.GetCoreOptions().Get<ComponentOptions>();
    }

    /// <summary>
    /// IWebHostEnvironment 类型限定名
    /// </summary>
    internal const string IWEBHOSTENVIRONMENT_TYPE_FULLNAME = "Microsoft.AspNetCore.Hosting.IWebHostEnvironment";
}