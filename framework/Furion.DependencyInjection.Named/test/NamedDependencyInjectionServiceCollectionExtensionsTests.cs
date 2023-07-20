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

public class NamedDependencyInjectionServiceCollectionExtensionsTests
{
    [Fact]
    public void AddNamedService_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddNamedService();

        var serviceDescriptor = services.FirstOrDefault();

        Assert.NotNull(serviceDescriptor);
        Assert.Single(services);
        Assert.Equal(typeof(INamedService<>), serviceDescriptor.ServiceType);
        Assert.Equal(typeof(NamedService<>), serviceDescriptor.ImplementationType);
    }

    [Fact]
    public void AddNamedService_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedService();
        services.AddNamedService();
        services.AddNamedService();

        Assert.Single(services);
    }

    [Fact]
    public void AddNamed_Invalid_Parameters()
    {
        var services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() =>
        {
            services.AddNamed(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            services.AddNamed(string.Empty, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            services.AddNamed("", null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            services.AddNamed("named", null!);
        });
    }

    [Fact]
    public void AddNamed_ReturnOK()
    {
        var services = new ServiceCollection();

        var serviceDescriptor = ServiceDescriptor.Transient<ITestNamedService, TestNamedService>();
        services.AddNamed("name", serviceDescriptor);
        var descriptor = services.LastOrDefault();

        Assert.Equal(2, services.Count);
        Assert.NotNull(descriptor);
        Assert.True(descriptor.ServiceType is NamedTypeDelegator);
    }

    [Fact]
    public void AddNamed_Duplicate()
    {
        var services = new ServiceCollection();

        var serviceDescriptor = ServiceDescriptor.Transient<ITestNamedService, TestNamedService>();
        services.AddNamed("name", serviceDescriptor);
        services.AddNamed("name", serviceDescriptor);
        services.AddNamed("name", serviceDescriptor);

        Assert.Equal(4, services.Count);
    }

    [Fact]
    public void TryAddNamed_Invalid_Parameters()
    {
        var services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() =>
        {
            services.TryAddNamed(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            services.TryAddNamed(string.Empty, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            services.TryAddNamed("", null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            services.TryAddNamed("named", null!);
        });
    }

    [Fact]
    public void TryAddNamed_ReturnOK()
    {
        var services = new ServiceCollection();

        var serviceDescriptor = ServiceDescriptor.Transient<ITestNamedService, TestNamedService>();
        services.TryAddNamed("name", serviceDescriptor);
        var descriptor = services.LastOrDefault();

        Assert.Equal(2, services.Count);
        Assert.NotNull(descriptor);
        Assert.True(descriptor.ServiceType is NamedTypeDelegator);
    }

    [Fact]
    public void TryAddNamed_Duplicate()
    {
        var services = new ServiceCollection();

        var serviceDescriptor = ServiceDescriptor.Transient<ITestNamedService, TestNamedService>();
        services.TryAddNamed("name", serviceDescriptor);
        services.TryAddNamed("name", serviceDescriptor);
        services.TryAddNamed("name", serviceDescriptor);

        Assert.Equal(2, services.Count);
    }

    [Fact]
    public void AddNamedTransient_ServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedTransient_ServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient("name", typeof(ITestNamedService), s => new TestNamedService());
        services.AddNamedTransient("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedTransient_ServiceType_With_ImplementationType()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient("name", typeof(ITestNamedService), typeof(TestNamedService));

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedTransient_ServiceType_With_ImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient("name", typeof(ITestNamedService), typeof(TestNamedService));
        services.AddNamedTransient("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedTransient_GenericServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedTransient_GenericServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService>("name", s => new TestNamedService());
        services.AddNamedTransient("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedTransient_GenericServiceType_With_GenericImplementationType()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedTransient_GenericServiceType_With_GenericImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedTransient_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedTransient_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name", s => new TestNamedService());
        services.AddNamedTransient<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedScoped_ServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.AddNamedScoped("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedScoped_ServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedScoped("name", typeof(ITestNamedService), s => new TestNamedService());
        services.AddNamedScoped("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedScoped_ServiceType_With_ImplementationType()
    {
        var services = new ServiceCollection();
        services.AddNamedScoped("name", typeof(ITestNamedService), typeof(TestNamedService));

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedScoped_ServiceType_With_ImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedScoped("name", typeof(ITestNamedService), typeof(TestNamedService));
        services.AddNamedScoped("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedScoped_GenericServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.AddNamedScoped<ITestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedScoped_GenericServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedScoped<ITestNamedService>("name", s => new TestNamedService());
        services.AddNamedScoped("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedScoped_GenericServiceType_With_GenericImplementationType()
    {
        var services = new ServiceCollection();
        services.AddNamedScoped<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedScoped_GenericServiceType_With_GenericImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedScoped<ITestNamedService, TestNamedService>("name");
        services.AddNamedScoped<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedScoped_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.AddNamedScoped<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedScoped_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedScoped<ITestNamedService, TestNamedService>("name", s => new TestNamedService());
        services.AddNamedScoped<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_ServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_ServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton("name", typeof(ITestNamedService), s => new TestNamedService());
        services.AddNamedSingleton("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_ServiceType_With_ImplementationType()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton("name", typeof(ITestNamedService), typeof(TestNamedService));

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_ServiceType_With_ImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton("name", typeof(ITestNamedService), typeof(TestNamedService));
        services.AddNamedSingleton("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_GenericServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton<ITestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_GenericServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton<ITestNamedService>("name", s => new TestNamedService());
        services.AddNamedSingleton("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_GenericServiceType_With_GenericImplementationType()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_GenericServiceType_With_GenericImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton<ITestNamedService, TestNamedService>("name");
        services.AddNamedSingleton<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton<ITestNamedService, TestNamedService>("name", s => new TestNamedService());
        services.AddNamedSingleton<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_GenericServiceType_With_ImplementationInstance()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton<ITestNamedService>("name", new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_GenericServiceType_With_ImplementationInstance_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton<ITestNamedService>("name", new TestNamedService());
        services.AddNamedSingleton<ITestNamedService>("name", new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_ServiceType_With_ImplementationInstance()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton("name", typeof(ITestNamedService), new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddNamedSingleton_ServiceType_With_ImplementationInstance_Duplicate()
    {
        var services = new ServiceCollection();
        services.AddNamedSingleton("name", typeof(ITestNamedService), new TestNamedService());
        services.AddNamedSingleton("name", typeof(ITestNamedService), new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(3, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedTransient_ServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.TryAddNamedTransient("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedTransient_ServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedTransient("name", typeof(ITestNamedService), s => new TestNamedService());
        services.TryAddNamedTransient("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedTransient_ServiceType_With_ImplementationType()
    {
        var services = new ServiceCollection();
        services.TryAddNamedTransient("name", typeof(ITestNamedService), typeof(TestNamedService));

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedTransient_ServiceType_With_ImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedTransient("name", typeof(ITestNamedService), typeof(TestNamedService));
        services.TryAddNamedTransient("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedTransient_GenericServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.TryAddNamedTransient<ITestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedTransient_GenericServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedTransient<ITestNamedService>("name", s => new TestNamedService());
        services.TryAddNamedTransient("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedTransient_GenericServiceType_With_GenericImplementationType()
    {
        var services = new ServiceCollection();
        services.TryAddNamedTransient<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedTransient_GenericServiceType_With_GenericImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedTransient<ITestNamedService, TestNamedService>("name");
        services.TryAddNamedTransient<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedTransient_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.TryAddNamedTransient<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedTransient_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedTransient<ITestNamedService, TestNamedService>("name", s => new TestNamedService());
        services.TryAddNamedTransient<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedScoped_ServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.TryAddNamedScoped("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedScoped_ServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedScoped("name", typeof(ITestNamedService), s => new TestNamedService());
        services.TryAddNamedScoped("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedScoped_ServiceType_With_ImplementationType()
    {
        var services = new ServiceCollection();
        services.TryAddNamedScoped("name", typeof(ITestNamedService), typeof(TestNamedService));

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedScoped_ServiceType_With_ImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedScoped("name", typeof(ITestNamedService), typeof(TestNamedService));
        services.TryAddNamedScoped("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedScoped_GenericServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.TryAddNamedScoped<ITestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedScoped_GenericServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedScoped<ITestNamedService>("name", s => new TestNamedService());
        services.TryAddNamedScoped("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedScoped_GenericServiceType_With_GenericImplementationType()
    {
        var services = new ServiceCollection();
        services.TryAddNamedScoped<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedScoped_GenericServiceType_With_GenericImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedScoped<ITestNamedService, TestNamedService>("name");
        services.TryAddNamedScoped<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedScoped_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.TryAddNamedScoped<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedScoped_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedScoped<ITestNamedService, TestNamedService>("name", s => new TestNamedService());
        services.TryAddNamedScoped<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_ServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_ServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton("name", typeof(ITestNamedService), s => new TestNamedService());
        services.TryAddNamedSingleton("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_ServiceType_With_ImplementationType()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton("name", typeof(ITestNamedService), typeof(TestNamedService));

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_ServiceType_With_ImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton("name", typeof(ITestNamedService), typeof(TestNamedService));
        services.TryAddNamedSingleton("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_GenericServiceType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton<ITestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_GenericServiceType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton<ITestNamedService>("name", s => new TestNamedService());
        services.TryAddNamedSingleton("name", typeof(ITestNamedService), s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_GenericServiceType_With_GenericImplementationType()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_GenericServiceType_With_GenericImplementationType_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton<ITestNamedService, TestNamedService>("name");
        services.TryAddNamedSingleton<ITestNamedService, TestNamedService>("name");

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_GenericServiceType_With_GenericImplementationType_With_ImplementationFactory_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton<ITestNamedService, TestNamedService>("name", s => new TestNamedService());
        services.TryAddNamedSingleton<ITestNamedService, TestNamedService>("name", s => new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_GenericServiceType_With_ImplementationInstance()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton<ITestNamedService>("name", new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_GenericServiceType_With_ImplementationInstance_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton<ITestNamedService>("name", new TestNamedService());
        services.TryAddNamedSingleton<ITestNamedService>("name", new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_ServiceType_With_ImplementationInstance()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton("name", typeof(ITestNamedService), new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void TryAddNamedSingleton_ServiceType_With_ImplementationInstance_Duplicate()
    {
        var services = new ServiceCollection();
        services.TryAddNamedSingleton("name", typeof(ITestNamedService), new TestNamedService());
        services.TryAddNamedSingleton("name", typeof(ITestNamedService), new TestNamedService());

        var serviceDescriptor = services.LastOrDefault();
        Assert.Equal(2, services.Count);
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }
}