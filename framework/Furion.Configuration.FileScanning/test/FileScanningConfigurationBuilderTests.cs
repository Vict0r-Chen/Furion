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

namespace Furion.Configuration.FileScanning.Tests;

#pragma warning disable

public class FileScanningConfigurationBuilderTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();

        Assert.NotNull(fileScanningConfigurationBuilder);
        Assert.NotNull(fileScanningConfigurationBuilder._directories);
        Assert.Empty(fileScanningConfigurationBuilder._directories);
        Assert.NotNull(fileScanningConfigurationBuilder._fileGlobbing);

        var fileGlobbing = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "**/**.json"
        };
        Assert.Equal(fileGlobbing, fileScanningConfigurationBuilder._fileGlobbing);

        Assert.NotNull(fileScanningConfigurationBuilder._blacklistFileGlobbing);

        var fileBlacklistGlobbing = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "**/**.runtimeconfig.json",
            "**/**.runtimeconfig.*.json",
            "**/**.deps.json",
            "**/**.staticwebassets.*.json",
            "**/**.nuget.dgspec.json",
            "**/project.assets.json",
            "**/MvcTestingAppManifest.json"
        };
        Assert.Equal(fileBlacklistGlobbing, fileScanningConfigurationBuilder._blacklistFileGlobbing);

        Assert.Equal(typeof(uint), fileScanningConfigurationBuilder.MaxScanDepth.GetType());
        Assert.Equal((uint)0, fileScanningConfigurationBuilder.MaxScanDepth);
        Assert.False(fileScanningConfigurationBuilder.DefaultOptional);
        Assert.False(fileScanningConfigurationBuilder.DefaultReloadOnChange);
        Assert.Equal(250, fileScanningConfigurationBuilder.DefaultReloadDelay);
        Assert.Null(fileScanningConfigurationBuilder.OnLoadException);
        Assert.False(fileScanningConfigurationBuilder.AllowEnvironmentSwitching);

        Assert.Null(fileScanningConfigurationBuilder._filterConfigure);
    }

    [Fact]
    public void AddFilter_Invalid_Parameters()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationBuilder.AddFilter(null!);
        });
    }

    [Fact]
    public void AddFilter_ReturnOK()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();
        fileScanningConfigurationBuilder.AddFilter(model => true);

        Assert.NotNull(fileScanningConfigurationBuilder._filterConfigure);
    }

    [Fact]
    public void AddDirectories_Invalid_Paramters()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationBuilder.AddDirectories(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationBuilder.AddDirectories(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationBuilder.AddDirectories("");
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationBuilder.AddDirectories(new string[] { null! });
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationBuilder.AddDirectories(new string[] { "**/**.xml", string.Empty });
        });
    }

    [Fact]
    public void AddDirectories_ReturnOK()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();
        fileScanningConfigurationBuilder.AddDirectories("C:/Workplace");
        fileScanningConfigurationBuilder.AddDirectories("C:/Workplace");

        Assert.NotEmpty(fileScanningConfigurationBuilder._directories);
        Assert.Single(fileScanningConfigurationBuilder._directories);
        Assert.Equal("C:\\Workplace", fileScanningConfigurationBuilder._directories.ElementAt(0));
    }

    [Fact]
    public void AddDirectories_Enumerable_ReturnOK()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();
        fileScanningConfigurationBuilder.AddDirectories(new List<string> { "C:/Workplace" });
        fileScanningConfigurationBuilder.AddDirectories(new List<string> { "C:/Workplace" });

        Assert.NotEmpty(fileScanningConfigurationBuilder._directories);
        Assert.Single(fileScanningConfigurationBuilder._directories);
        Assert.Equal("C:\\Workplace", fileScanningConfigurationBuilder._directories.ElementAt(0));
    }

    [Fact]
    public void AddGlobbings_Invalid_Paramters()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationBuilder.AddGlobbings(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationBuilder.AddGlobbings(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationBuilder.AddGlobbings("");
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationBuilder.AddGlobbings(new string[] { null! });
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationBuilder.AddGlobbings(new string[] { "**/**.xml", null! });
        });
    }

    [Fact]
    public void AddGlobbings_ReturnOK()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();
        fileScanningConfigurationBuilder.AddGlobbings("**/**.xml");
        fileScanningConfigurationBuilder.AddGlobbings("**/**.xml");

        Assert.NotEmpty(fileScanningConfigurationBuilder._fileGlobbing);
        Assert.Equal(2, fileScanningConfigurationBuilder._fileGlobbing.Count);
        Assert.Equal("**/**.xml", fileScanningConfigurationBuilder._fileGlobbing.ElementAt(1));
    }

    [Fact]
    public void AddGlobbings_Enumerable_ReturnOK()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();
        fileScanningConfigurationBuilder.AddGlobbings(new List<string> { "**/**.xml" });
        fileScanningConfigurationBuilder.AddGlobbings(new List<string> { "**/**.xml" });

        Assert.NotEmpty(fileScanningConfigurationBuilder._fileGlobbing);
        Assert.Equal(2, fileScanningConfigurationBuilder._fileGlobbing.Count);
        Assert.Equal("**/**.xml", fileScanningConfigurationBuilder._fileGlobbing.ElementAt(1));
    }

    [Fact]
    public void AddBlacklistGlobbings_Invalid_Paramters()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationBuilder.AddBlacklistGlobbings(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationBuilder.AddBlacklistGlobbings(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationBuilder.AddBlacklistGlobbings("");
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationBuilder.AddBlacklistGlobbings(new string[] { null! });
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationBuilder.AddBlacklistGlobbings(new string[] { "**/**.xml", null! });
        });
    }

    [Fact]
    public void AddBlacklistGlobbings_ReturnOK()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();
        fileScanningConfigurationBuilder.AddBlacklistGlobbings("**/**.unknown");
        fileScanningConfigurationBuilder.AddBlacklistGlobbings("**/**.unknown");

        Assert.NotEmpty(fileScanningConfigurationBuilder._blacklistFileGlobbing);
        Assert.Equal(8, fileScanningConfigurationBuilder._blacklistFileGlobbing.Count);
        Assert.Equal("**/**.unknown", fileScanningConfigurationBuilder._blacklistFileGlobbing.Last());
    }

    [Fact]
    public void AddBlacklistGlobbings_Enumerable_ReturnOK()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();
        fileScanningConfigurationBuilder.AddBlacklistGlobbings(new List<string> { "**/**.unknown" });
        fileScanningConfigurationBuilder.AddBlacklistGlobbings(new List<string> { "**/**.unknown" });

        Assert.NotEmpty(fileScanningConfigurationBuilder._blacklistFileGlobbing);
        Assert.Equal(8, fileScanningConfigurationBuilder._blacklistFileGlobbing.Count);
        Assert.Equal("**/**.unknown", fileScanningConfigurationBuilder._blacklistFileGlobbing.Last());
    }

    [Fact]
    public void Build_Invalid_Parameters()
    {
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationBuilder.Build(null!);
        });
    }

    [Fact]
    public void Build()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();

        var directory = Path.Combine(AppContext.BaseDirectory, "directories");
        fileScanningConfigurationBuilder.AddDirectories(directory);

        fileScanningConfigurationBuilder.Build(configurationBuilder);

        Assert.NotEmpty(configurationBuilder.Sources);
        Assert.Equal(2, configurationBuilder.Sources.Count);

        var firstSource = configurationBuilder.Sources.OfType<JsonConfigurationSource>().First();
        Assert.Equal("config.json", firstSource.Path);
    }
}