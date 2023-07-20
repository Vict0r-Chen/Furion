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

namespace Furion.Reflection;

/// <summary>
/// <see cref="DispatchProxy"/> 装饰器
/// </summary>
/// <remarks>用于生成接口代理</remarks>
public abstract class DispatchProxyDecorator : DispatchProxy
{
    /// <summary>
    /// <inheritdoc cref="DispatchProxyDecorator"/>
    /// </summary>
    public DispatchProxyDecorator()
        : base()
    {
    }

    /// <summary>
    /// 目标实例对象
    /// </summary>
    public object? Target { get; private set; }

    /// <summary>
    /// 额外数据
    /// </summary>
    public Dictionary<object, object?>? Properties { get; protected set; }

    /// <summary>
    /// 生成代理装饰类
    /// </summary>
    /// <param name="interfaceType">接口类型</param>
    /// <param name="proxyType"><see cref="DispatchProxyDecorator"/> 类型</param>
    /// <param name="target">目标实例对象</param>
    /// <param name="properties">额外数据</param>
    /// <returns><see cref="object"/></returns>
    public static object? Decorate(Type interfaceType, Type proxyType, object? target = null, Dictionary<object, object?>? properties = null)
    {
        // 接口类型检查
        if (!interfaceType.IsInterface)
        {
            throw new InvalidOperationException($"Type '{interfaceType.Name}' is not an interface.");
        }

        // 代理类型检查
        if (!typeof(DispatchProxyDecorator).IsAssignableFrom(proxyType))
        {
            throw new InvalidOperationException($"Type '{proxyType.Name}' is not assignable from '{nameof(DispatchProxyDecorator)}'.");
        }

        // 检查 target 是否是接口实例类型
        if (target is not null && !interfaceType.IsInstanceOfType(target))
        {
            throw new InvalidOperationException($"The target object is not instance of type '{interfaceType.Name}'.");
        }

        // 输出调试事件
        Debugging.Trace("Creating {0} interface proxy class.", interfaceType.Name);

        // 创建代理类对象
        var proxyObject = Create(interfaceType, proxyType) as DispatchProxyDecorator;

        // 空检查
        ArgumentNullException.ThrowIfNull(proxyObject);

        // 设置目标对象数据
        proxyObject.Target = target;
        proxyObject.Properties = properties;

        return proxyObject;
    }

    /// <summary>
    /// 生成代理装饰类
    /// </summary>
    /// <typeparam name="TService">接口类型</typeparam>
    /// <param name="proxyType"><see cref="DispatchProxyDecorator"/> 类型</param>
    /// <param name="target">目标实例对象</param>
    /// <param name="properties">额外数据</param>
    /// <returns><typeparamref name="TService"/></returns>
    public static TService? Decorate<TService>(Type proxyType, TService? target = null, Dictionary<object, object?>? properties = null)
        where TService : class
    {
        return Decorate(typeof(TService), proxyType, target, properties) as TService;
    }

    /// <summary>
    /// 生成代理装饰类
    /// </summary>
    /// <typeparam name="TService">接口类型</typeparam>
    /// <typeparam name="TProxy"><see cref="DispatchProxyDecorator"/> 类型</typeparam>
    /// <param name="target">目标实例对象</param>
    /// <param name="properties">额外数据</param>
    /// <returns><typeparamref name="TService"/></returns>
    public static TService? Decorate<TService, TProxy>(TService? target = null, Dictionary<object, object?>? properties = null)
        where TService : class
        where TProxy : DispatchProxyDecorator
    {
        return Decorate(typeof(TService), typeof(TProxy), target, properties) as TService;
    }

    /// <summary>
    /// 同步拦截
    /// </summary>
    /// <returns><see cref="object"/></returns>
    public abstract object? Invoke(Invocation invocation);

    /// <summary>
    /// 异步无返回值拦截
    /// </summary>
    /// <returns><see cref="object"/></returns>
    public abstract Task InvokeAsync(Invocation invocation);

    /// <summary>
    /// 异步带返回值拦截
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <returns><typeparamref name="T"/></returns>
    public abstract Task<T?> InvokeAsync<T>(Invocation invocation);

    /// <summary>
    /// 重写拦截调用方法
    /// </summary>
    /// <param name="targetMethod">接口方法</param>
    /// <param name="args">方法传递参数</param>
    /// <returns><see cref="object"/></returns>
    protected override sealed object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(targetMethod);

        // 创建代理方法调用器
        var invocation = new Invocation(targetMethod, args, Target, Properties);

        // 获取方法返回值类型
        var returnType = targetMethod.ReturnType;

        // 处理返回值 Task 方法
        if (returnType == typeof(Task))
        {
            return InvokeAsync(invocation);
        }
        // 处理返回值 Task<> 方法
        else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            // 空检查
            ArgumentNullException.ThrowIfNull(_invokeAsyncOfTMethod);

            return _invokeAsyncOfTMethod.MakeGenericMethod(returnType.GenericTypeArguments)
                                        .Invoke(this, new[] { invocation });
        }
        // 处理同步方法
        else
        {
            return Invoke(invocation);
        }
    }

    /// <summary>
    /// <see cref="InvokeAsync{T}(Invocation)"/>
    /// </summary>
    internal static readonly MethodInfo? _invokeAsyncOfTMethod = typeof(DispatchProxyDecorator).GetMethod(nameof(InvokeAsync), 1, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, new[] { typeof(Invocation) }, null);
}