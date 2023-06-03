﻿namespace Furion.Tests.DependencyInjection.Named;

public class NamedServiceProviderTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetNamedService_EmptyName_ReturnOops(string name)
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        if (name == null)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                serviceProvider.GetNamedService(name!, null!);
            });
        }

        if (name == string.Empty)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                serviceProvider.GetNamedService(name!, null!);
            });
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetRequiredNamedService_EmptyName_ReturnOops(string name)
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        if (name == null)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                serviceProvider.GetRequiredNamedService(name!, null!);
            });
        }

        if (name == string.Empty)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                serviceProvider.GetRequiredNamedService(name!, null!);
            });
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetNamedServices_EmptyName_ReturnOops(string name)
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        if (name == null)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                serviceProvider.GetNamedServices(name!, null!);
            });
        }

        if (name == string.Empty)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                serviceProvider.GetNamedServices(name!, null!);
            });
        }
    }

    [Fact]
    public void GetNamedService_EmptyType_ReturnOops()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedService("name1", null!);
        });
    }

    [Fact]
    public void GetRequiredNamedService_EmptyType_ReturnOops()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetRequiredNamedService("name1", null!);
        });
    }

    [Fact]
    public void GetNamedServices_EmptyType_ReturnOops()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            serviceProvider.GetNamedServices("name1", null!);
        });
    }

    [Theory]
    [InlineData("name1")]
    [InlineData("name2")]
    public void GetNamedService_ExistsResolve_ReturnOK(string name)
    {
        var services = new ServiceCollection();
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetNamedService<INamedClass>(name);
        Assert.NotNull(service1);

        var service2 = serviceProvider.GetNamedService(name, typeof(INamedClass));
        Assert.NotNull(service2);
    }

    [Theory]
    [InlineData("name1")]
    [InlineData("name2")]
    public void GetRequiredNamedService_ExistsResolve_ReturnOK(string name)
    {
        var services = new ServiceCollection();
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetRequiredNamedService<INamedClass>(name);
        Assert.NotNull(service1);

        var service2 = serviceProvider.GetRequiredNamedService(name, typeof(INamedClass));
        Assert.NotNull(service2);
    }

    [Theory]
    [InlineData("name1")]
    [InlineData("name2")]
    public void GetNamedServices_ExistsResolve_ReturnOK(string name)
    {
        var services = new ServiceCollection();
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var service1s = serviceProvider.GetNamedServices<INamedClass>(name);
        var count1 = service1s.LongCount();
        Assert.NotNull(service1s);
        Assert.Equal(1, count1);

        var service2s = serviceProvider.GetNamedServices(name, typeof(INamedClass));
        var count2 = service1s.LongCount();
        Assert.NotNull(service2s);
        Assert.Equal(1, count2);
    }

    [Fact]
    public void GetNamedService_NotExistsResolve_ReturnOK()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamed(name + "_miss", ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetNamedService<INamedClass>(name);
        var service2 = serviceProvider.GetNamedService(name, typeof(INamedClass));

        Assert.Null(service1);
        Assert.Null(service2);
    }

    [Fact]
    public void GetRequiredNamedService_NotExistsResolve_ReturnOops()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamed(name + "_miss", ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() =>
        {
            serviceProvider.GetRequiredNamedService<INamedClass>(name);
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            serviceProvider.GetRequiredNamedService(name, typeof(INamedClass));
        });
    }

    [Fact]
    public void GetNamedServices_NotExistsResolve_ReturnOK()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamed(name + "_miss", ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var service1s = serviceProvider.GetNamedServices<INamedClass>(name);
        var service2s = serviceProvider.GetNamedServices(name, typeof(INamedClass));

        var count1 = service1s.LongCount();
        var count2 = service2s.LongCount();
        Assert.Equal(0, count1);
        Assert.Equal(0, count2);
    }

    [Fact]
    public void GetNamedService_ResolveObject_ReturnOk()
    {
        var name = "name1";
        var obj = new NamedClass();

        var services = new ServiceCollection();
        services.AddNamedSingleton("name1", obj);

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetNamedService<NamedClass>(name);
        var service2 = serviceProvider.GetNamedService(name, typeof(NamedClass));

        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.Equal(obj, service1);
        Assert.Equal(obj, service2);
    }

    [Fact]
    public void GetRequiredNamedService_ResolveObject_ReturnOk()
    {
        var name = "name1";
        var obj = new NamedClass();

        var services = new ServiceCollection();
        services.AddNamedSingleton("name1", obj);

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetRequiredNamedService<NamedClass>(name);
        var service2 = serviceProvider.GetRequiredNamedService(name, typeof(NamedClass));

        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.Equal(obj, service1);
        Assert.Equal(obj, service2);
    }

    [Fact]
    public void GetNamedService_ResolveFactory_ReturnOk()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamedScoped<INamedClass, NamedClass>("name1", sp =>
        {
            return new NamedClass();
        });

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetNamedService<INamedClass>(name);
        var service2 = serviceProvider.GetNamedService(name, typeof(INamedClass));

        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.Equal(service1, service2);
    }

    [Fact]
    public void GetRequiredNamedService_ResolveFactory_ReturnOk()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamedScoped<INamedClass, NamedClass>("name1", sp =>
        {
            return new NamedClass();
        });

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetRequiredNamedService<INamedClass>(name);
        var service2 = serviceProvider.GetRequiredNamedService(name, typeof(INamedClass));

        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.Equal(service1, service2);
    }

    [Fact]
    public void GetNamedService_ResolveTransientType_ReturnOk()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamedTransient<INamedClass, NamedClass>(name);
        var serviceProvider = services.BuildServiceProvider();

        var service1 = serviceProvider.GetNamedService<INamedClass>(name);
        var service2 = serviceProvider.GetNamedService<INamedClass>(name);
        Assert.NotEqual(service1, service2);

        using var scoped = serviceProvider.CreateScope();
        var service3 = scoped.ServiceProvider.GetNamedService<INamedClass>(name);
        var service4 = scoped.ServiceProvider.GetNamedService<INamedClass>(name);
        Assert.NotEqual(service3, service4);

        Assert.NotEqual(service1, service3);
        Assert.NotEqual(service1, service4);
        Assert.NotEqual(service2, service3);
        Assert.NotEqual(service2, service4);
    }

    [Fact]
    public void GetNamedService_ResolveScopedType_ReturnOk()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamedScoped<INamedClass, NamedClass>(name);
        var serviceProvider = services.BuildServiceProvider();

        var service1 = serviceProvider.GetNamedService<INamedClass>(name);
        var service2 = serviceProvider.GetNamedService<INamedClass>(name);
        Assert.Equal(service1, service2);

        using var scoped = serviceProvider.CreateScope();
        var service3 = scoped.ServiceProvider.GetNamedService<INamedClass>(name);
        var service4 = scoped.ServiceProvider.GetNamedService<INamedClass>(name);
        Assert.Equal(service3, service4);

        Assert.NotEqual(service1, service3);
        Assert.NotEqual(service1, service4);
        Assert.NotEqual(service2, service3);
        Assert.NotEqual(service2, service4);
    }

    [Fact]
    public void GetNamedService_ResolveSingletonType_ReturnOk()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamedSingleton<INamedClass, NamedClass>(name);
        var serviceProvider = services.BuildServiceProvider();

        var service1 = serviceProvider.GetNamedService<INamedClass>(name);
        var service2 = serviceProvider.GetNamedService<INamedClass>(name);
        Assert.Equal(service1, service2);

        using var scoped = serviceProvider.CreateScope();
        var service3 = scoped.ServiceProvider.GetNamedService<INamedClass>(name);
        var service4 = scoped.ServiceProvider.GetNamedService<INamedClass>(name);
        Assert.Equal(service3, service4);

        Assert.Equal(service1, service3);
        Assert.Equal(service1, service4);
        Assert.Equal(service2, service3);
        Assert.Equal(service2, service4);
    }

    [Fact]
    public void GetNamedService_ResolveScopedFactory_ReturnOk()
    {
        var name1 = "name1";
        var name2 = "name2";
        var services = new ServiceCollection();
        services.AddNamedScoped<INamedClass, NamedClass>(name1, sp =>
        {
            return new NamedClass();
        });

        services.AddNamedScoped<INamedClass, NamedClass>(name2, sp =>
        {
            return new NamedClass();
        });

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetNamedService<INamedClass>(name1);
        var service2 = serviceProvider.GetNamedService<INamedClass>(name1);
        Assert.Equal(service1, service2);

        using var scoped = serviceProvider.CreateScope();
        var service3 = scoped.ServiceProvider.GetNamedService<INamedClass>(name2);
        var service4 = scoped.ServiceProvider.GetNamedService<INamedClass>(name2);
        Assert.Equal(service3, service4);

        Assert.NotEqual(service1, service3);
        Assert.NotEqual(service1, service4);
        Assert.NotEqual(service2, service3);
        Assert.NotEqual(service2, service4);
    }

    [Fact]
    public void GetNamedService_RepeatName_ReturnOk()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamedScoped<INamedClass, NamedClass>(name);
        services.AddNamedScoped<INamedClass2, NamedClass2>(name);

        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetNamedService<INamedClass>(name);
        var service2 = serviceProvider.GetNamedService<INamedClass2>(name);

        Assert.NotNull(service1);
        Assert.NotNull(service2);

        var service1Type = service1.GetType();
        var service2Type = service2.GetType();

        Assert.Equal(typeof(NamedClass), service1Type);
        Assert.Equal(typeof(NamedClass2), service2Type);
    }
}