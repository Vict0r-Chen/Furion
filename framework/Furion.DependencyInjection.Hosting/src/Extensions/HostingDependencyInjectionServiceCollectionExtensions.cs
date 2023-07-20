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

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 依赖注入模块 <see cref="IServiceCollection"/> 拓展类
/// </summary>
public static class HostingDependencyInjectionServiceCollectionExtensions
{
    /// <summary>
    /// 添加自动装配成员激活器的主机服务
    /// </summary>
    /// <typeparam name="THostedService"><see cref="IHostedService"/></typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddAutowiredHostedService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THostedService>(this IServiceCollection services)
        where THostedService : class, IHostedService
    {
        return services.AddAutowiredHostedService(serviceProvider =>
        {
            // 解析或初始化主机服务对象
            var hostedService = serviceProvider.GetService<THostedService>()
                ?? serviceProvider.GetRequiredService<ITypeActivatorCache>()
                    .CreateInstance<THostedService>(serviceProvider, typeof(THostedService));

            return hostedService;
        });
    }

    /// <summary>
    /// 添加自动装配成员激活器的主机服务
    /// </summary>
    /// <typeparam name="THostedService"><see cref="IHostedService"/></typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="implementationFactory">实现类服务工厂</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddAutowiredHostedService<THostedService>(this IServiceCollection services, Func<IServiceProvider, THostedService> implementationFactory)
        where THostedService : class, IHostedService
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(implementationFactory);

        // 注册自动装配成员激活器服务
        services.AddAutowiredMemberActivator();

        // 注册主机服务
        services.AddHostedService(serviceProvider =>
        {
            // 调用主机服务工厂创建主机服务对象
            var hostedService = implementationFactory(serviceProvider);

            // 解析自动装配成员激活器服务
            var autowiredMemberActivator = serviceProvider.GetRequiredService<IAutowiredMemberActivator>();

            // 自动装配成员值
            autowiredMemberActivator.AutowiredMembers(hostedService, serviceProvider);

            return hostedService;
        });

        return services;
    }
}