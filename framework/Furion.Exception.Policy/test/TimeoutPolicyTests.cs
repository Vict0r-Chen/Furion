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

public class TimeoutPolicyTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var policy = new TimeoutPolicy<object>();

        Assert.Equal(typeof(TimeoutPolicy<object>), typeof(TimeoutPolicy).BaseType);
        Assert.NotNull(policy);
        Assert.Equal("The operation has timed out.", TimeoutPolicy<object>.TIMEOUT_MESSAGE);
        Assert.Null(policy.PolicyName);
        Assert.Equal(TimeSpan.Zero, policy.Timeout);
        Assert.Null(policy.TimeoutAction);

        var policy2 = new TimeoutPolicy();
        Assert.NotNull(policy2);

        var policy3 = new TimeoutPolicy<object>(TimeSpan.FromSeconds(1));
        Assert.Equal(TimeSpan.FromSeconds(1), policy3.Timeout);

        var policy4 = new TimeoutPolicy(TimeSpan.FromSeconds(1));
        Assert.Equal(TimeSpan.FromSeconds(1), policy4.Timeout);
    }

    [Fact]
    public void OnTimeout_Invalid_Parameters()
    {
        var policy = new TimeoutPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.OnTimeout(null!);
        });
    }

    [Fact]
    public void OnTimeout_ReturnOK()
    {
        var policy = new TimeoutPolicy<object>();
        policy.OnTimeout(context => { });

        Assert.NotNull(policy.TimeoutAction);
    }

    [Fact]
    public async Task ExecuteAsync_Invalid_Parameters()
    {
        var policy = new TimeoutPolicy<object>();

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await policy.ExecuteAsync(null!);
        });
    }

    [Fact]
    public async Task ExecuteAsync_NoTimeout_ReturnOK()
    {
        var policy = new TimeoutPolicy<string>();

        var str = await policy.ExecuteAsync(async () =>
        {
            return await Task.FromResult("furion");
        });

        Assert.Equal("furion", str);

        var policy2 = new TimeoutPolicy<string>(TimeSpan.FromSeconds(1));

        var str2 = await policy2.ExecuteAsync(async () =>
        {
            return await Task.FromResult("furion");
        });

        Assert.Equal("furion", str2);
    }

    [Fact]
    public async Task ExecuteAsync_Timeout_ReturnOK()
    {
        var policy = new TimeoutPolicy<string>(TimeSpan.FromSeconds(1));

        var i = 0;
        await Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            var str = await policy
            .OnTimeout(context =>
            {
                i++;
            })
            .ExecuteAsync(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                return "furion";
            });
        });

        Assert.Equal(1, i);
    }

    [Fact]
    public async Task ExecuteAsync_WithCancellationToken_ReturnOK()
    {
        var policy = new TimeoutPolicy<string>(TimeSpan.FromSeconds(1));

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(800);

        var i = 0;
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            var str = await policy
            .OnTimeout(context =>
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
    public void ExecuteAction_ReturnOK()
    {
        var policy = new TimeoutPolicy<string>(TimeSpan.FromSeconds(1));
        var timeout = 0;

        static void action()
        {
            Task.Delay(TimeSpan.FromSeconds(2)).Wait();
            Console.WriteLine("...");
        }

        Assert.Throws<System.TimeoutException>(() =>
        {
            policy.OnTimeout((context) =>
            {
                timeout++;
            })
            .Execute(action);
        });

        Assert.Equal(1, timeout);
    }

    [Fact]
    public async Task ExecuteAsyncFunc_ReturnOK()
    {
        var policy = new TimeoutPolicy<string>(TimeSpan.FromSeconds(1));
        var timeout = 0;

        await Assert.ThrowsAsync<System.TimeoutException>(async () =>
        {
            await policy.OnTimeout((context) =>
            {
                timeout++;
            })
            .ExecuteAsync(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                return "furion";
            });
        });

        Assert.Equal(1, timeout);
    }

    [Fact]
    public void ExecuteFunc_ReturnOK()
    {
        var policy = new TimeoutPolicy<string>(TimeSpan.FromSeconds(1));
        var timeout = 0;

        Assert.Throws<System.TimeoutException>(() =>
        {
            var str = policy.OnTimeout((context) =>
            {
                timeout++;
            })
            .Execute(() =>
            {
                var value = "furion";
                Task.Delay(TimeSpan.FromSeconds(2)).Wait();
                return value;
            });
        });

        Assert.Equal(1, timeout);
    }

    [Fact]
    public void ThrowTimeoutException_ReturnOK()
    {
        var timeout = 0;
        var policy = new TimeoutPolicy<string>(TimeSpan.FromSeconds(1))
            .OnTimeout(context =>
            {
                timeout++;
            });

        Assert.Throws<TimeoutException>(() =>
        {
            policy.ThrowTimeoutException();
        });

        Assert.Equal(1, timeout);
    }
}