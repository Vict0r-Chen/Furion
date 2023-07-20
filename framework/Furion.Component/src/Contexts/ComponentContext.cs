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

namespace Furion.Component;

/// <summary>
/// 组件上下文基类
/// </summary>
public abstract class ComponentContext
{
    /// <summary>
    /// <inheritdoc cref="ComponentContext"/>
    /// </summary>
    /// <param name="options"><see cref="ComponentOptions"/></param>
    internal ComponentContext(ComponentOptions options)
    {
        Options = options;
        Properties = new Dictionary<object, object?>();
    }

    /// <summary>
    /// 附加属性
    /// </summary>
    public IDictionary<object, object?> Properties { get; }

    /// <inheritdoc cref="ComponentOptions"/>
    internal ComponentOptions Options { get; }

    /// <summary>
    /// 添加组件配置
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <param name="configure">自定义组件配置委托</param>
    public void Props<TProps>(Action<TProps> configure)
        where TProps : class, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        Options.PropsActions.AddOrUpdate(typeof(TProps), configure);
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

        // 获取配置实例
        var props = configuration.Get<TProps>();

        // 空检查
        ArgumentNullException.ThrowIfNull(props);

        // 创建组件配置委托
        var configure = new Action<TProps>(destination =>
        {
            ObjectMapper.Map(props, destination);
        });

        // 添加组件配置
        Props(configure);
    }

    /// <summary>
    /// 获取组件配置
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <returns><typeparamref name="TProps"/></returns>
    public TProps? GetProps<TProps>()
        where TProps : class, new()
    {
        // 获取组件配置委托
        var cascadeAction = GetPropsAction<TProps>();

        if (cascadeAction is null)
        {
            return null;
        }

        // 初始化组件配置实例并调用配置委托
        var props = Activator.CreateInstance<TProps>();
        cascadeAction(props);

        return props;
    }

    /// <summary>
    /// 获取组件配置
    /// </summary>
    /// <remarks>若组件配置不存在返回默认实例</remarks>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <returns><typeparamref name="TProps"/></returns>
    public TProps GetPropsOrNew<TProps>()
        where TProps : class, new()
    {
        return GetProps<TProps>() ?? Activator.CreateInstance<TProps>();
    }

    /// <summary>
    /// 获取组件配置委托
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <returns><see cref="Action{T}"/></returns>
    public Action<TProps>? GetPropsAction<TProps>()
        where TProps : class, new()
    {
        return Options.GetPropsAction<TProps>();
    }
}