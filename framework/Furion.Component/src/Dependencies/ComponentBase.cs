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
    /// <param name="configure">自定义组件配置委托</param>
    public void Props<TProps>(Action<TProps> configure)
        where TProps : class, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));
        ArgumentNullException.ThrowIfNull(Options, nameof(Options));

        Options.PropsActions.AddOrUpdate(typeof(TProps), configure);
    }

    /// <summary>
    /// 前置配置服务
    /// </summary>
    /// <remarks>将在组件初始化完成后立即调用</remarks>
    /// <param name="context"><see cref="ServiceComponentContext"/></param>
    public virtual void PreConfigureServices(ServiceComponentContext context)
    {
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <remarks>根据组件依赖关系顺序调用，被依赖的组件优先调用</remarks>
    /// <param name="context"><see cref="ServiceComponentContext"/></param>
    public virtual void ConfigureServices(ServiceComponentContext context)
    {
    }

    /// <summary>
    /// 创建组件拓扑排序集合
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="predicate">自定义过滤委托</param>
    /// <returns><see cref="List{T}"/></returns>
    public static List<Type> CreateTopological(Type componentType, Func<Type, bool>? predicate = null)
    {
        return CreateTopological(CreateDependencies(componentType), predicate);
    }

    /// <summary>
    /// 创建拓扑排序集合
    /// </summary>
    /// <param name="dependencies">组件依赖关系集合</param>
    /// <param name="predicate">自定义过滤委托</param>
    /// <returns><see cref="List{T}"/></returns>
    public static List<Type> CreateTopological(Dictionary<Type, Type[]> dependencies, Func<Type, bool>? predicate = null)
    {
        // 检查组件依赖关系集合有效性
        CheckDependencies(dependencies);

        // 获取拓扑排序集合
        var topologicalSets = Topological.Sort(dependencies);

        // 筛选集合
        return predicate is null
            ? topologicalSets
            : topologicalSets.Where(predicate).ToList();
    }

    /// <summary>
    /// 创建组件依赖关系集合
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <returns><see cref="Dictionary{TKey, TValue}"/></returns>
    public static Dictionary<Type, Type[]> CreateDependencies(Type componentType)
    {
        // 检查组件类型合法性
        Check(componentType);

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
            var dependedTypes = currentType.GetDefinedCustomAttributeOrNew<DependsOnAttribute>(false).DependedTypes;
            dependencies.Add(currentType, dependedTypes);

            // 将依赖类型集合加入下一次待访问集合中
            toVisit.AddRange(dependedTypes);
        }

        // 检查组件依赖关系集合有效性
        CheckDependencies(dependencies);

        return dependencies;
    }

    /// <summary>
    /// 检查组件依赖关系集合有效性
    /// </summary>
    /// <param name="dependencies">组件依赖关系集合</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void CheckDependencies(Dictionary<Type, Type[]> dependencies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(dependencies, nameof(dependencies));

        // 查找集合中所有类型
        var componentTypes = dependencies.Keys.Concat(dependencies.Values.SelectMany(t => t))
                                                             .Distinct();

        // 检查所有组件类型合法性
        foreach (var componentType in componentTypes)
        {
            Check(componentType);
        }

        // 是否存在循环依赖
        if (Topological.HasCycle(dependencies))
        {
            throw new InvalidOperationException("The dependency relationship has a circular dependency.");
        }
    }

    /// <summary>
    /// 是否是 WebComponent
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <returns><see cref="bool"/></returns>
    public static bool IsWebComponent(Type componentType)
    {
        var baseType = componentType.BaseType;

        return typeof(ComponentBase).IsAssignableFrom(componentType)
            && baseType is not null
            && baseType.FullName == Constants.WEBCOMPONENT_TYPE_FULLNAME
            && componentType.IsInstantiable()
            && componentType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Length > 0;
    }

    /// <summary>
    /// 检查组件类型合法性
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Check(Type componentType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentType, nameof(componentType));

        // 是否派生自 ComponentBase
        var componentBaseType = typeof(ComponentBase);
        if (!componentBaseType.IsAssignableFrom(componentType))
        {
            throw new InvalidOperationException($"`{componentType.Name}` component type is not assignable from `{componentBaseType.Name}`.");
        }

        // 类型不能是 ComponentBase 或 WebComponent
        if (componentType == componentBaseType || componentType.FullName == Constants.WEBCOMPONENT_TYPE_FULLNAME)
        {
            throw new InvalidOperationException($"Component type cannot be a `{componentBaseType.Name}` or `WebComponent`.");
        }

        // 类型基类只能是 ComponentBase 或 WebComponent
        var baseType = componentType.BaseType!;
        if (!(baseType == componentBaseType || baseType.FullName == Constants.WEBCOMPONENT_TYPE_FULLNAME))
        {
            throw new InvalidOperationException($"`{componentType.Name}` component type cannot inherit from other component types.");
        }

        // 类型必须可以实例化
        if (!componentType.IsInstantiable())
        {
            throw new InvalidOperationException($"`{componentType.Name}` component type must be able to be instantiated.");
        }

        // 类型至少包含一个公开的构造函数
        if (componentType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Length == 0)
        {
            throw new InvalidOperationException($"`{componentType.Name}` component type must have at least one public constructor.");
        }
    }

    /// <summary>
    /// 创建组件实例
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="componentOptions"><see cref="ComponentOptions"/></param>
    /// <returns><see cref="ComponentBase"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal static ComponentBase CreateInstance(Type componentType, ComponentOptions componentOptions)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentOptions, nameof(componentOptions));

        // 检查组件类型合法性
        Check(componentType);

        // 获取所有公开的实例构造函数
        var constructors = componentType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

        // 查找是否贴有 [ActivatorComponentConstructor] 特性的构造函数
        // 若没找到则选择构造函数参数最多的一个
        var buildingConstructor = constructors.FirstOrDefault(c => c.IsDefined(typeof(ActivatorComponentConstructorAttribute), false))
                                                 ?? constructors.OrderByDescending(c => c.GetParameters().Length).First();

        // 获取构造函数参数定义
        var parameters = buildingConstructor.GetParameters();

        // 实例化构造函数参数
        var args = new object?[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            args[i] = GetProps(parameters[i].ParameterType, componentOptions);
        }

        // 调用组件构造函数进行实例化
        var component = buildingConstructor.Invoke(args) as ComponentBase;
        ArgumentNullException.ThrowIfNull(component, nameof(component));

        // 组件模块配置选项
        component.Options = componentOptions;

        // 查找贴有 [ComponentProps] 特性的组件配置属性
        var properties = componentType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                                            .Where(p => p.IsDefined(typeof(ComponentPropsAttribute), false));

        // 存在组件配置属性
        if (properties.Any())
        {
            foreach (var property in properties)
            {
                property.SetValue(component, GetProps(property.PropertyType, componentOptions));
            }
        }

        return component;
    }

    /// <summary>
    /// 获取组件配置
    /// </summary>
    /// <param name="propsType">组件配置类型</param>
    /// <param name="componentOptions"><see cref="ComponentOptions"/></param>
    /// <returns><see cref="object"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal static object? GetProps(Type propsType, ComponentOptions componentOptions)
    {
        // 检查是否是 Action<TProps> 类型
        if (propsType.IsGenericType
            && propsType.GetGenericTypeDefinition() == typeof(Action<>)
            && propsType.GenericTypeArguments[0].HasParameterlessConstructorDefined())
        {
            return componentOptions.GetPropsAction(propsType.GenericTypeArguments[0]);
        }

        // 检查是否是可 new() 类型
        if (propsType.HasParameterlessConstructorDefined())
        {
            var cascadeAction = componentOptions.GetPropsAction(propsType);
            if (cascadeAction is null)
            {
                return null;
            }

            // 创建组件配置实例
            var props = Activator.CreateInstance(propsType);
            cascadeAction.DynamicInvoke(props);
            return props;
        }

        throw new InvalidOperationException($"`{propsType.Name}` parameter type is an invalid component options.");
    }

    /// <summary>
    /// 创建组件依赖关系对象集合
    /// </summary>
    /// <typeparam name="TTargetComponent">目标组件类型</typeparam>
    /// <param name="topologicalSets">组件拓扑排序集合</param>
    /// <param name="componentOptions"><see cref="ComponentOptions"/></param>
    /// <param name="predicate">自定义配置委托</param>
    /// <returns><see cref="List{T}"/></returns>
    internal static List<TTargetComponent> CreateComponents<TTargetComponent>(List<Type> topologicalSets, ComponentOptions componentOptions, Action<TTargetComponent>? predicate = null)
        where TTargetComponent : ComponentBase
    {
        // 组件依赖关系对象集合
        var components = new List<TTargetComponent>();

        // 组件重复调用检查标识
        var suppressDuplicateCall = typeof(TTargetComponent).FullName == Constants.WEBCOMPONENT_TYPE_FULLNAME
            ? nameof(ComponentOptions.SuppressDuplicateCallForWeb)
            : nameof(ComponentOptions.SuppressDuplicateCall);

        // 从尾部依次初始化组件实例
        for (var i = topologicalSets.Count - 1; i >= 0; i--)
        {
            var componentType = topologicalSets[i];

            // 组件重复调用检测
            var recordName = componentType.FullName + $" (Type '{typeof(TTargetComponent).Name}')";
            if (componentOptions[suppressDuplicateCall])
            {
                if (componentOptions.CallRecords.Any(t => t == recordName))
                {
                    // 输出调试事件
                    Debugging.Warn("`{0}` component has been prevented from duplicate invocation.", componentType.Name);
                    continue;
                }

                componentOptions.CallRecords.Add(recordName);
            }

            // 创建组件实例
            var component = (TTargetComponent)CreateInstance(componentType, componentOptions);
            components.Insert(0, component);

            // 调用自定义配置委托
            predicate?.Invoke(component);
        }

        return components;
    }

    /// <summary>
    /// 创建组件依赖关系对象集合
    /// </summary>
    /// <param name="topologicalSets">组件拓扑排序集合</param>
    /// <param name="componentOptions"><see cref="ComponentOptions"/></param>
    /// <param name="predicate">自定义配置委托</param>
    /// <returns><see cref="List{T}"/></returns>
    internal static List<ComponentBase> CreateComponents(List<Type> topologicalSets, ComponentOptions componentOptions, Action<ComponentBase>? predicate = null)
    {
        return CreateComponents<ComponentBase>(topologicalSets, componentOptions, predicate);
    }
}