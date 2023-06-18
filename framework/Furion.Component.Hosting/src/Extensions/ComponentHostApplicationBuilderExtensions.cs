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
/// 组件化模块拓展
/// </summary>
public static class ComponentHostApplicationBuilderExtensions
{
    /// <summary>
    /// 配置入口组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="ComponentBase"/></typeparam>
    /// <param name="hostApplicationBuilder"><see cref="HostApplicationBuilder"/></param>
    /// <returns><see cref="IHost"/></returns>
    public static IHost Entry<TComponent>(this HostApplicationBuilder hostApplicationBuilder)
        where TComponent : ComponentBase
    {
        return hostApplicationBuilder.AddComponent<TComponent>().Build();
    }

    /// <summary>
    /// 添加组件服务
    /// </summary>
    /// <param name="hostApplicationBuilder"><see cref="HostApplicationBuilder"/></param>
    /// <param name="configure">自定义构建器配置</param>
    /// <returns><see cref="HostApplicationBuilder"/></returns>
    public static HostApplicationBuilder AddComponentService(this HostApplicationBuilder hostApplicationBuilder, Action<ComponentBuilder>? configure = null)
    {
        hostApplicationBuilder.Services.AddComponentService(configure);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// 添加组件服务
    /// </summary>
    /// <param name="hostApplicationBuilder"><see cref="HostApplicationBuilder"/></param>
    /// <param name="componentBuilder"><see cref="ComponentBuilder"/></param>
    /// <returns><see cref="HostApplicationBuilder"/></returns>
    public static HostApplicationBuilder AddComponentService(this HostApplicationBuilder hostApplicationBuilder, ComponentBuilder componentBuilder)
    {
        hostApplicationBuilder.Services.AddComponentService(componentBuilder);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="ComponentBase"/></typeparam>
    /// <param name="hostApplicationBuilder"><see cref="HostApplicationBuilder"/></param>
    /// <param name="configure">自定义构建器配置</param>
    /// <returns><see cref="HostApplicationBuilder"/></returns>
    public static HostApplicationBuilder AddComponent<TComponent>(this HostApplicationBuilder hostApplicationBuilder, Action<ComponentBuilderBase>? configure = null)
        where TComponent : ComponentBase
    {
        hostApplicationBuilder.Services.AddComponent<TComponent>(hostApplicationBuilder.Configuration, configure);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="hostApplicationBuilder"><see cref="HostApplicationBuilder"/></param>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="configure">自定义构建器配置</param>
    /// <returns><see cref="HostApplicationBuilder"/></returns>
    public static HostApplicationBuilder AddComponent(this HostApplicationBuilder hostApplicationBuilder, Type componentType, Action<ComponentBuilderBase>? configure = null)
    {
        hostApplicationBuilder.Services.AddComponent(componentType, hostApplicationBuilder.Configuration, configure);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="hostApplicationBuilder"><see cref="HostApplicationBuilder"/></param>
    /// <param name="dependencies">组件依赖字典</param>
    /// <param name="configure">自定义构建器配置</param>
    /// <returns><see cref="HostApplicationBuilder"/></returns>
    public static HostApplicationBuilder AddComponent(this HostApplicationBuilder hostApplicationBuilder, Dictionary<Type, Type[]> dependencies, Action<ComponentBuilderBase>? configure = null)
    {
        hostApplicationBuilder.Services.AddComponent(dependencies, hostApplicationBuilder.Configuration, configure);

        return hostApplicationBuilder;
    }
}