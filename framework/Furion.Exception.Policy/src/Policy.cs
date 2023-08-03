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

namespace Furion.Exception;

/// <summary>
/// 异常策略静态类
/// </summary>
public static class Policy
{
    /// <summary>
    /// 添加自定义策略
    /// </summary>
    /// <typeparam name="TPolicy"><see cref="PolicyBase{TResult}"/></typeparam>
    /// <returns><typeparamref name="TPolicy"/></returns>
    public static TPolicy For<TPolicy>()
        where TPolicy : PolicyBase<object>, new()
    {
        return new();
    }

    /// <summary>
    /// 添加自定义策略
    /// </summary>
    /// <typeparam name="TPolicy"><see cref="PolicyBase{TResult}"/></typeparam>
    /// <param name="policy"><typeparamref name="TPolicy"/></param>
    /// <returns><typeparamref name="TPolicy"/></returns>
    public static TPolicy For<TPolicy>(TPolicy policy)
        where TPolicy : PolicyBase<object>
    {
        return policy;
    }

    /// <summary>
    /// 初始化重试策略（默认 3 次）
    /// </summary>
    /// <returns><see cref="RetryPolicy"/></returns>
    public static RetryPolicy Retry()
    {
        return Retry(3);
    }

    /// <summary>
    /// 初始化重试策略
    /// </summary>
    /// <param name="maxRetryCount">最大重试次数</param>
    /// <returns><see cref="RetryPolicy"/></returns>
    public static RetryPolicy Retry(int maxRetryCount)
    {
        return new(maxRetryCount);
    }

    /// <summary>
    /// 初始化超时策略（默认 10 秒）
    /// </summary>
    /// <remarks>
    /// <para>若需要测试同步阻塞，请使用 <c>Task.Delay(...).Wait()</c> 替代 <c>Thread.Sleep(...)</c></para>
    /// <para>若需使用 <c>Thread.Sleep(...)</c> 可以使用 <c>Task.Run(()=> ...)</c> 包装代码逻辑</para>
    /// </remarks>
    /// <returns><see cref="TimeoutPolicy"/></returns>
    public static TimeoutPolicy Timeout()
    {
        return Timeout(TimeSpan.FromSeconds(10));
    }

    /// <summary>
    /// 初始化超时策略
    /// </summary>
    /// <remarks>
    /// <para>若需要测试同步阻塞，请使用 <c>Task.Delay(...).Wait()</c> 替代 <c>Thread.Sleep(...)</c></para>
    /// <para>若需使用 <c>Thread.Sleep(...)</c> 可以使用 <c>Task.Run(()=> ...)</c> 包装代码逻辑</para>
    /// </remarks>
    /// <param name="timeout">超时时间（毫秒）</param>
    /// <returns><see cref="TimeoutPolicy"/></returns>
    public static TimeoutPolicy Timeout(double timeout)
    {
        return new(timeout);
    }

    /// <summary>
    /// 初始化超时策略
    /// </summary>
    /// <remarks>
    /// <para>若需要测试同步阻塞，请使用 <c>Task.Delay(...).Wait()</c> 替代 <c>Thread.Sleep(...)</c></para>
    /// <para>若需使用 <c>Thread.Sleep(...)</c> 可以使用 <c>Task.Run(()=> ...)</c> 包装代码逻辑</para>
    /// </remarks>
    /// <param name="timeout">超时时间</param>
    /// <returns><see cref="TimeoutPolicy"/></returns>
    public static TimeoutPolicy Timeout(TimeSpan timeout)
    {
        return new(timeout);
    }

    /// <summary>
    /// 初始化后备策略
    /// </summary>
    /// <returns><see cref="FallbackPolicy"/></returns>
    public static FallbackPolicy Fallback()
    {
        return new();
    }

    /// <summary>
    /// 初始化后备策略
    /// </summary>
    /// <param name="fallbackAction">后备操作方法</param>
    /// <returns><see cref="FallbackPolicy"/></returns>
    public static FallbackPolicy Fallback(Func<FallbackPolicyContext<object>, object?> fallbackAction)
    {
        return new(fallbackAction);
    }

    /// <summary>
    /// 初始化后备策略
    /// </summary>
    /// <param name="fallbackAction">后备操作方法</param>
    /// <returns><see cref="FallbackPolicy"/></returns>
    public static FallbackPolicy Fallback(Action<FallbackPolicyContext<object>> fallbackAction)
    {
        return new(fallbackAction);
    }

    /// <summary>
    /// 初始化组合策略
    /// </summary>
    /// <returns><see cref="CompositePolicy"/></returns>
    public static CompositePolicy Composite()
    {
        return new();
    }

    /// <summary>
    /// 初始化组合策略
    /// </summary>
    /// <param name="policies">策略集合</param>
    /// <returns><see cref="CompositePolicy"/></returns>
    public static CompositePolicy Composite(params PolicyBase<object>[] policies)
    {
        return new(policies);
    }

    /// <summary>
    /// 初始化组合策略
    /// </summary>
    /// <param name="policies">策略集合</param>
    /// <returns><see cref="CompositePolicy"/></returns>
    public static CompositePolicy Composite(IEnumerable<PolicyBase<object>> policies)
    {
        return new(policies);
    }
}

/// <summary>
/// 异常策略静态类
/// </summary>
/// <typeparam name="TResult">操作返回值类型</typeparam>
public static class Policy<TResult>
{
    /// <summary>
    /// 添加自定义策略
    /// </summary>
    /// <typeparam name="TPolicy"><see cref="PolicyBase{TResult}"/></typeparam>
    /// <returns><typeparamref name="TPolicy"/></returns>
    public static TPolicy For<TPolicy>()
        where TPolicy : PolicyBase<TResult>, new()
    {
        return new();
    }

    /// <summary>
    /// 添加自定义策略
    /// </summary>
    /// <typeparam name="TPolicy"><see cref="PolicyBase{TResult}"/></typeparam>
    /// <param name="policy"><typeparamref name="TPolicy"/></param>
    /// <returns><typeparamref name="TPolicy"/></returns>
    public static TPolicy For<TPolicy>(TPolicy policy)
        where TPolicy : PolicyBase<TResult>
    {
        return policy;
    }

    /// <summary>
    /// 初始化重试策略（默认 3 次）
    /// </summary>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public static RetryPolicy<TResult> Retry()
    {
        return Retry(3);
    }

    /// <summary>
    /// 初始化重试策略
    /// </summary>
    /// <param name="maxRetryCount">最大重试次数</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public static RetryPolicy<TResult> Retry(int maxRetryCount)
    {
        return new(maxRetryCount);
    }

    /// <summary>
    /// 初始化超时策略（默认 10 秒）
    /// </summary>
    /// <remarks>
    /// <para>若需要测试同步阻塞，请使用 <c>Task.Delay(...).Wait()</c> 替代 <c>Thread.Sleep(...)</c></para>
    /// <para>若需使用 <c>Thread.Sleep(...)</c> 可以使用 <c>Task.Run(()=> ...)</c> 包装代码逻辑</para>
    /// </remarks>
    /// <returns><see cref="TimeoutPolicy{TResult}"/></returns>
    public static TimeoutPolicy<TResult> Timeout()
    {
        return Timeout(TimeSpan.FromSeconds(10));
    }

    /// <summary>
    /// 初始化超时策略
    /// </summary>
    /// <remarks>
    /// <para>若需要测试同步阻塞，请使用 <c>Task.Delay(...).Wait()</c> 替代 <c>Thread.Sleep(...)</c></para>
    /// <para>若需使用 <c>Thread.Sleep(...)</c> 可以使用 <c>Task.Run(()=> ...)</c> 包装代码逻辑</para>
    /// </remarks>
    /// <param name="timeout">超时时间（毫秒）</param>
    /// <returns><see cref="TimeoutPolicy{TResult}"/></returns>
    public static TimeoutPolicy<TResult> Timeout(double timeout)
    {
        return new(timeout);
    }

    /// <summary>
    /// 初始化超时策略
    /// </summary>
    /// <remarks>
    /// <para>若需要测试同步阻塞，请使用 <c>Task.Delay(...).Wait()</c> 替代 <c>Thread.Sleep(...)</c></para>
    /// <para>若需使用 <c>Thread.Sleep(...)</c> 可以使用 <c>Task.Run(()=> ...)</c> 包装代码逻辑</para>
    /// </remarks>
    /// <param name="timeout">超时时间</param>
    /// <returns><see cref="TimeoutPolicy{TResult}"/></returns>
    public static TimeoutPolicy<TResult> Timeout(TimeSpan timeout)
    {
        return new(timeout);
    }

    /// <summary>
    /// 初始化后备策略
    /// </summary>
    /// <returns><see cref="FallbackPolicy{TResult}"/></returns>
    public static FallbackPolicy<TResult> Fallback()
    {
        return new();
    }

    /// <summary>
    /// 初始化后备策略
    /// </summary>
    /// <param name="fallbackAction">后备操作方法</param>
    /// <returns><see cref="FallbackPolicy{TResult}"/></returns>
    public static FallbackPolicy<TResult> Fallback(Func<FallbackPolicyContext<TResult>, TResult?> fallbackAction)
    {
        return new(fallbackAction);
    }

    /// <summary>
    /// 初始化后备策略
    /// </summary>
    /// <param name="fallbackAction">后备操作方法</param>
    /// <returns><see cref="FallbackPolicy{TResult}"/></returns>
    public static FallbackPolicy<TResult> Fallback(Action<FallbackPolicyContext<TResult>> fallbackAction)
    {
        return new(fallbackAction);
    }

    /// <summary>
    /// 初始化组合策略
    /// </summary>
    /// <returns><see cref="CompositePolicy{TResult}"/></returns>
    public static CompositePolicy<TResult> Composite()
    {
        return new();
    }

    /// <summary>
    /// 初始化组合策略
    /// </summary>
    /// <param name="policies">策略集合</param>
    /// <returns><see cref="CompositePolicy{TResult}"/></returns>
    public static CompositePolicy<TResult> Composite(params PolicyBase<TResult>[] policies)
    {
        return new(policies);
    }

    /// <summary>
    /// 初始化组合策略
    /// </summary>
    /// <param name="policies">策略集合</param>
    /// <returns><see cref="CompositePolicy{TResult}"/></returns>
    public static CompositePolicy<TResult> Composite(IEnumerable<PolicyBase<TResult>> policies)
    {
        return new(policies);
    }
}