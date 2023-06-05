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
    public void AddDependencyInjection_Default_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            var assembly = GetType().Assembly;
            builder.AddAssemblies(assembly);
            builder.AddAssemblies(assembly);
        });

        var count = services.Count;
        Assert.True(count > 0);

        var includePublicTestClass = services.Any(s => s.ImplementationType == typeof(PublicTestClass));
        var includeNotPublicTestClass = services.Any(s => s.ImplementationType == typeof(NotPublicTestClass));
        var includeAbstractPublicTestClass = services.Any(s => s.ImplementationType == typeof(AbstractPublicTestClass));

        Assert.True(includePublicTestClass);
        Assert.True(includeNotPublicTestClass);
        Assert.False(includeAbstractPublicTestClass);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SuppressNotPublicType_ReturnOK(bool enable)
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
            builder.SuppressNotPublicType = enable;
        });

        var count = services.Count;
        Assert.True(count > 0);

        var includePublicTestClass = services.Any(s => s.ImplementationType == typeof(PublicTestClass));
        var includeNotPublicTestClass = services.Any(s => s.ImplementationType == typeof(NotPublicTestClass));
        var includeAbstractPublicTestClass = services.Any(s => s.ImplementationType == typeof(AbstractPublicTestClass));

        Assert.True(includePublicTestClass);
        Assert.True(enable ? includeNotPublicTestClass == false : includeNotPublicTestClass == true);
        Assert.False(includeAbstractPublicTestClass);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SuppressAssemblyScanning_ReturnOK(bool enable)
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
            builder.SuppressAssemblyScanning = enable;
        });

        var count = services.Count;
        Assert.True(enable ? count == 0 : count > 0);
    }

    [Fact]
    public void IncludingBase_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });

        var includePublicBaseClass = services.Any(s => s.ServiceType == typeof(PublicBaseClass));
        var includeNotPublicBaseClass = services.Any(s => s.ServiceType == typeof(NotPublicBaseClass));
        var includeAbstractBaseClass = services.Any(s => s.ServiceType == typeof(AbstractBaseClass));

        Assert.True(includePublicBaseClass);
        Assert.False(includeNotPublicBaseClass);
        Assert.True(includeAbstractBaseClass);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void IncludingSelf_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });

        var includeIncludeSelfClass = services.Any(s => s.ServiceType == typeof(IncludeSelfClass));
        var includeIncludeSelfClassWithNotInterface = services.Any(s => s.ServiceType == typeof(IncludeSelfClassWithNotInterface));
        var includeIncludeSelfClassWithBaseClass = services.Any(s => s.ServiceType == typeof(IncludeSelfClassWithBaseClass));

        Assert.True(includeIncludeSelfClass);
        Assert.True(includeIncludeSelfClassWithNotInterface);
        Assert.False(includeIncludeSelfClassWithBaseClass);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void Order_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });

        var indexedServices = services.Select((s, i) => new { s.ImplementationType, i });
        var orderClass1 = indexedServices.First(u => u.ImplementationType == typeof(OrderClass1)).i;
        var orderClass2 = indexedServices.First(u => u.ImplementationType == typeof(OrderClass2)).i;

        Assert.True(orderClass2 > orderClass1);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }
}