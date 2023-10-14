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

public class FallbackPolicyTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var policy = new FallbackPolicy<object>(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var policy = new FallbackPolicy(null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();

        Assert.Equal(typeof(FallbackPolicy<object>), typeof(FallbackPolicy).BaseType);
        Assert.NotNull(policy);
        Assert.Equal("Operation execution failed! The backup operation will be called shortly.", FallbackPolicy<object>.FALLBACK_MESSAGE);
        Assert.Null(policy.PolicyName);
        Assert.Null(policy.HandleExceptions);
        Assert.Null(policy.HandleInnerExceptions);
        Assert.Null(policy.ResultConditions);
        Assert.Null(policy.FallbackAction);

        var policy2 = new FallbackPolicy<object>(context => "");
        Assert.NotNull(policy2.FallbackAction);

        var policy3 = new FallbackPolicy(context => "");
        Assert.NotNull(policy3.FallbackAction);

        var policy4 = new FallbackPolicy();
        Assert.Null(policy4.FallbackAction);

        var policy5 = new FallbackPolicy<object>(context => { });
        Assert.NotNull(policy5.FallbackAction);

        var policy6 = new FallbackPolicy(context => { });
        Assert.NotNull(policy6.FallbackAction);
    }

    [Fact]
    public void Handle_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();
        policy.Handle<System.Exception>();

        Assert.NotNull(policy.HandleExceptions);
        Assert.Single(policy.HandleExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleExceptions.First());
    }

    [Fact]
    public void Handle_WithCondition_Invalid_Parameters()
    {
        var policy = new FallbackPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.Handle<System.Exception>(null!);
        });
    }

    [Fact]
    public void Handle_WithCondition_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();
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
        var policy = new FallbackPolicy<object>();
        policy.Or<System.Exception>();

        Assert.NotNull(policy.HandleExceptions);
        Assert.Single(policy.HandleExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleExceptions.First());
    }

    [Fact]
    public void Or_WithCondition_Invalid_Parameters()
    {
        var policy = new FallbackPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.Or<System.Exception>(null!);
        });
    }

    [Fact]
    public void Or_WithCondition_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();
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
        var policy = new FallbackPolicy<object>();
        policy.HandleInner<System.Exception>();

        Assert.NotNull(policy.HandleInnerExceptions);
        Assert.Single(policy.HandleInnerExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleInnerExceptions.First());
    }

    [Fact]
    public void HandleInner_WithCondition_Invalid_Parameters()
    {
        var policy = new FallbackPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.HandleInner<System.Exception>(null!);
        });
    }

    [Fact]
    public void HandleInner_WithCondition_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();
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
        var policy = new FallbackPolicy<object>();
        policy.OrInner<System.Exception>();

        Assert.NotNull(policy.HandleInnerExceptions);
        Assert.Single(policy.HandleInnerExceptions);
        Assert.Equal(typeof(System.Exception), policy.HandleInnerExceptions.First());
    }

    [Fact]
    public void OrInner_WithCondition_Invalid_Parameters()
    {
        var policy = new FallbackPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.OrInner<System.Exception>(null!);
        });
    }

    [Fact]
    public void OrInner_WithCondition_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();
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
        var policy = new FallbackPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.HandleInner<System.Exception>(null!);
        });
    }

    [Fact]
    public void HandleResult_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();
        policy.HandleResult(context => context.Result is not null);

        Assert.NotNull(policy.ResultConditions);
        Assert.Single(policy.ResultConditions);
    }

    [Fact]
    public void OrResult_Invalid_Parameters()
    {
        var policy = new FallbackPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.OrResult(null!);
        });
    }

    [Fact]
    public void OrResult_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();
        policy.OrResult(context => context.Result is not null);

        Assert.NotNull(policy.ResultConditions);
        Assert.Single(policy.ResultConditions);
    }

    [Fact]
    public void OnFallback_Invalid_Parameters()
    {
        var policy = new FallbackPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.OnFallback(null!);
        });
    }

    [Fact]
    public void OnFallback_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();
        policy.OnFallback(context => "");

        Assert.NotNull(policy.FallbackAction);
    }

    [Fact]
    public void OnFallbackAction_Invalid_Parameters()
    {
        var policy = new FallbackPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.OnFallback((Action<FallbackPolicyContext<object>>)null!);
        });
    }

    [Fact]
    public void OnFallbackAction_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();
        policy.OnFallback(context => { });

        Assert.NotNull(policy.FallbackAction);
    }

    [Fact]
    public void ShouldHandle_Invalid_Parameters()
    {
        var policy = new FallbackPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.ShouldHandle(null!);
        });
    }

    [Fact]
    public void ShouldHandle_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();

        Assert.False(policy.ShouldHandle(new()));
        Assert.True(policy.ShouldHandle(new()
        {
            Exception = new()
        }));

        policy.HandleResult(context => context.Exception is null);
        Assert.True(policy.ShouldHandle(new()));
        Assert.False(policy.ShouldHandle(new()
        {
            Exception = new()
        }));
    }

    [Fact]
    public async Task ExecuteAsync_Invalid_Parameters()
    {
        var policy = new FallbackPolicy<string>();

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            var result = await policy.ExecuteAsync(null!);
        });

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            var result = await policy.ExecuteAsync(async () =>
            {
                var result = await Task.FromResult("str");

                if (result == "str")
                {
                    throw new InvalidOperationException("无效操作");
                }

                return result;
            });
        });
    }

    [Fact]
    public async Task ExecuteAsync_NoException_ReturnOK()
    {
        var policy = new FallbackPolicy<string>();

        var str = await policy.ExecuteAsync(async () =>
        {
            return await Task.FromResult("furion");
        });

        Assert.Equal("furion", str);
    }

    [Theory]
    [InlineData("fur", "fur")]
    [InlineData("furion", "百小僧")]
    [InlineData("furion v5", "furion v5")]
    public async Task ExecuteAsync_HasException_ReturnOK(string value, string result)
    {
        var policy = new FallbackPolicy<string>();

        var str = await policy
            .OnFallback(context =>
            {
                return "百小僧";
            })
            .ExecuteAsync(async () =>
            {
                var stringValue = await Task.FromResult(value);

                if (stringValue == "furion")
                {
                    throw new InvalidOperationException("无效操作");
                }

                return stringValue;
            });

        Assert.Equal(result, str);
    }

    [Fact]
    public async Task ExecuteAsync_WithHandleException_ReturnOK()
    {
        var policy = new FallbackPolicy<string>();

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            var str = await policy
            .Handle<InvalidCastException>()
            .OnFallback(context =>
            {
                return "百小僧";
            })
            .ExecuteAsync(async () =>
            {
                var stringValue = await Task.FromResult("furion");

                if (stringValue == "furion")
                {
                    throw new InvalidOperationException("无效操作");
                }

                return stringValue;
            });
        });

        var str = await policy
            .Handle<InvalidCastException>()
            .HandleResult(context => context.Exception is InvalidOperationException)
            .OnFallback(context =>
            {
                return "百小僧";
            })
            .ExecuteAsync(async () =>
            {
                var stringValue = await Task.FromResult("furion");

                if (stringValue == "furion")
                {
                    throw new InvalidOperationException("无效操作");
                }

                return stringValue;
            });

        Assert.Equal("百小僧", str);
    }

    [Fact]
    public async Task ExecuteAsync_WithCancellationToken_ReturnOK()
    {
        var policy = new FallbackPolicy<string>();

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(800);

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            var str = await policy
             .OnFallback(context =>
             {
                 return "百小僧";
             })
             .ExecuteAsync(async () =>
             {
                 var stringValue = await Task.FromResult("furion");

                 await Task.Delay(1000);

                 return stringValue;
             }, cancellationTokenSource.Token);
        });
    }

    [Fact]
    public void ExecuteAction_ReturnOK()
    {
        var policy = new FallbackPolicy<string>();

        static void action()
        {
            throw new System.Exception("出错了");
        }

        var i = 0;
        policy
            .OnFallback(context =>
            {
                i++;
            })
            .Execute(action);

        Assert.Equal(1, i);
    }

    [Fact]
    public async Task ExecuteAsyncFunc_ReturnOK()
    {
        var policy = new FallbackPolicy<string>();

        var i = 0;
        await policy
               .OnFallback(context =>
               {
                   i++;
               })
               .ExecuteAsync(async () =>
               {
                   var str = await Task.FromResult("furion");
                   throw new System.Exception("出错了");
               });

        Assert.Equal(1, i);
    }

    [Fact]
    public void ExecuteFunc_ReturnOK()
    {
        var policy = new FallbackPolicy<string>();

        var str = policy
              .OnFallback(context =>
              {
                  return "furion";
              })
              .Execute(() =>
              {
                  throw new System.Exception("出错了");
              });

        Assert.Equal("furion", str);
    }

    [Fact]
    public void CanHandleException_Invalid_Parameters()
    {
        var policy = new FallbackPolicy<object>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            policy.CanHandleException(null!, null!, null!);
        });
    }

    [Fact]
    public void CanHandleException_ReturnOK()
    {
        var policy = new FallbackPolicy<object>();

        Assert.False(policy.CanHandleException(new FallbackPolicyContext<object>(), null!, null!));
        Assert.False(policy.CanHandleException(new FallbackPolicyContext<object>(), [], null!));
        Assert.True(policy.CanHandleException(new FallbackPolicyContext<object>(), [], new System.Exception()));
        Assert.True(policy.CanHandleException(new FallbackPolicyContext<object>(), [typeof(System.Exception)], new InvalidOperationException()));
        Assert.False(policy.CanHandleException(new FallbackPolicyContext<object>(), [typeof(NotSupportedException)], new InvalidOperationException()));

        policy.HandleResult(context => context.Exception?.Message.Length > 0);
        Assert.False(policy.CanHandleException(new FallbackPolicyContext<object>(), [typeof(System.Exception)], new InvalidOperationException()));
    }

    [Fact]
    public void ReturnOrThrowIfException_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            FallbackPolicy<string>.ReturnOrThrowIfException(null!);
        });

        Assert.Throws<System.Exception>(() =>
        {
            FallbackPolicy<string>.ReturnOrThrowIfException(new()
            {
                Exception = new System.Exception()
            });
        });
    }

    [Fact]
    public void ReturnOrThrowIfException_ReturnOK()
    {
        var result = FallbackPolicy<string>.ReturnOrThrowIfException(new()
        {
            Result = "furion"
        });

        Assert.Equal("furion", result);
    }
}