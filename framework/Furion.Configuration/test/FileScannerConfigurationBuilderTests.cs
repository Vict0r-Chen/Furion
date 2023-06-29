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

namespace Furion.Configuration.Tests;

public class FileScannerConfigurationBuilderTests
{
    [Fact]
    public void ScanDirectory_Invalid_Arguments_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            FileScannerConfigurationBuilder.ScanDirectory(null!, 0);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            FileScannerConfigurationBuilder.ScanDirectory(string.Empty, 0);
        });
    }

    [Fact]
    public void ScanDirectory_Default()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "folder1");
        Assert.True(Directory.Exists(filePath));

        var files = FileScannerConfigurationBuilder.ScanDirectory(filePath);
        Assert.NotEmpty(files);
        Assert.Single(files);
        Assert.Equal("folder1.json", Path.GetFileName(files[0]));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 4)]
    [InlineData(4, 4)]
    [InlineData(1000, 4)]
    public void ScanDirectory_With_MaxDepth(uint maxDepth, int count)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "folder1");
        Assert.True(Directory.Exists(filePath));

        var files = FileScannerConfigurationBuilder.ScanDirectory(filePath, maxDepth);
        Assert.NotEmpty(files);
        Assert.Equal(count, files.Count);
    }

    [Fact]
    public void ScanDirectory_With_Matcher()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "configs");
        Assert.True(Directory.Exists(filePath));

        var fileGlobbing = new[] { "*.json" };
        var matcher = new Matcher();
        matcher.AddIncludePatterns(fileGlobbing);

        var files = FileScannerConfigurationBuilder.ScanDirectory(filePath, 0, matcher);
        Assert.NotEmpty(files);
        Assert.Equal(4, files.Count);

        var fileBlacklistGlobbing = new[]
        {
            "*.runtimeconfig.json",
            "*.runtimeconfig.*.json",
            "*.deps.json",
            "*.staticwebassets.*.json",
            "*.nuget.dgspec.json",
            "launchSettings.json",
            "tsconfig.json",
            "package.json",
            "project.assets.json",
            "manifest.json"
        };
        matcher.AddExcludePatterns(fileBlacklistGlobbing);

        var files2 = FileScannerConfigurationBuilder.ScanDirectory(filePath, 0, matcher);
        Assert.NotEmpty(files2);
    }
}