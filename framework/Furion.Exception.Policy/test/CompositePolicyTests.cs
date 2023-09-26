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

namespace Furion.Exception.Policy1.Tests;

public class CompositePolicyTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var policy = new CompositePolicy<object>();

        Assert.Equal(typeof(CompositePolicy<object>), typeof(CompositePolicy).BaseType);
        Assert.NotNull(policy);
        Assert.Null(policy.PolicyName);
        Assert.NotNull(policy.Policies);
        Assert.Empty(policy.Policies);
        Assert.Null(policy.ExecutionFailureAction);

        var policy2 = new CompositePolicy();
        Assert.NotNull(policy2);

        var policy3 = new CompositePolicy<object>(new RetryPolicy());
        Assert.Single(policy3.Policies);

        var policy4 = new CompositePolicy<object>(new List<PolicyBase<object>> { new RetryPolicy() });
        Assert.Single(policy4.Policies);

        var policy5 = new CompositePolicy(new RetryPolicy());
        Assert.Single(policy5.Policies);

        var policy6 = new CompositePolicy(new List<PolicyBase<object>> { new RetryPolicy() });
        Assert.Single(policy6.Policies);
    }

    [Fact]
    public void OnTimeout_Invalid_Parameters()
    {
        var policy = new CompositePolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.OnExecutionFailure(null!);
        });
    }

    [Fact]
    public void OnTimeout_ReturnOK()
    {
        var policy = new CompositePolicy<object>();
        policy.OnExecutionFailure(context => { });

        Assert.NotNull(policy.ExecutionFailureAction);
    }

    [Fact]
    public void Join_Invalid_Paramters()
    {
        var policy = new CompositePolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.Join(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            policy.Join([null!]);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            policy.Join([new RetryPolicy(), null!]);
        });
    }

    [Fact]
    public void Join_ReturnOK()
    {
        var policy = new CompositePolicy<object>();
        policy.Join(new RetryPolicy());
        policy.Join(new RetryPolicy());

        Assert.NotEmpty(policy.Policies);
        Assert.Equal(2, policy.Policies.Count);
    }

    [Fact]
    public void Join_Enumerable_ReturnOK()
    {
        var policy = new CompositePolicy<object>();
        policy.Join(new List<PolicyBase<object>> { new RetryPolicy() });
        policy.Join(new List<PolicyBase<object>> { new RetryPolicy() });

        Assert.NotEmpty(policy.Policies);
        Assert.Equal(2, policy.Policies.Count);
    }

    [Fact]
    public async Task ExecuteAsync_Invalid_Parameters()
    {
        var policy = new CompositePolicy<object>();

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await policy.ExecuteAsync(null!);
        });
    }

    [Fact]
    public async Task ExecuteAsync_NoException_ReturnOK()
    {
        var policy1 = new CompositePolicy();

        var str = await policy1.ExecuteAsync(async () =>
        {
            return await Task.FromResult("furion");
        });

        Assert.Equal("furion", str);

        var policy2 = new CompositePolicy<string>(
            new TimeoutPolicy<string>(3000)
            , new FallbackPolicy<string>(context => "furion")
            , new RetryPolicy<string>(3));

        var str2 = await policy2.ExecuteAsync(async () =>
        {
            return await Task.FromResult("百小僧");
        });

        Assert.Equal("百小僧", str2);
    }

    [Fact]
    public async Task ExecuteAsync_HasException_ReturnOK()
    {
        var policy = new CompositePolicy<string>(
            new TimeoutPolicy<string>(3000)
            , new FallbackPolicy<string>(context => "furion")
            , new RetryPolicy<string>(3));

        var i = 0;
        var str2 = await policy
            .OnExecutionFailure(context =>
            {
                i++;
            })
            .ExecuteAsync(async () =>
            {
                var str = "百小僧";

                if (str == "百小僧")
                {
                    throw new System.Exception("出错了");
                }

                return await Task.FromResult(str);
            });

        Assert.Equal("furion", str2);
        Assert.Equal(0, i);
    }

    [Fact]
    public async Task ExecuteAsync_WithCancellationToken_ReturnOK()
    {
        var policy = new CompositePolicy<string>(
           new TimeoutPolicy<string>(3000)
           , new FallbackPolicy<string>(context => "furion")
           , new RetryPolicy<string>(3));

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(800);

        var i = 0;
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            var str = await policy
            .OnExecutionFailure(context =>
            {
                i++;
            })
            .ExecuteAsync(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                return "furion";
            }, cancellationTokenSource.Token);
        });

        Assert.Equal(0, i);
    }

    [Fact]
    public void ExecuteAsync_Context_Policy1_ReturnOK()
    {
        Assert.Throws<System.InvalidOperationException>(() =>
        {
            var timeoutPolicy = new TimeoutPolicy(1000)
            .OnTimeout(context =>
            {
            });

            var retryPolicy = new RetryPolicy(3)
                .Handle<System.Exception>()
                .OnWaitRetry((context, delay) =>
                {
                })
                .OnRetrying(context =>
                {
                })
                .WaitAndRetry(TimeSpan.FromMilliseconds(100));

            new CompositePolicy(timeoutPolicy, retryPolicy)
                .OnExecutionFailure(context =>
                {
                    Assert.Equal(typeof(RetryPolicy), context.Policy.GetType());
                })
                .Execute(() =>
                {
                    throw new System.InvalidOperationException("我出错了");
                });
        });
    }

    [Fact]
    public void ExecuteAsync_Context_Policy2_ReturnOK()
    {
        Assert.Throws<System.TimeoutException>(() =>
        {
            var timeoutPolicy = new TimeoutPolicy(200)
            .OnTimeout(context =>
            {
            });

            var retryPolicy = new RetryPolicy(3)
                .Handle<System.Exception>()
                .OnWaitRetry((context, delay) =>
                {
                })
                .OnRetrying(context =>
                {
                })
                .WaitAndRetry(TimeSpan.FromMilliseconds(100));

            new CompositePolicy(timeoutPolicy, retryPolicy)
                .OnExecutionFailure(context =>
                {
                    Assert.Equal(typeof(TimeoutPolicy), context.Policy.GetType());
                })
                .Execute(() =>
                {
                    throw new System.InvalidOperationException("我出错了");
                });
        });
    }

    [Fact]
    public void ExecuteAction_ReturnOK()
    {
        var policy = new CompositePolicy<string>(
            new TimeoutPolicy<string>(3000)
            , new FallbackPolicy<string>(context => "furion")
            , new RetryPolicy<string>(3));

        static void action()
        {
            throw new System.Exception("出错了");
        }

        var i = 0;
        policy
            .OnExecutionFailure(context =>
            {
                i++;
            })
           .Execute(action);

        Assert.Equal(0, i);
    }

    [Fact]
    public async Task ExecuteAsyncFunc_ReturnOK()
    {
        var policy = new CompositePolicy<string>(
            new TimeoutPolicy<string>(3000)
            , new FallbackPolicy<string>(context => "furion")
            , new RetryPolicy<string>(3));

        var i = 0;
        await policy
            .OnExecutionFailure(context =>
            {
                i++;
            })
            .ExecuteAsync(async () =>
            {
                await Task.CompletedTask;
                throw new System.Exception("出错了");
            });

        Assert.Equal(0, i);
    }

    [Fact]
    public void ExecuteFunc_ReturnOK()
    {
        var policy = new CompositePolicy<string>(
            new TimeoutPolicy<string>(3000)
            , new FallbackPolicy<string>(context => "furion")
            , new RetryPolicy<string>(3));

        var i = 0;
        var str2 = policy
            .OnExecutionFailure(context =>
            {
                i++;
            })
            .Execute(() =>
            {
                var str = "百小僧";

                if (str == "百小僧")
                {
                    throw new System.Exception("出错了");
                }

                return str;
            });

        Assert.Equal("furion", str2);
        Assert.Equal(0, i);
    }

    [Fact]
    public void ExecutePolicyChain_ReturnOK()
    {
        var policy = new CompositePolicy<string>(
            new TimeoutPolicy<string>(3000)
            , new FallbackPolicy<string>(context => "furion")
            , new RetryPolicy<string>(3));

        var cascadeExecuteAsync = policy.ExecutePolicyChain(new(policy.Policies[0].ExecuteAsync), new(policy.Policies[1].ExecuteAsync));
        Assert.NotNull(cascadeExecuteAsync);
    }

    [Fact]
    public void EnsureLegalData_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            CompositePolicy.EnsureLegalData(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            CompositePolicy.EnsureLegalData(null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            CompositePolicy.EnsureLegalData(new List<PolicyBase<object>> { new RetryPolicy(), null!, new CompositePolicy() });
        });
        Assert.Equal("The policy collection contains a null value. (Parameter 'policies')", exception.Message);
    }
}