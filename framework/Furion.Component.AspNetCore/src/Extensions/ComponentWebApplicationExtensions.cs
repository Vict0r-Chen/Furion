﻿// 麻省理工学院许可证
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
/// 组件模块 <see cref="WebApplication"/> 拓展类
/// </summary>
public static class ComponentWebApplicationExtensions
{
    /// <summary>
    /// 添加应用组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="ComponentBase"/></typeparam>
    /// <param name="webApplication"><see cref="WebApplication"/></param>
    /// <returns><see cref="WebApplication"/></returns>
    public static WebApplication UseComponent<TComponent>(this WebApplication webApplication)
        where TComponent : WebComponent
    {
        return webApplication.UseComponent(typeof(TComponent));
    }

    /// <summary>
    /// 添加应用组件
    /// </summary>
    /// <param name="webApplication"><see cref="WebApplication"/></param>
    /// <param name="componentType"><see cref="WebComponent"/></param>
    /// <returns><see cref="WebApplication"/></returns>
    public static WebApplication UseComponent(this WebApplication webApplication, Type componentType)
    {
        // 生成组件依赖字典
        var dependencies = ComponentBase.CreateDependencies(componentType);

        return webApplication.UseComponent(dependencies);
    }

    /// <summary>
    /// 添加应用组件
    /// </summary>
    /// <param name="webApplication"><see cref="WebApplication"/></param>
    /// <param name="dependencies">组件依赖关系集合</param>
    /// <returns><see cref="WebApplication"/></returns>
    public static WebApplication UseComponent(this WebApplication webApplication, Dictionary<Type, Type[]> dependencies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(dependencies, nameof(dependencies));

        // 根据组件依赖关系依次调用
        ComponentBase.InvokeComponents(dependencies
            , new ApplicationComponentContext(webApplication)
            , new[] { nameof(WebComponent.PreConfigure), nameof(WebComponent.Configure) }
            , ComponentBase.IsWebComponent);

        return webApplication;
    }

    /// <summary>
    /// 获取组件模块配置选项
    /// </summary>
    /// <param name="webApplication"><see cref="WebApplication"/></param>
    /// <returns><see cref="ComponentOptions"/></returns>
    internal static ComponentOptions GetComponentOptions(this WebApplication webApplication)
    {
        return webApplication.Services.GetRequiredService<CoreOptions>().Get<ComponentOptions>();
    }
}