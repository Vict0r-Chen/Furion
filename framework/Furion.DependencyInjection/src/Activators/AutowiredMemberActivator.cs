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
/// 自动装配成员激活器
/// </summary>
internal sealed class AutowiredMemberActivator
{
    /// <summary>
    /// 实例类型
    /// </summary>
    internal readonly Type _instanceType;

    /// <summary>
    /// <see cref="AutowiredServiceAttribute"/> 类型
    /// </summary>
    internal readonly Type _autowiredServiceAttributeType;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="instance">对象实例</param>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    internal AutowiredMemberActivator(object instance
        , IServiceProvider serviceProvider)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        Instance = instance;
        _instanceType = instance.GetType();
        Services = serviceProvider;

        _autowiredServiceAttributeType = typeof(AutowiredServiceAttribute);
    }

    /// <summary>
    /// 对象实例
    /// </summary>
    internal object Instance { get; init; }

    /// <inheritdoc cref="IServiceProvider"/>
    internal IServiceProvider Services { get; init; }

    /// <summary>
    /// 反射搜索成员方式
    /// </summary>
    internal BindingFlags Bindings { get; set; } = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    /// <summary>
    /// 自动装配成员值
    /// </summary>
    internal void AutowiredMembers()
    {
        // 自动装配属性值
        AutowriedProperties();

        // 自动装配字段值
        AutowriedFields();
    }

    /// <summary>
    /// 自动装配属性值
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    internal void AutowriedProperties()
    {
        // 查找所有符合反射搜索成员方式的属性
        var declaredProperties = _instanceType.GetProperties(Bindings)
            .Where(property => property.IsDefined(_autowiredServiceAttributeType, false));

        // 遍历属性并初始化值
        foreach (var property in declaredProperties)
        {
            // 检查属性是否可写
            if (!property.CanWrite)
            {
                throw new InvalidOperationException($"Cannot automatically assign read-only property `{property.Name}` of type `{_instanceType}`.");
            }

            // 获取 [AutowiredService] 特性定义
            var autowiredServiceAttribute = property.GetCustomAttribute<AutowiredServiceAttribute>(false);

            // 空检查
            ArgumentNullException.ThrowIfNull(autowiredServiceAttribute);

            // 解析属性值
            var value = autowiredServiceAttribute.AllowNullValue
                ? Services.GetService(property.PropertyType)
                : Services.GetRequiredService(property.PropertyType);

            // 设置属性值
            property.SetValue(Instance, value);

            // 调试事件消息
            var debugMessage = "The property {0} of type {1} has been successfully injected into the service.";
            if (value is null)
            {
                debugMessage += " (Value is null)";
            }

            // 输出调试事件
            Debugging.Warn(debugMessage, property.Name, _instanceType);
        }
    }

    /// <summary>
    /// 自动装配字段值
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    internal void AutowriedFields()
    {
        // 查找所有符合反射搜索成员方式的字段
        var declaredFields = _instanceType.GetFields(Bindings)
            .Where(field => field.IsDefined(_autowiredServiceAttributeType, false));

        // 遍历字段并初始化值
        foreach (var field in declaredFields)
        {
            // 检查字段是否可写
            if (field.IsInitOnly)
            {
                throw new InvalidOperationException($"Cannot automatically assign read-only field `{field.Name}` of type `{_instanceType}`.");
            }

            // 获取 [AutowiredService] 特性定义
            var autowiredServiceAttribute = field.GetCustomAttribute<AutowiredServiceAttribute>(false);

            // 空检查
            ArgumentNullException.ThrowIfNull(autowiredServiceAttribute);

            // 解析属性值
            var value = autowiredServiceAttribute.AllowNullValue
                ? Services.GetService(field.FieldType)
                : Services.GetRequiredService(field.FieldType);

            // 设置字段值
            field.SetValue(Instance, value);

            // 调试事件消息
            var debugMessage = "The field {0} of type {1} has been successfully injected into the service.";
            if (value is null)
            {
                debugMessage += " (Value is null)";
            }

            // 输出调试事件
            Debugging.Warn(debugMessage, field.Name, _instanceType);
        }
    }
}