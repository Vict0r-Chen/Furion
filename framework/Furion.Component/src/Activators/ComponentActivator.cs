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
    /// 组件类型
    /// </summary>
    internal readonly Type _componentType;

    /// <inheritdoc cref="ComponentOptions" />
    internal readonly ComponentOptions _componentOptions;

    /// <summary>
    /// <inheritdoc cref="ComponentActivator" />
    /// </summary>
    /// <param name="componentType">组件类型</param>
    /// <param name="componentOptions"><see cref="ComponentOptions"/></param>
    internal ComponentActivator(Type componentType, ComponentOptions componentOptions)
    {
        // 检查组件类型合法性
        EnsureLegalComponent(componentType);

        // 空检查
        ArgumentNullException.ThrowIfNull(componentOptions);

        _componentType = componentType;
        _componentOptions = componentOptions;
    }

    /// <summary>
    /// 创建组件实例
    /// </summary>
    /// <returns></returns>
    internal ComponentBase Create()
    {
        // 反射查找成员绑定标记
        var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        // 获取所有公开的实例构造函数
        var constructors = _componentType.GetConstructors(bindingFlags);

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
            args[i] = _componentOptions.GetProps(parameters[i].ParameterType);
        }

        // 调用组件构造函数进行实例化
        var component = buildingConstructor.Invoke(args) as ComponentBase;
        ArgumentNullException.ThrowIfNull(component);

        // 组件模块配置选项
        component.Options = _componentOptions;

        // 查找贴有 [ComponentProps] 特性的组件配置属性（这里也考虑一下字段）
        var properties = _componentType.GetProperties(bindingFlags)
            .Where(p => p.IsDefined(typeof(ComponentPropsAttribute), false));

        // 存在组件配置属性
        if (properties.Any())
        {
            foreach (var property in properties)
            {
                property.SetValue(component, _componentOptions.GetProps(property.PropertyType));
            }
        }

        return component;
    }

    /// <summary>
    /// 检查组件类型合法性
    /// </summary>
    /// <param name="componentType"><see cref="ComponentBase"/></param>
    /// <exception cref="InvalidOperationException"></exception>
    internal static void EnsureLegalComponent(Type componentType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(componentType);

        // 是否派生自 ComponentBase
        var componentBaseType = typeof(ComponentBase);
        if (!componentBaseType.IsAssignableFrom(componentType))
        {
            throw new InvalidOperationException($"`{componentType.Name}` component type is not assignable from `{componentBaseType.Name}`.");
        }

        // 类型不能是 ComponentBase 或 WebComponent
        if (componentType == componentBaseType || componentType.FullName == Constants.WEB_COMPONENT_TYPE_FULLNAME)
        {
            throw new InvalidOperationException($"Component type cannot be a `{componentBaseType.Name}` or `WebComponent`.");
        }

        // 类型基类只能是 ComponentBase 或 WebComponent
        var baseType = componentType.BaseType!;
        if (!(baseType == componentBaseType || baseType.FullName == Constants.WEB_COMPONENT_TYPE_FULLNAME))
        {
            throw new InvalidOperationException($"`{componentType.Name}` component type cannot inherit from other component types.");
        }

        // 类型必须可以实例化
        if (!componentType.IsInstantiable())
        {
            throw new InvalidOperationException($"`{componentType.Name}` component type must be able to be instantiated.");
        }
    }
}