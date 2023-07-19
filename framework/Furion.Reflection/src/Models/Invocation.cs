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
/// 代理方法调用器
/// </summary>
/// <remarks>负责动态调用方法</remarks>
public sealed class Invocation
{
    /// <summary>
    /// <inheritdoc cref="Invocation"/>
    /// </summary>
    /// <param name="targetMethod">接口方法</param>
    /// <param name="args">调用参数</param>
    /// <param name="target">目标实例对象</param>
    /// <param name="properties">额外数据</param>
    public Invocation(MethodInfo targetMethod
        , object?[]? args
        , object? target = null
        , Dictionary<object, object?>? properties = null)
    {
        Args = args;
        TargetMethod = targetMethod;
        Target = target;
        Properties = properties;

        Method = GetRealMethod(targetMethod, target);
    }

    /// <summary>
    /// 目标实例对象
    /// </summary>
    public object? Target { get; }

    /// <summary>
    /// 被代理方法
    /// </summary>
    public MethodInfo Method { get; }

    /// <summary>
    /// 接口方法
    /// </summary>
    private MethodInfo TargetMethod { get; }

    /// <summary>
    /// 调用参数
    /// </summary>
    public object?[]? Args { get; }

    /// <summary>
    /// 额外数据
    /// </summary>
    public Dictionary<object, object?>? Properties { get; }

    /// <summary>
    /// 调用同步方法
    /// </summary>
    /// <returns><see cref="object"/></returns>
    public object? Proceed()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(Target, nameof(Target));

        //方法返回值
        var returnType = Method.ReturnType;

        // 处理 Task 和 Task<> 异步方法调用
        if (returnType == typeof(Task)
            || returnType.IsGenericType
            && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            // 调用方法并返回 Task 类型
            var task = Method.Invoke(Target, Args) as Task;

            // 空检查
            ArgumentNullException.ThrowIfNull(task);

            // 创建 TaskCompletionSource 实例，用于控制 Task 什么时候结束、取消、错误
            var taskCompletionSource = new TaskCompletionSource<object>();
            task.ContinueWith(t =>
            {
                // 异步执行失败处理
                if (t.IsFaulted)
                {
                    taskCompletionSource.TrySetException(t.Exception);
                }
                // 异步被取消处理
                else if (t.IsCanceled)
                {
                    taskCompletionSource.TrySetCanceled();
                }
                // 异步成功返回处理
                else
                {
                    taskCompletionSource.TrySetResult(returnType == typeof(Task) ? null : ((dynamic)t).Result);
                }
            });

            return taskCompletionSource.Task;
        }
        // 处理同步方法
        else
        {
            return Method.Invoke(Target, Args);
        }
    }

    /// <summary>
    /// 调用异步方法
    /// </summary>
    /// <returns><see cref="Task"/></returns>
    public Task ProceedAsync()
    {
        return (Task)Proceed()!;
    }

    /// <summary>
    /// 调用异步方法带返回值
    /// </summary>
    /// <typeparam name="T">泛型值</typeparam>
    /// <returns><see cref="Task{TResult}"/></returns>
    public async Task<T?> ProceedAsync<T>()
    {
        return (T)await (Task<object>)Proceed()!;
    }

    /// <summary>
    /// 解析真实的方法对象
    /// </summary>
    /// <param name="targetMethod">接口方法</param>
    /// <param name="target">目标实例对象</param>
    /// <returns><see cref="MethodInfo"/></returns>
    internal static MethodInfo GetRealMethod(MethodInfo targetMethod, object? target)
    {
        // 处理无目标实例对象情况
        if (target is null)
        {
            return targetMethod;
        }

        // 查找被代理方法
        var method = target.GetType()
                                     .GetMethods()
                                     .SingleOrDefault(m => m.GetRuntimeBaseDefinition() == targetMethod.GetRuntimeBaseDefinition());

        // 接口默认实现方法
        if (method is null)
        {
            return targetMethod;
        }

        // 处理泛型方法
        if (targetMethod.IsGenericMethod)
        {
            method = method.MakeGenericMethod(targetMethod.GetGenericArguments());
        }

        return method;
    }
}