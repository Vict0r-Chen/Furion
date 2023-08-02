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
/// 超时策略
/// </summary>
public sealed class TimeoutPolicy : TimeoutPolicy<object>
{
}

/// <summary>
/// 超时策略
/// </summary>
/// <typeparam name="TResult">操作返回值类型</typeparam>
public class TimeoutPolicy<TResult> : PolicyBase<TResult>
{
    /// <summary>
    /// 超时消息
    /// </summary>
    internal const string TIMEOUT_MESSAGE = "The operation has timed out.";

    /// <summary>
    /// 超时时间
    /// </summary>
    public TimeSpan Timeout { get; set; }

    /// <summary>
    /// 超时时操作方法
    /// </summary>
    public Action<TimeoutPolicyContext<TResult>>? TimeoutAction { get; set; }

    /// <summary>
    /// 添加超时时操作方法
    /// </summary>
    /// <param name="timeoutAction">超时时操作方法</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public TimeoutPolicy<TResult> OnTimeout(Action<TimeoutPolicyContext<TResult>> timeoutAction)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(timeoutAction);

        TimeoutAction = timeoutAction;

        return this;
    }

    /// <inheritdoc />
    public override async Task<TResult?> ExecuteAsync(Func<Task<TResult?>> operation, CancellationToken cancellationToken = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        // 创建关键的取消标记
        using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        // 设置超时时间
        cancellationTokenSource.CancelAfter(Timeout);

        try
        {
            // 初始化一个超时任务
            var timeoutTask = Task.Delay(Timeout, cancellationTokenSource.Token);

            // 获取操作方法任务
            var operationTask = operation();

            // 等待超时任务和操作方法任务任何一个完成
            await Task.WhenAny(timeoutTask, operationTask);

            // 检查超时任务是否提前完成
            if (timeoutTask.Status == TaskStatus.RanToCompletion)
            {
                // 抛出超时异常
                ThrowTimeoutException();
            }

            // 检查是否存在取消请求
            cancellationToken.ThrowIfCancellationRequested();

            // 返回操作方法结果
            return await operationTask;
        }
        catch (OperationCanceledException exception) when (exception.CancellationToken == cancellationTokenSource.Token)
        {
            // 抛出超时异常
            ThrowTimeoutException();
        }

        return default;
    }

    /// <summary>
    /// 抛出超时异常
    /// </summary>
    /// <exception cref="TimeoutException"></exception>
    [DoesNotReturn]
    internal void ThrowTimeoutException()
    {
        // 输出调试事件
        Debugging.Error(TIMEOUT_MESSAGE);

        // 调用重试时操作方法
        TimeoutAction?.Invoke(new()
        {
            PolicyName = PolicyName
        });

        // 抛出超时异常
        throw new TimeoutException(TIMEOUT_MESSAGE);
    }
}