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

namespace Furion.Exception.Policy.Tests;

public class RetryPolicyTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var policy = new RetryPolicy<object>();

        Assert.Equal(typeof(RetryPolicy<object>), typeof(RetryPolicy).BaseType);
        Assert.NotNull(policy);
        Assert.Equal("Retry after {0} seconds.", RetryPolicy<object>.WAIT_RETRY_MESSAGE);
        Assert.Equal("Retrying for the {0}nd time.", RetryPolicy<object>.RETRY_MESSAGE);
        Assert.Null(policy.PolicyName);
        Assert.Equal(0, policy.MaxRetryCount);
        Assert.Null(policy.RetryIntervals);
        Assert.Null(policy.HandleExceptions);
        Assert.Null(policy.HandleInnerExceptions);
        Assert.Null(policy.ResultConditions);
        Assert.Null(policy.RetryingAction);

        var policy2 = new RetryPolicy();
        Assert.NotNull(policy2);

        var policy3 = new RetryPolicy<object>(3);
        Assert.Equal(3, policy3.MaxRetryCount);

        var policy4 = new RetryPolicy(3);
        Assert.Equal(3, policy4.MaxRetryCount);
    }

    [Fact]
    public void Handle_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.Handle<System.Exception>();

        Assert.NotNull(policy.HandleExceptions);
        Assert.Single(policy.HandleExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleExceptions.First());
    }

    [Fact]
    public void Handle_WithCondition_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.Handle<System.Exception>(null!);
        });
    }

    [Fact]
    public void Handle_WithCondition_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.Handle<System.Exception>(ex => ex.Message.Contains("furion"));

        Assert.NotNull(policy.HandleExceptions);
        Assert.Single(policy.HandleExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleExceptions.First());
        Assert.NotNull(policy.ResultConditions);
        Assert.Single(policy.ResultConditions);
    }

    [Fact]
    public void Or_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.Or<System.Exception>();

        Assert.NotNull(policy.HandleExceptions);
        Assert.Single(policy.HandleExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleExceptions.First());
    }

    [Fact]
    public void Or_WithCondition_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.Or<System.Exception>(null!);
        });
    }

    [Fact]
    public void Or_WithCondition_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.Or<System.Exception>(ex => ex.Message.Contains("furion"));

        Assert.NotNull(policy.HandleExceptions);
        Assert.Single(policy.HandleExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleExceptions.First());
        Assert.NotNull(policy.ResultConditions);
        Assert.Single(policy.ResultConditions);
    }

    [Fact]
    public void HandleInner_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.HandleInner<System.Exception>();

        Assert.NotNull(policy.HandleInnerExceptions);
        Assert.Single(policy.HandleInnerExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleInnerExceptions.First());
    }

    [Fact]
    public void HandleInner_WithCondition_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.HandleInner<System.Exception>(null!);
        });
    }

    [Fact]
    public void HandleInner_WithCondition_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.HandleInner<System.Exception>(ex => ex.Message.Contains("furion"));

        Assert.NotNull(policy.HandleInnerExceptions);
        Assert.Single(policy.HandleInnerExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleInnerExceptions.First());
        Assert.NotNull(policy.ResultConditions);
        Assert.Single(policy.ResultConditions);
    }

    [Fact]
    public void OrInner_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.OrInner<System.Exception>();

        Assert.NotNull(policy.HandleInnerExceptions);
        Assert.Single(policy.HandleInnerExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleInnerExceptions.First());
    }

    [Fact]
    public void OrInner_WithCondition_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.OrInner<System.Exception>(null!);
        });
    }

    [Fact]
    public void OrInner_WithCondition_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.OrInner<System.Exception>(ex => ex.Message.Contains("furion"));

        Assert.NotNull(policy.HandleInnerExceptions);
        Assert.Single(policy.HandleInnerExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleInnerExceptions.First());
        Assert.NotNull(policy.ResultConditions);
        Assert.Single(policy.ResultConditions);
    }

    [Fact]
    public void HandleResult_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.HandleInner<System.Exception>(null!);
        });
    }

    [Fact]
    public void HandleResult_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.HandleResult(context => context.Result is not null);

        Assert.NotNull(policy.ResultConditions);
        Assert.Single(policy.ResultConditions);
    }

    [Fact]
    public void OrResult_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.OrResult(null!);
        });
    }

    [Fact]
    public void OrResult_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.OrResult(context => context.Result is not null);

        Assert.NotNull(policy.ResultConditions);
        Assert.Single(policy.ResultConditions);
    }

    [Fact]
    public void WaitAndRetry_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.WaitAndRetry(null!);
        });
    }

    [Fact]
    public void WaitAndRetry_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.WaitAndRetry(TimeSpan.FromSeconds(1));

        Assert.NotNull(policy.RetryIntervals);
    }

    [Fact]
    public void Forever_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.Forever();

        Assert.Equal(int.MaxValue, policy.MaxRetryCount);
    }

    [Fact]
    public void WaitAndRetryForever_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.WaitAndRetryForever(TimeSpan.FromSeconds(1));

        Assert.NotNull(policy.RetryIntervals);
        Assert.Equal(int.MaxValue, policy.MaxRetryCount);
    }

    [Fact]
    public void OnWaitRetry_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.OnWaitRetry(null!);
        });
    }

    [Fact]
    public void OnWaitRetry_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.OnWaitRetry((context, delay) => { });

        Assert.NotNull(policy.WaitRetryAction);
    }

    [Fact]
    public void OnRetrying_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.OnRetrying(null!);
        });
    }

    [Fact]
    public void OnRetrying_ReturnOK()
    {
        var policy = new RetryPolicy<object>();
        policy.OnRetrying(context => { });

        Assert.NotNull(policy.RetryingAction);
    }

    [Fact]
    public void ShouldHandle_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.ShouldHandle(null!);
        });
    }

    [Fact]
    public void ShouldHandle_ReturnOK()
    {
        var policy = new RetryPolicy<object>
        {
            MaxRetryCount = 1
        };
        Assert.False(policy.ShouldHandle(new()));
        Assert.True(policy.ShouldHandle(new()
        {
            Exception = new(),
            Result = 0
        }));

        policy.HandleResult(context => context.Exception == null);
        Assert.True(policy.ShouldHandle(new()));
        Assert.False(policy.ShouldHandle(new()
        {
            Exception = new()
        }));

        policy.MaxRetryCount = -1;
        Assert.False(policy.ShouldHandle(new()
        {
            Exception = new()
        }));

        policy.MaxRetryCount = 2;
        Assert.False(policy.ShouldHandle(new()
        {
            Exception = new(),
            RetryCount = 2
        }));
    }

    [Fact]
    public async Task ExecuteAsync_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await policy.ExecuteAsync(null!);
        });
    }

    [Fact]
    public async Task ExecuteAsync_NoException_ReturnOK()
    {
        var policy = new RetryPolicy<string>();

        var str = await policy.ExecuteAsync(async () =>
        {
            return await Task.FromResult("furion");
        });

        Assert.Equal("furion", str);
    }

    [Fact]
    public async Task ExecuteAsync_HasException_ReturnOK()
    {
        var policy = new RetryPolicy<object>(3)
            .WaitAndRetry(TimeSpan.FromMicroseconds(100));

        var waitRetry = 0;
        var retrying = 0;

        await Assert.ThrowsAsync<System.Exception>(async () =>
        {
            var str = await policy
            .OnWaitRetry((context, delay) =>
            {
                waitRetry++;
            })
            .OnRetrying(context =>
            {
                retrying++;
            })
            .ExecuteAsync(async () =>
            {
                var value = "furion";
                if (value == "furion")
                {
                    throw new System.Exception("出错了");
                }

                return await Task.FromResult(value);
            });
        });

        Assert.Equal(3, waitRetry);
        Assert.Equal(3, retrying);
    }

    [Fact]
    public async Task ExecuteAsync_WithHandleException_ReturnOK()
    {
        var policy = new RetryPolicy<string>(3)
            .WaitAndRetry(TimeSpan.FromMicroseconds(100));

        var waitRetry = 0;
        var retrying = 0;

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            var str = await policy
            .Handle<InvalidCastException>()
            .OnWaitRetry((context, delay) =>
            {
                waitRetry++;
            })
            .OnRetrying(context =>
            {
                retrying++;
            })
            .ExecuteAsync(async () =>
            {
                var value = "furion";
                if (value == "furion")
                {
                    throw new System.InvalidOperationException("出错了");
                }

                return await Task.FromResult(value);
            });
        });

        Assert.Equal(0, waitRetry);
        Assert.Equal(0, retrying);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            var str = await policy
            .Handle<InvalidCastException>()
            .HandleResult(context => context.Exception is InvalidOperationException)
            .OnWaitRetry((context, delay) =>
            {
                waitRetry++;
            })
            .OnRetrying(context =>
            {
                retrying++;
            })
            .ExecuteAsync(async () =>
            {
                var value = "furion";
                if (value == "furion")
                {
                    throw new System.InvalidOperationException("出错了");
                }

                return await Task.FromResult(value);
            });
        });

        Assert.Equal(3, waitRetry);
        Assert.Equal(3, retrying);
    }

    [Fact]
    public async Task ExecuteAsync_WithCancellationToken_ReturnOK()
    {
        var policy = new RetryPolicy<string>(3)
            .WaitAndRetry(TimeSpan.FromSeconds(1));

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(800);

        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            var str = await policy
            .ExecuteAsync(async () =>
             {
                 var value = "furion";
                 if (value == "furion")
                 {
                     throw new System.Exception("出错了");
                 }

                 return await Task.FromResult(value);
             }, cancellationTokenSource.Token);
        });
    }

    [Fact]
    public void ExecuteAction_ReturnOK()
    {
        var policy = new RetryPolicy<string>(3)
            .WaitAndRetry(TimeSpan.FromMicroseconds(100));

        var waitRetry = 0;
        var retrying = 0;

        static void action()
        {
            throw new System.Exception("出错了");
        }

        Assert.Throws<System.Exception>(() =>
        {
            policy.OnWaitRetry((context, delay) =>
            {
                waitRetry++;
            })
            .OnRetrying(context =>
            {
                retrying++;
            })
            .Execute(action);
        });

        Assert.Equal(3, waitRetry);
        Assert.Equal(3, retrying);
    }

    [Fact]
    public async Task ExecuteAsyncFunc_ReturnOK()
    {
        var policy = new RetryPolicy<string>(3)
           .WaitAndRetry(TimeSpan.FromMicroseconds(100));

        var waitRetry = 0;
        var retrying = 0;

        await Assert.ThrowsAsync<System.Exception>(async () =>
        {
            await policy.OnWaitRetry((context, delay) =>
            {
                waitRetry++;
            })
            .OnRetrying(context =>
            {
                retrying++;
            })
            .ExecuteAsync(async () =>
            {
                await Task.CompletedTask;
                throw new System.Exception("出错了");
            });
        });

        Assert.Equal(3, waitRetry);
        Assert.Equal(3, retrying);
    }

    [Fact]
    public void ExecuteFunc_ReturnOK()
    {
        var policy = new RetryPolicy<string>(3)
            .WaitAndRetry(TimeSpan.FromMicroseconds(100));

        var waitRetry = 0;
        var retrying = 0;

        Assert.Throws<System.Exception>(() =>
        {
            var str = policy.OnWaitRetry((context, delay) =>
            {
                waitRetry++;
            })
            .OnRetrying(context =>
            {
                retrying++;
            })
            .Execute(() =>
            {
                var value = "furion";
                if (value == "furion")
                {
                    throw new System.Exception("出错了");
                }

                return value;
            });
        });

        Assert.Equal(3, waitRetry);
        Assert.Equal(3, retrying);
    }

    [Fact]
    public void CanHandleException_Invalid_Parameters()
    {
        var policy = new RetryPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.CanHandleException(null!, null!, null!);
        });
    }

    [Fact]
    public void CanHandleException_ReturnOK()
    {
        var policy = new RetryPolicy<object>();

        Assert.False(policy.CanHandleException(new RetryPolicyContext<object>(), null!, null!));
        Assert.False(policy.CanHandleException(new RetryPolicyContext<object>(), new(), null!));
        Assert.True(policy.CanHandleException(new RetryPolicyContext<object>(), new(), new System.Exception()));
        Assert.True(policy.CanHandleException(new RetryPolicyContext<object>(), new() { typeof(System.Exception) }, new InvalidOperationException()));
        Assert.False(policy.CanHandleException(new RetryPolicyContext<object>(), new() { typeof(NotSupportedException) }, new InvalidOperationException()));

        policy.HandleResult(context => context.Exception?.Message.Length > 0);
        Assert.False(policy.CanHandleException(new RetryPolicyContext<object>(), new() { typeof(System.Exception) }, new InvalidOperationException()));
    }

    [Fact]
    public void ReturnOrThrowIfException_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            RetryPolicy<string>.ReturnOrThrowIfException(null!);
        });

        Assert.Throws<System.Exception>(() =>
        {
            RetryPolicy<string>.ReturnOrThrowIfException(new()
            {
                Exception = new System.Exception()
            });
        });
    }

    [Fact]
    public void ReturnOrThrowIfException_ReturnOK()
    {
        var result = RetryPolicy<string>.ReturnOrThrowIfException(new()
        {
            Result = "furion"
        });

        Assert.Equal("furion", result);
    }
}