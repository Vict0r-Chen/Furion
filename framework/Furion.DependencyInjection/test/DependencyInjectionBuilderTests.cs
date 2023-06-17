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

public class DependencyInjectionBuilderTests
{
    [Fact]
    public void NewInstance_With_Parameterless()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();

        Assert.Equal(typeof(IDependency), dependencyInjectionBuilder._dependencyType);
        Assert.Single(dependencyInjectionBuilder._assemblies);
        Assert.Equal(Assembly.GetEntryAssembly(), dependencyInjectionBuilder._assemblies.First());

        var backlist = new[]
        {
            typeof(IDisposable), typeof(IAsyncDisposable),
            typeof(IDependency), typeof(IEnumerator),
            typeof(IEnumerable), typeof(ICollection),
            typeof(IDictionary), typeof(IComparable),
            typeof(object), typeof(DynamicObject)
        };
        Assert.True(backlist.SequenceEqual(dependencyInjectionBuilder._serviceTypeBlacklist.ToArray()));

        Assert.Null(dependencyInjectionBuilder._filterConfigure);
        Assert.False(dependencyInjectionBuilder.SuppressAssemblyScanning);
        Assert.False(dependencyInjectionBuilder.SuppressNonPublicType);
        Assert.True(dependencyInjectionBuilder.ValidateLifetime);
    }

    [Fact]
    public void AddAssemblies_Null_Throw()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            dependencyInjectionBuilder.AddAssemblies(null!);
        });
    }

    [Fact]
    public void AddAssemblies_NotExists_AddedSuccessfully()
    {
        var currentAssembly = GetType().Assembly;
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        dependencyInjectionBuilder.AddAssemblies(currentAssembly);

        Assert.Equal(2, dependencyInjectionBuilder._assemblies.Count);
        Assert.Equal(currentAssembly, dependencyInjectionBuilder._assemblies.Last());
    }

    [Fact]
    public void AddAssemblies_Exists_Skip()
    {
        var currentAssembly = GetType().Assembly;
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        dependencyInjectionBuilder.AddAssemblies(Assembly.GetEntryAssembly()!);

        Assert.Single(dependencyInjectionBuilder._assemblies);

        dependencyInjectionBuilder.AddAssemblies(currentAssembly);
        dependencyInjectionBuilder.AddAssemblies(currentAssembly);
        Assert.Equal(2, dependencyInjectionBuilder._assemblies.Count);
        Assert.Equal(currentAssembly, dependencyInjectionBuilder._assemblies.Last());
    }

    [Fact]
    public void AddAssemblies_Multiple_Arguments_ReturnOK()
    {
        var currentAssembly = GetType().Assembly;
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        dependencyInjectionBuilder.AddAssemblies(Assembly.GetEntryAssembly()!, currentAssembly);
        Assert.Equal(2, dependencyInjectionBuilder._assemblies.Count);
        Assert.Equal(currentAssembly, dependencyInjectionBuilder._assemblies.Last());
    }

    [Fact]
    public void AddFilter_Null_Throw()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            dependencyInjectionBuilder.AddFilter(null!);
        });
    }

    [Fact]
    public void AddFilter_ReturnOK()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        dependencyInjectionBuilder.AddFilter(s =>
        {
            return true;
        });
        Assert.NotNull(dependencyInjectionBuilder._filterConfigure);
    }

    [Theory]
    [InlineData(typeof(ITransientDependency), ServiceLifetime.Transient)]
    [InlineData(typeof(IScopedDependency), ServiceLifetime.Scoped)]
    [InlineData(typeof(ISingletonDependency), ServiceLifetime.Singleton)]
    public void GetServiceLifetime_Match_ReturnServiceLifetime(Type dependencyType, ServiceLifetime lifetime)
    {
        var serviceLifetime = DependencyInjectionBuilder.GetServiceLifetime(dependencyType);
        Assert.Equal(lifetime, serviceLifetime);
    }

    [Theory]
    [InlineData(typeof(IDependency))]
    [InlineData(typeof(IDictionary))]
    public void GetServiceLifetime_NotMatch_Throw(Type dependencyType)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var serviceLifetime = DependencyInjectionBuilder.GetServiceLifetime(dependencyType);
        });

        Assert.Equal($"`{dependencyType}` type is not a valid service lifetime type. (Parameter 'dependencyType')", exception.Message);
    }

    [Fact]
    public void GetServiceLifetime_Null_Throw()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var serviceLifetime = DependencyInjectionBuilder.GetServiceLifetime(null);
        });

        Assert.Equal($"`{typeof(IDependency)}` type is not a valid service lifetime type. (Parameter 'dependencyType')", exception.Message);
    }

    [Theory]
    [InlineData(typeof(IDependency))]
    [InlineData(typeof(IDictionary))]
    public void GetServiceLifetime_NotMatch_NotValidate_ReturnOK(Type dependencyType)
    {
        var serviceLifetime = DependencyInjectionBuilder.GetServiceLifetime(dependencyType, false);
        Assert.Null(serviceLifetime);
    }

    [Fact]
    public void GetServiceLifetime_Null_NotValidate_ReturnOK()
    {
        var serviceLifetime = DependencyInjectionBuilder.GetServiceLifetime(null, false);
        Assert.Null(serviceLifetime);
    }

    [Fact]
    public void Release_ClearAll()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        dependencyInjectionBuilder.Release();

        Assert.Empty(dependencyInjectionBuilder._assemblies);
        Assert.Empty(dependencyInjectionBuilder._serviceTypeBlacklist);
        Assert.Null(dependencyInjectionBuilder._filterConfigure);
    }

    [Theory]
    [InlineData(typeof(Service), typeof(ITransientDependency), typeof(IService), typeof(IService<string>), typeof(ISecondService<string, int>))]
    [InlineData(typeof(Service1), typeof(IScopedDependency), typeof(IService), typeof(ISecondService), typeof(IService<string>), typeof(ISecondService<string, int>))]
    [InlineData(typeof(Service2), typeof(ITransientDependency), typeof(IService))]
    [InlineData(typeof(Service3), typeof(IScopedDependency), typeof(IService))]
    [InlineData(typeof(Service4), typeof(IScopedDependency))]
    [InlineData(typeof(Service5), typeof(IScopedDependency), typeof(IService))]
    [InlineData(typeof(Service6), typeof(ITransientDependency), typeof(IService))]
    [InlineData(typeof(NonLifetimeClass), null, typeof(IService))]
    public void GetEffectiveServiceTypes_ReturnServices_And_DependencyType(Type type, Type dependencyType, params Type[] serviceTypes)
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        var types = dependencyInjectionBuilder.GetEffectiveServiceTypes(type, null, out var depType);

        Assert.Equal(dependencyType, depType);
        Assert.True(serviceTypes.SequenceEqual(types));
    }

    [Theory]
    [InlineData(typeof(GenericService<>), typeof(IScopedDependency), typeof(ISecondService<>))]
    [InlineData(typeof(GenericService1<>), typeof(IScopedDependency), typeof(IService<>), typeof(ISecondService<>))]
    [InlineData(typeof(GenericService2<,>), typeof(ISingletonDependency), typeof(IService<,>), typeof(ISecondService<,>))]
    [InlineData(typeof(GenericService3<,>), typeof(IScopedDependency), typeof(ISecondService<,>))]
    public void GetEffectiveServiceTypes_ForGenericType_ReturnServices_And_DependencyType(Type genericType, Type dependencyType, params Type[] serviceTypes)
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        var type = GetType().Assembly.GetTypes().Single(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericType);
        var types = dependencyInjectionBuilder.GetEffectiveServiceTypes(type, null, out var depType);

        Assert.Equal(dependencyType, depType);
        Assert.True(serviceTypes.SequenceEqual(types));
    }

    [Fact]
    public void CreateServiceDescriptors_NonServiceInjectionAttribute()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        var descriptors = dependencyInjectionBuilder.CreateServiceDescriptors(typeof(Service8));
        var serviceTypes = descriptors.Select(s => s.Descriptor.ServiceType);

        Assert.Single(serviceTypes);
        Assert.Contains(typeof(IService), serviceTypes);
    }

    [Fact]
    public void CreateServiceDescriptors_WhenSetIgnore_ReturnEmpty()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        var descriptors = dependencyInjectionBuilder.CreateServiceDescriptors(typeof(IgnoreService));
        Assert.Empty(descriptors);
    }

    [Fact]
    public void CreateServiceDescriptors_EmptyServices_ReturnSelf()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        var descriptors = dependencyInjectionBuilder.CreateServiceDescriptors(typeof(Service4));

        Assert.Single(descriptors);
        Assert.Equal(typeof(Service4), descriptors.First().Descriptor.ServiceType);
    }

    [Fact]
    public void CreateServiceDescriptors_IncludeSelf()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        var descriptors = dependencyInjectionBuilder.CreateServiceDescriptors(typeof(Service5));
        var serviceTypes = descriptors.Select(s => s.Descriptor.ServiceType);

        Assert.Equal(2, descriptors.Count());
        Assert.Contains(typeof(Service5), serviceTypes);
    }

    [Fact]
    public void CreateServiceDescriptors_IncludeBase()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        var descriptors = dependencyInjectionBuilder.CreateServiceDescriptors(typeof(Service6));
        var serviceTypes = descriptors.Select(s => s.Descriptor.ServiceType);

        Assert.Equal(2, descriptors.Count());
        Assert.Contains(typeof(ServiceBase), serviceTypes);
    }

    [Fact]
    public void CreateServiceDescriptors_NonLifetimeDependency_Throw()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var descriptors = dependencyInjectionBuilder.CreateServiceDescriptors(typeof(NonLifetimeClass)).ToList();
        });
    }

    [Fact]
    public void CreateServiceDescriptors_IncludeSelf_And_IncludeBase()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        var descriptors = dependencyInjectionBuilder.CreateServiceDescriptors(typeof(Service7));
        var serviceTypes = descriptors.Select(s => s.Descriptor.ServiceType);

        Assert.Equal(3, descriptors.Count());
        Assert.Contains(typeof(Service7), serviceTypes);
        Assert.Contains(typeof(ServiceBase), serviceTypes);
    }

    [Fact]
    public void CreateServiceDescriptors_AddFilter()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        dependencyInjectionBuilder.AddFilter(s =>
        {
            if (s.Descriptor.ServiceType == typeof(IService))
            {
                return false;
            }

            s.Order = 10;

            return true;
        });

        var descriptors = dependencyInjectionBuilder.CreateServiceDescriptors(typeof(Service5));
        Assert.Single(descriptors);
        Assert.Equal(typeof(Service5), descriptors.First().Descriptor.ServiceType);

        var descriptors2 = dependencyInjectionBuilder.CreateServiceDescriptors(typeof(Service4));
        Assert.Equal(10, descriptors2.First().Order);
    }

    [Fact]
    public void ScanAssemblies_Default_ReturnEmpty()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder();
        var descriptors = dependencyInjectionBuilder.ScanAssemblies();
        Assert.Empty(descriptors);
    }

    [Fact]
    public void ScanAssemblies_IfAdd_NotEmpty()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder()
        {
            ValidateLifetime = false
        };
        dependencyInjectionBuilder.AddAssemblies(GetType().Assembly);

        var descriptors = dependencyInjectionBuilder.ScanAssemblies();
        Assert.True(descriptors.Any());
    }

    [Fact]
    public void ScanAssemblies_Order()
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder()
        {
            ValidateLifetime = false
        };
        dependencyInjectionBuilder.AddAssemblies(GetType().Assembly);
        var descriptors = dependencyInjectionBuilder.ScanAssemblies().ToList();

        var order1 = descriptors.FindIndex(u => u.Descriptor.ImplementationType == typeof(Order1Service));
        var order2 = descriptors.FindIndex(u => u.Descriptor.ImplementationType == typeof(Order2Service));
        var order3 = descriptors.FindIndex(u => u.Descriptor.ImplementationType == typeof(Order3Service));
        Assert.True(order1 > order3 && order3 > order2);
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void ScanAssemblies_SuppressNonPublicType(bool suppressNonPublicType, bool include)
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder()
        {
            ValidateLifetime = false,
            SuppressNonPublicType = suppressNonPublicType
        };
        dependencyInjectionBuilder.AddAssemblies(GetType().Assembly);
        var descriptors = dependencyInjectionBuilder.ScanAssemblies();

        var includeNotPublicService = descriptors.Any(s => s.Descriptor.ServiceType == typeof(NotPublicService));
        Assert.Equal(include, includeNotPublicService);
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void ScanAssemblies_SuppressAssemblyScanning_ReturnEmpty(bool suppressAssemblyScanning, bool hasData)
    {
        var dependencyInjectionBuilder = new DependencyInjectionBuilder()
        {
            ValidateLifetime = false,
            SuppressAssemblyScanning = suppressAssemblyScanning
        };
        dependencyInjectionBuilder.AddAssemblies(GetType().Assembly);
        var descriptors = dependencyInjectionBuilder.ScanAssemblies();

        Assert.Equal(hasData, descriptors.Any());
    }

    [Fact]
    public void AddingToServices()
    {
        var services = new ServiceCollection();

        var serviceDescriptorModel = new ServiceDescriptorModel(typeof(IService), typeof(Service), ServiceLifetime.Scoped, ServiceAddition.Add);
        DependencyInjectionBuilder.AddingToServices(services, serviceDescriptorModel);
        Assert.Single(services);

        var serviceDescriptorModel2 = new ServiceDescriptorModel(typeof(IService), typeof(Service2), ServiceLifetime.Scoped, ServiceAddition.TryAdd);
        DependencyInjectionBuilder.AddingToServices(services, serviceDescriptorModel2);
        Assert.Single(services);

        var serviceDescriptorModel3 = new ServiceDescriptorModel(typeof(IService), typeof(Service2), ServiceLifetime.Scoped, ServiceAddition.TryAddEnumerable);
        DependencyInjectionBuilder.AddingToServices(services, serviceDescriptorModel3);
        Assert.Equal(2, services.Count);

        var serviceDescriptorModel4 = new ServiceDescriptorModel(typeof(IService), typeof(Service3), ServiceLifetime.Scoped, ServiceAddition.Replace);
        DependencyInjectionBuilder.AddingToServices(services, serviceDescriptorModel4);
        Assert.Equal(2, services.Count);
    }

    [Fact]
    public void Build()
    {
        var services = new ServiceCollection();

        var dependencyInjectionBuilder = new DependencyInjectionBuilder()
        {
            ValidateLifetime = false
        };

        dependencyInjectionBuilder.AddAssemblies(GetType().Assembly);
        dependencyInjectionBuilder.Build(services);
        Assert.NotEmpty(services);
    }
}