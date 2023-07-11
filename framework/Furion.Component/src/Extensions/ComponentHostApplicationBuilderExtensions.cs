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

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// 组件模块 <see cref="IHostApplicationBuilder"/> 拓展类
/// </summary>
public static class ComponentHostApplicationBuilderExtensions
{
    /// <summary>
    /// 添加组件模块服务
    /// </summary>
    /// <param name="hostApplicationBuilder"><see cref="IHostApplicationBuilder"/></param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="IHostApplicationBuilder"/></returns>
    public static IHostApplicationBuilder AddComponentCore(this IHostApplicationBuilder hostApplicationBuilder, Action<ComponentBuilder>? configure = null)
    {
        hostApplicationBuilder.Services.AddComponentCore(configure);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// 添加组件模块服务
    /// </summary>
    /// <param name="hostApplicationBuilder"><see cref="IHostApplicationBuilder"/></param>
    /// <param name="componentBuilder"><see cref="ComponentBuilder"/></param>
    /// <returns><see cref="IHostApplicationBuilder"/></returns>
    public static IHostApplicationBuilder AddComponentCore(this IHostApplicationBuilder hostApplicationBuilder, ComponentBuilder componentBuilder)
    {
        hostApplicationBuilder.Services.AddComponentCore(componentBuilder);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// 添加服务组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="ComponentBase"/></typeparam>
    /// <param name="hostApplicationBuilder"><see cref="IHostApplicationBuilder"/></param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="IHostApplicationBuilder"/></returns>
    public static IHostApplicationBuilder AddComponent<TComponent>(this IHostApplicationBuilder hostApplicationBuilder, Action<ComponentBuilderBase>? configure = null)
        where TComponent : ComponentBase
    {
        hostApplicationBuilder.Services.AddComponent<TComponent>(hostApplicationBuilder.Configuration, hostApplicationBuilder.Logging, configure);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// 添加服务组件
    /// </summary>
    /// <param name="hostApplicationBuilder"><see cref="IHostApplicationBuilder"/></param>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="IHostApplicationBuilder"/></returns>
    public static IHostApplicationBuilder AddComponent(this IHostApplicationBuilder hostApplicationBuilder, Type componentType, Action<ComponentBuilderBase>? configure = null)
    {
        hostApplicationBuilder.Services.AddComponent(componentType, hostApplicationBuilder.Configuration, hostApplicationBuilder.Logging, configure);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// 添加服务组件
    /// </summary>
    /// <param name="hostApplicationBuilder"><see cref="IHostApplicationBuilder"/></param>
    /// <param name="dependencies">组件依赖关系集合</param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="IHostApplicationBuilder"/></returns>
    public static IHostApplicationBuilder AddComponent(this IHostApplicationBuilder hostApplicationBuilder, Dictionary<Type, Type[]> dependencies, Action<ComponentBuilderBase>? configure = null)
    {
        hostApplicationBuilder.Services.AddComponent(dependencies, hostApplicationBuilder.Configuration, hostApplicationBuilder.Logging, configure);

        return hostApplicationBuilder;
    }
}