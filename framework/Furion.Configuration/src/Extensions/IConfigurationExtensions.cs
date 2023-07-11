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

namespace Microsoft.Extensions.Configuration;

/// <summary>
/// <see cref="IConfiguration"/> 拓展类
/// </summary>
public static class IConfigurationExtensions
{
    /// <summary>
    /// 判断配置节点是否存在
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="key">节点路径</param>
    /// <returns><see cref="bool"/></returns>
    public static bool Exists(this IConfiguration configuration, string key)
    {
        return configuration.GetSection(key).Exists();
    }

    /// <summary>
    /// 获取配置节点并转换成指定类型
    /// </summary>
    /// <typeparam name="T">节点类型</typeparam>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="key">节点路径</param>
    /// <returns><typeparamref name="T"/></returns>
    public static T? Get<T>(this IConfiguration configuration, string key)
    {
        return configuration.GetSection(key).Get<T>();
    }

    /// <summary>
    /// 获取配置节点并转换成指定类型
    /// </summary>
    /// <typeparam name="T">节点类型</typeparam>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="key">节点路径</param>
    /// <param name="configureOptions">配置值绑定到指定类型额外配置</param>
    /// <returns><typeparamref name="T"/></returns>
    public static T? Get<T>(this IConfiguration configuration, string key, Action<BinderOptions> configureOptions)
    {
        return configuration.GetSection(key).Get<T>(configureOptions);
    }

    /// <summary>
    /// 获取节点配置
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="key">节点路径</param>
    /// <param name="type">节点类型</param>
    /// <returns><see cref="object"/></returns>
    public static object? Get(this IConfiguration configuration, string key, Type type)
    {
        return configuration.GetSection(key).Get(type);
    }

    /// <summary>
    /// 获取节点配置
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="key">节点路径</param>
    /// <param name="type">节点类型</param>
    /// <param name="configureOptions">配置值绑定到指定类型额外配置</param>
    /// <returns><see cref="object"/></returns>
    public static object? Get(this IConfiguration configuration, string key, Type type, Action<BinderOptions> configureOptions)
    {
        return configuration.GetSection(key).Get(type, configureOptions);
    }

    /// <summary>
    /// 重新加载应用配置
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    public static void Reload(this IConfiguration configuration)
    {
        var configurationRoot = configuration as IConfigurationRoot;

        // 空检查
        ArgumentNullException.ThrowIfNull(configurationRoot, nameof(configurationRoot));

        configurationRoot.Reload();
    }
}