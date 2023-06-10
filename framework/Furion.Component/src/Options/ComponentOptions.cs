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
/// 组件配置选项
/// </summary>
public sealed class ComponentOptions
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public ComponentOptions()
    {
        OptionsActions = new();
    }

    /// <summary>
    /// 组件参数委托
    /// </summary>
    public Dictionary<Type, List<Action<object>>> OptionsActions { get; }

    /// <summary>
    /// 添加或更新组件参数委托
    /// </summary>
    /// <typeparam name="TOptions">组件参数类型</typeparam>
    /// <param name="configure">配置委托</param>
    internal void AddOrUpdateOptionsAction<TOptions>(Action<TOptions> configure)
        where TOptions : class, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        // 创建 Action<object> 委托
        void configureObject(object obj) => configure((TOptions)obj);

        // 组件参数 Key
        var typeOptionsKey = typeof(TOptions);

        // 如果组件参数未配置则插入新的
        if (!OptionsActions.ContainsKey(typeOptionsKey))
        {
            OptionsActions.Add(typeOptionsKey, new List<Action<object>> { configureObject });
        }
        // 更新
        else
        {
            var oldValue = OptionsActions[typeOptionsKey];
            oldValue.Add(configureObject);
            OptionsActions[typeOptionsKey] = oldValue;
        }
    }

    /// <summary>
    /// 获取组件参数
    /// </summary>
    /// <typeparam name="TOptions">组件参数类型</typeparam>
    /// <returns><typeparamref name="TOptions"/></returns>
    internal TOptions? GetOptions<TOptions>()
        where TOptions : class, new()
    {
        // 组件参数 Key
        var typeOptionsKey = typeof(TOptions);

        // 如果未找到组件类型参数则返回空
        if (!OptionsActions.ContainsKey(typeOptionsKey))
        {
            return null;
        }

        var actions = OptionsActions[typeOptionsKey];
        TOptions currentValue = Activator.CreateInstance<TOptions>();

        // 遍历委托并调用
        foreach (Action<object> action in actions)
        {
            action(currentValue);
        }

        return currentValue;
    }
}