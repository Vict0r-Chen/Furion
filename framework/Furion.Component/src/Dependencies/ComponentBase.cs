// 麻省理工学院许可证
//
// 版权所有 © 2020-2023 百小僧，百签科技（广东）有限公司
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

namespace Furion.Component;

/// <summary>
/// 组件抽象基类
/// </summary>
public abstract class ComponentBase
{
    /// <inheritdoc cref="ComponentOptions"/>
    internal ComponentOptions? Options { get; set; }

    /// <summary>
    /// 添加组件配置
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <param name="configure">自定义配置委托</param>
    public void Props<TProps>(Action<TProps> configure)
        where TProps : class, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);
        ArgumentNullException.ThrowIfNull(Options);

        // 添加组件配置
        Options.Props(configure);
    }

    /// <summary>
    /// 添加组件配置
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    public void Props<TProps>(IConfiguration configuration)
        where TProps : class, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(Options);

        // 添加组件配置
        Options.Props<TProps>(configuration);
    }

    /// <summary>
    /// 检查组件是否已激活
    /// </summary>
    /// <param name="context"><see cref="ComponentContext"/></param>
    /// <returns><see cref="bool"/></returns>
    public virtual bool CanActivate(ComponentContext context)
    {
        return true;
    }

    /// <summary>
    /// 前置配置服务
    /// </summary>
    /// <remarks>将在组件初始化完成后立即调用</remarks>
    /// <param name="context"><see cref="ServiceComponentContext"/></param>
    public virtual void PreConfigureServices(ServiceComponentContext context)
    { }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <remarks>根据组件依赖关系顺序调用，被依赖的组件优先调用</remarks>
    /// <param name="context"><see cref="ServiceComponentContext"/></param>
    public virtual void ConfigureServices(ServiceComponentContext context)
    { }

    /// <summary>
    /// 监听依赖关系链中的组件执行回调
    /// </summary>
    /// <param name="context"><see cref="ComponentInvocationContext"/></param>
    public virtual void OnDependencyInvocation(ComponentInvocationContext context)
    { }

    /// <summary>
    /// 重载组件配置
    /// </summary>
    public void ReloadProps()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(Options);

        // 初始化组件激活器
        var componentActivator = new ComponentActivator(GetType(), Options);

        // 自动装配组件配置属性和配置字段
        componentActivator.AutowiredProps(this);
    }

    /// <summary>
    /// 检查是否是 Web 组件
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsWebComponent(Type componentType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentType);

        // 获取组件类型继承类型
        var baseType = componentType.BaseType;

        return typeof(ComponentBase).IsAssignableFrom(componentType)
            && baseType is not null
            && baseType.FullName == Constants.WEB_COMPONENT_TYPE_FULLNAME
            && componentType.IsInstantiable();
    }
}