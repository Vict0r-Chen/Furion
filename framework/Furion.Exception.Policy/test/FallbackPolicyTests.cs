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
        Assert.Null(policy.FallbackAction);
        Assert.Null(policy.ResultConditions);
        Assert.Null(policy.HandleExceptions);
        Assert.Null(policy.HandleInnerExceptions);

        var policy2 = new FallbackPolicy<object>(context => "");
        Assert.NotNull(policy2.FallbackAction);
        var policy3 = new FallbackPolicy(context => "");
        Assert.NotNull(policy3.FallbackAction);
        var policy4 = new FallbackPolicy();
        Assert.Null(policy.FallbackAction);
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
        policy.OnFallback(context =>
        {
        });

        Assert.NotNull(policy.FallbackAction);
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
        Assert.False(policy.CanHandleException(new FallbackPolicyContext<object>(), new(), null!));
        Assert.True(policy.CanHandleException(new FallbackPolicyContext<object>(), new(), new System.Exception()));
        Assert.True(policy.CanHandleException(new FallbackPolicyContext<object>(), new() { typeof(System.Exception) }, new InvalidOperationException()));
        Assert.False(policy.CanHandleException(new FallbackPolicyContext<object>(), new() { typeof(NotSupportedException) }, new InvalidOperationException()));

        policy.HandleResult(context => context.Exception?.Message.Length > 0);
        Assert.False(policy.CanHandleException(new FallbackPolicyContext<object>(), new() { typeof(System.Exception) }, new InvalidOperationException()));
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