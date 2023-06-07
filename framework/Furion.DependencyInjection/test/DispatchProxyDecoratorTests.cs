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

namespace Furion.DependencyInjection.Tests;

public class DispatchProxyDecoratorTests
{
    [Fact]
    public async Task DecorateOfT_ReturnOK()
    {
        var proxyObject = DispatchProxyDecorator<ITestProxyClass>.Decorate<TestDispatchProxyDecoratorOfT>(new TestProxyClass());
        Assert.NotNull(proxyObject);

        var syncResult = proxyObject.SyncMethod("test");
        Assert.Equal("test", syncResult);

        await proxyObject.AsyncMethod();

        var asyncResult = await proxyObject.AsyncMethodWithResult(100);
        Assert.Equal(100, asyncResult);

        var GenericResult = proxyObject.GenericMethod(true);
        Assert.True(GenericResult);
    }

    [Fact]
    public async Task DecorateOfT2_ReturnOK()
    {
        var proxyObject = DispatchProxyDecorator<ITestProxyClass>.Decorate(typeof(TestDispatchProxyDecoratorOfT), new TestProxyClass());
        Assert.NotNull(proxyObject);

        var syncResult = proxyObject.SyncMethod("test");
        Assert.Equal("test", syncResult);

        await proxyObject.AsyncMethod();

        var asyncResult = await proxyObject.AsyncMethodWithResult(100);
        Assert.Equal(100, asyncResult);

        var GenericResult = proxyObject.GenericMethod(true);
        Assert.True(GenericResult);
    }

    [Fact]
    public async Task DecorateOfObject_ReturnOK()
    {
        var proxyObject = DispatchProxyDecorator.Decorate<ITestProxyClass, TestDispatchProxyDecorator>(new TestProxyClass()) as ITestProxyClass;
        Assert.NotNull(proxyObject);

        var syncResult = proxyObject.SyncMethod("test");
        Assert.Equal("test", syncResult);

        await proxyObject.AsyncMethod();

        var asyncResult = await proxyObject.AsyncMethodWithResult(100);
        Assert.Equal(100, asyncResult);

        var GenericResult = proxyObject.GenericMethod(true);
        Assert.True(GenericResult);
    }

    [Fact]
    public async Task DecorateOfObject2_ReturnOK()
    {
        var proxyObject = DispatchProxyDecorator.Decorate(typeof(ITestProxyClass), typeof(TestDispatchProxyDecorator), new TestProxyClass()) as ITestProxyClass;
        Assert.NotNull(proxyObject);

        var syncResult = proxyObject.SyncMethod("test");
        Assert.Equal("test", syncResult);

        await proxyObject.AsyncMethod();

        var asyncResult = await proxyObject.AsyncMethodWithResult(100);
        Assert.Equal(100, asyncResult);

        var GenericResult = proxyObject.GenericMethod(true);
        Assert.True(GenericResult);
    }

    [Fact]
    public async Task DecorateOfObject3_ReturnOK()
    {
        var proxyObject = DispatchProxyDecorator.Decorate<ITestProxyClass>(typeof(TestDispatchProxyDecorator), new TestProxyClass()) as ITestProxyClass;
        Assert.NotNull(proxyObject);

        var syncResult = proxyObject.SyncMethod("test");
        Assert.Equal("test", syncResult);

        await proxyObject.AsyncMethod();

        var asyncResult = await proxyObject.AsyncMethodWithResult(100);
        Assert.Equal(100, asyncResult);

        var GenericResult = proxyObject.GenericMethod(true);
        Assert.True(GenericResult);
    }

    [Fact]
    public void DecorateOfObject_NotInstanceOf_ReturnOops()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var proxyObject = DispatchProxyDecorator.Decorate(typeof(ITestProxyClass), typeof(TestDispatchProxyDecorator), new TestProxyClassWithNotInterface()) as ITestProxyClass;
        });

        Assert.Equal("The target object is not instance of type 'ITestProxyClass'.", exception.Message);
    }

    [Fact]
    public void DecorateOfObject_NotAssignableFrom_ReturnOops()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var proxyObject = DispatchProxyDecorator.Decorate(typeof(ITestProxyClass), typeof(TestDispatchProxyDecoratorOfT), new TestProxyClass()) as ITestProxyClass;
        });

        Assert.Equal("Type 'TestDispatchProxyDecoratorOfT' is not assignable from 'DispatchProxyDecorator'.", exception.Message);
    }

    [Fact]
    public async Task DecorateOfT_NullTarget_ReturnOops()
    {
        var proxyObject = DispatchProxyDecorator<ITestProxyClass>.Decorate<TestDispatchProxyDecoratorOfT>();
        Assert.NotNull(proxyObject);

        Assert.Throws<ArgumentNullException>(() =>
        {
            var syncResult = proxyObject.SyncMethod("test");
        });

        await Assert.ThrowsAsync<ArgumentNullException>(proxyObject.AsyncMethod);

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            var asyncResult = await proxyObject.AsyncMethodWithResult(100);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var GenericResult = proxyObject.GenericMethod(true);
        });
    }

    [Fact]
    public async Task DecorateOfT_NullTarget_ReturnOK()
    {
        var proxyObject = DispatchProxyDecorator<ITestProxyClass>.Decorate<TestDispatchProxyDecoratorOfT_NotClass>();
        Assert.NotNull(proxyObject);

        var syncResult = proxyObject.SyncMethod("test");
        Assert.Equal("test", syncResult);

        await proxyObject.AsyncMethod();

        var asyncResult = await proxyObject.AsyncMethodWithResult(100);
        Assert.Equal(100, asyncResult);

        var GenericResult = proxyObject.GenericMethod(true);
        Assert.True(GenericResult);
    }
}