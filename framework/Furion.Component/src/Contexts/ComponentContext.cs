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
/// 组件上下文抽象基类
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
    public IDictionary<object, object?> Properties { get; init; }

    /// <inheritdoc cref="ComponentOptions"/>
    internal ComponentOptions Options { get; init; }

    /// <summary>
    /// 添加组件配置
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <param name="configure">组件配置委托</param>
    public void Props<TProps>(Action<TProps> configure)
        where TProps : class, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        // 添加或更新组件配置
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

        // 将配置绑定到组件配置类型对象中
        var props = configuration.Get<TProps>();

        // 空检查
        ArgumentNullException.ThrowIfNull(props);

        // 添加组件配置
        Props(new Action<TProps>(destination =>
        {
            // 将组件配置类型对象映射到新的上下文组件配置中
            ObjectMapper.Map(props, destination);
        }));
    }

    /// <summary>
    /// 获取组件配置
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <returns><typeparamref name="TProps"/></returns>
    public TProps? GetProps<TProps>()
        where TProps : class, new()
    {
        // 获取组件配置级联委托
        var cascadeAction = GetPropsAction<TProps>();

        // 空检查
        if (cascadeAction is null)
        {
            return null;
        }

        // 初始化组件配置实例
        var props = new TProps();

        // 调用组件配置级联委托
        cascadeAction(props);

        return props;
    }

    /// <summary>
    /// 获取组件配置
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <returns><typeparamref name="TProps"/></returns>
    public TProps GetPropsOrNew<TProps>()
        where TProps : class, new()
    {
        return GetProps<TProps>() ?? new();
    }

    /// <summary>
    /// 获取组件配置
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <returns><see cref="Action{T}"/></returns>
    public Action<TProps>? GetPropsAction<TProps>()
        where TProps : class, new()
    {
        return Options.GetPropsAction<TProps>();
    }
}