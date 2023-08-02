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
/// 组合策略
/// </summary>
/// <typeparam name="TResult">操作返回值类型</typeparam>
public class CompositePolicy<TResult> : AbstractPolicy<TResult>
{
    /// <summary>
    /// <inheritdoc cref="CompositePolicy{TResult}"/>
    /// </summary>
    /// <param name="policies">策略集合</param>
    public CompositePolicy(params AbstractPolicy<TResult>[] policies)
        : this((policies ?? throw new ArgumentNullException(nameof(policies))).ToList())
    {
    }

    /// <summary>
    /// <inheritdoc cref="CompositePolicy{TResult}"/>
    /// </summary>
    /// <param name="policies">策略集合</param>
    public CompositePolicy(IList<AbstractPolicy<TResult>> policies)
    {
        // 检查策略集合合法性
        EnsureLegalData(policies);

        Policies = policies;
    }

    /// <summary>
    /// 策略集合
    /// </summary>
    public IList<AbstractPolicy<TResult>> Policies { get; init; }

    /// <summary>
    /// 执行失败时操作方法
    /// </summary>
    public Action<CompositePolicyContext<TResult>>? ExecutionFailureAction { get; set; }

    /// <summary>
    /// 添加执行失败时操作方法
    /// </summary>
    /// <param name="executionFailureAction">执行失败时操作方法</param>
    /// <returns><see cref="RetryPolicy{TResult}"/></returns>
    public CompositePolicy<TResult> OnExecutionFailure(Action<CompositePolicyContext<TResult>> executionFailureAction)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(executionFailureAction);

        ExecutionFailureAction = executionFailureAction;

        return this;
    }

    /// <inheritdoc />
    public override async Task<TResult?> ExecuteAsync(Func<Task<TResult?>> operation, CancellationToken cancellationToken = default)
    {
        // 检查策略集合合法性
        EnsureLegalData(Policies);

        // 生成异步操作方法级联委托
        var cascadeExecuteAsync = Policies
            .Select(p => new Func<Func<Task<TResult?>>, CancellationToken, Task<TResult?>>(p.ExecuteAsync))
            .Aggregate((previous, current) => async (opt, token) =>
            {
                // 初始化异常的策略
                object? policy = null;

                try
                {
                    return await previous(async () =>
                    {
                        try
                        {
                            return await current(opt, token);
                        }
                        catch (System.Exception)
                        {
                            // 将当前异常的委托对象赋值给异常策略
                            policy ??= current.Target;

                            throw;
                        }
                    }, token);
                }
                catch (System.Exception exception)
                {
                    // 将当前异常的委托对象赋值给异常策略
                    policy ??= previous.Target;

                    // 调用执行失败时操作方法
                    ExecutionFailureAction?.Invoke(new((AbstractPolicy<TResult>)policy!)
                    {
                        PolicyName = PolicyName,
                        Exception = exception
                    });

                    throw;
                }
            });

        // 调用异步操作方法级联委托
        return await cascadeExecuteAsync(operation, cancellationToken);
    }

    /// <summary>
    /// 检查策略集合合法性
    /// </summary>
    /// <param name="policies">策略集合</param>
    /// <exception cref="ArgumentException"></exception>
    internal static void EnsureLegalData(IList<AbstractPolicy<TResult>> policies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(policies);

        // 子项空检查
        if (policies.Any(policy => policy is null))
        {
            throw new ArgumentException("The policy collection contains a null value.", nameof(policies));
        }
    }
}