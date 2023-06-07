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
    private readonly Dictionary<object, object?> _properties;
    private readonly ITestOutputHelper _output;

    public DispatchProxyDecoratorTests(ITestOutputHelper output)
    {
        _output = output;
        _properties = new Dictionary<object, object?>
        {
            {nameof(output),_output }
        };
    }

    [Fact]
    public async Task DecorateOfObject_ReturnOK()
    {
        var proxyObject = DispatchProxyDecorator.Decorate<ITestProxyClass, TestDispatchProxyDecorator>(new TestProxyClass(), _properties);
        Assert.NotNull(proxyObject);

        proxyObject.Name = "Furion";
        var name = proxyObject.Name;
        Assert.Equal("Furion", name);

        var syncResult = proxyObject.SyncMethod("test");
        Assert.Equal("test", syncResult);

        await proxyObject.AsyncMethod();

        var asyncResult = await proxyObject.AsyncMethodWithResult(100);
        Assert.Equal(100, asyncResult);

        var GenericResult = proxyObject.GenericMethod(true);
        Assert.True(GenericResult);

        await proxyObject.AsyncGenericMethod<string>();

        var asyncResult2 = await proxyObject.AsyncGenericMethodWithResult(100);
        Assert.Equal(100, asyncResult2);

        var internalCallResult = proxyObject.InternalCallSyncMethod("test");
        Assert.Equal("test", internalCallResult);

        var displayResult = proxyObject.DisplayImplementation("test");
        Assert.Equal("test", displayResult);

        var defaultResult = proxyObject.Default("test");
        Assert.Equal("test", defaultResult);
    }

    [Fact]
    public async Task DecorateOfObject2_ReturnOK()
    {
        var proxyObject = DispatchProxyDecorator.Decorate(typeof(ITestProxyClass), typeof(TestDispatchProxyDecorator), new TestProxyClass(), _properties) as ITestProxyClass;
        Assert.NotNull(proxyObject);

        proxyObject.Name = "Furion";
        var name = proxyObject.Name;
        Assert.Equal("Furion", name);

        var syncResult = proxyObject.SyncMethod("test");
        Assert.Equal("test", syncResult);

        await proxyObject.AsyncMethod();

        var asyncResult = await proxyObject.AsyncMethodWithResult(100);
        Assert.Equal(100, asyncResult);

        var GenericResult = proxyObject.GenericMethod(true);
        Assert.True(GenericResult);

        await proxyObject.AsyncGenericMethod<string>();

        var asyncResult2 = await proxyObject.AsyncGenericMethodWithResult(100);
        Assert.Equal(100, asyncResult2);

        var internalCallResult = proxyObject.InternalCallSyncMethod("test");
        Assert.Equal("test", internalCallResult);

        var displayResult = proxyObject.DisplayImplementation("test");
        Assert.Equal("test", displayResult);

        var defaultResult = proxyObject.Default("test");
        Assert.Equal("test", defaultResult);
    }

    [Fact]
    public async Task DecorateOfObject3_ReturnOK()
    {
        var proxyObject = DispatchProxyDecorator.Decorate<ITestProxyClass>(typeof(TestDispatchProxyDecorator), new TestProxyClass(), _properties);
        Assert.NotNull(proxyObject);

        proxyObject.Name = "Furion";
        var name = proxyObject.Name;
        Assert.Equal("Furion", name);

        var syncResult = proxyObject.SyncMethod("test");
        Assert.Equal("test", syncResult);

        await proxyObject.AsyncMethod();

        var asyncResult = await proxyObject.AsyncMethodWithResult(100);
        Assert.Equal(100, asyncResult);

        var GenericResult = proxyObject.GenericMethod(true);
        Assert.True(GenericResult);

        await proxyObject.AsyncGenericMethod<string>();

        var asyncResult2 = await proxyObject.AsyncGenericMethodWithResult(100);
        Assert.Equal(100, asyncResult2);

        var internalCallResult = proxyObject.InternalCallSyncMethod("test");
        Assert.Equal("test", internalCallResult);

        var displayResult = proxyObject.DisplayImplementation("test");
        Assert.Equal("test", displayResult);

        var defaultResult = proxyObject.Default("test");
        Assert.Equal("test", defaultResult);
    }

    [Fact]
    public void DecorateOfObject_NotInstanceOf_ReturnOops()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var proxyObject = DispatchProxyDecorator.Decorate(typeof(ITestProxyClass), typeof(TestDispatchProxyDecorator), new TestProxyClassWithNotInterface(), _properties) as ITestProxyClass;
        });

        Assert.Equal("The target object is not instance of type 'ITestProxyClass'.", exception.Message);
    }

    [Fact]
    public void DecorateOfObject_NotAssignableFrom_ReturnOops()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var proxyObject = DispatchProxyDecorator.Decorate(typeof(ITestProxyClass), typeof(TestProxyClassWithNotInterface), new TestProxyClass(), _properties) as ITestProxyClass;
        });

        Assert.Equal("Type 'TestProxyClassWithNotInterface' is not assignable from 'DispatchProxyDecorator'.", exception.Message);
    }

    [Fact]
    public async Task DecorateOfT_NullTarget_ReturnOops()
    {
        var proxyObject = DispatchProxyDecorator.Decorate<ITestProxyClass, TestDispatchProxyDecorator>(properties: _properties);
        Assert.NotNull(proxyObject);

        Assert.Throws<ArgumentNullException>(() =>
        {
            proxyObject.Name = "Furion";
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var name = proxyObject.Name;
        });

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

        await Assert.ThrowsAsync<ArgumentNullException>(proxyObject.AsyncGenericMethod<string>);

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            var asyncResult2 = await proxyObject.AsyncGenericMethodWithResult(100);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var internalCallResult = proxyObject.InternalCallSyncMethod("test");
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var displayResult = proxyObject.DisplayImplementation("test");
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var defaultResult = proxyObject.Default("test");
        });
    }

    [Fact]
    public async Task DecorateOfT_NullTarget_ReturnOK()
    {
        var proxyObject = DispatchProxyDecorator.Decorate<ITestProxyClass, TestDispatchProxyDecorator_NotClass>(properties: _properties);
        Assert.NotNull(proxyObject);

        proxyObject.Name = "Furion";

        Assert.Throws<IndexOutOfRangeException>(() =>
        {
            var name = proxyObject.Name;
        });

        var syncResult = proxyObject.SyncMethod("test");
        Assert.Equal("test", syncResult);

        await proxyObject.AsyncMethod();

        var asyncResult = await proxyObject.AsyncMethodWithResult(100);
        Assert.Equal(100, asyncResult);

        var GenericResult = proxyObject.GenericMethod(true);
        Assert.True(GenericResult);

        await proxyObject.AsyncGenericMethod<string>();

        var asyncResult2 = await proxyObject.AsyncGenericMethodWithResult(100);
        Assert.Equal(100, asyncResult2);

        var internalCallResult = proxyObject.InternalCallSyncMethod("test");
        Assert.Equal("test", internalCallResult);

        var displayResult = proxyObject.DisplayImplementation("test");
        Assert.Equal("test", displayResult);

        var defaultResult = proxyObject.Default("test");
        Assert.Equal("test", defaultResult);
    }
}