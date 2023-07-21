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