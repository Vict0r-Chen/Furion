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
/// 组件抽象基类
/// </summary>
public abstract class ComponentBase
{
    /// <inheritdoc cref="ComponentOptions"/>
    internal ComponentOptions? Options { get; set; }

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
        ArgumentNullException.ThrowIfNull(Options);

        // 添加组件配置
        Options.Props(configure);
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
        ArgumentNullException.ThrowIfNull(Options);

        // 添加组件配置
        Options.Props<TProps>(configuration);
    }

    /// <summary>
    /// 检查组件是否已激活
    /// </summary>
    /// <param name="context"><see cref="ComponentContext"/></param>
    /// <returns><see cref="bool"/></returns>
    public virtual bool CanActivate(ComponentContext context)
    {
        return true;
    }

    /// <summary>
    /// 前置配置服务
    /// </summary>
    /// <remarks>将在组件初始化完成后立即调用</remarks>
    /// <param name="context"><see cref="ServiceComponentContext"/></param>
    public virtual void PreConfigureServices(ServiceComponentContext context)
    { }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <remarks>根据组件依赖关系顺序调用，被依赖的组件优先调用</remarks>
    /// <param name="context"><see cref="ServiceComponentContext"/></param>
    public virtual void ConfigureServices(ServiceComponentContext context)
    { }

    /// <summary>
    /// 监听依赖关系链中的组件执行回调
    /// </summary>
    /// <param name="context"><see cref="ComponentInvocationContext"/></param>
    public virtual void OnDependencyInvocation(ComponentInvocationContext context)
    { }

    /// <summary>
    /// 重载组件配置
    /// </summary>
    public void ReloadProps()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(Options);

        // 初始化组件激活器
        var componentActivator = new ComponentActivator(GetType(), Options);

        // 自动装配组件配置属性和配置字段
        componentActivator.AutowiredProps(this);
    }

    /// <summary>
    /// 创建组件依赖关系集合
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <returns><see cref="Dictionary{TKey, TValue}"/></returns>
    internal static Dictionary<Type, Type[]> CreateDependencies(Type componentType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentType);

        // 初始化组件依赖关系集合
        var dependencies = new Dictionary<Type, Type[]>();

        // 初始化未访问类型集合
        var toVisit = new List<Type> { componentType };

        // 循环检查未访问的类型集合中的数量直至为 0
        while (toVisit.Count > 0)
        {
            // 取出未访问类型集合中的第一项
            var currentType = toVisit[0];

            // 标记当前取出的项已被访问
            toVisit.RemoveAt(0);

            // 检查类型是否已生成依赖关系集合
            if (dependencies.ContainsKey(currentType))
            {
                continue;
            }

            // 检查类型是否贴有 [DependsOn] 特性，如果有则取出所有依赖关系集合
            var dependedTypes = currentType.GetDefinedCustomAttribute<DependsOnAttribute>(false)?.DependedTypes
                ?? Array.Empty<Type>();

            // 将依赖关系集合添加到组件依赖关系集合中
            dependencies.Add(currentType, dependedTypes);

            // 将依赖关系集合配置添加到未访问类型集合中
            toVisit.AddRange(dependedTypes);
        }

        return dependencies;
    }

    /// <summary>
    /// 创建入口组件
    /// </summary>
    /// <param name="entryComponentType">入口组件类型</param>
    /// <param name="componentContext"><see cref="ComponentContext"/></param>
    /// <param name="methodNames">调用方法集合</param>
    /// <param name="predicate">自定义配置委托</param>
    /// <exception cref="ArgumentException"></exception>
    internal static void CreateEntry(Type entryComponentType
        , ComponentContext componentContext
        , string[] methodNames
        , Func<Type, bool>? predicate = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(entryComponentType);
        ArgumentNullException.ThrowIfNull(componentContext);
        ArgumentNullException.ThrowIfNull(methodNames);

        // 创建组件依赖关系集合
        var dependencies = CreateDependencies(entryComponentType);

        // 初始化拓扑图
        var topologicalGraph = new TopologicalGraph(dependencies);

        // 循环依赖检查
        if (topologicalGraph.HasCycle())
        {
            throw new ArgumentException("The dependency relationship has a circular dependency.");
        }

        // 获取排序后的依赖关系集合
        var sortedDependencies = topologicalGraph.Sort();
        if (predicate is not null)
        {
            sortedDependencies = sortedDependencies.Where(predicate).ToList();
        }

        // 初始化依赖关系图
        var dependencyGraph = new DependencyGraph(dependencies);

        // 初始化组件实例集合
        var components = new List<ComponentBase>();

        // 初始化无需激活的组件类型集合
        var inactiveComponents = new List<Type>();

        // 从尾部依次初始化组件实例
        for (var i = sortedDependencies.Count - 1; i >= 0; i--)
        {
            // 获取组件类型
            var componentType = sortedDependencies[i];

            // 检查当前组件类型是否已标记为无需激活
            if (inactiveComponents.Contains(componentType))
            {
                continue;
            }

            // 获取或创建组件实例
            var component = ComponentActivator.GetOrCreate(componentType, componentContext.Options);

            // 检查组件是否允许激活
            if (!component.CanActivate(componentContext))
            {
                // 将无需激活的且只有自身依赖的组件类型添加到集合中
                inactiveComponents.AddRange(dependencies[componentType]
                    .Except(dependencies.Where(dept => dept.Key != componentType && !dependencies[componentType].Contains(dept.Key))
                        .SelectMany(u => u.Value.Concat(new[] { u.Key }))));

                continue;
            }

            // 添加组件实例到集合头部
            components.Insert(0, component);

            // 调用组件前置方法
            InvokeMethod(dependencyGraph, component, componentContext, methodNames[0]);
        }

        // 调用组件后置方法
        components.ForEach(component => InvokeMethod(dependencyGraph, component, componentContext, methodNames[1]));
    }

    /// <summary>
    /// 调用组件方法
    /// </summary>
    /// <param name="dependencyGraph"><see cref="DependencyGraph"/></param>
    /// <param name="component"><see cref="ComponentBase"/></param>
    /// <param name="componentContext"><see cref="ComponentContext"/></param>
    /// <param name="methodName">方法名称</param>
    internal static void InvokeMethod(DependencyGraph dependencyGraph
        , ComponentBase component
        , ComponentContext componentContext
        , string methodName)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(dependencyGraph);
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(componentContext);
        ArgumentNullException.ThrowIfNull(methodName);

        // 检查方法是否定义
        if (!component.GetType().IsDeclarationMethod(methodName, BindingFlags.Public, out var methodInfo))
        {
            return;
        }

        // 空检查
        ArgumentNullException.ThrowIfNull(methodInfo);

        // 调用方法
        methodInfo.Invoke(component, new object[] { componentContext });

        // 输出调试事件
        Debugging.Trace("`{0}.{1}` method has been called.", component.GetType(), methodName);

        // 通知依赖关系链中的组件执行回调方法
        NotifyInvocation(dependencyGraph, component, componentContext, methodName);
    }

    /// <summary>
    /// 通知依赖关系链中的组件执行回调操作
    /// </summary>
    /// <param name="dependencyGraph"><see cref="DependencyGraph"/></param>
    /// <param name="component"><see cref="ComponentBase"/></param>
    /// <param name="componentContext"><see cref="ComponentContext"/></param>
    /// <param name="methodName">方法名称</param>
    internal static void NotifyInvocation(DependencyGraph dependencyGraph
        , ComponentBase component
        , ComponentContext componentContext
        , string methodName)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(dependencyGraph);
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(componentContext);
        ArgumentNullException.ThrowIfNull(methodName);

        // 获取组件类型
        var componentType = component.GetType();

        // 查找组件类型依赖关系链上的类型集合
        var ancestors = dependencyGraph.FindAncestors(componentType);

        // 将当前组件类型插入到集合头部
        ancestors.Insert(0, componentType);

        // 初始化组件调用上下文
        var componentInvocationContext = new ComponentInvocationContext(component, componentContext, methodName);

        // 循环调用依赖关系链中的组件回调操作
        ancestors.Where(componentType => componentType.IsDeclarationMethod(nameof(OnDependencyInvocation), BindingFlags.Public, out _))
            .Select(componentType => ComponentActivator.GetOrCreate(componentType, componentContext.Options))
            .ToList()
            .ForEach(cmp => cmp.OnDependencyInvocation(componentInvocationContext));
    }

    /// <summary>
    /// 检查是否是 Web 组件
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsWebComponent(Type componentType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentType);

        // 获取组件类型继承类型
        var baseType = componentType.BaseType;

        return typeof(ComponentBase).IsAssignableFrom(componentType)
            && baseType is not null
            && baseType.FullName == Constants.WEB_COMPONENT_TYPE_FULLNAME
            && componentType.IsInstantiable();
    }
}