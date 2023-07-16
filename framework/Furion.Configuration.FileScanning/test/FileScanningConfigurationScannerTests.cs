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
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(new ConfigurationBuilder(), null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(new ConfigurationBuilder(), new());

        Assert.NotNull(fileScanningConfigurationScanner);
        Assert.NotNull(fileScanningConfigurationScanner._configurationBuilder);
        Assert.NotNull(fileScanningConfigurationScanner._fileScanningConfigurationBuilder);

        Assert.Null(fileScanningConfigurationScanner.ContentRoot);
        Assert.Null(fileScanningConfigurationScanner.EnvironmentName);
    }

    [Fact]
    public void New_WithConfigurationManager()
    {
        var builder = WebApplication.CreateBuilder();
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(builder.Configuration, new());

        Assert.NotNull(fileScanningConfigurationScanner);
        Assert.NotNull(fileScanningConfigurationScanner._configurationBuilder);
        Assert.NotNull(fileScanningConfigurationScanner._fileScanningConfigurationBuilder);

        Assert.NotNull(fileScanningConfigurationScanner.ContentRoot);
        Assert.Null(fileScanningConfigurationScanner.EnvironmentName);
    }

    [Fact]
    public void Initialize_ReturnOK()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "Development"
        });
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(builder.Configuration, new());
        fileScanningConfigurationScanner.Initialize();

        Assert.NotNull(fileScanningConfigurationScanner.ContentRoot);
        Assert.NotNull(fileScanningConfigurationScanner.EnvironmentName);
        Assert.Equal("Development", fileScanningConfigurationScanner.EnvironmentName);
    }

    [Fact]
    public void Initialize_DisableEnvironmentSwitching()
    {
        var builder = WebApplication.CreateBuilder();
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(builder.Configuration, new()
        {
            AllowEnvironmentSwitching  = false
        });
        fileScanningConfigurationScanner.Initialize();

        Assert.NotNull(fileScanningConfigurationScanner.ContentRoot);
        Assert.Null(fileScanningConfigurationScanner.EnvironmentName);
    }

    [Fact]
    public void ScanToAddFiles_ReturnOK()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();

        var directory = Path.Combine(AppContext.BaseDirectory, "directories");
        fileScanningConfigurationBuilder.AddDirectories(directory);

        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, fileScanningConfigurationBuilder);
        fileScanningConfigurationScanner.ScanToAddFiles();

        Assert.NotEmpty(configurationBuilder.Sources);
        Assert.Equal(2, configurationBuilder.Sources.Count);

        var firstSource = configurationBuilder.Sources.OfType<JsonConfigurationSource>().First();
        Assert.Equal("config.json", firstSource.Path);
    }

    [Fact]
    public void ScanToAddFiles_WithOrder()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();
        fileScanningConfigurationBuilder.AddFilter(model =>
        {
            if (model.Group == "config")
            {
                model.Order = 1;
            }
            return true;
        });

        var directory = Path.Combine(AppContext.BaseDirectory, "directories");
        fileScanningConfigurationBuilder.AddDirectories(directory);

        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, fileScanningConfigurationBuilder);
        fileScanningConfigurationScanner.ScanToAddFiles();

        Assert.NotEmpty(configurationBuilder.Sources);
        Assert.Equal(2, configurationBuilder.Sources.Count);

        var firstSource = configurationBuilder.Sources.OfType<JsonConfigurationSource>().First();
        Assert.Equal("appsettings.json", firstSource.Path);
    }

    [Fact]
    public void CreateModels()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();

        var directory = Path.Combine(AppContext.BaseDirectory, "directories");
        fileScanningConfigurationBuilder.AddDirectories(directory);

        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, fileScanningConfigurationBuilder);
        var models = fileScanningConfigurationScanner.CreateModels().ToList();

        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Equal(3, models.Count);
    }

    [Fact]
    public void CreateModels_WithFilter()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var fileScanningConfigurationBuilder = new FileScanningConfigurationBuilder();
        fileScanningConfigurationBuilder.AddFilter(model =>
        {
            return model.FileName != "config.json";
        });

        var directory = Path.Combine(AppContext.BaseDirectory, "directories");
        fileScanningConfigurationBuilder.AddDirectories(directory);

        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, fileScanningConfigurationBuilder);
        var models = fileScanningConfigurationScanner.CreateModels().ToList();

        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Equal(2, models.Count);
    }

    [Fact]
    public void CreateModel_Invalid_Parameters()
    {
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(new ConfigurationBuilder(), new());

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationScanner.CreateModel(null!, true);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationScanner.CreateModel(string.Empty, true);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationScanner.CreateModel("", true);
        });
    }

    [Fact]
    public void CreateModel_ReturnOK()
    {
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(new ConfigurationBuilder(), new());
        var model = fileScanningConfigurationScanner.CreateModel("C:/workplace/config.json", true);

        Assert.NotNull(model);
    }

    [Fact]
    public void AddFile_Invalid_Parameters()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, new());

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationScanner.AddFile(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationScanner.AddFile(new(), null!);
        });

        Assert.Throws<FileNotFoundException>(() =>
        {
            fileScanningConfigurationScanner.AddFile(new(), new("C:/workplace/config.json", false));
            configurationBuilder.Build();
        });
    }

    [Fact]
    public void AddFile_ReturnOK()
    {
        var directory = Path.Combine(AppContext.BaseDirectory, "directories");
        var configurationBuilder = new ConfigurationBuilder();

        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, new());

        fileScanningConfigurationScanner.AddFile(new(), new(Path.Combine(directory, "config.json"), true));
        Assert.Single(configurationBuilder.Sources);
    }

    [Fact]
    public void AddFile_Optional_IsTrue()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, new());

        fileScanningConfigurationScanner.AddFile(new(), new("C:/workplace/config.json", false)
        {
            Optional = true
        });
        configurationBuilder.Build();
    }

    [Fact]
    public void AddFileByEnvironment_Invalid_Parameters()
    {
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(new ConfigurationBuilder(), new());

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationScanner.AddFileByEnvironment(null!, null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationScanner.AddFileByEnvironment(new(), null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationScanner.AddFileByEnvironment(new(), null!, string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationScanner.AddFileByEnvironment(new(), null!, "");
        });
    }

    [Fact]
    public void AddFileByEnvironment_ReturnOK()
    {
        var directory = Path.Combine(AppContext.BaseDirectory, "directories");
        var configurationBuilder = new ConfigurationBuilder();
        var file = Path.Combine(directory, "config.json");

        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, new());

        fileScanningConfigurationScanner.AddFileByEnvironment(new(), new(file, true), file);
        Assert.Single(configurationBuilder.Sources);
    }

    [Fact]
    public void AddFileByEnvironment_AllowEnvironmentSwitching ()
    {
        var directory = Path.Combine(AppContext.BaseDirectory, "directories");
        var configurationBuilder = new ConfigurationBuilder();
        var file = Path.Combine(directory, "config.json");

        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, new()
        {
            AllowEnvironmentSwitching  = true
        });

        fileScanningConfigurationScanner.AddFileByEnvironment(new(), null, file);
        Assert.Single(configurationBuilder.Sources);
    }

    [Fact]
    public void AddFileByEnvironment_DisableEnvironmentSwitching()
    {
        var directory = Path.Combine(AppContext.BaseDirectory, "directories");
        var configurationBuilder = new ConfigurationBuilder();
        var file = Path.Combine(directory, "config.json");

        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, new()
        {
            AllowEnvironmentSwitching  = false
        });

        fileScanningConfigurationScanner.AddFileByEnvironment(new(), null, file);
        Assert.Empty(configurationBuilder.Sources);
    }

    [Fact]
    public void GetAddedFiles()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "Development"
        });
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(builder.Configuration, new());

        var files = fileScanningConfigurationScanner.GetAddedFiles();
        Assert.NotNull(files);
        Assert.Equal(2, files.Count);
    }

    [Fact]
    public void ResolvePublicationFile_Invalid_Parameters()
    {
        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(new ConfigurationBuilder(), new());

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScanningConfigurationScanner.ResolvePublicationFile(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationScanner.ResolvePublicationFile(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScanningConfigurationScanner.ResolvePublicationFile("");
        });
    }

    [Theory]
    [InlineData("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration.FileScanning\\test\\assets\\appsettings.json", "C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration.FileScanning\\test\\bin\\Debug\\net8.0\\assets\\appsettings.json")]
    [InlineData("C:/Workspace/furion.net/Furion/framework/Furion.Configuration.FileScanning/test/assets/appsettings.json", "C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration.FileScanning\\test\\bin\\Debug\\net8.0\\assets\\appsettings.json")]
    [InlineData("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration.FileScanning\\test\\appsettings.json", "C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration.FileScanning\\test\\bin\\Debug\\net8.0\\appsettings.json")]
    public void ResolvePublicationFile(string filePath, string publicationFile)
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"CONTENTROOT","C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration.FileScanning\\test" }
        });

        var fileScanningConfigurationScanner = new FileScanningConfigurationScanner(configurationBuilder, new());
        var filePublishPaths = fileScanningConfigurationScanner.ResolvePublicationFile(filePath);
        Assert.Equal(publicationFile, filePublishPaths.Last());
    }

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