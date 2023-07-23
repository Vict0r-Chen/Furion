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
/// 组件模块选项
/// </summary>
internal sealed class ComponentOptions
{
    /// <summary>
    /// <see cref="GetPropsAction{TProps}()"/> 方法对象
    /// </summary>
    internal readonly MethodInfo _GetPropsActionMethod;

    /// <summary>
    /// <inheritdoc cref="ComponentOptions"/>
    /// </summary>
    public ComponentOptions()
    {
        PropsActions = new();
        Components = new();

        _GetPropsActionMethod = GetType().GetMethod(nameof(GetPropsAction)
            , 1
            , BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly
            , null
            , Type.EmptyTypes
            , null)!;
    }

    /// <summary>
    /// 组件配置委托集合
    /// </summary>
    internal Dictionary<Type, List<Delegate>> PropsActions { get; init; }

    /// <summary>
    /// 组件对象集合
    /// </summary>
    internal ConcurrentDictionary<Type, ComponentBase> Components { get; init; }

    /// <summary>
    /// 添加组件配置
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <param name="configure">自定义配置委托</param>
    internal void Props<TProps>(Action<TProps> configure)
        where TProps : class, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        // 添加或更新组件配置
        PropsActions.AddOrUpdate(typeof(TProps), configure);
    }

    /// <summary>
    /// 添加组件配置
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    internal void Props<TProps>(IConfiguration configuration)
        where TProps : class, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configuration);

        // 获取配置实例
        var props = configuration.Get<TProps>();

        // 空检查
        ArgumentNullException.ThrowIfNull(props);

        // 创建组件配置委托
        var configure = new Action<TProps>(draft =>
        {
            ObjectMapper.Map(props, draft);
        });

        // 添加组件配置
        Props(configure);
    }

    /// <summary>
    /// 获取组件配置委托
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <returns><see cref="Action{T}"/></returns>
    internal Action<TProps>? GetPropsAction<TProps>()
        where TProps : class, new()
    {
        // 检查组件配置是否存在
        if (!PropsActions.TryGetValue(typeof(TProps), out var values))
        {
            return null;
        }

        // 生成组件配置级联委托
        var cascadeAction = values.Cast<Action<TProps>>()
            .Aggregate((previous, current) => (props) =>
            {
                previous(props);
                current(props);
            });

        return cascadeAction;
    }

    /// <summary>
    /// 获取组件配置委托
    /// </summary>
    /// <param name="propsType">组件配置类型</param>
    /// <returns><see cref="Delegate"/></returns>
    internal Delegate? GetPropsAction(Type propsType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(propsType);

        // 反射调用方法并将结果转换为委托对象
        var @delegate = _GetPropsActionMethod.MakeGenericMethod(propsType)
            .Invoke(this, null) as Delegate;

        return @delegate;
    }

    /// <summary>
    /// 获取组件配置
    /// </summary>
    /// <param name="propsType">组件配置类型</param>
    /// <returns><see cref="object"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal object? GetProps(Type propsType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(propsType);

        // 检查类型是否是 Action<TProps> 类型，同时泛型参数 TProps 符合 class, new() 约束
        if (propsType.IsGenericType
            && propsType.GetGenericTypeDefinition() == typeof(Action<>)
            && propsType.GenericTypeArguments[0].IsClass
            && propsType.GenericTypeArguments[0].HasDefinePublicParameterlessConstructor())
        {
            return GetPropsAction(propsType.GenericTypeArguments[0]);
        }

        // 检查类型是否是非泛型类型，同时类型符合 class, new() 约束
        if (propsType.IsClass
            && !propsType.IsGenericType
            && propsType.HasDefinePublicParameterlessConstructor())
        {
            // 获取组件配置委托
            var propsAction = GetPropsAction(propsType);

            // 空检查
            if (propsAction is null)
            {
                return null;
            }

            // 初始化组件配置
            var props = Activator.CreateInstance(propsType);

            // 调用委托
            propsAction.DynamicInvoke(props);

            return props;
        }

        throw new InvalidOperationException($"Type `{propsType}` is not a valid component props.");
    }
}