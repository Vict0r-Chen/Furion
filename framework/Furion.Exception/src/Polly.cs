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
/// 策略静态类
/// </summary>
public static class Polly
{
    /// <summary>
    /// 初始化重试策略
    /// </summary>
    /// <returns><see cref="RetryPolicy"/></returns>
    public static RetryPolicy Retry()
    {
        return Retry(1);
    }

    /// <summary>
    /// 初始化重试策略
    /// </summary>
    /// <typeparam name="TResult">操作返回值类型</typeparam>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public static RetryPolicy<TResult> Retry<TResult>()
    {
        return Retry<TResult>(1);
    }

    /// <summary>
    /// 初始化重试策略
    /// </summary>
    /// <param name="maxRetryCount">最大重试次数</param>
    /// <returns><see cref="RetryPolicy"/></returns>
    public static RetryPolicy Retry(uint maxRetryCount)
    {
        return new RetryPolicy
        {
            MaxRetryCount = maxRetryCount
        };
    }

    /// <summary>
    /// 初始化重试策略
    /// </summary>
    /// <typeparam name="TResult">操作返回值类型</typeparam>
    /// <param name="maxRetryCount">最大重试次数</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public static RetryPolicy<TResult> Retry<TResult>(uint maxRetryCount)
    {
        return new RetryPolicy<TResult>
        {
            MaxRetryCount = maxRetryCount
        };
    }

    /// <summary>
    /// 初始化超时策略
    /// </summary>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public static TimeoutPolicy Timeout()
    {
        return Timeout(TimeSpan.FromSeconds(10));
    }

    /// <summary>
    /// 初始化超时策略
    /// </summary>
    /// <typeparam name="TResult">操作返回值类型</typeparam>
    /// <returns><see cref="TimeoutPolicy{TResult}"/></returns>
    public static TimeoutPolicy<TResult> Timeout<TResult>()
    {
        return Timeout<TResult>(TimeSpan.FromSeconds(10));
    }

    /// <summary>
    /// 初始化超时策略
    /// </summary>
    /// <param name="timeout">超时时间（毫秒）</param>
    /// <returns><see cref="TimeoutPolicy"/></returns>
    public static TimeoutPolicy Timeout(double timeout)
    {
        return new TimeoutPolicy
        {
            Timeout = TimeSpan.FromMilliseconds(timeout)
        };
    }

    /// <summary>
    /// 初始化超时策略
    /// </summary>
    /// <param name="timeout">超时时间</param>
    /// <returns><see cref="TimeoutPolicy"/></returns>
    public static TimeoutPolicy Timeout(TimeSpan timeout)
    {
        return new TimeoutPolicy
        {
            Timeout = timeout
        };
    }

    /// <summary>
    /// 初始化超时策略
    /// </summary>
    /// <typeparam name="TResult">操作返回值类型</typeparam>
    /// <param name="timeout">超时时间（毫秒）</param>
    /// <returns><see cref="TimeoutPolicy{TResult}"/></returns>
    public static TimeoutPolicy<TResult> Timeout<TResult>(double timeout)
    {
        return new TimeoutPolicy<TResult>
        {
            Timeout = TimeSpan.FromMilliseconds(timeout)
        };
    }

    /// <summary>
    /// 初始化超时策略
    /// </summary>
    /// <typeparam name="TResult">操作返回值类型</typeparam>
    /// <param name="timeout">超时时间</param>
    /// <returns><see cref="TimeoutPolicy{TResult}"/></returns>
    public static TimeoutPolicy<TResult> Timeout<TResult>(TimeSpan timeout)
    {
        return new TimeoutPolicy<TResult>
        {
            Timeout = timeout
        };
    }

    /// <summary>
    /// 初始化后备策略
    /// </summary>
    /// <returns><see cref="FallbackPolicy{TResult}"/></returns>
    public static FallbackPolicy Fallback()
    {
        return new FallbackPolicy();
    }

    /// <summary>
    /// 初始化后备策略
    /// </summary>
    /// <typeparam name="TResult">操作返回值类型</typeparam>
    /// <returns><see cref="FallbackPolicy{TResult}"/></returns>
    public static FallbackPolicy<TResult> Fallback<TResult>()
    {
        return new FallbackPolicy<TResult>();
    }

    /// <summary>
    /// 初始化后备策略
    /// </summary>
    /// <param name="fallbackAction">后备操作方法</param>
    /// <returns><see cref="FallbackPolicy{TResult}"/></returns>
    public static FallbackPolicy Fallback(Func<FallbackPolicyContext<object>, object?> fallbackAction)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(fallbackAction);

        return new FallbackPolicy
        {
            FallbackAction = fallbackAction
        };
    }

    /// <summary>
    /// 初始化后备策略
    /// </summary>
    /// <typeparam name="TResult">操作返回值类型</typeparam>
    /// <param name="fallbackAction">后备操作方法</param>
    /// <returns><see cref="FallbackPolicy{TResult}"/></returns>
    public static FallbackPolicy<TResult> Fallback<TResult>(Func<FallbackPolicyContext<TResult>, TResult?> fallbackAction)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(fallbackAction);

        return new FallbackPolicy<TResult>
        {
            FallbackAction = fallbackAction
        };
    }
}