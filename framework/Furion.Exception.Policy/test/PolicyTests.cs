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

public class PolicyTests
{
    [Fact]
    public void For_ReturnOK()
    {
        var policy = Policy.For<RetryPolicy>();
        Assert.NotNull(policy);

        var policy2 = Policy.For(new RetryPolicy());
        Assert.NotNull(policy2);
    }

    [Fact]
    public void Retry_ReturnOK()
    {
        var policy = Policy.Retry();
        Assert.NotNull(policy);
        Assert.Equal(3, policy.MaxRetryCount);

        var policy2 = Policy.Retry(3);
        Assert.NotNull(policy2);
        Assert.Equal(3, policy2.MaxRetryCount);
    }

    [Fact]
    public void Timeout_ReturnOK()
    {
        var policy = Policy.Timeout();
        Assert.NotNull(policy);
        Assert.Equal(TimeSpan.FromSeconds(10), policy.Timeout);

        var policy2 = Policy.Timeout(3000);
        Assert.NotNull(policy2);
        Assert.Equal(TimeSpan.FromSeconds(3), policy2.Timeout);

        var policy3 = Policy.Timeout(TimeSpan.FromSeconds(10));
        Assert.NotNull(policy3);
        Assert.Equal(TimeSpan.FromSeconds(10), policy3.Timeout);
    }

    [Fact]
    public void Fallback_ReturnOK()
    {
        var policy = Policy.Fallback();
        Assert.NotNull(policy);
        Assert.Null(policy.FallbackAction);

        var policy2 = Policy.Fallback(context => "");
        Assert.NotNull(policy2);
        Assert.NotNull(policy2.FallbackAction);

        var policy3 = Policy.Fallback(context => { });
        Assert.NotNull(policy3);
        Assert.NotNull(policy3.FallbackAction);
    }

    [Fact]
    public void Composite_ReturnOK()
    {
        var policy = Policy.Composite();
        Assert.NotNull(policy);
        Assert.Empty(policy.Policies);

        var policy2 = Policy.Composite(new RetryPolicy());
        Assert.NotNull(policy2);
        Assert.Single(policy2.Policies);

        var policy3 = Policy.Composite(new List<PolicyBase<object>> { new RetryPolicy() });
        Assert.NotNull(policy3);
        Assert.Single(policy3.Policies);
    }

    [Fact]
    public void ForGeneric_ReturnOK()
    {
        var policy = Policy<object>.For<RetryPolicy>();
        Assert.NotNull(policy);

        var policy2 = Policy<object>.For(new RetryPolicy());
        Assert.NotNull(policy2);
    }

    [Fact]
    public void RetryGeneric_ReturnOK()
    {
        var policy = Policy<object>.Retry();
        Assert.NotNull(policy);
        Assert.Equal(3, policy.MaxRetryCount);

        var policy2 = Policy<object>.Retry(3);
        Assert.NotNull(policy2);
        Assert.Equal(3, policy2.MaxRetryCount);
    }

    [Fact]
    public void TimeoutGeneric_ReturnOK()
    {
        var policy = Policy<object>.Timeout();
        Assert.NotNull(policy);
        Assert.Equal(TimeSpan.FromSeconds(10), policy.Timeout);

        var policy2 = Policy<object>.Timeout(3000);
        Assert.NotNull(policy2);
        Assert.Equal(TimeSpan.FromSeconds(3), policy2.Timeout);

        var policy3 = Policy<object>.Timeout(TimeSpan.FromSeconds(10));
        Assert.NotNull(policy3);
        Assert.Equal(TimeSpan.FromSeconds(10), policy3.Timeout);
    }

    [Fact]
    public void FallbackGeneric_ReturnOK()
    {
        var policy = Policy<object>.Fallback();
        Assert.NotNull(policy);
        Assert.Null(policy.FallbackAction);

        var policy2 = Policy<object>.Fallback(context => "");
        Assert.NotNull(policy2);
        Assert.NotNull(policy2.FallbackAction);

        var policy3 = Policy<object>.Fallback(context => { });
        Assert.NotNull(policy3);
        Assert.NotNull(policy3.FallbackAction);
    }

    [Fact]
    public void CompositeGeneric_ReturnOK()
    {
        var policy = Policy<object>.Composite();
        Assert.NotNull(policy);
        Assert.Empty(policy.Policies);

        var policy2 = Policy<object>.Composite(new RetryPolicy());
        Assert.NotNull(policy2);
        Assert.Single(policy2.Policies);

        var policy3 = Policy<object>.Composite(new List<PolicyBase<object>> { new RetryPolicy() });
        Assert.NotNull(policy3);
        Assert.Single(policy3.Policies);
    }
}