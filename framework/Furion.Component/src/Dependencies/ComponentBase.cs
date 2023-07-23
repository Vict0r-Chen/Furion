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
    /// 是否激活组件
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
    /// 调用事件监听
    /// </summary>
    /// <param name="context"><see cref="ComponentInvokeContext"/></param>
    public virtual void InvokeEvents(ComponentInvokeContext context)
    { }

    /// <summary>
    /// 重载组件配置
    /// </summary>
    public virtual void ReloadProps()
    {
        // TODO!
    }

    /// <summary>
    /// 创建拓扑图排序集合
    /// </summary>
    /// <param name="dependencies">组件依赖关系集合</param>
    /// <param name="predicate">自定义过滤委托</param>
    /// <returns><see cref="List{T}"/></returns>
    internal static List<Type> CreateTopologicalGraph(Dictionary<Type, Type[]> dependencies, Func<Type, bool>? predicate = null)
    {
        // 检查组件依赖关系集合合法性
        EnsureLegalDependencies(dependencies);

        // 创建拓扑图对象
        var topologicalGraph = new TopologicalGraph(dependencies);

        // 获取排序集合
        var sortedList = topologicalGraph.Sort();

        // 筛选集合
        return predicate is null
            ? sortedList
            : sortedList.Where(predicate).ToList();
    }

    /// <summary>
    /// 创建组件依赖关系集合
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <returns><see cref="Dictionary{TKey, TValue}"/></returns>
    internal static Dictionary<Type, Type[]> CreateDependencies(Type componentType)
    {
        // 检查组件类型合法性
        ComponentActivator.EnsureLegalComponentType(componentType);

        // 组件依赖关系集合
        var dependencies = new Dictionary<Type, Type[]>();

        // 待访问的类型集合
        var toVisit = new List<Type> { componentType };

        while (toVisit.Count > 0)
        {
            // 取出列表中的第一个类型
            var currentType = toVisit[0];

            // 移除已访问的类型
            toVisit.RemoveAt(0);

            // 已访问过检查
            if (dependencies.ContainsKey(currentType))
            {
                continue;
            }

            // 查找 [DependsOn] 特性配置，禁止继承查找
            var dependedTypes = currentType.GetDefinedCustomAttribute<DependsOnAttribute>(false)?.DependedTypes
                ?? Array.Empty<Type>();
            dependencies.Add(currentType, dependedTypes);

            // 将依赖类型集合加入下一次待访问集合中
            toVisit.AddRange(dependedTypes);
        }

        // 检查组件依赖关系集合合法性
        EnsureLegalDependencies(dependencies);

        return dependencies;
    }

    /// <summary>
    /// 检查组件依赖关系集合合法性
    /// </summary>
    /// <param name="dependencies">组件依赖关系集合</param>
    /// <exception cref="InvalidOperationException"></exception>
    internal static void EnsureLegalDependencies(Dictionary<Type, Type[]> dependencies)
    {
        // 空项检查
        if (dependencies.Count == 0)
        {
            throw new ArgumentException("The dependency relationship cannot be empty.", nameof(dependencies));
        }

        // 查找集合中所有类型
        var componentTypes = dependencies.Keys
            .Concat(dependencies.Values.SelectMany(t => t))
            .Distinct();

        // 检查所有组件类型合法性
        foreach (var componentType in componentTypes)
        {
            ComponentActivator.EnsureLegalComponentType(componentType);
        }

        // 创建拓扑图对象
        var topologicalGraph = new TopologicalGraph(dependencies);

        // 是否存在循环依赖
        if (topologicalGraph.HasCycle())
        {
            throw new InvalidOperationException("The dependency relationship has a circular dependency.");
        }
    }

    /// <summary>
    /// 是否是 WebComponent
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsWebComponent(Type componentType)
    {
        var baseType = componentType.BaseType;

        return typeof(ComponentBase).IsAssignableFrom(componentType)
            && baseType is not null
            && baseType.FullName == Constants.WEB_COMPONENT_TYPE_FULLNAME
            && componentType.IsInstantiable();
    }

    /// <summary>
    /// 获取或创建组件实例
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="componentOptions"><see cref="ComponentOptions"/></param>
    /// <returns><see cref="ComponentBase"/></returns>
    internal static ComponentBase GetOrCreateComponent(Type componentType, ComponentOptions componentOptions)
    {
        return componentOptions.Components.GetOrAdd(componentType, type =>
        {
            // 初始化组件激活器
            var componentActivator = new ComponentActivator(type, componentOptions);

            return componentActivator.Create();
        });
    }

    /// <summary>
    /// 根据组件依赖关系依次调用
    /// </summary>
    /// <param name="dependencies">组件依赖关系集合</param>
    /// <param name="componentContext"><see cref="ComponentContext"/></param>
    /// <param name="methods">调用方法集合</param>
    /// <param name="topologicalGraphPredicate">拓扑图排序过滤</param>
    /// <returns><see cref="List{T}"/></returns>
    internal static void InvokeComponents(Dictionary<Type, Type[]> dependencies
        , ComponentContext componentContext
        , string[] methods
        , Func<Type, bool>? topologicalGraphPredicate = null)
    {
        // 创建组件拓扑图排序集合
        var topologicalGraph = CreateTopologicalGraph(dependencies, topologicalGraphPredicate);

        // 创建依赖关系图
        var dependencyGraph = new DependencyGraph(dependencies);

        // 组件依赖关系对象集合
        var components = new List<ComponentBase>();

        // 获取组件模块配置选项
        var componentOptions = componentContext.Options;

        // 存储未激活的且只有自身依赖的组件类型
        var inactiveComponents = new List<Type>();

        // 从尾部依次初始化组件实例
        for (var i = topologicalGraph.Count - 1; i >= 0; i--)
        {
            var componentType = topologicalGraph[i];

            // 检查当前组件类型的下游是否未激活
            if (inactiveComponents.Contains(componentType))
            {
                continue;
            }

            // 创建组件实例
            var component = GetOrCreateComponent(componentType, componentOptions);

            // 检查组件是否激活
            if (!component.CanActivate(componentContext))
            {
                // 存储未激活的且只有自身依赖的组件依赖类型
                inactiveComponents.AddRange(dependencies[componentType].Except(
                    dependencies.Where(d => d.Key != componentType && !dependencies[componentType].Contains(d.Key))
                                      .SelectMany(u => u.Value.Concat(new[] { u.Key }))));
                continue;
            }

            // 添加到组件集合头部
            components.Insert(0, component);

            // 调用前置方法
            InvokeMethod(dependencyGraph, component, componentContext, methods[0]);
        }

        // 调用后置方法
        components.ForEach(component => InvokeMethod(dependencyGraph, component, componentContext, methods[1]));
    }

    /// <summary>
    /// 调用组件方法
    /// </summary>
    /// <param name="dependencyGraph"><see cref="DependencyGraph"/></param>
    /// <param name="component"><see cref="ComponentBase"/></param>
    /// <param name="componentContext"><see cref="ComponentContext"/></param>
    /// <param name="invokeMethod">方法名称</param>
    internal static void InvokeMethod(DependencyGraph dependencyGraph
        , ComponentBase component
        , ComponentContext componentContext
        , string invokeMethod)
    {
        // 若方法未定义则跳过
        if (!component.GetType().IsDeclareOnlyMethod(invokeMethod, BindingFlags.Public, out var methodInfo))
        {
            return;
        }

        // 空检查
        ArgumentNullException.ThrowIfNull(methodInfo);

        // 调用方法
        methodInfo.Invoke(component, new object[] { componentContext });

        // 输出调试事件
        Debugging.Trace("`{0}.{1}` method has been called.", component.GetType(), invokeMethod);

        // 调用事件监听
        InvokeEvents(dependencyGraph, component, componentContext, invokeMethod);
    }

    /// <summary>
    /// 调用事件监听
    /// </summary>
    /// <param name="dependencyGraph"><see cref="DependencyGraph"/></param>
    /// <param name="component"><see cref="ComponentBase"/></param>
    /// <param name="componentContext"><see cref="ComponentContext"/></param>
    /// <param name="event">事件</param>
    internal static void InvokeEvents(DependencyGraph dependencyGraph
        , ComponentBase component
        , ComponentContext componentContext
        , string @event)
    {
        // 查找组件依赖关系集合中匹配的祖先组件类型集合
        var componentType = component.GetType();
        var ancestors = dependencyGraph.FindAncestors(componentType);

        // 将当前组件插入到集合头部
        ancestors.Insert(0, componentType);

        // 创建组件事件上下文
        var componentEventContext = new ComponentInvokeContext(component, componentContext, @event);

        // 循环调用所有组件组件（含自己）的监听方法
        ancestors.Where(componentType => componentType.IsDeclareOnlyMethod(nameof(InvokeEvents), BindingFlags.Public, out _))
            .Select(componentType => GetOrCreateComponent(componentType, componentContext.Options))
            .ToList()
            .ForEach(cmp => cmp.InvokeEvents(componentEventContext));
    }
}