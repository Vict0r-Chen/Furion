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

#pragma warning disable IDE0052

public interface IActivatorClass
{
    bool IsDisposed { get; }
}

public class ActivatorClass : IActivatorClass, IDisposable
{
    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        IsDisposed = true;
        GC.SuppressFinalize(this);
    }
}

public class ActivatorClassWithParameters : IActivatorClass, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _name;
    private readonly int _age;

    public ActivatorClassWithParameters(
        IServiceProvider provider
        , IServiceScopeFactory scopeFactory
        , string name
        , int age)
    {
        _serviceProvider = provider;
        _scopeFactory = scopeFactory;
        _name = name;
        _age = age;
    }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        IsDisposed = true;
        GC.SuppressFinalize(this);
    }
}

public class ActivatorClassWithParameters2 : IActivatorClass, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _name;
    private readonly int _age;

    public ActivatorClassWithParameters2(
        IServiceProvider provider
        , string name
        , IServiceScopeFactory scopeFactory
        , int age)
    {
        _serviceProvider = provider;
        _scopeFactory = scopeFactory;
        _name = name;
        _age = age;
    }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        IsDisposed = true;
        GC.SuppressFinalize(this);
    }
}

public class ActivatorClassWithParameters3 : IActivatorClass, IDisposable
{
    private readonly IServiceProvider? _serviceProvider;
    private readonly IServiceScopeFactory? _scopeFactory;
    private readonly string _name;
    private readonly int _age;

    public ActivatorClassWithParameters3(
        IServiceProvider provider
        , string name
        , IServiceScopeFactory scopeFactory
        , int age)
    {
        _serviceProvider = provider;
        _scopeFactory = scopeFactory;
        _name = name;
        _age = age;
    }

    [ActivatorUtilitiesConstructor]
    public ActivatorClassWithParameters3(string name, int age)
    {
        _name = name;
        _age = age;
    }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        IsDisposed = true;
        GC.SuppressFinalize(this);
    }
}