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
/// 组件激活器
/// </summary>
internal sealed class ComponentActivator
{
    /// <summary>
    /// <see cref="ComponentBase"/>
    /// </summary>
    internal readonly Type _componentType;

    /// <inheritdoc cref="ComponentOptions" />
    internal readonly ComponentOptions _componentOptions;

    /// <summary>
    /// 反射搜索成员方式
    /// </summary>
    internal const BindingFlags _bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    /// <summary>
    /// <inheritdoc cref="ComponentActivator" />
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="componentOptions"><see cref="ComponentOptions"/></param>
    internal ComponentActivator(Type componentType, ComponentOptions componentOptions)
    {
        // 检查类型合法性
        EnsureLegalComponentType(componentType);

        // 空检查
        ArgumentNullException.ThrowIfNull(componentOptions);

        _componentType = componentType;
        _componentOptions = componentOptions;
    }

    /// <summary>
    /// 反射搜索成员方式
    /// </summary>
    internal BindingFlags BindingAttr { get; set; } = _bindingAttr;

    /// <summary>
    /// 获取或创建组件实例
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <param name="componentOptions"><see cref="ComponentOptions"/></param>
    /// <returns><see cref="ComponentBase"/></returns>
    internal static ComponentBase GetOrCreate(Type componentType, ComponentOptions componentOptions)
    {
        // 检查类型合法性
        EnsureLegalComponentType(componentType);

        // 空检查
        ArgumentNullException.ThrowIfNull(componentOptions);

        // 查找或创建组件实例
        return componentOptions.Components
            .GetOrAdd(componentType, type =>
            {
                // 初始化组件激活器
                var componentActivator = new ComponentActivator(type, componentOptions);

                // 创建组件实例
                return componentActivator.Create();
            });
    }

    /// <summary>
    /// 创建组件实例
    /// </summary>
    /// <returns><see cref="ComponentBase"/></returns>
    internal ComponentBase Create()
    {
        // 获取组件初始化构造函数
        GetNewConstructor(out var newConstructor, out var args);

        // 调用构造函数并创建组件实例
        var component = newConstructor.Invoke(args) as ComponentBase;

        // 空检查
        ArgumentNullException.ThrowIfNull(component);

        // 设置组件模块选项
        component.Options = _componentOptions;

        // 自动装配组件配置属性和配置字段
        AutowiredProps(component);

        return component;
    }

    /// <summary>
    /// 获取组件初始化构造函数
    /// </summary>
    /// <param name="newConstructor">构造函数</param>
    /// <param name="args">构造函数参数集合</param>
    internal void GetNewConstructor(out ConstructorInfo newConstructor, out object?[] args)
    {
        // 获取组件构造函数集合
        var constructors = _componentType.GetConstructors(BindingAttr);

        // 查找贴有 [ActivatorComponentConstructor] 特性的构造函数，若没找到则选择构造函数参数最多的一个
        newConstructor = constructors.FirstOrDefault(ctor => ctor.IsDefined(typeof(ActivatorComponentConstructorAttribute), false))
            ?? constructors.OrderBy(c => c.GetParameters().Length).Last();

        // 获取构造函数参数
        var parameters = newConstructor.GetParameters();

        // 遍历构造函数参数并初始化
        args = new object?[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            args[i] = _componentOptions.GetProps(parameters[i].ParameterType);
        }
    }

    /// <summary>
    /// 自动装配组件配置属性和配置字段
    /// </summary>
    /// <param name="component"><see cref="ComponentBase"/></param>
    /// <exception cref="InvalidOperationException"></exception>
    internal void AutowiredProps(ComponentBase component)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(component);

        // 查找贴有 [ComponentProps] 特性的组件配置属性集合
        var properties = _componentType.GetProperties(BindingAttr)
            .Where(property => property.IsDefined(typeof(ComponentPropsAttribute), false));

        // 遍历组件配置属性集合并初始化
        foreach (var property in properties)
        {
            // 检查属性是否可写
            if (!property.CanWrite)
            {
                throw new InvalidOperationException($"Cannot automatically assign read-only property `{property.Name}` of type `{_componentType}`.");
            }

            // 设置属性值
            property.SetValue(component, _componentOptions.GetProps(property.PropertyType));
        }

        // 查找贴有 [ComponentProps] 特性的组件配置字段集合
        var fields = _componentType.GetFields(BindingAttr)
            .Where(field => field.IsDefined(typeof(ComponentPropsAttribute), false));

        // 遍历组件配置字段集合并初始化
        foreach (var field in fields)
        {
            // 检查字段是否可写
            if (field.IsInitOnly)
            {
                throw new InvalidOperationException($"Cannot automatically assign read-only field `{field.Name}` of type `{_componentType}`.");
            }

            // 设置字段值
            field.SetValue(component, _componentOptions.GetProps(field.FieldType));
        }
    }

    /// <summary>
    /// 检查类型合法性
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <exception cref="InvalidOperationException"></exception>
    internal static void EnsureLegalComponentType(Type componentType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentType);

        // 检查类型是否派生自 ComponentBase 类型
        var componentBaseType = typeof(ComponentBase);
        if (!componentBaseType.IsAssignableFrom(componentType))
        {
            throw new InvalidOperationException($"`{componentType}` type is not assignable from `{componentBaseType}`.");
        }

        // 检查类型是否是 ComponentBase 或 WebComponent 类型
        if (componentType == componentBaseType || componentType.FullName == Constants.WEB_COMPONENT_TYPE_FULLNAME)
        {
            throw new InvalidOperationException($"Type cannot be a `{componentBaseType}` or `{Constants.WEB_COMPONENT_TYPE_FULLNAME}`.");
        }

        // 检查类型是否继承非 ComponentBase 或 WebComponent 类型
        var baseType = componentType.BaseType!;
        if (!(baseType == componentBaseType || baseType.FullName == Constants.WEB_COMPONENT_TYPE_FULLNAME))
        {
            throw new InvalidOperationException($"`{componentType}` type cannot inherit from other component types.");
        }

        // 检查类型是否可以实例化
        if (!componentType.IsInstantiable())
        {
            throw new InvalidOperationException($"`{componentType}` type must be able to be instantiated.");
        }
    }
}