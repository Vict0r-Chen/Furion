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

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 数据验证模块 <see cref="IServiceCollection"/> 拓展类
/// </summary>
public static class ValidationServiceCollectionExtensions
{
    /// <summary>
    /// 添加数据验证服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddValidation(this IServiceCollection services, Action<ValidationBuilder>? configure = null)
    {
        // 初始化数据验证构建器
        var validationBuilder = new ValidationBuilder();

        // 调用自定义配置委托
        configure?.Invoke(validationBuilder);

        return services.AddValidation(validationBuilder);
    }

    /// <summary>
    /// 添加数据验证服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="validationBuilder"><see cref="ValidationBuilder"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddValidation(this IServiceCollection services, ValidationBuilder validationBuilder)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(validationBuilder);

        // 构建模块服务
        validationBuilder.Build(services);

        return services;
    }
}