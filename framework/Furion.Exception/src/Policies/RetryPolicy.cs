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
public sealed class RetryPolicy : RetryPolicy<object>
{
    /// <summary>
    /// 执行操作方法
    /// </summary>
    /// <param name="operation">操作方法</param>
    public void Execute(Action operation)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        // 执行操作方法
        Execute(() =>
        {
            operation();
            return default;
        });
    }

    /// <summary>
    /// 执行操作方法
    /// </summary>
    /// <param name="operation">操作方法</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task{TResult}"/></returns>
    public async Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        // 执行操作方法
        await ExecuteAsync(async () =>
        {
            await operation();
            return default;
        }, cancellationToken);
    }
}

/// <summary>
/// 重试策略
/// </summary>
/// <typeparam name="TResult">执行方法返回值类型</typeparam>
public class RetryPolicy<TResult>
{
    /// <summary>
    /// <inheritdoc cref="RetryPolicy{TResult}" />
    /// </summary>
    public RetryPolicy()
    {
    }

    /// <summary>
    /// <inheritdoc cref="RetryPolicy{TResult}" />
    /// </summary>
    /// <param name="handleExceptions">捕获的异常集合</param>
    public RetryPolicy(params Type[] handleExceptions)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(handleExceptions);

        HandleExceptions = handleExceptions;
    }

    /// <summary>
    /// 最大重试次数
    /// </summary>
    public uint MaxRetryCount { get; set; }

    /// <summary>
    /// 重试间隔集合
    /// </summary>
    public TimeSpan[]? RetryIntervals { get; set; }

    /// <summary>
    /// 重试触发操作
    /// </summary>
    public Action<RetryPolicyContext<TResult>>? RetryAction { get; set; }

    /// <summary>
    /// 条件
    /// </summary>
    public Func<RetryPolicyContext<TResult>, bool>? Condition { get; set; }

    /// <summary>
    /// 捕获的异常集合
    /// </summary>
    public Type[]? HandleExceptions { get; set; }

    /// <summary>
    /// 检查是否可以执行重试策略
    /// </summary>
    /// <param name="context"><see cref="RetryPolicyContext{TResult}"/></param>
    /// <returns><see cref="bool"/></returns>
    internal bool ShouldRetry(RetryPolicyContext<TResult> context)
    {
        // 检查最大重试次数是否大于 0
        if (MaxRetryCount <= 0)
        {
            return false;
        }

        // 检查是否存在异常
        if (context.Exception is not null)
        {
            // 检查是否定义了需要捕获的异常
            if (HandleExceptions.IsNullOrEmpty()
                || HandleExceptions!.Any(ex => ex.IsInstanceOfType(context.Exception)))
            {
                // 检查是否配置了条件
                if (Condition is not null)
                {
                    return Condition(context);
                }

                return true;
            }
        }

        // 检查是否配置了条件
        if (Condition is not null)
        {
            return Condition(context);
        }

        return false;
    }

    /// <summary>
    /// 添加捕获异常类型
    /// </summary>
    /// <typeparam name="TException"><see cref="System.Exception"/></typeparam>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> Handle<TException>()
        where TException : System.Exception
    {
        HandleExceptions ??= Array.Empty<Type>();
        HandleExceptions[HandleExceptions.Length] = typeof(TException);

        return this;
    }

    /// <summary>
    /// 添加捕获异常类型
    /// </summary>
    /// <typeparam name="TException"><see cref="System.Exception"/></typeparam>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> Or<TException>()
        where TException : System.Exception
    {
        return Handle<TException>();
    }

    /// <summary>
    /// 添加重试间隔
    /// </summary>
    /// <param name="retryIntervals"></param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> WaitAndRetry(params TimeSpan[] retryIntervals)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(retryIntervals);

        RetryIntervals = retryIntervals;

        return this;
    }

    /// <summary>
    /// 添加条件
    /// </summary>
    /// <param name="condition">条件</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> OrResult(Func<RetryPolicyContext<TResult>, bool> condition)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(condition);

        Condition = condition;

        return this;
    }

    /// <summary>
    /// 执行操作方法
    /// </summary>
    /// <param name="operation">操作方法</param>
    /// <returns><typeparamref name="TResult"/></returns>
    public TResult? Execute(Func<TResult?> operation)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        // 初始化重试策略上下文
        var context = new RetryPolicyContext<TResult>
        {
            HandleExceptions = HandleExceptions
        };

        while (true)
        {
            try
            {
                // 获取执行结果
                context.Result = operation();
            }
            catch (System.Exception ex)
            {
                context.Exception = ex;
            }

            // 检查是否可以执行重试策略
            if (ShouldRetry(context))
            {
                // 递增重试次数
                context.RetryCount++;

                // 检查重试次数是否大于最大重试次数
                if (context.RetryCount > MaxRetryCount)
                {
                    // 返回结果或抛出异常
                    return ReturnResultOrThrow(context);
                }

                // 调用重试触发操作
                RetryAction?.Invoke(context);

                // 检查是否配置了重试间隔
                if (RetryIntervals is { Length: > 0 })
                {
                    Thread.Sleep(RetryIntervals[context.RetryCount % RetryIntervals.Length]);
                }

                continue;
            }

            // 返回结果或抛出异常
            return ReturnResultOrThrow(context);
        }
    }

    /// <summary>
    /// 执行操作方法
    /// </summary>
    /// <param name="operation">操作方法</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task{TResult}"/></returns>
    public async Task<TResult?> ExecuteAsync(Func<Task<TResult?>> operation, CancellationToken cancellationToken = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        // 初始化重试策略上下文
        var context = new RetryPolicyContext<TResult>
        {
            HandleExceptions = HandleExceptions
        };

        while (true)
        {
            try
            {
                // 获取执行结果
                context.Result = await operation();
            }
            catch (System.Exception ex)
            {
                context.Exception = ex;
            }

            // 检查是否可以执行重试策略
            if (ShouldRetry(context))
            {
                // 递增重试次数
                context.RetryCount++;

                // 检查重试次数是否大于最大重试次数
                if (context.RetryCount > MaxRetryCount)
                {
                    // 返回结果或抛出异常
                    return ReturnResultOrThrow(context);
                }

                // 调用重试触发操作
                RetryAction?.Invoke(context);

                // 检查是否配置了重试间隔
                if (RetryIntervals is { Length: > 0 })
                {
                    await Task.Delay(RetryIntervals[context.RetryCount % RetryIntervals.Length], cancellationToken);
                }

                continue;
            }

            // 返回结果或抛出异常
            return ReturnResultOrThrow(context);
        }
    }

    /// <summary>
    /// 返回结果或抛出异常
    /// </summary>
    /// <param name="context"><see cref="RetryPolicyContext{TResult}"/></param>
    /// <returns><typeparamref name="TResult"/></returns>
    internal static TResult? ReturnResultOrThrow(RetryPolicyContext<TResult> context)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(context);

        // 检查是否存在异常
        if (context.Exception is not null)
        {
            throw context.Exception;
        }

        return context.Result;
    }
}