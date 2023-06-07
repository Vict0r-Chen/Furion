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
/// <see cref="DispatchProxy"/> 装饰器
/// </summary>
/// <typeparam name="TService">接口类型</typeparam>
/// <remarks>用于生成接口代理</remarks>
public abstract class DispatchProxyDecorator<TService> : DispatchProxy
    where TService : class
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DispatchProxyDecorator()
        : base()
    {
        // 接口类型检查
        if (!typeof(TService).IsInterface)
        {
            throw new InvalidOperationException($"Type '{typeof(TService).Name}' is not an interface.");
        }
    }

    /// <summary>
    /// <typeparamref name="TService"/> 实现类实例
    /// </summary>
    public TService? Target { get; private set; }

    /// <summary>
    /// 额外数据
    /// </summary>
    public Dictionary<object, object?>? Properties { get; protected set; }

    /// <summary>
    /// 生成代理装饰类
    /// </summary>
    /// <param name="proxyType"><see cref="DispatchProxyDecorator{TService}"/> 类型</param>
    /// <param name="target">目标实例对象</param>
    /// <param name="properties">额外数据</param>
    /// <returns><typeparamref name="TService"/></returns>
    public static TService? Decorate(Type proxyType, TService? target = null, Dictionary<object, object?>? properties = null)
    {
        // 代理类型检查
        if (!typeof(DispatchProxyDecorator<TService>).IsAssignableFrom(proxyType))
        {
            throw new InvalidOperationException($"Type '{proxyType.Name}' is not assignable from '{nameof(DispatchProxyDecorator<TService>)}'.");
        }

        // 创建代理类对象
        var proxy = Create(typeof(TService), proxyType) as DispatchProxyDecorator<TService>;

        // 空检查
        ArgumentNullException.ThrowIfNull(proxy, nameof(proxy));

        // 设置目标对象数据
        proxy.Target = target;
        proxy.Properties = properties ?? new();

        return proxy as TService;
    }

    /// <summary>
    /// 生成代理装饰类
    /// </summary>
    /// <typeparam name="TProxy"><see cref="DispatchProxyDecorator{TService}"/> 类型</typeparam>
    /// <param name="target">目标实例对象</param>
    /// <param name="properties">额外数据</param>
    /// <returns><typeparamref name="TService"/></returns>
    public static TService? Decorate<TProxy>(TService? target = null, Dictionary<object, object?>? properties = null)
        where TProxy : DispatchProxyDecorator<TService>
    {
        return Decorate(typeof(TProxy), target, properties);
    }

    /// <summary>
    /// 同步拦截
    /// </summary>
    /// <param name="invocation"><typeparamref name="TService"/></param>
    /// <returns><see cref="object"/></returns>
    public abstract object? Invoke(Invocation<TService> invocation);

    /// <summary>
    /// 异步无返回值拦截
    /// </summary>
    /// <param name="invocation"><typeparamref name="TService"/></param>
    /// <returns><see cref="object"/></returns>
    public abstract Task InvokeAsync(Invocation<TService> invocation);

    /// <summary>
    /// 异步带返回值拦截
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="invocation"><typeparamref name="TService"/></param>
    /// <returns><typeparamref name="T"/></returns>
    public abstract Task<T?> InvokeAsync<T>(Invocation<TService> invocation);

    /// <summary>
    /// 重写拦截调用方法
    /// </summary>
    /// <param name="targetMethod">接口方法</param>
    /// <param name="args">方法传递参数</param>
    /// <returns><see cref="object"/></returns>
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(targetMethod, nameof(targetMethod));

        // 创建代理方法调用器
        var invocation = new Invocation<TService>(targetMethod, args, Target, Properties);

        // 方法返回值类型
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
            ArgumentNullException.ThrowIfNull(_invokeAsyncOfTMethod, nameof(_invokeAsyncOfTMethod));

            return _invokeAsyncOfTMethod.MakeGenericMethod(returnType.GenericTypeArguments)
                                        .Invoke(this, new[] { invocation });
        }
        // 处理同步方法
        else
        {
            return !targetMethod.IsGenericMethod
                      ? Invoke(invocation)
                      : invocation.Proceed();
        }
    }

    /// <summary>
    /// <see cref="InvokeAsync{T}(Invocation{TService})"/>
    /// </summary>
    private static readonly MethodInfo? _invokeAsyncOfTMethod = typeof(DispatchProxyDecorator<TService>).GetMethod(nameof(InvokeAsync), 1, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, new[] { typeof(Invocation<TService>) }, null);
}