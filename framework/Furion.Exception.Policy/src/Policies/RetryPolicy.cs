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
}

/// <summary>
/// 重试策略
/// </summary>
/// <typeparam name="TResult">操作返回值类型</typeparam>
public class RetryPolicy<TResult> : AbstractPolicy<TResult>
{
    /// <summary>
    /// 操作结果条件集合
    /// </summary>
    public IList<Func<RetryPolicyContext<TResult>, bool>>? ResultConditions { get; set; }

    /// <summary>
    /// 捕获的异常集合
    /// </summary>
    public IList<Type>? HandleExceptions { get; set; }

    /// <summary>
    /// 捕获的内部异常集合
    /// </summary>
    public IList<Type>? HandleInnerExceptions { get; set; }

    /// <summary>
    /// 最大重试次数
    /// </summary>
    public uint MaxRetryCount { get; set; }

    /// <summary>
    /// 重试间隔集合
    /// </summary>
    public TimeSpan[]? RetryIntervals { get; set; }

    /// <summary>
    /// 重试时操作方法
    /// </summary>
    public Action<RetryPolicyContext<TResult>>? RetryAction { get; set; }

    /// <summary>
    /// 添加捕获异常类型
    /// </summary>
    /// <typeparam name="TException"><see cref="System.Exception"/></typeparam>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> Handle<TException>()
        where TException : System.Exception
    {
        HandleExceptions ??= new List<Type>();
        HandleExceptions.Add(typeof(TException));

        return this;
    }

    /// <summary>
    /// 添加捕获异常类型
    /// </summary>
    /// <typeparam name="TException"><see cref="System.Exception"/></typeparam>
    /// <param name="exceptionCondition">异常条件</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> Handle<TException>(Func<TException, bool> exceptionCondition)
        where TException : System.Exception
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(exceptionCondition);

        // 添加捕获异常类型和条件
        Handle<TException>();
        HandleResult(context => context.Exception is TException exception && exceptionCondition(exception));

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
    /// 添加捕获异常类型
    /// </summary>
    /// <typeparam name="TException"><see cref="System.Exception"/></typeparam>
    /// <param name="exceptionCondition">异常条件</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> Or<TException>(Func<TException, bool> exceptionCondition)
        where TException : System.Exception
    {
        return Handle(exceptionCondition);
    }

    /// <summary>
    /// 添加捕获内部异常类型
    /// </summary>
    /// <typeparam name="TException"><see cref="System.Exception"/></typeparam>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> HandleInner<TException>()
        where TException : System.Exception
    {
        HandleInnerExceptions ??= new List<Type>();
        HandleInnerExceptions.Add(typeof(TException));

        return this;
    }

    /// <summary>
    /// 添加捕获内部异常类型
    /// </summary>
    /// <typeparam name="TException"><see cref="System.Exception"/></typeparam>
    /// <param name="exceptionCondition">异常条件</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> HandleInner<TException>(Func<TException, bool> exceptionCondition)
        where TException : System.Exception
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(exceptionCondition);

        // 添加捕获异常类型和条件
        HandleInner<TException>();
        HandleResult(context => context.Exception?.InnerException is TException exception && exceptionCondition(exception));

        return this;
    }

    /// <summary>
    /// 添加捕获内部异常类型
    /// </summary>
    /// <typeparam name="TException"><see cref="System.Exception"/></typeparam>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> OrInner<TException>()
        where TException : System.Exception
    {
        return HandleInner<TException>();
    }

    /// <summary>
    /// 添加捕获内部异常类型
    /// </summary>
    /// <typeparam name="TException"><see cref="System.Exception"/></typeparam>
    /// <param name="exceptionCondition">异常条件</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> OrInner<TException>(Func<TException, bool> exceptionCondition)
        where TException : System.Exception
    {
        return HandleInner(exceptionCondition);
    }

    /// <summary>
    /// 添加操作结果条件
    /// </summary>
    /// <param name="resultCondition">操作结果条件</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> HandleResult(Func<RetryPolicyContext<TResult>, bool> resultCondition)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(resultCondition);

        ResultConditions ??= new List<Func<RetryPolicyContext<TResult>, bool>>();
        ResultConditions.Add(resultCondition);

        return this;
    }

    /// <summary>
    /// 添加操作结果条件
    /// </summary>
    /// <param name="resultCondition">操作结果条件</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> OrResult(Func<RetryPolicyContext<TResult>, bool> resultCondition)
    {
        return HandleResult(resultCondition);
    }

    /// <summary>
    /// 检查是否可以执行重试操作
    /// </summary>
    /// <param name="context"><see cref="RetryPolicy{TResult}"/></param>
    /// <returns><see cref="bool"/></returns>
    internal bool ShouldRetry(RetryPolicyContext<TResult> context)
    {
        // 检查最大重试次数是否大于等于 0
        if (MaxRetryCount <= 0)
        {
            return false;
        }

        // 检查异常或内部异常是否能够捕获处理
        if (WhenCatchException(context, HandleExceptions, context.Exception)
            || WhenCatchException(context, HandleInnerExceptions, context.Exception?.InnerException))
        {
            return true;
        }

        // 检查是否配置了操作结果条件
        if (ResultConditions is not null && ResultConditions.Count > 0)
        {
            return ResultConditions.Any(condition => condition(context));
        }

        return false;
    }

    /// <summary>
    /// 添加重试间隔
    /// </summary>
    /// <param name="retryIntervals">重试间隔</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> WaitAndRetry(params TimeSpan[] retryIntervals)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(retryIntervals);

        RetryIntervals = retryIntervals;

        return this;
    }

    /// <summary>
    /// 添加重试时操作方法
    /// </summary>
    /// <param name="retryAction">重试时操作方法</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> OnRetry(Action<RetryPolicyContext<TResult>> retryAction)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(retryAction);

        RetryAction = retryAction;

        return this;
    }

    /// <summary>
    /// 永久重试
    /// </summary>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public RetryPolicy<TResult> Forever()
    {
        MaxRetryCount = int.MaxValue;

        return this;
    }

    /// <inheritdoc />
    public override async Task<TResult?> ExecuteAsync(Func<Task<TResult?>> operation, CancellationToken cancellationToken = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        // 初始化重试策略上下文
        var context = new RetryPolicyContext<TResult>
        {
            PolicyName = PolicyName
        };

        while (true)
        {
            try
            {
                // 获取操作方法执行结果
                context.Result = await operation();
            }
            catch (System.Exception exception)
            {
                // 获取操作方法执行异常
                context.Exception = exception;
            }

            // 检查是否可以执行重试操作
            if (ShouldRetry(context))
            {
                // 检查重试次数是否大于最大重试次数减 1
                if (context.RetryCount > MaxRetryCount - 1)
                {
                    // 返回结果或抛出异常
                    return ReturnResultOrThrow(context);
                }

                // 递增上下文数据
                context.Increment();

                // 调用重试时操作方法
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
            // 抛出操作异常
            throw context.Exception;
        }

        return context.Result;
    }

    /// <summary>
    /// 检查异常信息是否匹配
    /// </summary>
    /// <param name="context"><see cref="RetryPolicyContext{TResult}"/></param>
    /// <param name="exceptionTypes">异常类型集合</param>
    /// <param name="exception"><see cref="System.Exception"/></param>
    /// <returns><see cref="bool"/></returns>
    internal bool WhenCatchException(RetryPolicyContext<TResult> context, IList<Type>? exceptionTypes, System.Exception? exception)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(context);

        // 检查是否存在异常
        if (exception is null)
        {
            return false;
        }

        // 检查是否配置了需要捕获的异常
        if (exceptionTypes?.Any(ex => ex.IsInstanceOfType(exception)) == true)
        {
            // 检查是否配置了操作结果条件
            if (ResultConditions is not null && ResultConditions.Count > 0)
            {
                return ResultConditions.Any(condition => condition(context));
            }

            return true;
        }

        return false;
    }
}