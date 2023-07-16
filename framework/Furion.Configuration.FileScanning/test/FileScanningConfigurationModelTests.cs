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

public class FileScanningConfigurationModelTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var fileScanningConfigurationModel = new FileScanningConfigurationModel(null!, true);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var fileScanningConfigurationModel = new FileScanningConfigurationModel(string.Empty, true);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var fileScanningConfigurationModel = new FileScanningConfigurationModel("", true);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var fileScanningConfigurationModel = new FileScanningConfigurationModel("C:/Workplace/configuration.json", true);

        Assert.NotNull(fileScanningConfigurationModel);
        Assert.Equal("C:\\Workplace\\configuration.json", fileScanningConfigurationModel.FilePath);
        Assert.Equal(".json", fileScanningConfigurationModel.Extension);
        Assert.Equal("configuration.json", fileScanningConfigurationModel.FileName);
        Assert.Equal("C:\\Workplace", fileScanningConfigurationModel.DirectoryName);
        Assert.Equal("configuration", fileScanningConfigurationModel.Group);
        Assert.False(fileScanningConfigurationModel.Optional);
        Assert.False(fileScanningConfigurationModel.ReloadOnChange);
        Assert.False(fileScanningConfigurationModel.ReloadOnChange);
        Assert.Equal(250, fileScanningConfigurationModel.ReloadDelay);
        Assert.Equal(0, fileScanningConfigurationModel.Order);
        Assert.Null(fileScanningConfigurationModel.OnLoadException);
        Assert.True(fileScanningConfigurationModel.EnvironmentFlag);
    }

    [Fact]
    public void New_But_NotExtension()
    {
        var fileScanningConfigurationModel = new FileScanningConfigurationModel("C:/Workplace/configuration", true);

        Assert.NotNull(fileScanningConfigurationModel);
        Assert.Equal(string.Empty, fileScanningConfigurationModel.Extension);
        Assert.Equal(string.Empty, fileScanningConfigurationModel.Group);
    }

    [Theory]
    [InlineData("C:/Workplace/configuration.json", true)]
    [InlineData("C:\\Workplace\\configuration.json", true)]
    [InlineData("/Workplace/configuration.json", true)]
    [InlineData("Workplace/configuration.json", false)]
    [InlineData("C:/Workplaces/configuration.json", false)]
    public void IsMatch(string filePath, bool result)
    {
        var fileScanningConfigurationModel = new FileScanningConfigurationModel("C:/Workplace/configuration.json", true);
        Assert.Equal(result, fileScanningConfigurationModel.IsMatch(filePath));
    }

    [Fact]
    public void ResolveGroup_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            FileScanningConfigurationModel.ResolveGroup(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            FileScanningConfigurationModel.ResolveGroup(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            FileScanningConfigurationModel.ResolveGroup("");
        });
    }

    [Theory]
    [InlineData("C:/Workplace/configuration.json", "configuration")]
    [InlineData("C:/Workplace/configuration.Development.json", "configuration")]
    [InlineData("C:/Workplace/configuration.Production.json", "configuration")]
    [InlineData("C:/Workplace/configuration", "")]
    [InlineData("C:/Workplace/configuration.settings.json", "configuration")]
    [InlineData("C:/Workplace/configuration.settings.Development.json", "configuration")]
    [InlineData("C:/Workplace/configuration.settings.Production.json", "configuration")]
    [InlineData("C:/Workplace/(configuration.settings).json", "(configuration.settings)")]
    [InlineData("C:/Workplace/(configuration.settings).Development.json", "(configuration.settings)")]
    [InlineData("C:/Workplace/(configuration.settings).Production.json", "(configuration.settings)")]
    public void ResolveGroup_ReturnOK(string filePath, string group)
    {
        Assert.Equal(group, FileScanningConfigurationModel.ResolveGroup(filePath));
    }

    [Fact]
    public void ToString_Output()
    {
        var fileScanningConfigurationModel = new FileScanningConfigurationModel("C:/Workplace/configuration.json", true);
        Assert.Equal("Group = configuration, FileName = configuration.json, Path = C:\\Workplace\\configuration.json", fileScanningConfigurationModel.ToString());
    }
}