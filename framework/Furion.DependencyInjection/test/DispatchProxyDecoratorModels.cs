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

public interface ITestProxyClass
{
    string SyncMethod(string str);

    Task AsyncMethod();

    Task<int> AsyncMethodWithResult(int num);

    T GenericMethod<T>(T obj);

    Task AsyncGenericMethod<T>();

    Task<T> AsyncGenericMethodWithResult<T>(T obj);
}

public class TestProxyClass : ITestProxyClass
{
    public string SyncMethod(string str)
    {
        return str;
    }

    public async Task AsyncMethod()
    {
        await Task.Delay(10);
    }

    public async Task<int> AsyncMethodWithResult(int num)
    {
        await Task.Delay(10);
        return num;
    }

    public T GenericMethod<T>(T obj)
    {
        return obj;
    }

    public async Task AsyncGenericMethod<T>()
    {
        await Task.Delay(10);
    }

    public async Task<T> AsyncGenericMethodWithResult<T>(T num)
    {
        await Task.Delay(10);
        return num;
    }
}

public class TestProxyClassWithNotInterface
{
}

public class TestDispatchProxyDecoratorOfT : DispatchProxyDecorator<ITestProxyClass>
{
    public override object? Invoke(Invocation<ITestProxyClass> invocation)
    {
        ((ITestOutputHelper)invocation.Properties!["output"]!).WriteLine($"{invocation.Method}");

        var result = invocation.Proceed();
        return result;
    }

    public override async Task InvokeAsync(Invocation<ITestProxyClass> invocation)
    {
        ((ITestOutputHelper)invocation.Properties!["output"]!).WriteLine($"{invocation.Method}");

        await invocation.ProceedAsync();
    }

    public override async Task<T?> InvokeAsync<T>(Invocation<ITestProxyClass> invocation)
        where T : default
    {
        ((ITestOutputHelper)invocation.Properties!["output"]!).WriteLine($"{invocation.Method}");

        var result = await invocation.ProceedAsync<T>();
        return result;
    }
}

public class TestDispatchProxyDecorator : DispatchProxyDecorator
{
    public override object? Invoke(Invocation invocation)
    {
        ((ITestOutputHelper)invocation.Properties!["output"]!).WriteLine($"{invocation.Method}");

        var result = invocation.Proceed();
        return result;
    }

    public override async Task InvokeAsync(Invocation invocation)
    {
        ((ITestOutputHelper)invocation.Properties!["output"]!).WriteLine($"{invocation.Method}");

        await invocation.ProceedAsync();
    }

    public override async Task<T?> InvokeAsync<T>(Invocation invocation)
        where T : default
    {
        ((ITestOutputHelper)invocation.Properties!["output"]!).WriteLine($"{invocation.Method}");

        var result = await invocation.ProceedAsync<T>();
        return result;
    }
}

public class TestDispatchProxyDecoratorOfT_NotClass : DispatchProxyDecorator<ITestProxyClass>
{
    public override object? Invoke(Invocation<ITestProxyClass> invocation)
    {
        ((ITestOutputHelper)invocation.Properties!["output"]!).WriteLine($"{invocation.Method}");

        return invocation.Args![0];
    }

    public override async Task InvokeAsync(Invocation<ITestProxyClass> invocation)
    {
        ((ITestOutputHelper)invocation.Properties!["output"]!).WriteLine($"{invocation.Method}");

        await Task.CompletedTask;
    }

    public override async Task<T?> InvokeAsync<T>(Invocation<ITestProxyClass> invocation)
        where T : default
    {
        ((ITestOutputHelper)invocation.Properties!["output"]!).WriteLine($"{invocation.Method}");

        return await Task.FromResult((T)invocation.Args![0]!);
    }
}