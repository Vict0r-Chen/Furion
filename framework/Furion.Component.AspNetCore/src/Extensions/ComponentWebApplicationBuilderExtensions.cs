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
/// 组件模块 <see cref="WebApplicationBuilder"/> 拓展类
/// </summary>
public static class ComponentWebApplicationBuilderExtensions
{
    /// <summary>
    /// 添加组件模块入口服务
    /// </summary>
    /// <typeparam name="TComponent"><see cref="WebComponent"/></typeparam>
    /// <param name="webApplicationBuilder"><see cref="WebApplicationBuilder"/></param>
    /// <returns><see cref="WebApplication"/></returns>
    public static WebApplication Entry<TComponent>(this WebApplicationBuilder webApplicationBuilder)
        where TComponent : WebComponent
    {
        return webApplicationBuilder.Entry<TComponent, TComponent>();
    }

    /// <summary>
    /// 添加组件模块入口服务
    /// </summary>
    /// <typeparam name="TComponent"><see cref="ComponentBase"/></typeparam>
    /// <typeparam name="TWebComponent"><see cref="WebComponent"/></typeparam>
    /// <param name="webApplicationBuilder"><see cref="WebApplicationBuilder"/></param>
    /// <returns><see cref="WebApplication"/></returns>
    public static WebApplication Entry<TComponent, TWebComponent>(this WebApplicationBuilder webApplicationBuilder)
        where TComponent : ComponentBase
        where TWebComponent : WebComponent
    {
        return webApplicationBuilder.AddComponent<TComponent>().Build()
                                    .UseComponent<TWebComponent>();
    }

    /// <summary>
    /// 添加组件模块服务
    /// </summary>
    /// <param name="webApplicationBuilder"><see cref="WebApplicationBuilder"/></param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="WebApplicationBuilder"/></returns>
    public static WebApplicationBuilder AddComponentCore(this WebApplicationBuilder webApplicationBuilder, Action<ComponentBuilder>? configure = null)
    {
        webApplicationBuilder.Services.AddComponentCore(configure);

        return webApplicationBuilder;
    }

    /// <summary>
    /// 添加组件模块服务
    /// </summary>
    /// <param name="webApplicationBuilder"><see cref="WebApplicationBuilder"/></param>
    /// <param name="componentBuilder"><see cref="ComponentBuilder"/></param>
    /// <returns><see cref="WebApplicationBuilder"/></returns>
    public static WebApplicationBuilder AddComponentCore(this WebApplicationBuilder webApplicationBuilder, ComponentBuilder componentBuilder)
    {
        webApplicationBuilder.Services.AddComponentCore(componentBuilder);

        return webApplicationBuilder;
    }

    /// <summary>
    /// 添加服务组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="ComponentBase"/></typeparam>
    /// <param name="webApplicationBuilder"><see cref="WebApplicationBuilder"/></param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="WebApplicationBuilder"/></returns>
    public static WebApplicationBuilder AddComponent<TComponent>(this WebApplicationBuilder webApplicationBuilder, Action<ComponentBuilderBase>? configure = null)
        where TComponent : ComponentBase
    {
        webApplicationBuilder.Services.AddComponent<TComponent>(webApplicationBuilder.Configuration, configure);

        return webApplicationBuilder;
    }

    /// <summary>
    /// 添加服务组件
    /// </summary>
    /// <param name="webApplicationBuilder"><see cref="WebApplicationBuilder"/></param>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="WebApplicationBuilder"/></returns>
    public static WebApplicationBuilder AddComponent(this WebApplicationBuilder webApplicationBuilder, Type componentType, Action<ComponentBuilderBase>? configure = null)
    {
        webApplicationBuilder.Services.AddComponent(componentType, webApplicationBuilder.Configuration, configure);

        return webApplicationBuilder;
    }

    /// <summary>
    /// 添加服务组件
    /// </summary>
    /// <param name="webApplicationBuilder"><see cref="WebApplicationBuilder"/></param>
    /// <param name="dependencies">组件依赖字典</param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="WebApplicationBuilder"/></returns>
    public static WebApplicationBuilder AddComponent(this WebApplicationBuilder webApplicationBuilder, Dictionary<Type, Type[]> dependencies, Action<ComponentBuilderBase>? configure = null)
    {
        webApplicationBuilder.Services.AddComponent(dependencies, webApplicationBuilder.Configuration, configure);

        return webApplicationBuilder;
    }
}