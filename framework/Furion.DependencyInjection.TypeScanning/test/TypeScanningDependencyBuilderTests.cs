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

public class TypeScanningDependencyBuilderTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();

        Assert.NotNull(typeScanningDependencyBuilder);
        Assert.NotNull(typeScanningDependencyBuilder._assemblies);
        Assert.Empty(typeScanningDependencyBuilder._assemblies);
        Assert.NotNull(typeScanningDependencyBuilder._blacklistServiceTypes);

        var hashSet = new HashSet<Type>()
        {
            typeof(IDisposable), typeof(IAsyncDisposable),
            typeof(IDependency), typeof(IEnumerator),
            typeof(IEnumerable), typeof(ICollection),
            typeof(IDictionary), typeof(IComparable),
            typeof(object), typeof(DynamicObject)
        };

        Assert.Equal(hashSet, typeScanningDependencyBuilder._blacklistServiceTypes);
        Assert.Null(typeScanningDependencyBuilder._filterConfigure);
        Assert.Null(typeScanningDependencyBuilder._typeFilterConfigure);
        Assert.False(typeScanningDependencyBuilder.SuppressAssemblyScanning);
        Assert.False(typeScanningDependencyBuilder.SuppressNonPublicType);
    }

    [Fact]
    public void AddFilter_Invalid_Parameters()
    {
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            typeScanningDependencyBuilder.AddFilter(null!);
        });
    }

    [Fact]
    public void AddFilter_ReturnOK()
    {
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddFilter(model => true);

        Assert.NotNull(typeScanningDependencyBuilder._filterConfigure);
    }

    [Fact]
    public void AddTypeFilter_Invalid_Parameters()
    {
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            typeScanningDependencyBuilder.AddTypeFilter(null!);
        });
    }

    [Fact]
    public void AddTypeFilter_ReturnOK()
    {
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddTypeFilter(model => true);

        Assert.NotNull(typeScanningDependencyBuilder._typeFilterConfigure);
    }

    [Fact]
    public void AddAssemblies_Invalid_Paramters()
    {
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            typeScanningDependencyBuilder.AddAssemblies(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            typeScanningDependencyBuilder.AddAssemblies(new Assembly[] { null! });
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            typeScanningDependencyBuilder.AddAssemblies(new Assembly[] { GetType().Assembly, null! });
        });
    }

    [Fact]
    public void AddAssemblies_ReturnOK()
    {
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddAssemblies(GetType().Assembly);
        typeScanningDependencyBuilder.AddAssemblies(GetType().Assembly);

        Assert.NotEmpty(typeScanningDependencyBuilder._assemblies);
        Assert.Single(typeScanningDependencyBuilder._assemblies);
        Assert.Equal(GetType().Assembly, typeScanningDependencyBuilder._assemblies.ElementAt(0));
    }

    [Fact]
    public void AddAssemblies_Enumerable_ReturnOK()
    {
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddAssemblies(new List<Assembly> { GetType().Assembly });
        typeScanningDependencyBuilder.AddAssemblies(new List<Assembly> { GetType().Assembly });

        Assert.NotEmpty(typeScanningDependencyBuilder._assemblies);
        Assert.Single(typeScanningDependencyBuilder._assemblies);
        Assert.Equal(GetType().Assembly, typeScanningDependencyBuilder._assemblies.ElementAt(0));
    }

    [Fact]
    public void AddBlacklistTypes_Invalid_Paramters()
    {
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            typeScanningDependencyBuilder.AddBlacklistTypes(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            typeScanningDependencyBuilder.AddBlacklistTypes(new Type[] { null! });
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            typeScanningDependencyBuilder.AddBlacklistTypes(new Type[] { typeof(ExportService1), null! });
        });
    }

    [Fact]
    public void AddBlacklistTypes_ReturnOK()
    {
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddBlacklistTypes(typeof(ExportService1));
        typeScanningDependencyBuilder.AddBlacklistTypes(typeof(ExportService1));

        Assert.NotEmpty(typeScanningDependencyBuilder._blacklistServiceTypes);
        Assert.Equal(11, typeScanningDependencyBuilder._blacklistServiceTypes.Count);
        Assert.Equal(typeof(ExportService1), typeScanningDependencyBuilder._blacklistServiceTypes.Last());
    }

    [Fact]
    public void AddBlacklistTypes_Enumerable_ReturnOK()
    {
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddBlacklistTypes(new List<Type> { typeof(ExportService1) });
        typeScanningDependencyBuilder.AddBlacklistTypes(new List<Type> { typeof(ExportService1) });

        Assert.NotEmpty(typeScanningDependencyBuilder._blacklistServiceTypes);
        Assert.Equal(11, typeScanningDependencyBuilder._blacklistServiceTypes.Count);
        Assert.Equal(typeof(ExportService1), typeScanningDependencyBuilder._blacklistServiceTypes.Last());
    }

    [Fact]
    public void Build()
    {
        var services = new ServiceCollection();
        var typeScanningDependencyBuilder = new TypeScanningDependencyBuilder();
        typeScanningDependencyBuilder.AddAssemblies(GetType().Assembly);

        typeScanningDependencyBuilder.Build(services);

        Assert.NotEmpty(services);

        _ = services.BuildServiceProvider();
    }
}