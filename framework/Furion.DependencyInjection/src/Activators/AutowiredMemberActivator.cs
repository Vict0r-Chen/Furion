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

/// <inheritdoc />
internal sealed class AutowiredMemberActivator : IAutowiredMemberActivator
{
    /// <summary>
    /// 可自动装配的类型属性集合工厂委托
    /// </summary>
    internal readonly Func<Type, List<PropertyInfo>> _getAutowiredPropertiesFactory;

    /// <summary>
    /// 可自动装配的类型字段集合工厂委托
    /// </summary>
    internal readonly Func<Type, List<FieldInfo>> _getAutowiredFieldsFactory;

    /// <summary>
    /// 可自动装配的类型属性缓存集合
    /// </summary>
    internal readonly ConcurrentDictionary<Type, List<PropertyInfo>> _typeAutowiredPropertiesCache;

    /// <summary>
    /// 可自动装配的类型字段缓存集合
    /// </summary>
    internal readonly ConcurrentDictionary<Type, List<FieldInfo>> _typeAutowiredFieldsCache;

    /// <summary>
    /// 构造函数
    /// </summary>
    public AutowiredMemberActivator()
    {
        _typeAutowiredPropertiesCache = new();
        _typeAutowiredFieldsCache = new();

        _getAutowiredPropertiesFactory = type =>
        {
            return type.GetProperties(GetBindingFlags())
                .Where(property => property.IsDefined(typeof(AutowiredServiceAttribute), false))
                .ToList();
        };

        _getAutowiredFieldsFactory = type =>
        {
            return type.GetFields(GetBindingFlags())
                .Where(field => field.IsDefined(typeof(AutowiredServiceAttribute), false))
                .ToList();
        };
    }

    /// <inheritdoc />
    public BindingFlags GetBindingFlags()
    {
        return BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic;
    }

    /// <inheritdoc />
    public void AutowiredMembers(object instance, IServiceProvider serviceProvider)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        // 自动装配属性值
        AutowriedProperties(instance, serviceProvider);

        // 自动装配字段值
        AutowriedFields(instance, serviceProvider);
    }

    /// <inheritdoc />
    public void AutowriedProperties(object instance, IServiceProvider serviceProvider)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        // 对象类型
        var instanceType = instance.GetType();

        // 获取可自动装配的类型属性集合
        var matchProperties = _typeAutowiredPropertiesCache.GetOrAdd(instanceType, _getAutowiredPropertiesFactory);

        // 遍历属性并设置值
        foreach (var property in matchProperties)
        {
            // 检查属性是否可写
            if (!property.CanWrite)
            {
                throw new InvalidOperationException($"Cannot automatically assign read-only property `{property.Name}` of type `{instanceType}`.");
            }

            // 获取 [AutowiredService] 特性对象
            var autowiredServiceAttribute = property.GetCustomAttribute<AutowiredServiceAttribute>(false);

            // 空检查
            ArgumentNullException.ThrowIfNull(autowiredServiceAttribute);

            // 解析属性值
            var value = autowiredServiceAttribute.CanBeNull
                ? serviceProvider.GetService(property.PropertyType)
                : serviceProvider.GetRequiredService(property.PropertyType);

            // 设置属性值
            property.SetValue(instance, value);

            // 调试事件消息
            var debugMessage = "The property {0} of type {1} has been successfully injected into the service.";
            if (value is null)
            {
                debugMessage += " (Value is null)";
            }

            // 输出调试事件
            Debugging.Warn(debugMessage, property.Name, instanceType);
        }
    }

    /// <inheritdoc />
    public void AutowriedFields(object instance, IServiceProvider serviceProvider)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        // 对象类型
        var instanceType = instance.GetType();

        // 获取可自动装配的类型字段集合
        var matchFields = _typeAutowiredFieldsCache.GetOrAdd(instanceType, _getAutowiredFieldsFactory);

        // 遍历字段并设置值
        foreach (var field in matchFields)
        {
            // 检查字段是否可写
            if (field.IsInitOnly)
            {
                throw new InvalidOperationException($"Cannot automatically assign read-only field `{field.Name}` of type `{instanceType}`.");
            }

            // 获取 [AutowiredService] 特性对象
            var autowiredServiceAttribute = field.GetCustomAttribute<AutowiredServiceAttribute>(false);

            // 空检查
            ArgumentNullException.ThrowIfNull(autowiredServiceAttribute);

            // 解析字段值
            var value = autowiredServiceAttribute.CanBeNull
                ? serviceProvider.GetService(field.FieldType)
                : serviceProvider.GetRequiredService(field.FieldType);

            // 设置字段值
            field.SetValue(instance, value);

            // 调试事件消息
            var debugMessage = "The field {0} of type {1} has been successfully injected into the service.";
            if (value is null)
            {
                debugMessage += " (Value is null)";
            }

            // 输出调试事件
            Debugging.Warn(debugMessage, field.Name, instanceType);
        }
    }
}