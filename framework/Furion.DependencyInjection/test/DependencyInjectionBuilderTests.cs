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
            builder.AddAssemblies(assembly)
                   .AddAssemblies(assembly);
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

    [Fact]
    public void AddDependencyInjection_Default2_ReturnOK()
    {
        var services = new ServiceCollection();
        var assembly = GetType().Assembly;
        var builder = new DependencyInjectionBuilder()
                                                .AddAssemblies(assembly)
                                                .AddAssemblies(assembly);
        services.AddDependencyInjection(builder);

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
        var includePublicInnerClass = services.Any(s => s.ImplementationType == typeof(PublicInner));

        Assert.True(includePublicTestClass);
        Assert.True(enable ? includeNotPublicTestClass == false : includeNotPublicTestClass == true);
        Assert.False(includeAbstractPublicTestClass);
        Assert.True(includePublicInnerClass);

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

    [Fact]
    public void SuppressServices_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });

        var includeISuppressDerivedType = services.Any(s => s.ServiceType == typeof(ISuppressDerivedType));
        var includeISuppressDerivedType2 = services.Any(s => s.ServiceType == typeof(ITestClass) && s.ImplementationType == typeof(SuppressDerivedTypeClass));

        Assert.False(includeISuppressDerivedType);
        Assert.True(includeISuppressDerivedType2);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void Ignore_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });

        var includeIgnoreClass = services.Any(s => s.ImplementationType == typeof(IgnoreClass));

        Assert.False(includeIgnoreClass);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void Generic_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });

        var includeNormalClassWithGenericClass = services.Any(s => s.ServiceType == typeof(IGenericClass<string>) && s.ImplementationType == typeof(NormalClassWithGenericClass));
        var includeNormalClassWithBaseClass = services.Any(s => s.ServiceType == typeof(BaseGenericClass<string>) && s.ImplementationType == typeof(NormalClassWithBaseClass));
        var includeGenericClass = services.Any(s => s.ServiceType == typeof(IGenericClass<>) && s.ImplementationType == typeof(GenericClass<>));
        var includeGenericClassWithFixedGenericClass = services.Any(s => s.ServiceType == typeof(IGenericClass<string>) && s.ImplementationType == typeof(GenericClassWithFixedGenericClass<>));
        var includeGenericClassWithFixedGenericClass2 = services.Any(s => s.ServiceType == typeof(GenericClassWithFixedGenericClass<>) && s.ImplementationType == typeof(GenericClassWithFixedGenericClass<>));
        var includeGenericClassAll = services.Any(s => s.ServiceType == typeof(IGenericClass<>) && s.ImplementationType == typeof(GenericClassAll<>));
        var includeGenericClassAll2 = services.Any(s => s.ServiceType == typeof(BaseGenericClass<>) && s.ImplementationType == typeof(GenericClassAll<>));
        var includeNotTGenericClass = services.Any(s => s.ServiceType == typeof(IGenericClass<>) && s.ImplementationType == typeof(NotTGenericClass<>));
        var includeGenericClassIncludeSelf = services.Any(s => s.ServiceType == typeof(GenericClassIncludeSelf<>) && s.ImplementationType == typeof(GenericClassIncludeSelf<>));
        var includeMultipleGenericClass = services.Any(s => s.ServiceType == typeof(IMultipleGenericClass<string, object>) && s.ImplementationType == typeof(MultipleGenericClass));
        var includeMultipleGenericClass2 = services.Any(s => s.ServiceType == typeof(IMultipleGenericClass<,>) && s.ImplementationType == typeof(MultipleGenericClass<,>));
        var includeManyGenericClass = services.Any(s => s.ImplementationType == typeof(ManyGenericClass<,>));

        Assert.True(includeNormalClassWithGenericClass);
        Assert.True(includeNormalClassWithBaseClass);
        Assert.True(includeGenericClass);
        Assert.False(includeGenericClassWithFixedGenericClass);
        Assert.True(includeGenericClassWithFixedGenericClass2);
        Assert.True(includeGenericClassAll);
        Assert.True(includeGenericClassAll2);
        Assert.True(includeNotTGenericClass);
        Assert.True(includeGenericClassIncludeSelf);
        Assert.True(includeMultipleGenericClass);
        Assert.True(includeMultipleGenericClass2);
        Assert.True(includeManyGenericClass);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void Add_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });

        var count = services.Where(s => s.ServiceType == typeof(IAddClass)).LongCount();
        Assert.Equal(2, count);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void TryAdd_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });

        var count = services.Where(s => s.ServiceType == typeof(ITryAddClass)).LongCount();
        Assert.Equal(1, count);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void TryAddEnumerable_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });

        var count = services.Where(s => s.ServiceType == typeof(ITryAddEnumerableClass)).LongCount();
        Assert.Equal(2, count);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void Replace_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });

        var count = services.Where(s => s.ServiceType == typeof(IReplaceClass)).LongCount();
        Assert.Equal(1, count);

        var implementationType = services.First(u => u.ServiceType == typeof(IReplaceClass)).ImplementationType;
        Assert.Equal(typeof(ReplaceClass2), implementationType);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void FilterConfigure_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly)
                   .FilterConfigure = (s) =>
                   {
                       if (s.Descriptor.ImplementationType == typeof(FilterConfigureClass))
                       {
                           return false;
                       }

                       return true;
                   };
        });

        var includeFilterConfigureClass = services.Any(s => s.ImplementationType == typeof(FilterConfigureClass));
        Assert.False(includeFilterConfigureClass);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void Muliple_Interfaces_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly);
        });

        var includeIMultiple1 = services.Any(s => s.ServiceType == typeof(IMultiple1));
        var includeIMultiple2 = services.Any(s => s.ServiceType == typeof(IMultiple2));
        var includeIMultiple3 = services.Any(s => s.ServiceType == typeof(IMultiple3));

        Assert.True(includeIMultiple1);
        Assert.True(includeIMultiple2);
        Assert.True(includeIMultiple3);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void GlobalSuppressServices_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddDependencyInjection(builder =>
        {
            builder.AddAssemblies(GetType().Assembly)
                   .SuppressServices(typeof(IGlobalSuppressDerivedType1));
        });

        var includeGlobalSuppressDerivedType1 = services.Any(s => s.ServiceType == typeof(IGlobalSuppressDerivedType1));
        var includeGlobalSuppressDerivedType2 = services.Any(s => s.ServiceType == typeof(IGlobalSuppressDerivedType2));

        Assert.False(includeGlobalSuppressDerivedType1);
        Assert.True(includeGlobalSuppressDerivedType2);

        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }
}