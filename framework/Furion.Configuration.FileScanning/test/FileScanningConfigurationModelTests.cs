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

public class FileScanningConfigurationModelTests
{
    [Fact]
    public void NewInstace_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var fileConfigurationModel = new FileScanningConfigurationModel(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var fileConfigurationModel = new FileScanningConfigurationModel(string.Empty);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            var fileConfigurationModel = new FileScanningConfigurationModel("folder/appsettings.json");
        });
        Assert.Equal("The path `folder/appsettings.json` is not an absolute path. (Parameter 'filePath')", exception.Message);

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "appsettings.json");
        Assert.True(File.Exists(filePath));

        var fileConfigurationModel = new FileScanningConfigurationModel(filePath);
        Assert.NotNull(fileConfigurationModel);
    }

    [Fact]
    public void NewInstace_Default()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "appsettings.json");
        Assert.True(File.Exists(filePath));

        var fileConfigurationModel = new FileScanningConfigurationModel(filePath);
        Assert.NotNull(fileConfigurationModel);

        Assert.Equal(filePath, fileConfigurationModel.FilePath);
        Assert.Equal(Path.GetExtension(filePath), fileConfigurationModel.Extension);
        Assert.Equal(Path.GetFileName(filePath), fileConfigurationModel.FileName);
        Assert.Equal(Path.GetDirectoryName(filePath), fileConfigurationModel.DirectoryName);
        Assert.True(fileConfigurationModel.Optional);
        Assert.True(fileConfigurationModel.ReloadOnChange);
        Assert.Equal(250, fileConfigurationModel.ReloadDelay);
        Assert.Equal(0, fileConfigurationModel.Order);
        Assert.Equal("appsettings", fileConfigurationModel.Group);
    }

    [Theory]
    [InlineData("appsettings.json", "appsettings")]
    [InlineData("appsettings.Production.json", "appsettings")]
    [InlineData("appsettings.Development.json", "appsettings")]
    [InlineData("appsettings.Application.Development.json", "appsettings")]
    [InlineData("(appsettings).Application.Development.json", "(appsettings)")]
    [InlineData("(appsettings.Application).json", "(appsettings.Application)")]
    [InlineData("(appsettings.Application).Development.json", "(appsettings.Application)")]
    [InlineData("appsettings.(Application).Development.json", "appsettings")]
    public void ResolveGroup(string fileName, string group)
    {
        Assert.Equal(group, FileScanningConfigurationModel.ResolveGroup(fileName));
    }

    [Fact]
    public void ToString_Output()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "appsettings.json");
        Assert.True(File.Exists(filePath));

        var fileConfigurationModel = new FileScanningConfigurationModel(filePath);
        Assert.Equal($"[{fileConfigurationModel.Group}] FileName: {fileConfigurationModel.FileName} Path: {fileConfigurationModel.FilePath}", fileConfigurationModel.ToString());
    }
}