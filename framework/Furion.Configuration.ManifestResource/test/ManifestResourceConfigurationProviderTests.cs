﻿// 麻省理工学院许可证
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

namespace Furion.Configuration.ManifestResource.Tests;

public class ManifestResourceConfigurationProviderTests
{
    [Fact]
    public void NewInstance_Null_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var manifestResourceConfigurationProvider = new ManifestResourceConfigurationProvider(null!);
        });
    }

    [Fact]
    public void NewInstance_Default()
    {
        var manifestResourceConfigurationProvider = new ManifestResourceConfigurationProvider(new());
        Assert.NotNull(manifestResourceConfigurationProvider);
        Assert.NotNull(manifestResourceConfigurationProvider.ManifestResources);
        Assert.Empty(manifestResourceConfigurationProvider.ManifestResources);
    }

    [Fact]
    public void ParseResource_NullOrEmpty_Prefix_Throw()
    {
        var fileConfigurationParser = new FileConfigurationParser();
        var assembly = GetType().Assembly;
        var resourceName = "Furion.Configuration.ManifestResource.Tests.Resources.embed.json";

        var manifestResourceConfigurationModel = new ManifestResourceConfigurationModel(assembly, resourceName);
        Assert.NotNull(manifestResourceConfigurationModel);

        manifestResourceConfigurationModel.Prefix = string.Empty;
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var data = ManifestResourceConfigurationProvider.ParseResource(fileConfigurationParser, manifestResourceConfigurationModel);
        });

        Assert.Equal($"The configuration prefix of assembly `{assembly.GetName().Name}` cannot be null or an empty string.", exception.Message);
    }

    [Fact]
    public void ParseResource()
    {
        var fileConfigurationParser = new FileConfigurationParser();
        var assembly = GetType().Assembly;
        var resourceName = "Furion.Configuration.ManifestResource.Tests.Resources.embed.json";

        var manifestResourceConfigurationModel = new ManifestResourceConfigurationModel(assembly, resourceName);
        Assert.NotNull(manifestResourceConfigurationModel);

        var data = ManifestResourceConfigurationProvider.ParseResource(fileConfigurationParser, manifestResourceConfigurationModel);
        Assert.NotNull(data);
        Assert.Single(data);
        Assert.Equal("Furion.Configuration.ManifestResource.Tests:Name", data.Keys.ElementAt(0));
        Assert.Equal("Furion - Embed", data["Furion.Configuration.ManifestResource.Tests:Name"]);
    }

    [Fact]
    public void ParseResource_Prefix()
    {
        var fileConfigurationParser = new FileConfigurationParser();
        var assembly = GetType().Assembly;
        var resourceName = "Furion.Configuration.ManifestResource.Tests.Resources.embed.json";

        var manifestResourceConfigurationModel = new ManifestResourceConfigurationModel(assembly, resourceName);
        Assert.NotNull(manifestResourceConfigurationModel);

        manifestResourceConfigurationModel.Prefix = "Custom";
        var data = ManifestResourceConfigurationProvider.ParseResource(fileConfigurationParser, manifestResourceConfigurationModel);
        Assert.NotNull(data);
        Assert.Single(data);
        Assert.Equal("Custom:Name", data.Keys.ElementAt(0));
        Assert.Equal("Furion - Embed", data["Custom:Name"]);
    }

    [Fact]
    public void Load()
    {
        var assembly = GetType().Assembly;
        var resourceName = "Furion.Configuration.ManifestResource.Tests.Resources.embed.json";

        var manifestResourceConfigurationModel = new ManifestResourceConfigurationModel(assembly, resourceName);
        Assert.NotNull(manifestResourceConfigurationModel);

        var manifestResourceConfigurationProvider = new ManifestResourceConfigurationProvider(new() { manifestResourceConfigurationModel });
        Assert.NotNull(manifestResourceConfigurationProvider);

        manifestResourceConfigurationProvider.Load();

        var result = manifestResourceConfigurationProvider.TryGet("Furion.Configuration.ManifestResource.Tests:Name", out var value);
        Assert.True(result);

        Assert.Equal("Furion - Embed", value);
    }
}