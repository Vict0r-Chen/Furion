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

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// 组件化模块拓展
/// </summary>
public static class ComponentApplicationBuilderExtensions
{
    /// <summary>
    /// 添加组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="Component"/></typeparam>
    /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="configure">自定义构建器配置</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IApplicationBuilder AddComponent<TComponent>(this IApplicationBuilder applicationBuilder, IConfiguration configuration, Action<WebComponentBuilder>? configure = null)
        where TComponent : WebComponent, new()
    {
        return applicationBuilder.AddComponent(typeof(TComponent), configuration, configure);
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="Component"/></typeparam>
    /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="componentBuilder"><see cref="WebComponentBuilder"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IApplicationBuilder AddComponent<TComponent>(this IApplicationBuilder applicationBuilder, IConfiguration configuration, WebComponentBuilder componentBuilder)
        where TComponent : WebComponent, new()
    {
        return applicationBuilder.AddComponent(typeof(TComponent), configuration, componentBuilder);
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
    /// <param name="componentType"><see cref="Component"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="configure">自定义构建器配置</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IApplicationBuilder AddComponent(this IApplicationBuilder applicationBuilder, Type componentType, IConfiguration configuration, Action<WebComponentBuilder>? configure = null)
    {
        // 生成组件依赖字典
        var dependencies = Component.GenerateDependencyMap<WebComponent>(componentType);

        return applicationBuilder.AddComponent(dependencies, configuration, configure);
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
    /// <param name="componentType"><see cref="Component"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="componentBuilder"><see cref="WebComponentBuilder"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IApplicationBuilder AddComponent(this IApplicationBuilder applicationBuilder, Type componentType, IConfiguration configuration, WebComponentBuilder componentBuilder)
    {
        // 生成组件依赖字典
        var dependencies = Component.GenerateDependencyMap<WebComponent>(componentType);

        return applicationBuilder.AddComponent(dependencies, configuration, componentBuilder);
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
    /// <param name="dependencies">组件依赖字典</param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="configure">自定义构建器配置</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IApplicationBuilder AddComponent(this IApplicationBuilder applicationBuilder, Dictionary<Type, Type[]> dependencies, IConfiguration configuration, Action<WebComponentBuilder>? configure = null)
    {
        // 创建组件模块构建器
        var componentBuilder = new WebComponentBuilder();

        // 调用自定义配置
        configure?.Invoke(componentBuilder);

        return applicationBuilder.AddComponent(dependencies, configuration, componentBuilder);
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
    /// <param name="dependencies">组件依赖字典</param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="componentBuilder"><see cref="WebComponentBuilder"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IApplicationBuilder AddComponent(this IApplicationBuilder applicationBuilder, Dictionary<Type, Type[]> dependencies, IConfiguration configuration, WebComponentBuilder componentBuilder)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentBuilder, nameof(componentBuilder));

        // 构建组件模块
        componentBuilder.Build(applicationBuilder);

        // 获取环境对象
        var environment = applicationBuilder.GetHostEnvironment();

        // 创建上下文
        var applicationContext = new ApplicationContext(applicationBuilder)
        {
            Configuration = configuration,
            Environment = environment,
        };

        // 生成组件依赖拓扑图
        var topologicalMap = Component.GenerateTopologicalMap<WebComponent>(dependencies);

        // 获取组件化配置选项
        var componentOptions = applicationBuilder.GetComponentOptions();

        // 组件对象集合
        var components = new List<WebComponent>();

        // 依次初始化组件实例
        foreach (var node in topologicalMap)
        {
            var component = Activator.CreateInstance(node) as WebComponent;
            ArgumentNullException.ThrowIfNull(component, nameof(component));

            component.Options = componentOptions;
            components.Add(component);
        }

        // 调用前置配置服务
        components.ForEach(component => component.PreConfigure(applicationContext));

        // 调用配置服务
        components.ForEach(component => component.Configure(applicationContext));

        return applicationBuilder;
    }
}