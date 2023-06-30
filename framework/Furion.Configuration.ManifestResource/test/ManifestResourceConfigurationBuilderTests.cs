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

using System.Reflection;

namespace Furion.Configuration.ManifestResource.Tests;

public class ManifestResourceConfigurationBuilderTests
{
    [Fact]
    public void NewInstance_Default()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        Assert.NotNull(manifestResourceConfigurationBuilder);

        Assert.NotNull(manifestResourceConfigurationBuilder._assemblies);
        Assert.Empty(manifestResourceConfigurationBuilder._assemblies);
        Assert.NotNull(manifestResourceConfigurationBuilder._fileGlobbing);
        Assert.Single(manifestResourceConfigurationBuilder._fileGlobbing);
        Assert.Equal("*.json", manifestResourceConfigurationBuilder._fileGlobbing.ElementAt(0));

        Assert.NotNull(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Empty(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Null(manifestResourceConfigurationBuilder._filterConfigure);
    }

    [Fact]
    public void AddFilter_Null_Throw()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            manifestResourceConfigurationBuilder.AddFilter(null!);
        });
    }

    [Fact]
    public void AddFilter_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddFilter(s =>
        {
            return true;
        });
        Assert.NotNull(manifestResourceConfigurationBuilder._filterConfigure);
    }

    [Fact]
    public void AddAssemblies_Null_Throw()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            manifestResourceConfigurationBuilder.AddAssemblies(null!);
        });
    }

    [Fact]
    public void AddAssemblies_NotExists_AddedSuccessfully()
    {
        var currentAssembly = GetType().Assembly;
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(currentAssembly);

        Assert.Single(manifestResourceConfigurationBuilder._assemblies);
        Assert.Equal(currentAssembly, manifestResourceConfigurationBuilder._assemblies.Last());
    }

    [Fact]
    public void AddAssemblies_Exists_Skip()
    {
        var currentAssembly = GetType().Assembly;
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(currentAssembly);

        Assert.Single(manifestResourceConfigurationBuilder._assemblies);

        manifestResourceConfigurationBuilder.AddAssemblies(currentAssembly);
        manifestResourceConfigurationBuilder.AddAssemblies(currentAssembly);
        Assert.Single(manifestResourceConfigurationBuilder._assemblies);
        Assert.Equal(currentAssembly, manifestResourceConfigurationBuilder._assemblies.Last());
    }

    [Fact]
    public void AddAssemblies_Multiple_Arguments_ReturnOK()
    {
        var currentAssembly = GetType().Assembly;
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(Assembly.GetEntryAssembly()!, currentAssembly);
        Assert.Equal(2, manifestResourceConfigurationBuilder._assemblies.Count);
        Assert.Equal(currentAssembly, manifestResourceConfigurationBuilder._assemblies.Last());
    }

    [Fact]
    public void AddAssemblies_IEnumerable_ReturnOK()
    {
        var currentAssembly = GetType().Assembly;
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(new List<Assembly> { Assembly.GetEntryAssembly()!, currentAssembly });
        Assert.Equal(2, manifestResourceConfigurationBuilder._assemblies.Count);
        Assert.Equal(currentAssembly, manifestResourceConfigurationBuilder._assemblies.Last());
    }

    [Fact]
    public void AddGlobbings_Throw()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            manifestResourceConfigurationBuilder.AddGlobbings(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            manifestResourceConfigurationBuilder.AddGlobbings(string.Empty);
        });
    }

    [Fact]
    public void AddGlobbings_NotExists_AddedSuccessfully()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddGlobbings("*.xml");

        Assert.Equal(2, manifestResourceConfigurationBuilder._fileGlobbing.Count);
        Assert.Equal("*.xml", manifestResourceConfigurationBuilder._fileGlobbing.Last());
    }

    [Fact]
    public void AddGlobbings_Exists_Skip()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddGlobbings("*.json");

        Assert.Single(manifestResourceConfigurationBuilder._fileGlobbing);
        manifestResourceConfigurationBuilder.AddGlobbings("*.json");
        manifestResourceConfigurationBuilder.AddGlobbings("*.json");
        Assert.Single(manifestResourceConfigurationBuilder._fileGlobbing);

        Assert.Equal("*.json", manifestResourceConfigurationBuilder._fileGlobbing.Last());
    }

    [Fact]
    public void AddGlobbings_Multiple_Arguments_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddGlobbings("*.xml", "*.ini");

        Assert.Equal(3, manifestResourceConfigurationBuilder._fileGlobbing.Count);
        Assert.Equal("*.ini", manifestResourceConfigurationBuilder._fileGlobbing.Last());
    }

    [Fact]
    public void AddGlobbings_IEnumerable_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddGlobbings(new List<string> { "*.xml", "*.ini" });

        Assert.Equal(3, manifestResourceConfigurationBuilder._fileGlobbing.Count);
        Assert.Equal("*.ini", manifestResourceConfigurationBuilder._fileGlobbing.Last());
    }

    [Fact]
    public void AddBlacklistGlobbings_Throw()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            manifestResourceConfigurationBuilder.AddBlacklistGlobbings(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            manifestResourceConfigurationBuilder.AddBlacklistGlobbings(string.Empty);
        });
    }

    [Fact]
    public void AddBlacklistGlobbings_NotExists_AddedSuccessfully()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddBlacklistGlobbings("embed.json");

        Assert.Single(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Equal("embed.json", manifestResourceConfigurationBuilder._fileBlacklistGlobbing.Last());
    }

    [Fact]
    public void AddBlacklistGlobbings_Exists_Skip()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddBlacklistGlobbings("*.runtimeconfig.json");

        Assert.Single(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        manifestResourceConfigurationBuilder.AddBlacklistGlobbings("*.runtimeconfig.json");
        manifestResourceConfigurationBuilder.AddBlacklistGlobbings("*.runtimeconfig.json");
        Assert.Single(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);

        Assert.Equal("*.runtimeconfig.json", manifestResourceConfigurationBuilder._fileBlacklistGlobbing.Last());
    }

    [Fact]
    public void AddBlacklistGlobbings_Multiple_Arguments_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddBlacklistGlobbings("embed.xml", "embed.ini");

        Assert.Equal(2, manifestResourceConfigurationBuilder._fileBlacklistGlobbing.Count);
        Assert.Equal("embed.ini", manifestResourceConfigurationBuilder._fileBlacklistGlobbing.Last());
    }

    [Fact]
    public void AddBlacklistGlobbings_IEnumerable_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddBlacklistGlobbings(new List<string> { "embed.xml", "embed.ini" });

        Assert.Equal(2, manifestResourceConfigurationBuilder._fileBlacklistGlobbing.Count);
        Assert.Equal("embed.ini", manifestResourceConfigurationBuilder._fileBlacklistGlobbing.Last());
    }

    [Fact]
    public void Release_ClearAll()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.Release();

        Assert.Empty(manifestResourceConfigurationBuilder._assemblies);
        Assert.Empty(manifestResourceConfigurationBuilder._fileGlobbing);
        Assert.Empty(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Null(manifestResourceConfigurationBuilder._filterConfigure);
    }

    [Fact]
    public void ScanAssemblies_Default()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);

        var manifestResources = manifestResourceConfigurationBuilder.ScanAssemblies();
        Assert.NotNull(manifestResources);
        Assert.Single(manifestResources);
    }

    [Fact]
    public void ScanAssemblies_Default_Matcher()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);

        var manifestResources = manifestResourceConfigurationBuilder.ScanAssemblies();
        Assert.NotNull(manifestResources);
        Assert.Single(manifestResources);

        manifestResourceConfigurationBuilder.AddBlacklistGlobbings("*.json");
        var manifestResources2 = manifestResourceConfigurationBuilder.ScanAssemblies();
        Assert.NotNull(manifestResources2);
        Assert.Empty(manifestResources2);
    }

    [Fact]
    public void Build_With_AddFilter()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddFilter(s =>
        {
            return false;
        });
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);
        var manifestResources = manifestResourceConfigurationBuilder.Build();
        Assert.NotNull(manifestResources);
        Assert.Empty(manifestResources);
    }

    [Fact]
    public void Build_Release()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddFilter(s =>
        {
            return false;
        });
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);
        var manifestResources = manifestResourceConfigurationBuilder.Build();
        Assert.NotNull(manifestResources);
        Assert.Empty(manifestResources);

        Assert.Empty(manifestResourceConfigurationBuilder._assemblies);
        Assert.Empty(manifestResourceConfigurationBuilder._fileGlobbing);
        Assert.Empty(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Null(manifestResourceConfigurationBuilder._filterConfigure);
    }
}