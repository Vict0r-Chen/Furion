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
public sealed class RetryPolicy : IExceptionPolicy
{
    /// <summary>
    /// <inheritdoc cref="RetryPolicy" />
    /// </summary>
    public RetryPolicy()
    {
        RetryIntervals = new[] { TimeSpan.FromSeconds(1) };
    }

    public int MaxRetryCount { get; set; }

    public TimeSpan[] RetryIntervals { get; set; }

    // 差参数 Exception
    public Func<bool>? Condition { get; set; }

    public Type[]? RetryExceptions { get; set; }

    public Action<System.Exception>? RetryCallback { get; set; }

    internal bool ShouldRetry(System.Exception exception)
    {
        if (RetryExceptions == null || RetryExceptions.Length == 0)
        {
            return true;
        }

        if (RetryExceptions is null || RetryExceptions.Length == 0)
        {
            return true;
        }

        for (int i = 0; i < RetryExceptions.Length; i++)
        {
            if (RetryExceptions[i].IsInstanceOfType(exception))
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc />
    public void Execute(Action predicate)
    {
        Execute<object>(() =>
        {
            predicate();
            return null!;
        });
    }

    /// <inheritdoc />
    public TResult Execute<TResult>(Func<TResult> predicate)
    {
        int retryCount = 0;
        while (retryCount <= MaxRetryCount && (Condition == null || Condition()))
        {
            try
            {
                return predicate();
            }
            catch (System.Exception ex)
            {
                if (retryCount == MaxRetryCount - 1 || !ShouldRetry(ex))
                {
                    throw;
                }
                else
                {
                    Console.WriteLine($"正在重试第 {retryCount} 次");
                    int intervalIndex = RetryIntervals.Length > 0 ? retryCount % RetryIntervals.Length : 0;

                    Thread.Sleep(RetryIntervals[intervalIndex]);
                }
            }
            finally
            {
                retryCount++;
            }
        }

        return default!;
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(Func<Task> predicate, CancellationToken cancellationToken)
    {
        await ExecuteAsync<object>(async () =>
        {
            await predicate();
            return Task.FromResult<object>(null!);
        }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> predicate, CancellationToken cancellationToken)
    {
        int retryCount = 0;
        while (retryCount < MaxRetryCount && (Condition == null || Condition()))
        {
            try
            {
                return await predicate();
            }
            catch (System.Exception ex)
            {
                if (retryCount == MaxRetryCount - 1 || !ShouldRetry(ex))
                {
                    throw;
                }
                else
                {
                    int intervalIndex = RetryIntervals.Length > 0 ? retryCount % RetryIntervals.Length : 0;

                    await Task.Delay(RetryIntervals[0], cancellationToken);
                }
            }
            finally
            {
                retryCount++;
            }
        }

        return default!;
    }
}