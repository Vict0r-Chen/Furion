﻿// 麻省理工学院许可证
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
/// 组件化依赖抽象基类
/// </summary>
public abstract class ComponentBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    protected ComponentBase()
    { }

    /// <inheritdoc cref="ComponentOptions"/>
    public ComponentOptions? Options { get; internal set; }

    /// <summary>
    /// 配置组件参数
    /// </summary>
    /// <typeparam name="TOptions">组件参数类型</typeparam>
    /// <param name="configure">配置委托</param>
    public void Configure<TOptions>(Action<TOptions> configure)
        where TOptions : class, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));
        ArgumentNullException.ThrowIfNull(Options, nameof(Options));

        Options.OptionsActions.AddOrUpdate(configure);
    }

    /// <summary>
    /// 前置配置服务
    /// </summary>
    /// <param name="context"><see cref="ServiceContext"/></param>
    public virtual void PreConfigureServices(ServiceContext context)
    { }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"><see cref="ServiceContext"/></param>
    public virtual void ConfigureServices(ServiceContext context)
    { }

    /// <summary>
    /// 生成组件依赖拓扑图
    /// </summary>
    /// <param name="componentType">组件类型</param>
    /// <returns><see cref="List{T}"/></returns>
    public static List<Type> GenerateTopologicalMap(Type componentType)
    {
        var dependencies = GenerateDependencyMap(componentType);
        return GenerateTopologicalMap(dependencies);
    }

    /// <summary>
    /// 生成组件依赖字典
    /// </summary>
    /// <param name="componentType">组件类型</param>
    /// <returns><see cref="Dictionary{TKey, TValue}"/></returns>
    public static Dictionary<Type, Type[]> GenerateDependencyMap(Type componentType)
    {
        // 创建空的组件依赖字典
        var dependencies = new Dictionary<Type, Type[]>();
        AddItems(componentType, dependencies);

        return dependencies;

        // 添加组件依赖字典子项
        static void AddItems(Type componentType, Dictionary<Type, Type[]> dependencies)
        {
            // 组件类型检查
            CheckComponent(componentType);

            // 已访问过检查
            if (dependencies.ContainsKey(componentType))
            {
                return;
            }

            // 查找 [DependsOn] 特性依赖配置
            var dependsOn = componentType.GetCustomAttribute<DependsOnAttribute>(false)?.Dependencies ?? Array.Empty<Type>();
            dependencies.Add(componentType, dependsOn);

            // 递归生成组件依赖字典项
            foreach (var dependency in dependsOn)
            {
                AddItems(dependency, dependencies);
            }
        }
    }

    /// <summary>
    /// 生成组件依赖拓扑图
    /// </summary>
    /// <param name="dependencies">组件依赖字典</param>
    /// <returns><see cref="List{T}"/></returns>
    public static List<Type> GenerateTopologicalMap(Dictionary<Type, Type[]> dependencies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(dependencies, nameof(dependencies));

        // 查找字典所有类型进行验证
        var componentTypes = dependencies.Keys.Concat(dependencies.Values.SelectMany(t => t)).Distinct();
        foreach (var type in componentTypes)
        {
            // 组件类型检查
            CheckComponent(type);
        }

        // 判断组件是否存在循环依赖
        if (Topological.HasCycle(dependencies))
        {
            throw new InvalidOperationException("The dependency relationship has a circular dependency.");
        }

        // 返回依赖拓扑图
        return Topological.Sort(dependencies);
    }

    /// <summary>
    /// 检查组件类型
    /// </summary>
    /// <param name="componentType">组件类型</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void CheckComponent(Type componentType)
    {
        var componentBaseType = typeof(ComponentBase);

        // 判断组件类型是否是 ComponentBase 派生类型
        if (!componentBaseType.IsAssignableFrom(componentType))
        {
            throw new InvalidOperationException($"Type '{componentType.Name}' is not assignable from '{componentBaseType.Name}'.");
        }

        // 判断组件是否相互继承（禁止继承）
        var baseType = componentType.BaseType!;
        if (!(baseType == componentBaseType || baseType.FullName == $"{baseType.Namespace}.WebComponent"))
        {
            throw new InvalidOperationException("Components are not allowed to inherit from each other.");
        }
    }
}