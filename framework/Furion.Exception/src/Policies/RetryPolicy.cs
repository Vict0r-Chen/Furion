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
/// 重试策略
/// </summary>
public class RetryPolicy : IExceptionPolicy
{
    /// <summary>
    /// 默认重试间隔
    /// </summary>
    internal static readonly TimeSpan _defaultRetryInterval = TimeSpan.FromSeconds(3);

    /// <summary>
    /// <inheritdoc cref="RetryPolicy" />
    /// </summary>
    public RetryPolicy()
    {
    }

    /// <summary>
    /// <inheritdoc cref="RetryPolicy" />
    /// </summary>
    /// <param name="retryExceptions">重试异常集合</param>
    public RetryPolicy(params Type[] retryExceptions)
    {
        RetryExceptions = retryExceptions;
    }

    /// <summary>
    /// 最大重试次数
    /// </summary>
    public int MaxRetryCount { get; set; }

    /// <summary>
    /// 重试间隔集合
    /// </summary>
    public TimeSpan[] RetryIntervals { get; set; } = Array.Empty<TimeSpan>();

    /// <summary>
    /// 条件
    /// </summary>
    public Func<System.Exception, bool>? Condition { get; set; }

    /// <summary>
    /// 重试异常集合
    /// </summary>
    public Type[]? RetryExceptions { get; set; }

    /// <summary>
    /// 每次重试触发的回调
    /// </summary>
    public Action<System.Exception, int>? RetryAction { get; set; }

    /// <summary>
    /// 检查是否需要处理异常
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    internal bool ShouldHandle(System.Exception exception)
    {
        if (Condition is not null && Condition(exception))
        {
            return false;
        }

        return RetryExceptions.IsNullOrEmpty()
            || RetryExceptions!.Any(ex => ex.IsInstanceOfType(exception));
    }

    /// <inheritdoc />
    public void Execute(Action predicate)
    {
        Execute(() =>
        {
            predicate();
            return 0;
        });
    }

    /// <inheritdoc />
    public TResult Execute<TResult>(Func<TResult> predicate)
    {
        var retryCount = 0;

        while (true)
        {
            try
            {
                return predicate();
            }
            catch (System.Exception ex)
            {
                retryCount++;

                if (!ShouldHandle(ex) || retryCount > MaxRetryCount)
                {
                    throw;
                }

                RetryAction?.Invoke(ex, retryCount);

                var retryInterval = ResolveRetryInterval(retryCount);
                Thread.Sleep(retryInterval);
            }
        }
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(Func<Task> predicate, CancellationToken cancellationToken)
    {
        await ExecuteAsync(async () =>
        {
            await predicate();
            return 0;
        }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> predicate, CancellationToken cancellationToken)
    {
        var retryCount = 0;

        while (true)
        {
            try
            {
                return await predicate();
            }
            catch (System.Exception ex)
            {
                retryCount++;

                if (!ShouldHandle(ex) || retryCount > MaxRetryCount)
                {
                    throw;
                }

                RetryAction?.Invoke(ex, retryCount);

                var retryInterval = ResolveRetryInterval(retryCount);
                await Task.Delay(retryInterval, cancellationToken);
            }
        }
    }

    internal TimeSpan ResolveRetryInterval(int retryCount)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(RetryIntervals);

        if (RetryIntervals.Length == 0)
        {
            return _defaultRetryInterval;
        }

        return RetryIntervals[retryCount % RetryIntervals.Length];
    }
}

/// <summary>
/// 重试策略
/// </summary>
/// <typeparam name="TException"><see cref="System.Exception" /></typeparam>
public sealed class RetryPolicy<TException> : RetryPolicy
    where TException : System.Exception
{
    /// <summary>
    /// <inheritdoc cref="RetryPolicy{TException}"/>
    /// </summary>
    public RetryPolicy()
        : base(typeof(TException))
    {
    }
}

/// <summary>
/// 重试策略
/// </summary>
/// <typeparam name="TException1"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException2"><see cref="System.Exception" /></typeparam>
public sealed class RetryPolicy<TException1, TException2> : RetryPolicy
    where TException1 : System.Exception
    where TException2 : System.Exception
{
    /// <summary>
    /// <inheritdoc cref="RetryPolicy{TException1, TException2}"/>
    /// </summary>
    public RetryPolicy()
        : base(typeof(TException1)
            , typeof(TException2))
    {
    }
}

/// <summary>
/// 重试策略
/// </summary>
/// <typeparam name="TException1"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException2"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException3"><see cref="System.Exception" /></typeparam>
public sealed class RetryPolicy<TException1, TException2, TException3> : RetryPolicy
    where TException1 : System.Exception
    where TException2 : System.Exception
    where TException3 : System.Exception
{
    /// <summary>
    /// <inheritdoc cref="RetryPolicy{TException1, TException2, TException3}"/>
    /// </summary>
    public RetryPolicy()
        : base(typeof(TException1)
            , typeof(TException2)
            , typeof(TException3))
    {
    }
}

/// <summary>
/// 重试策略
/// </summary>
/// <typeparam name="TException1"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException2"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException3"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException4"><see cref="System.Exception" /></typeparam>
public sealed class RetryPolicy<TException1, TException2, TException3, TException4> : RetryPolicy
    where TException1 : System.Exception
    where TException2 : System.Exception
    where TException3 : System.Exception
    where TException4 : System.Exception
{
    /// <summary>
    /// <inheritdoc cref="RetryPolicy{TException1, TException2, TException3, TException4}"/>
    /// </summary>
    public RetryPolicy()
        : base(typeof(TException1)
            , typeof(TException2)
            , typeof(TException3)
            , typeof(TException4))
    {
    }
}

/// <summary>
/// 重试策略
/// </summary>
/// <typeparam name="TException1"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException2"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException3"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException4"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException5"><see cref="System.Exception" /></typeparam>
public sealed class RetryPolicy<TException1, TException2, TException3, TException4, TException5> : RetryPolicy
    where TException1 : System.Exception
    where TException2 : System.Exception
    where TException3 : System.Exception
    where TException4 : System.Exception
    where TException5 : System.Exception
{
    /// <summary>
    /// <inheritdoc cref="RetryPolicy{TException1, TException2, TException3, TException4, TException5}"/>
    /// </summary>
    public RetryPolicy()
        : base(typeof(TException1)
            , typeof(TException2)
            , typeof(TException3)
            , typeof(TException4)
            , typeof(TException5))
    {
    }
}

/// <summary>
/// 重试策略
/// </summary>
/// <typeparam name="TException1"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException2"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException3"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException4"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException5"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException6"><see cref="System.Exception" /></typeparam>
public sealed class RetryPolicy<TException1, TException2, TException3, TException4, TException5, TException6> : RetryPolicy
    where TException1 : System.Exception
    where TException2 : System.Exception
    where TException3 : System.Exception
    where TException4 : System.Exception
    where TException5 : System.Exception
    where TException6 : System.Exception
{
    /// <summary>
    /// <inheritdoc cref="RetryPolicy{TException1, TException2, TException3, TException4, TException5, TException6}"/>
    /// </summary>
    public RetryPolicy()
        : base(typeof(TException1)
            , typeof(TException2)
            , typeof(TException3)
            , typeof(TException4)
            , typeof(TException5)
            , typeof(TException6))
    {
    }
}

/// <summary>
/// 重试策略
/// </summary>
/// <typeparam name="TException1"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException2"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException3"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException4"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException5"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException6"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException7"><see cref="System.Exception" /></typeparam>
public sealed class RetryPolicy<TException1, TException2, TException3, TException4, TException5, TException6, TException7> : RetryPolicy
    where TException1 : System.Exception
    where TException2 : System.Exception
    where TException3 : System.Exception
    where TException4 : System.Exception
    where TException5 : System.Exception
    where TException6 : System.Exception
    where TException7 : System.Exception
{
    /// <summary>
    /// <inheritdoc cref="RetryPolicy{TException1, TException2, TException3, TException4, TException5, TException6, TException7}"/>
    /// </summary>
    public RetryPolicy()
        : base(typeof(TException1)
            , typeof(TException2)
            , typeof(TException3)
            , typeof(TException4)
            , typeof(TException5)
            , typeof(TException6)
            , typeof(TException7))
    {
    }
}

/// <summary>
/// 重试策略
/// </summary>
/// <typeparam name="TException1"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException2"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException3"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException4"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException5"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException6"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException7"><see cref="System.Exception" /></typeparam>
/// <typeparam name="TException8"><see cref="System.Exception" /></typeparam>
public sealed class RetryPolicy<TException1, TException2, TException3, TException4, TException5, TException6, TException7, TException8> : RetryPolicy
    where TException1 : System.Exception
    where TException2 : System.Exception
    where TException3 : System.Exception
    where TException4 : System.Exception
    where TException5 : System.Exception
    where TException6 : System.Exception
    where TException7 : System.Exception
    where TException8 : System.Exception
{
    /// <summary>
    /// <inheritdoc cref="RetryPolicy{TException1, TException2, TException3, TException4, TException5, TException6, TException7, TException8}"/>
    /// </summary>
    public RetryPolicy()
        : base(typeof(TException1)
            , typeof(TException2)
            , typeof(TException3)
            , typeof(TException4)
            , typeof(TException5)
            , typeof(TException6)
            , typeof(TException7)
            , typeof(TException8))
    {
    }
}