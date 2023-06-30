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

namespace Furion.Configuration;

/// <summary>
/// 配置模块嵌入资源构建器
/// </summary>
public sealed class ManifestResourceConfigurationBuilder
{
    /// <summary>
    /// 待扫描的程序集集合
    /// </summary>
    internal readonly HashSet<Assembly> _assemblies;

    /// <summary>
    /// 嵌入文件配置过滤器
    /// </summary>
    internal Func<ManifestResourceConfigurationModel, bool>? _filterConfigure;

    /// <summary>
    /// 构造函数
    /// </summary>
    public ManifestResourceConfigurationBuilder()
    {
        _assemblies = new();
    }

    /// <summary>
    /// 添加文件扫描过滤器
    /// </summary>
    /// <param name="configure"><see cref="Func{T1, T2, TResult}"/></param>
    public void AddFilter(Func<ManifestResourceConfigurationModel, bool> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));

        _filterConfigure = configure;
    }

    /// <summary>
    /// 添加待扫描的程序集
    /// </summary>
    /// <param name="assemblies"><see cref="Assembly"/>[]</param>
    /// <returns><see cref="ManifestResourceConfigurationBuilder"/></returns>
    public ManifestResourceConfigurationBuilder AddAssemblies(params Assembly[] assemblies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(assemblies, nameof(assemblies));

        Array.ForEach(assemblies, assembly =>
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));

            _assemblies.Add(assembly);
        });

        return this;
    }

    /// <summary>
    /// 添加待扫描的程序集
    /// </summary>
    /// <param name="assemblies"><see cref="IEnumerable{T}"/></param>
    /// <returns><see cref="ManifestResourceConfigurationBuilder"/></returns>
    public ManifestResourceConfigurationBuilder AddAssemblies(IEnumerable<Assembly> assemblies)
    {
        return AddAssemblies(assemblies.ToArray());
    }

    /// <summary>
    /// 构建模块服务
    /// </summary>
    /// <returns><see cref="Dictionary{TKey, TValue}"/></returns>
    internal List<ManifestResourceConfigurationModel> Build()
    {
        var resources = new List<ManifestResourceConfigurationModel>();
        foreach (var assembly in _assemblies)
        {
            var resourceNames = assembly.GetManifestResourceNames();
            Array.ForEach(resourceNames, resourceName =>
            {
                var manifestResourceModel = new ManifestResourceConfigurationModel(assembly, resourceName);

                // 调用文件配置模型过滤器
                if (_filterConfigure is null || _filterConfigure.Invoke(manifestResourceModel))
                {
                    resources.Add(manifestResourceModel);
                }
            });
        }

        _assemblies.Clear();
        _filterConfigure = null;

        return resources;
    }
}