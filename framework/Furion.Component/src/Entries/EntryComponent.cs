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
/// 入口组件
/// </summary>
internal sealed class EntryComponent
{
    /// <summary>
    /// 依赖关系集合
    /// </summary>
    internal readonly Dictionary<Type, Type[]> _dependencies;

    /// <summary>
    /// 拓扑排序后的依赖关系集合
    /// </summary>
    internal readonly List<Type> _sortedDependencies;

    /// <inheritdoc cref="DependencyGraph"/>
    internal readonly DependencyGraph _dependencyGraph;

    /// <summary>
    /// <inheritdoc cref="EntryComponent"/>
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="componentContext"><see cref="Component.ComponentContext"/></param>
    internal EntryComponent(Type componentType, ComponentContext componentContext)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentType);
        ArgumentNullException.ThrowIfNull(componentContext);

        ComponentType = componentType;
        ComponentContext = componentContext;

        // 创建依赖关系集合
        _dependencies = DependencyGraph.Create(componentType);

        // 初始化拓扑图
        var topologicalGraph = new TopologicalGraph(_dependencies);

        // 获取拓扑排序后的依赖关系集合
        _sortedDependencies = topologicalGraph.Sort(true)
            .WhereIf(componentContext is not ServiceComponentContext, ComponentBase.IsWebComponent) // WebComponent
            .ToList();

        // 初始化依赖关系图
        _dependencyGraph = new DependencyGraph(_dependencies);
    }

    /// <summary>
    /// <see cref="ComponentBase"/>
    /// </summary>
    internal Type ComponentType { get; init; }

    /// <inheritdoc cref="Component.ComponentContext"/>
    internal ComponentContext ComponentContext { get; init; }

    /// <summary>
    /// 启动
    /// </summary>
    internal void Start()
    {
        // 初始化组件实例集合
        var components = new List<ComponentBase>();

        // 初始化无需激活的组件类型集合
        var inactiveComponents = new List<Type>();

        // 获取初始化方法名集合
        var methodNames = GetInitializationMethodNames();

        // 从尾部依次初始化组件实例
        for (var i = _sortedDependencies.Count - 1; i >= 0; i--)
        {
            // 获取当前组件类型
            var dependencyType = _sortedDependencies[i];

            // 检查当前组件类型是否已标记为无需激活
            if (inactiveComponents.Contains(dependencyType))
            {
                continue;
            }

            // 获取或创建组件实例
            var component = ComponentActivator.GetOrCreate(dependencyType, ComponentContext.Options);

            // 检查组件是否允许激活
            if (!component.CanActivate(ComponentContext))
            {
                // 将无需激活的且只有自身依赖的组件类型添加到集合中
                inactiveComponents.AddRange(_dependencies[dependencyType]
                    .Except(_dependencies.Where(dept => dept.Key != dependencyType && !_dependencies[dependencyType].Contains(dept.Key))
                        .SelectMany(u => u.Value.Concat(new[] { u.Key }))));

                continue;
            }

            // 添加组件实例到集合头部
            components.Insert(0, component);

            // 调用组件前置方法
            InvokeMethod(component, methodNames[0]);
        }

        // 调用组件后置方法
        components.ForEach(component => InvokeMethod(component, methodNames[1]));
    }

    /// <summary>
    /// 调用组件方法
    /// </summary>
    /// <param name="component"><see cref="ComponentBase"/></param>
    /// <param name="methodName">方法名称</param>
    internal void InvokeMethod(ComponentBase component, string methodName)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(methodName);

        // 检查方法是否定义
        if (!component.GetType().IsDeclarationMethod(methodName, BindingFlags.Public, out var methodInfo))
        {
            return;
        }

        // 空检查
        ArgumentNullException.ThrowIfNull(methodInfo);

        // 调用方法
        methodInfo.Invoke(component, new object[] { ComponentContext });

        // 输出调试事件
        Debugging.Trace("`{0}.{1}` method has been called.", component.GetType(), methodName);

        // 通知依赖关系链中的组件执行回调方法
        NotifyInvocation(component, methodName);
    }

    /// <summary>
    /// 通知依赖关系链中的组件执行回调操作
    /// </summary>
    /// <param name="component"><see cref="ComponentBase"/></param>
    /// <param name="methodName">方法名称</param>
    internal void NotifyInvocation(ComponentBase component, string methodName)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(methodName);

        // 获取组件类型
        var componentType = component.GetType();

        // 查找组件类型依赖关系链上的类型集合
        var ancestorTypes = _dependencyGraph.FindAncestors(componentType);

        // 将当前组件类型插入到集合头部
        ancestorTypes.Insert(0, componentType);

        // 初始化组件调用上下文
        var componentInvocationContext = new ComponentInvocationContext(component, ComponentContext, methodName);

        // 循环调用依赖关系链中的组件回调操作
        ancestorTypes.Where(type => type.IsDeclarationMethod(nameof(component.OnDependencyInvocation), BindingFlags.Public, out _))
            .Select(type => ComponentActivator.GetOrCreate(type, ComponentContext.Options))
            .ToList()
            .ForEach(cmp => cmp.OnDependencyInvocation(componentInvocationContext));
    }

    /// <summary>
    /// 获取初始化方法名集合
    /// </summary>
    /// <returns><see cref="string"/>[]</returns>
    internal string[] GetInitializationMethodNames()
    {
        return ComponentContext is ServiceComponentContext
            ? new[] { nameof(ComponentBase.PreConfigureServices), nameof(ComponentBase.ConfigureServices) }
            : new[] { Constants.WEB_COMPONENT_METHOD_PRECONFIGURE, Constants.WEB_COMPONENT_METHOD_CONFIGURE };
    }
}