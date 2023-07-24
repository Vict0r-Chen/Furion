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
/// 组件模块构建器
/// </summary>
public sealed class ComponentBuilder
{
    /// <summary>
    /// 组件配置委托集合
    /// </summary>
    internal readonly Dictionary<Type, List<Delegate>> _propsActions;

    /// <summary>
    /// <inheritdoc cref="ComponentBuilder"/>
    /// </summary>
    public ComponentBuilder()
    {
        _propsActions = new();
    }

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

        // 添加或更新组件配置
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
        ArgumentNullException.ThrowIfNull(configuration);

        // 将配置绑定到组件配置类型对象中
        var props = configuration.Get<TProps>();

        // 空检查
        ArgumentNullException.ThrowIfNull(props);

        // 添加组件配置
        Props(new Action<TProps>(draft =>
        {
            // 将组件配置类型对象映射到新的上下文组件配置中
            ObjectMapper.Map(props, draft);
        }));
    }

    /// <summary>
    /// 构建模块服务
    /// </summary>
    /// <param name="hostApplicationBuilder"><see cref="IHostApplicationBuilder"/></param>
    internal void Build(IHostApplicationBuilder hostApplicationBuilder)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(hostApplicationBuilder);

        // 添加核心模块选项服务
        hostApplicationBuilder.Services.AddCoreOptions();

        // 添加组件释放器服务
        hostApplicationBuilder.Services.AddHostedService<ComponentReleaser>();

        // 添加组件配置
        hostApplicationBuilder.GetComponentOptions()
            .PropsActions.AddOrUpdate(_propsActions);
    }
}