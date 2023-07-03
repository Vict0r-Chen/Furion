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
/// 组件模块构建器基类
/// </summary>
public class WebComponentBuilderBase
{
    /// <summary>
    /// 组件配置委托集合
    /// </summary>
    internal readonly Dictionary<Type, List<Delegate>> _propsActions;

    /// <summary>
    /// 构造函数
    /// </summary>
    internal WebComponentBuilderBase()
    {
        _propsActions = new();
    }

    /// <summary>
    /// 添加组件配置
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <param name="configure">自定义组件配置委托</param>
    public void Props<TProps>(Action<TProps> configure)
        where TProps : class, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));

        _propsActions.AddOrUpdate(typeof(TProps), configure);
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
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        // 获取配置实例
        var props = configuration.Get<TProps>();

        // 空检查
        ArgumentNullException.ThrowIfNull(props, nameof(props));

        // 创建组件配置委托
        void configure(TProps destination)
        {
            ObjectMapper.Map(props, destination);
        }

        // 添加组件配置
        Props((Action<TProps>)configure);
    }

    /// <summary>
    /// 构建模块服务
    /// </summary>
    /// <param name="webApplication"><see cref="WebApplication"/></param>
    internal virtual void Build(WebApplication webApplication)
    {
        // 添加组件配置
        var componentOptions = webApplication.GetComponentOptions();
        componentOptions.PropsActions.AddOrUpdate(_propsActions);

        _propsActions.Clear();
    }
}