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

namespace Furion.DependencyInjection;

/// <summary>
/// 类型扫描依赖关系构建器
/// </summary>
public sealed class TypeScanningDependencyBuilder
{
    /// <summary>
    /// 待扫描的程序集集合
    /// </summary>
    internal readonly HashSet<Assembly> _assemblies;

    /// <summary>
    /// 黑名单服务类型集合
    /// </summary>
    internal readonly HashSet<Type> _blacklistServiceTypes;

    /// <summary>
    /// 类型扫描依赖关系模型过滤器
    /// </summary>
    internal Func<TypeScanningDependencyModel, bool>? _filterConfigure;

    /// <summary>
    /// <inheritdoc cref="TypeScanningDependencyBuilder"/>
    /// </summary>
    public TypeScanningDependencyBuilder()
    {
        _assemblies = new()
        {
            Assembly.GetEntryAssembly()!
        };

        _blacklistServiceTypes = new()
        {
            typeof(IDisposable), typeof(IAsyncDisposable),
            typeof(IDependency), typeof(IEnumerator),
            typeof(IEnumerable), typeof(ICollection),
            typeof(IDictionary), typeof(IComparable),
            typeof(object), typeof(DynamicObject)
        };
    }

    /// <summary>
    /// 禁用程序集扫描
    /// </summary>
    public bool SuppressAssemblyScanning { get; set; }

    /// <summary>
    /// 禁用非公开类型
    /// </summary>
    public bool SuppressNonPublicType { get; set; }

    /// <summary>
    /// 验证服务生存期合法化
    /// </summary>
    /// <remarks>设置 false 将跳过合法验证</remarks>
    public bool ValidateLifetime { get; set; } = true;

    /// <summary>
    /// 验证服务导出配置合法化
    /// </summary>
    /// <remarks>设置 false 将跳过合法验证</remarks>
    public bool ValidateExposeService { get; set; } = true;

    /// <summary>
    /// 添加类型扫描依赖关系模型过滤器
    /// </summary>
    /// <param name="configure">自定义配置委托</param>
    public void AddFilter(Func<TypeScanningDependencyModel, bool> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        _filterConfigure = configure;
    }

    /// <summary>
    /// 添加程序集
    /// </summary>
    /// <param name="assemblies"><see cref="Assembly"/>[]</param>
    /// <returns><see cref="TypeScanningDependencyBuilder"/></returns>
    public TypeScanningDependencyBuilder AddAssemblies(params Assembly[] assemblies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(assemblies);

        Array.ForEach(assemblies, assembly =>
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(assembly);

            _assemblies.Add(assembly);
        });

        return this;
    }

    /// <summary>
    /// 添加程序集
    /// </summary>
    /// <param name="assemblies"><see cref="IEnumerable{T}"/></param>
    /// <returns><see cref="TypeScanningDependencyBuilder"/></returns>
    public TypeScanningDependencyBuilder AddAssemblies(IEnumerable<Assembly> assemblies)
    {
        return AddAssemblies(assemblies.ToArray());
    }

    /// <summary>
    /// 添加黑名单服务类型
    /// </summary>
    /// <param name="types"><see cref="Type"/>[]</param>
    /// <returns><see cref="TypeScanningDependencyBuilder"/></returns>
    public TypeScanningDependencyBuilder AddBlacklistTypes(params Type[] types)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(types);

        // 逐条添加黑名单文件通配符到集合中
        Array.ForEach(types, type =>
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(type);

            _blacklistServiceTypes.Add(type);
        });

        return this;
    }

    /// <summary>
    /// 添加黑名单服务类型
    /// </summary>
    /// <param name="types"><see cref="IEnumerable{T}"/></param>
    /// <returns><see cref="TypeScanningDependencyBuilder"/></returns>
    public TypeScanningDependencyBuilder AddBlacklistTypes(IEnumerable<Type> types)
    {
        return AddBlacklistTypes(types.ToArray());
    }

    /// <summary>
    /// 构建模块服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    internal void Build(IServiceCollection services)
    {
        // 创建类型扫描依赖关系扫描器对象
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, this);

        // 扫描并添加服务
        typeScanningDependencyScanner.ScanToAddServices();
    }
}