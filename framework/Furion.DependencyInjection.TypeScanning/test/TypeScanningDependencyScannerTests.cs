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

namespace Furion.DependencyInjection.TypeScanning.Tests;

public class TypeScanningDependencyScannerTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() => { var typeScanningDependencyScanner = new TypeScanningDependencyScanner(null!, null!); }); Assert.Throws<ArgumentNullException>(() =>
        {
            var typeScanningDependencyScanner = new TypeScanningDependencyScanner(new ServiceCollection(), null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        Assert.NotNull(typeScanningDependencyScanner._services);
        Assert.NotNull(typeScanningDependencyScanner._typeScanningDependencyBuilder);

        Assert.Single(typeScanningDependencyScanner._typeScanningDependencyBuilder._assemblies);
    }

    [Fact]
    public void Initialize_ReturnOK()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        typeScanningDependencyScanner.Initialize();
        Assert.Single(typeScanningDependencyScanner._typeScanningDependencyBuilder._assemblies);
    }

    [Fact]
    public void ScanToAddServices()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddAssemblies(GetType().Assembly);
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        typeScanningDependencyScanner.ScanToAddServices();
        Assert.NotNull(services);
        Assert.NotEmpty(services);
    }

    [Fact]
    public void ScanToAddServices_SuppressAssemblyScanning()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder
        {
            SuppressAssemblyScanning = true
        };
        typeScanningDependencyBuilder.AddAssemblies(GetType().Assembly);
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        typeScanningDependencyScanner.ScanToAddServices();
        Assert.NotNull(services);
        Assert.Empty(services);
    }

    [Fact]
    public void ScanToAddServices_Order()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddFilter(model =>
        {
            if (model.Descriptor.ImplementationType == typeof(DependencyClass))
            {
                model.Order = 1;
            }

            return true;
        });

        typeScanningDependencyBuilder.AddAssemblies(GetType().Assembly);
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        typeScanningDependencyScanner.ScanToAddServices();
        Assert.NotNull(services);
        Assert.NotEmpty(services);
        Assert.Equal(typeof(DependencyClass), services.Last().ImplementationType);
    }

    [Theory]
    [InlineData(0, typeof(DependencyClass), typeof(AbstractDependencyService), typeof(IDependencyService), typeof(IDependencyService<string>), typeof(IDependencyService<string, int>))]
    [InlineData(1, typeof(GenericDependencyClass<>))]
    [InlineData(2, typeof(IDependencyService<>))]
    [InlineData(3, typeof(IDependencyService<,>))]
    [InlineData(4, typeof(GenericDependencyClass2<,>))]
    [InlineData(5, typeof(IDependencyService), typeof(IDependencyService<string, int>))]
    [InlineData(6, typeof(AbstractDependencyService))]
    [InlineData(7, typeof(DependencyClass5))]
    public void CreateModels(int index, params Type[] serviceTypes)
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddAssemblies(GetType().Assembly);
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        var models = typeScanningDependencyScanner.CreateModels().ToList();
        Assert.NotNull(models);
        Assert.NotEmpty(models);

        var groups = models.GroupBy(u => u.Descriptor.ImplementationType).ToList();
        var groupServiceTypes = groups.ElementAt(index).Select(c => c.Descriptor.ServiceType).ToArray();
        Assert.NotNull(groupServiceTypes);
        Assert.Equal(serviceTypes, groupServiceTypes);
    }

    [Fact]
    public void CreateModels_SuppressNonPublicType()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder
        {
            SuppressNonPublicType = true
        };
        typeScanningDependencyBuilder.AddAssemblies(GetType().Assembly);
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        var models = typeScanningDependencyScanner.CreateModels().ToList();
        Assert.NotNull(models);
        Assert.NotEmpty(models);

        Assert.DoesNotContain(models, m => m.Descriptor.ImplementationType == typeof(DependencyClass5));
    }

    [Fact]
    public void CreateModels_AddTypeFilter()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddTypeFilter(t => t != typeof(DependencyClass));

        typeScanningDependencyBuilder.AddAssemblies(GetType().Assembly);
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        var models = typeScanningDependencyScanner.CreateModels().ToList();
        Assert.NotNull(models);
        Assert.NotEmpty(models);

        Assert.DoesNotContain(models, m => m.Descriptor.ImplementationType == typeof(DependencyClass));
    }

    [Fact]
    public void CreateModels_AddFilter()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddFilter(model => model.Descriptor.ImplementationType != typeof(DependencyClass));

        typeScanningDependencyBuilder.AddAssemblies(GetType().Assembly);
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        var models = typeScanningDependencyScanner.CreateModels().ToList();
        Assert.NotNull(models);
        Assert.NotEmpty(models);

        Assert.DoesNotContain(models, m => m.Descriptor.ImplementationType == typeof(DependencyClass));
    }

    [Fact]
    public void GetServiceTypes_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        Assert.Throws<ArgumentNullException>(() =>
        {
            typeScanningDependencyScanner.GetServiceTypes(null!, out _);
        });
    }

    [Theory]
    [InlineData(0, ServiceLifetime.Transient, typeof(IDependencyService), typeof(IDependencyService<string>), typeof(IDependencyService<string, int>))]
    [InlineData(1, ServiceLifetime.Transient)]
    [InlineData(2, ServiceLifetime.Transient, typeof(IDependencyService<>))]
    [InlineData(3, ServiceLifetime.Transient, typeof(IDependencyService<,>))]
    [InlineData(4, ServiceLifetime.Transient)]
    [InlineData(5, ServiceLifetime.Transient, typeof(IDependencyService), typeof(IDependencyService<string, int>))]
    [InlineData(6, ServiceLifetime.Transient)]
    [InlineData(7, ServiceLifetime.Transient)]
    [InlineData(8, ServiceLifetime.Transient)]
    public void GetServiceTypes_ReturnOK(int index, ServiceLifetime lifetime, params Type[] serviceTypes)
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        var types = GetType().Assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract && typeof(IDependency).IsAssignableFrom(type)).ToList();

        Assert.NotNull(types);
        Assert.NotEmpty(types);

        var type = types.ElementAt(index);
        Assert.NotNull(type);
        var typeServiceTypes = typeScanningDependencyScanner.GetServiceTypes(type, out var serviceLifetime);
        Assert.NotNull(typeServiceTypes);
        Assert.Equal(lifetime, serviceLifetime);
        Assert.Equal(serviceTypes, typeServiceTypes.ToArray());
    }

    [Fact]
    public void AddService_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        Assert.Throws<ArgumentNullException>(() =>
        {
            typeScanningDependencyScanner.AddService(null!);
        });
    }

    [Fact]
    public void AddService_ReturnOK()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        var typeScanningDependencyScanner = new TypeScanningDependencyScanner(services, typeScanningDependencyBuilder);

        var typeScanningDependencyModel = new TypeScanningDependencyModel(typeof(IDependencyService), typeof(DependencyClass), ServiceLifetime.Transient, RegistrationType.Add);
        typeScanningDependencyScanner.AddService(typeScanningDependencyModel);

        Assert.Single(typeScanningDependencyScanner._services);
        var descriptor = typeScanningDependencyScanner._services.FirstOrDefault();
        Assert.NotNull(descriptor);
        Assert.Equal(typeof(IDependencyService), descriptor.ServiceType);
        Assert.Equal(typeof(DependencyClass), descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);

        var typeScanningDependencyModel2 = new TypeScanningDependencyModel(typeof(IDependencyService), typeof(DependencyClass), ServiceLifetime.Transient, RegistrationType.Add);
        typeScanningDependencyScanner.AddService(typeScanningDependencyModel2);
        Assert.Equal(2, typeScanningDependencyScanner._services.Count);

        var typeScanningDependencyModel3 = new TypeScanningDependencyModel(typeof(IDependencyService), typeof(DependencyClass2), ServiceLifetime.Transient, RegistrationType.TryAdd);
        typeScanningDependencyScanner.AddService(typeScanningDependencyModel3);
        Assert.Equal(2, typeScanningDependencyScanner._services.Count);

        var typeScanningDependencyModel4 = new TypeScanningDependencyModel(typeof(IDependencyService), typeof(DependencyClass2), ServiceLifetime.Transient, RegistrationType.TryAddEnumerable);
        typeScanningDependencyScanner.AddService(typeScanningDependencyModel4);
        Assert.Equal(3, typeScanningDependencyScanner._services.Count);

        var typeScanningDependencyModel5 = new TypeScanningDependencyModel(typeof(IDependencyService), typeof(DependencyClass3), ServiceLifetime.Transient, RegistrationType.Replace);
        typeScanningDependencyScanner.AddService(typeScanningDependencyModel5);
        Assert.Equal(3, typeScanningDependencyScanner._services.Count);
    }

    [Fact]
    public void GetServiceLifetime_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            TypeScanningDependencyScanner.GetServiceLifetime(null!);
        });

        Assert.Throws<NotSupportedException>(() =>
        {
            TypeScanningDependencyScanner.GetServiceLifetime(typeof(IDependency));
        });
    }

    [Theory]
    [InlineData(typeof(ITransientDependency), ServiceLifetime.Transient)]
    [InlineData(typeof(IScopedDependency), ServiceLifetime.Scoped)]
    [InlineData(typeof(ISingletonDependency), ServiceLifetime.Singleton)]
    public void GetServiceLifetime_ReturnOK(Type dependencyType, ServiceLifetime lifetime)
    {
        Assert.Equal(lifetime, TypeScanningDependencyScanner.GetServiceLifetime(dependencyType));
    }

    [Fact]
    public void GetTypeDefinition_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            TypeScanningDependencyScanner.GetTypeDefinition(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            TypeScanningDependencyScanner.GetTypeDefinition(typeof(DependencyClass), null!);
        });
    }

    [Fact]
    public void GetTypeDefinition_ReturnOK()
    {
        var type = TypeScanningDependencyScanner.GetTypeDefinition(typeof(DependencyClass), typeof(DependencyClass));

        Assert.NotNull(type);
        Assert.Equal(typeof(DependencyClass), type);

        var type1 = TypeScanningDependencyScanner.GetTypeDefinition(typeof(GenericDependencyClass<>), typeof(GenericDependencyClass<string>));
        Assert.NotNull(type1);
        Assert.Equal(typeof(GenericDependencyClass<>), type1);
    }
}