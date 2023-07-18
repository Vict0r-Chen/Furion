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
    public void New_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();

        Assert.NotNull(manifestResourceConfigurationBuilder);
        Assert.NotNull(manifestResourceConfigurationBuilder._assemblies);
        Assert.Empty(manifestResourceConfigurationBuilder._assemblies);
        Assert.NotNull(manifestResourceConfigurationBuilder._fileGlobbing);
        Assert.Single(manifestResourceConfigurationBuilder._fileGlobbing);
        Assert.Equal("*.json", manifestResourceConfigurationBuilder._fileGlobbing.First());
        Assert.NotNull(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Empty(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Null(manifestResourceConfigurationBuilder._filterConfigure);

        Assert.Null(manifestResourceConfigurationBuilder.DefaultPrefix);
    }

    [Fact]
    public void AddFilter_Invalid_Parameters()
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
        manifestResourceConfigurationBuilder.AddFilter(model => true);

        Assert.NotNull(manifestResourceConfigurationBuilder._filterConfigure);
    }

    [Fact]
    public void AddAssemblies_Invalid_Paramters()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            manifestResourceConfigurationBuilder.AddAssemblies(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            manifestResourceConfigurationBuilder.AddAssemblies(new Assembly[] { null! });
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            manifestResourceConfigurationBuilder.AddAssemblies(new Assembly[] { GetType().Assembly, null! });
        });
    }

    [Fact]
    public void AddAssemblies_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);

        Assert.NotEmpty(manifestResourceConfigurationBuilder._assemblies);
        Assert.Single(manifestResourceConfigurationBuilder._assemblies);
        Assert.Equal(GetType().Assembly, manifestResourceConfigurationBuilder._assemblies.ElementAt(0));
    }

    [Fact]
    public void AddAssemblies_Enumerable_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(new List<Assembly> { GetType().Assembly });
        manifestResourceConfigurationBuilder.AddAssemblies(new List<Assembly> { GetType().Assembly });

        Assert.NotEmpty(manifestResourceConfigurationBuilder._assemblies);
        Assert.Single(manifestResourceConfigurationBuilder._assemblies);
        Assert.Equal(GetType().Assembly, manifestResourceConfigurationBuilder._assemblies.ElementAt(0));
    }

    [Fact]
    public void AddGlobbings_Invalid_Paramters()
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

        Assert.Throws<ArgumentException>(() =>
        {
            manifestResourceConfigurationBuilder.AddGlobbings("");
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            manifestResourceConfigurationBuilder.AddGlobbings(new string[] { null! });
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            manifestResourceConfigurationBuilder.AddGlobbings(new string[] { "*.xml", null! });
        });
    }

    [Fact]
    public void AddGlobbings_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddGlobbings("*.xml");
        manifestResourceConfigurationBuilder.AddGlobbings("*.xml");

        Assert.NotEmpty(manifestResourceConfigurationBuilder._fileGlobbing);
        Assert.Equal(2, manifestResourceConfigurationBuilder._fileGlobbing.Count);
        Assert.Equal("*.xml", manifestResourceConfigurationBuilder._fileGlobbing.ElementAt(1));
    }

    [Fact]
    public void AddGlobbings_Enumerable_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddGlobbings(new List<string> { "*.xml" });
        manifestResourceConfigurationBuilder.AddGlobbings(new List<string> { "*.xml" });

        Assert.NotEmpty(manifestResourceConfigurationBuilder._fileGlobbing);
        Assert.Equal(2, manifestResourceConfigurationBuilder._fileGlobbing.Count);
        Assert.Equal("*.xml", manifestResourceConfigurationBuilder._fileGlobbing.ElementAt(1));
    }

    [Fact]
    public void AddBlacklistGlobbings_Invalid_Paramters()
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

        Assert.Throws<ArgumentException>(() =>
        {
            manifestResourceConfigurationBuilder.AddBlacklistGlobbings("");
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            manifestResourceConfigurationBuilder.AddBlacklistGlobbings(new string[] { null! });
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            manifestResourceConfigurationBuilder.AddBlacklistGlobbings(new string[] { "*.xml", null! });
        });
    }

    [Fact]
    public void AddBlacklistGlobbings_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddBlacklistGlobbings("*.xml");
        manifestResourceConfigurationBuilder.AddBlacklistGlobbings("*.xml");

        Assert.NotEmpty(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Single(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Equal("*.xml", manifestResourceConfigurationBuilder._fileBlacklistGlobbing.ElementAt(0));
    }

    [Fact]
    public void AddBlacklistGlobbings_Enumerable_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddBlacklistGlobbings(new List<string> { "*.xml" });
        manifestResourceConfigurationBuilder.AddBlacklistGlobbings(new List<string> { "*.xml" });

        Assert.NotEmpty(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Single(manifestResourceConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Equal("*.xml", manifestResourceConfigurationBuilder._fileBlacklistGlobbing.ElementAt(0));
    }

    [Fact]
    public void CreateModels()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);
        manifestResourceConfigurationBuilder.AddGlobbings("*.ini");

        var models = manifestResourceConfigurationBuilder.CreateModels().ToList();
        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Equal(2, models.Count);
    }

    [Fact]
    public void CreateModels_WithFilter()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);
        manifestResourceConfigurationBuilder.AddGlobbings("*.ini");

        manifestResourceConfigurationBuilder.AddFilter(model =>
        {
            return model.ToString().EndsWith("embed.json");
        });

        var models = manifestResourceConfigurationBuilder.CreateModels().ToList();
        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Single(models);
        Assert.Equal("Furion.Configuration.ManifestResource.Tests.embed.json", models.First().ToString());
    }

    [Fact]
    public void CreateModels_WithBlacklistGlobbing()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);
        manifestResourceConfigurationBuilder.AddBlacklistGlobbings("*.json");

        var models = manifestResourceConfigurationBuilder.CreateModels().ToList();
        Assert.NotNull(models);
        Assert.Empty(models);
    }

    [Fact]
    public void CreateModels_WithDefaultPrefix()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);
        manifestResourceConfigurationBuilder.AddGlobbings("*.ini");
        manifestResourceConfigurationBuilder.DefaultPrefix = "Embed";

        var models = manifestResourceConfigurationBuilder.CreateModels().ToList();
        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Equal(2, models.Count);
        Assert.True(models.First().Prefix == "Embed");
        Assert.True(models.Last().Prefix == "Embed");
    }

    [Fact]
    public void Build_Empty()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        var models = manifestResourceConfigurationBuilder.Build(out var manifestResourceConfigurationParser);

        Assert.NotNull(models);
        Assert.Empty(models);
        Assert.Null(manifestResourceConfigurationParser);
    }

    [Fact]
    public void Build_ReturnOK()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);
        manifestResourceConfigurationBuilder.AddGlobbings("*.ini");

        var models = manifestResourceConfigurationBuilder.Build(out var manifestResourceConfigurationParser);

        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Equal(2, models.Count);
        Assert.NotNull(manifestResourceConfigurationParser);
    }

    [Fact]
    public void Build_Order()
    {
        var manifestResourceConfigurationBuilder = new ManifestResourceConfigurationBuilder();
        manifestResourceConfigurationBuilder.AddAssemblies(GetType().Assembly);
        manifestResourceConfigurationBuilder.AddGlobbings("*.ini");

        manifestResourceConfigurationBuilder.AddFilter(model =>
        {
            if (model.ToString() == "Furion.Configuration.ManifestResource.Tests.embed.json")
            {
                model.Order = 1;
            }
            return true;
        });

        var models = manifestResourceConfigurationBuilder.Build(out var manifestResourceConfigurationParser);

        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Equal(2, models.Count);
        Assert.NotNull(manifestResourceConfigurationParser);
        Assert.Equal("Furion.Configuration.ManifestResource.Tests.embed.json", models.Last().ToString());
    }
}