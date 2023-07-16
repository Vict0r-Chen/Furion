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

namespace Furion.Configuration.FileScanning.Tests;

public class FileScanningConfigurationScannerTests
{
    [Fact]
    public void EnsureLegalDirectory_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            FileScanningConfigurationScanner.EnsureLegalDirectory(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            FileScanningConfigurationScanner.EnsureLegalDirectory(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            FileScanningConfigurationScanner.EnsureLegalDirectory("");
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            FileScanningConfigurationScanner.EnsureLegalDirectory("workplace");
        });
        Assert.Equal("The path `workplace` is not an absolute path. (Parameter 'directory')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            FileScanningConfigurationScanner.EnsureLegalDirectory("C:/workplace/configuration.json");
        });
        Assert.Equal("The path `C:/workplace/configuration.json` is not a directory. (Parameter 'directory')", exception2.Message);
    }

    [Theory]
    [InlineData("C:/workplace")]
    [InlineData("C:\\workplace")]
    [InlineData("C:/workplace/")]
    [InlineData("C:\\workplace\\")]
    [InlineData("/workplace")]
    public void EnsureLegalDirectory_ReturnOK(string directory)
    {
        FileScanningConfigurationScanner.EnsureLegalDirectory(directory);
    }

    [Fact]
    public void ScanDirectory_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            FileScanningConfigurationScanner.ScanDirectory(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            FileScanningConfigurationScanner.ScanDirectory(string.Empty, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            FileScanningConfigurationScanner.ScanDirectory("", null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            FileScanningConfigurationScanner.ScanDirectory("C:/workplace", null!);
        });
    }

    [Fact]
    public void ScanDirectory_ReturnOK()
    {
        var directory = Path.Combine(AppContext.BaseDirectory, "directories");
        var matcher = new Matcher();
        matcher.AddIncludePatterns(new[] { "**/**.json" });

        var files1 = FileScanningConfigurationScanner.ScanDirectory(directory, matcher);
        Assert.Single(files1);
        Assert.Equal(new List<string> { Path.Combine(directory, "config.json") }, files1);

        var directory2 = Path.Combine(directory, "sub1");
        var files2 = FileScanningConfigurationScanner.ScanDirectory(directory, matcher, 1);
        Assert.Equal(3, files2.Count);
        Assert.Equal(new List<string> { Path.Combine(directory, "config.json"), Path.Combine(directory2, "config.json"), Path.Combine(directory2, "config1.json") }, files2);

        var directory3 = Path.Combine(directory2, "sub2");
        var files3 = FileScanningConfigurationScanner.ScanDirectory(directory, matcher, 2);
        Assert.Equal(5, files3.Count);
        Assert.Equal(new List<string> { Path.Combine(directory, "config.json"), Path.Combine(directory2, "config.json"), Path.Combine(directory2, "config1.json"), Path.Combine(directory3, "config1.json"), Path.Combine(directory3, "config2.json") }, files3);
    }
}