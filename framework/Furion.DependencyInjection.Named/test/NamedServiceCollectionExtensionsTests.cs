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

namespace Furion.DependencyInjection.Named.Tests;

public class NamedServiceCollectionExtensionsTests
{
    [Fact]
    public void AddNamed_Parameterless_Duplicate_OnlyOne()
    {
        var services = new ServiceCollection();
        services.AddNamed();
        Assert.Single(services);

        services.AddNamed();
        Assert.Single(services);

        var serviceDescriptor = services.First();
        Assert.Equal(typeof(INamedService<>), serviceDescriptor.ServiceType);
        Assert.Equal(typeof(NamedService<>), serviceDescriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void CreateDelegator_Parameters_Null_Throw()
    {
        var serviceDescriptor = ServiceDescriptor.Describe(typeof(INamedServiceClass), typeof(NamedServiceClass), ServiceLifetime.Transient);
        Assert.Throws<ArgumentNullException>(() =>
        {
            NamedServiceCollectionExtensions.CreateDelegator(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            NamedServiceCollectionExtensions.CreateDelegator(string.Empty, serviceDescriptor);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            NamedServiceCollectionExtensions.CreateDelegator(null!, serviceDescriptor);
        });
    }

    [Fact]
    public void CreateDelegator_NewServiceDescriptor()
    {
        var name = "name1";
        var serviceDescriptor = ServiceDescriptor.Describe(typeof(INamedServiceClass), typeof(NamedServiceClass), ServiceLifetime.Transient);
        var newServiceDescriptor = NamedServiceCollectionExtensions.CreateDelegator(name, serviceDescriptor);

        Assert.NotNull(newServiceDescriptor);
        Assert.NotNull(newServiceDescriptor.ImplementationType);
        Assert.True(newServiceDescriptor.ServiceType is NamedType);
        Assert.Equal(name, ((NamedType)newServiceDescriptor.ServiceType).name);
        Assert.Equal(new NamedType(name, serviceDescriptor.ServiceType), newServiceDescriptor.ServiceType);
        Assert.Equal(serviceDescriptor.Lifetime, newServiceDescriptor.Lifetime);

        var name2 = "name2";
        var instance = new NamedServiceClass();
        var serviceDescriptor2 = new ServiceDescriptor(typeof(INamedServiceClass), instance);
        var newServiceDescriptor2 = NamedServiceCollectionExtensions.CreateDelegator(name2, serviceDescriptor2);

        Assert.NotNull(newServiceDescriptor2);
        Assert.NotNull(newServiceDescriptor2.ImplementationInstance);
        Assert.Equal(instance, newServiceDescriptor2.ImplementationInstance);
        Assert.Equal(serviceDescriptor2.ImplementationInstance, newServiceDescriptor2.ImplementationInstance);
        Assert.True(newServiceDescriptor2.ServiceType is NamedType);
        Assert.Equal(name2, ((NamedType)newServiceDescriptor2.ServiceType).name);
        Assert.Equal(new NamedType(name2, serviceDescriptor2.ServiceType), newServiceDescriptor2.ServiceType);
        Assert.Equal(serviceDescriptor2.Lifetime, newServiceDescriptor2.Lifetime);

        var name3 = "name3";
        var serviceDescriptor3 = ServiceDescriptor.Describe(typeof(INamedServiceClass), sp =>
        {
            return new NamedServiceClass();
        }, ServiceLifetime.Singleton);
        var newServiceDescriptor3 = NamedServiceCollectionExtensions.CreateDelegator(name3, serviceDescriptor3);

        Assert.NotNull(newServiceDescriptor3);
        Assert.NotNull(newServiceDescriptor3.ImplementationFactory);
        Assert.True(newServiceDescriptor3.ServiceType is NamedType);
        Assert.Equal(name3, ((NamedType)newServiceDescriptor3.ServiceType).name);
        Assert.Equal(new NamedType(name3, serviceDescriptor3.ServiceType), newServiceDescriptor3.ServiceType);
        Assert.Equal(serviceDescriptor3.Lifetime, newServiceDescriptor3.Lifetime);
    }

    [Fact]
    public void AddNamed_With_Name_And_ServiceDescriptor_NullCheck_Throw()
    {
        var services = new ServiceCollection();
        var serviceDescriptor = ServiceDescriptor.Describe(typeof(INamedServiceClass), typeof(NamedServiceClass), ServiceLifetime.Transient);

        Assert.Throws<ArgumentNullException>(() =>
        {
            services.AddNamed(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            services.AddNamed(string.Empty, serviceDescriptor);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            services.AddNamed(null!, serviceDescriptor);
        });
    }

    [Fact]
    public void AddNamed_With_Name_And_ServiceDescriptor()
    {
        var name = "name1";
        var serviceDescriptor = ServiceDescriptor.Describe(typeof(INamedServiceClass), typeof(NamedServiceClass), ServiceLifetime.Transient);

        var services = new ServiceCollection();
        services.AddNamed(name, serviceDescriptor);

        Assert.Equal(2, services.Count);
        var lastServiceDescriptor = services.Last();
        Assert.True(lastServiceDescriptor.ServiceType is NamedType);
        Assert.Equal(name, ((NamedType)lastServiceDescriptor.ServiceType).name);

        services.AddNamed(name, serviceDescriptor);
        Assert.Equal(3, services.Count);
    }

    [Fact]
    public void TryAddNamed_With_Name_And_ServiceDescriptor_NullCheck_Throw()
    {
        var services = new ServiceCollection();
        var serviceDescriptor = ServiceDescriptor.Describe(typeof(INamedServiceClass), typeof(NamedServiceClass), ServiceLifetime.Transient);

        Assert.Throws<ArgumentNullException>(() =>
        {
            services.TryAddNamed(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            services.TryAddNamed(string.Empty, serviceDescriptor);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            services.TryAddNamed(null!, serviceDescriptor);
        });
    }

    [Fact]
    public void TryAddNamed_With_Name_And_ServiceDescriptor()
    {
        var name = "name1";
        var serviceDescriptor = ServiceDescriptor.Describe(typeof(INamedServiceClass), typeof(NamedServiceClass), ServiceLifetime.Transient);

        var services = new ServiceCollection();
        services.TryAddNamed(name, serviceDescriptor);

        Assert.Equal(2, services.Count);
        var lastServiceDescriptor = services.Last();
        Assert.True(lastServiceDescriptor.ServiceType is NamedType);
        Assert.Equal(name, ((NamedType)lastServiceDescriptor.ServiceType).name);

        services.TryAddNamed(name, serviceDescriptor);
        Assert.Equal(2, services.Count);
    }
}