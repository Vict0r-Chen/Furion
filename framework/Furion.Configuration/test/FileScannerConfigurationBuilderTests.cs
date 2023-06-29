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
    public void NewInstance_Default()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        Assert.NotNull(fileScannerConfigurationBuilder);

        Assert.NotNull(fileScannerConfigurationBuilder._directories);
        Assert.Empty(fileScannerConfigurationBuilder._directories);

        Assert.NotNull(fileScannerConfigurationBuilder._fileGlobbing);
        Assert.Single(fileScannerConfigurationBuilder._fileGlobbing);
        Assert.Equal("*.json", fileScannerConfigurationBuilder._fileGlobbing.ElementAt(0));

        Assert.NotNull(fileScannerConfigurationBuilder._fileBlacklistGlobbing);
        var fileBlacklistGlobbing = new[] {
            "*.runtimeconfig.json",
            "*.runtimeconfig.*.json",
            "*.deps.json",
            "*.staticwebassets.*.json",
            "*.nuget.dgspec.json",
            "launchSettings.json",
            "tsconfig.json",
            "package.json",
            "project.assets.json",
            "manifest.json",
            "MvcTestingAppManifest.json"
        };
        Assert.Equal(fileBlacklistGlobbing, fileScannerConfigurationBuilder._fileBlacklistGlobbing.ToArray());

        Assert.NotNull(fileScannerConfigurationBuilder._fileConfigurationSources);
        Assert.Equal(3, fileScannerConfigurationBuilder._fileConfigurationSources.Count);
        Assert.Equal(".json", fileScannerConfigurationBuilder._fileConfigurationSources.Keys.ElementAt(0));
        Assert.Equal(typeof(JsonConfigurationSource), fileScannerConfigurationBuilder._fileConfigurationSources.Values.ElementAt(0));
        Assert.Equal(".xml", fileScannerConfigurationBuilder._fileConfigurationSources.Keys.ElementAt(1));
        Assert.Equal(typeof(XmlConfigurationSource), fileScannerConfigurationBuilder._fileConfigurationSources.Values.ElementAt(1));
        Assert.Equal(".ini", fileScannerConfigurationBuilder._fileConfigurationSources.Keys.ElementAt(2));
        Assert.Equal(typeof(IniConfigurationSource), fileScannerConfigurationBuilder._fileConfigurationSources.Values.ElementAt(2));

        Assert.Null(fileScannerConfigurationBuilder._filterConfigure);
        Assert.Equal((uint)0, fileScannerConfigurationBuilder.MaxDepth);
    }

    [Fact]
    public void AddFilter_Null_Throw()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScannerConfigurationBuilder.AddFilter(null!);
        });
    }

    [Fact]
    public void AddFilter_ReturnOK()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddFilter(s =>
        {
            return true;
        });
        Assert.NotNull(fileScannerConfigurationBuilder._filterConfigure);
    }

    [Fact]
    public void AddDirectories_Throw()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScannerConfigurationBuilder.AddDirectories(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScannerConfigurationBuilder.AddDirectories(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScannerConfigurationBuilder.AddDirectories("assets/folder1");
        });
    }

    [Fact]
    public void AddDirectories_NotExists_AddedSuccessfully()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddDirectories(currentDirectory);

        Assert.Single(fileScannerConfigurationBuilder._directories);
        Assert.Equal(currentDirectory, fileScannerConfigurationBuilder._directories.Last());
    }

    [Fact]
    public void AddDirectories_Exists_Skip()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddDirectories(currentDirectory);

        Assert.Single(fileScannerConfigurationBuilder._directories);

        fileScannerConfigurationBuilder.AddDirectories(currentDirectory);
        fileScannerConfigurationBuilder.AddDirectories(currentDirectory);
        Assert.Single(fileScannerConfigurationBuilder._directories);
        Assert.Equal(currentDirectory, fileScannerConfigurationBuilder._directories.Last());
    }

    [Fact]
    public void AddDirectories_Multiple_Arguments_ReturnOK()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var folder1Directory = Path.Combine(Directory.GetCurrentDirectory(), "assets", "folder1");

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddDirectories(currentDirectory, folder1Directory);
        Assert.Equal(2, fileScannerConfigurationBuilder._directories.Count);

        Assert.Equal(currentDirectory, fileScannerConfigurationBuilder._directories.First());
        Assert.Equal(folder1Directory, fileScannerConfigurationBuilder._directories.Last());
    }

    [Fact]
    public void AddDirectories_IEnumerable_ReturnOK()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var folder1Directory = Path.Combine(Directory.GetCurrentDirectory(), "assets", "folder1");

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddDirectories(new List<string> { currentDirectory, folder1Directory });
        Assert.Equal(2, fileScannerConfigurationBuilder._directories.Count);

        Assert.Equal(currentDirectory, fileScannerConfigurationBuilder._directories.First());
        Assert.Equal(folder1Directory, fileScannerConfigurationBuilder._directories.Last());
    }

    [Fact]
    public void AddGlobbings_Throw()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScannerConfigurationBuilder.AddGlobbings(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileScannerConfigurationBuilder.AddGlobbings(string.Empty);
        });
    }

    [Fact]
    public void AddGlobbings_NotExists_AddedSuccessfully()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddGlobbings("*.xml");

        Assert.Equal(2, fileScannerConfigurationBuilder._fileGlobbing.Count);
        Assert.Equal("*.xml", fileScannerConfigurationBuilder._fileGlobbing.Last());
    }

    [Fact]
    public void AddGlobbings_Exists_Skip()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddGlobbings("*.json");

        Assert.Single(fileScannerConfigurationBuilder._fileGlobbing);
        fileScannerConfigurationBuilder.AddGlobbings("*.json");
        fileScannerConfigurationBuilder.AddGlobbings("*.json");
        Assert.Single(fileScannerConfigurationBuilder._fileGlobbing);

        Assert.Equal("*.json", fileScannerConfigurationBuilder._fileGlobbing.Last());
    }

    [Fact]
    public void AddGlobbings_Multiple_Arguments_ReturnOK()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddGlobbings("*.xml", "*.ini");

        Assert.Equal(3, fileScannerConfigurationBuilder._fileGlobbing.Count);
        Assert.Equal("*.ini", fileScannerConfigurationBuilder._fileGlobbing.Last());
    }

    [Fact]
    public void AddGlobbings_IEnumerable_ReturnOK()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddGlobbings(new List<string> { "*.xml", "*.ini" });

        Assert.Equal(3, fileScannerConfigurationBuilder._fileGlobbing.Count);
        Assert.Equal("*.ini", fileScannerConfigurationBuilder._fileGlobbing.Last());
    }

    [Fact]
    public void AddConfigurationSources_Throw()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScannerConfigurationBuilder.AddConfigurationSources(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileScannerConfigurationBuilder.AddConfigurationSources(".jpg", null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            fileScannerConfigurationBuilder.AddConfigurationSources("json", typeof(JsonConfigurationSource));
        });
        Assert.Equal("`json` is not a valid file extension. (Parameter 'extension')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            fileScannerConfigurationBuilder.AddConfigurationSources(".unknown", typeof(UnknownConfigurationSource));
        });
        Assert.Equal("`UnknownConfigurationSource` type is not assignable from `FileConfigurationSource`. (Parameter 'configurationSourceType')", exception2.Message);
    }

    [Fact]
    public void AddConfigurationSources_ReturnOK()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddConfigurationSources(".json", typeof(JsonConfigurationSource));
        fileScannerConfigurationBuilder.AddConfigurationSources(".yaml", typeof(YamlConfigurationSource));

        Assert.Equal(4, fileScannerConfigurationBuilder._fileConfigurationSources.Count);
        Assert.Equal(".yaml", fileScannerConfigurationBuilder._fileConfigurationSources.Keys.ElementAt(3));
        Assert.Equal(typeof(YamlConfigurationSource), fileScannerConfigurationBuilder._fileConfigurationSources.Values.ElementAt(3));
    }

    [Fact]
    public void AddConfigurationSourcesOfT_ReturnOK()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddConfigurationSources<YamlConfigurationSource>(".yaml");

        Assert.Equal(4, fileScannerConfigurationBuilder._fileConfigurationSources.Count);
        Assert.Equal(".yaml", fileScannerConfigurationBuilder._fileConfigurationSources.Keys.ElementAt(3));
        Assert.Equal(typeof(YamlConfigurationSource), fileScannerConfigurationBuilder._fileConfigurationSources.Values.ElementAt(3));
    }

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

    [Fact]
    public void Release_ClearAll()
    {
        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.Release();

        Assert.Empty(fileScannerConfigurationBuilder._directories);
        Assert.Empty(fileScannerConfigurationBuilder._fileGlobbing);
        Assert.Empty(fileScannerConfigurationBuilder._fileBlacklistGlobbing);
        Assert.Empty(fileScannerConfigurationBuilder._fileConfigurationSources);
        Assert.Null(fileScannerConfigurationBuilder._filterConfigure);
    }

    [Fact]
    public void ScanDirectories()
    {
        var configsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "configs");
        var folder1FilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "folder1");
        Assert.True(Directory.Exists(configsFilePath));
        Assert.True(Directory.Exists(folder1FilePath));

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder
        {
            MaxDepth = 1
        };
        fileScannerConfigurationBuilder.AddDirectories(configsFilePath, folder1FilePath);

        Assert.Equal(2, fileScannerConfigurationBuilder._directories.Count);

        var configurationBuilder = new ConfigurationBuilder();
        var fileConfigurationModels = fileScannerConfigurationBuilder.ScanDirectories(configurationBuilder).ToList();

        Assert.NotNull(fileConfigurationModels);
        Assert.Equal(3, fileConfigurationModels.Count);

        Assert.Equal("appsettings.json", fileConfigurationModels.ElementAt(0).FileName);
        Assert.Equal("folder1.json", fileConfigurationModels.ElementAt(1).FileName);
        Assert.Equal("folder2.json", fileConfigurationModels.ElementAt(2).FileName);
    }

    [Fact]
    public void ScanDirectories_With_Filter()
    {
        var configsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "configs");
        var folder1FilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "folder1");
        Assert.True(Directory.Exists(configsFilePath));
        Assert.True(Directory.Exists(folder1FilePath));

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder
        {
            MaxDepth = 1
        };
        fileScannerConfigurationBuilder.AddDirectories(configsFilePath, folder1FilePath);
        fileScannerConfigurationBuilder.AddFilter(model =>
        {
            if (model.FileName == "appsettings.json")
            {
                return false;
            }

            return true;
        });

        Assert.Equal(2, fileScannerConfigurationBuilder._directories.Count);

        var configurationBuilder = new ConfigurationBuilder();
        var fileConfigurationModels = fileScannerConfigurationBuilder.ScanDirectories(configurationBuilder).ToList();

        Assert.NotNull(fileConfigurationModels);
        Assert.Equal(2, fileConfigurationModels.Count);

        Assert.Equal("folder1.json", fileConfigurationModels.ElementAt(0).FileName);
        Assert.Equal("folder2.json", fileConfigurationModels.ElementAt(1).FileName);
    }

    [Fact]
    public void ScanDirectories_With_FileGlobbing()
    {
        var configsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "configs");
        var folder1FilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "folder1");
        Assert.True(Directory.Exists(configsFilePath));
        Assert.True(Directory.Exists(folder1FilePath));

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder
        {
            MaxDepth = 1
        };
        fileScannerConfigurationBuilder.AddDirectories(configsFilePath, folder1FilePath);
        fileScannerConfigurationBuilder.AddGlobbings("*.xml");

        Assert.Equal(2, fileScannerConfigurationBuilder._directories.Count);

        var configurationBuilder = new ConfigurationBuilder();
        var fileConfigurationModels = fileScannerConfigurationBuilder.ScanDirectories(configurationBuilder).ToList();

        Assert.NotNull(fileConfigurationModels);
        Assert.Equal(4, fileConfigurationModels.Count);

        Assert.Equal("appsettings.json", fileConfigurationModels.ElementAt(0).FileName);
        Assert.Equal("appsettings.xml", fileConfigurationModels.ElementAt(1).FileName);
        Assert.Equal("folder1.json", fileConfigurationModels.ElementAt(2).FileName);
        Assert.Equal("folder2.json", fileConfigurationModels.ElementAt(3).FileName);
    }

    [Fact]
    public void ScanDirectories_IfExists()
    {
        var configsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "configs");
        var folder1FilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "folder1");
        Assert.True(Directory.Exists(configsFilePath));
        Assert.True(Directory.Exists(folder1FilePath));

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder
        {
            MaxDepth = 1
        };
        fileScannerConfigurationBuilder.AddDirectories(configsFilePath, folder1FilePath);

        Assert.Equal(2, fileScannerConfigurationBuilder._directories.Count);

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile(Path.Combine(configsFilePath, "appsettings.json"));
        var fileConfigurationModels = fileScannerConfigurationBuilder.ScanDirectories(configurationBuilder).ToList();

        Assert.NotNull(fileConfigurationModels);
        Assert.Equal(2, fileConfigurationModels.Count);

        Assert.Equal("folder1.json", fileConfigurationModels.ElementAt(0).FileName);
        Assert.Equal("folder2.json", fileConfigurationModels.ElementAt(1).FileName);
    }

    [Fact]
    public void AddFileConfigurationSource_Null_ReturnOk()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddFileConfigurationSource(configurationBuilder, null);
        Assert.Empty(configurationBuilder.Sources);
    }

    [Fact]
    public void AddFileConfigurationSource_IfExtensionNotExists_Throw()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            fileScannerConfigurationBuilder.AddFileConfigurationSource(configurationBuilder, new("D:/appsettings.yml"));
        });
        Assert.Equal("Configuration provider for `.yml` extension not found.", exception.Message);
    }

    [Fact]
    public void ScanDirectories_AddFileConfigurationSource()
    {
        var configsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "configs");
        var folder1FilePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "folder1");
        Assert.True(Directory.Exists(configsFilePath));
        Assert.True(Directory.Exists(folder1FilePath));

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder
        {
            MaxDepth = 1
        };
        fileScannerConfigurationBuilder.AddDirectories(configsFilePath, folder1FilePath);

        Assert.Equal(2, fileScannerConfigurationBuilder._directories.Count);

        var configurationBuilder = new ConfigurationBuilder();
        var fileConfigurationModels = fileScannerConfigurationBuilder.ScanDirectories(configurationBuilder).ToList();

        foreach (var fileConfigurationModel in fileConfigurationModels)
        {
            fileScannerConfigurationBuilder.AddFileConfigurationSource(configurationBuilder, fileConfigurationModel);
        }

        Assert.Equal(3, configurationBuilder.Sources.Count);
        Assert.Equal(3, configurationBuilder.Sources.OfType<FileConfigurationSource>().Count());

        var fileConfigurationModel1 = fileConfigurationModels.ElementAt(0);
        var fileConfigurationSource1 = configurationBuilder.Sources.ElementAt(0) as JsonConfigurationSource;
        Assert.NotNull(fileConfigurationSource1);

        Assert.Equal(fileConfigurationSource1.Path, fileConfigurationModel1.FileName);
        Assert.Equal((fileConfigurationSource1.FileProvider as PhysicalFileProvider)!.Root, fileConfigurationModel1.DirectoryName + "\\");
        Assert.Equal(fileConfigurationSource1.Optional, fileConfigurationModel1.Optional);
        Assert.Equal(fileConfigurationSource1.ReloadOnChange, fileConfigurationModel1.ReloadOnChange);
        Assert.Equal(fileConfigurationSource1.ReloadDelay, fileConfigurationModel1.ReloadDelay);
        Assert.NotNull(fileConfigurationSource1.FileProvider);
    }

    [Theory]
    [InlineData("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration\\test\\assets\\appsettings.json", "C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration\\test\\bin\\Debug\\net8.0\\assets\\appsettings.json")]
    [InlineData("C:/Workspace/furion.net/Furion/framework/Furion.Configuration/test/assets/appsettings.json", "C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration\\test\\bin\\Debug\\net8.0\\assets\\appsettings.json")]
    [InlineData("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration\\test\\appsettings.json", "C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration\\test\\bin\\Debug\\net8.0\\appsettings.json")]
    public void GetFilePublishPaths(string filePath, string filePublishPath)
    {
        var filePublishPaths = FileScannerConfigurationBuilder.GetFilePublishPaths(filePath, "C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Configuration\\test");
        Assert.Equal(filePublishPath, filePublishPaths[^1]);
    }

    [Fact]
    public void Build_Not_Environment()
    {
        var filesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "assets", "files");

        var configurationBuilder = new ConfigurationBuilder();

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddDirectories(filesDirectory);
        fileScannerConfigurationBuilder.Build(configurationBuilder);

        Assert.Equal(2, configurationBuilder.Sources.Count);
        Assert.Equal("appsettings.json", (configurationBuilder.Sources.ElementAt(0) as JsonConfigurationSource)!.Path);
        Assert.Equal("test.json", (configurationBuilder.Sources.ElementAt(1) as JsonConfigurationSource)!.Path);
    }

    [Fact]
    public void Build_With_Web_Environment()
    {
        var filesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "assets", "files");

        var webApplicationBuilder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "Development"
        });
        var configurationBuilder = webApplicationBuilder.Configuration;

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddDirectories(filesDirectory);
        fileScannerConfigurationBuilder.Build(configurationBuilder);

        var sources = configurationBuilder.Sources;
        Assert.True(sources.Count > 4);

        var lastFourSources = sources.Skip(sources.Count - 4);
        Assert.Equal("appsettings.json", (lastFourSources.ElementAt(0) as JsonConfigurationSource)!.Path);
        Assert.Equal("appsettings.Development.json", (lastFourSources.ElementAt(1) as JsonConfigurationSource)!.Path);
        Assert.Equal("test.json", (lastFourSources.ElementAt(2) as JsonConfigurationSource)!.Path);
        Assert.Equal("test.Development.json", (lastFourSources.ElementAt(3) as JsonConfigurationSource)!.Path);
    }

    [Fact]
    public void Build_With_Hosting_Environment()
    {
        var filesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "assets", "files");

        var hostApplicationBuilder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            EnvironmentName = "Development"
        });
        var configurationBuilder = hostApplicationBuilder.Configuration;

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddDirectories(filesDirectory);
        fileScannerConfigurationBuilder.Build(configurationBuilder);

        var sources = configurationBuilder.Sources;
        Assert.True(sources.Count > 4);

        var lastFourSources = sources.Skip(sources.Count - 4);
        Assert.Equal("appsettings.json", (lastFourSources.ElementAt(0) as JsonConfigurationSource)!.Path);
        Assert.Equal("appsettings.Development.json", (lastFourSources.ElementAt(1) as JsonConfigurationSource)!.Path);
        Assert.Equal("test.json", (lastFourSources.ElementAt(2) as JsonConfigurationSource)!.Path);
        Assert.Equal("test.Development.json", (lastFourSources.ElementAt(3) as JsonConfigurationSource)!.Path);
    }

    [Fact]
    public void Build_With_Order()
    {
        var filesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "assets", "files");

        var webApplicationBuilder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "Development"
        });
        var configurationBuilder = webApplicationBuilder.Configuration;

        var fileScannerConfigurationBuilder = new FileScannerConfigurationBuilder();
        fileScannerConfigurationBuilder.AddDirectories(filesDirectory);
        fileScannerConfigurationBuilder.AddFilter(model =>
        {
            if (model.FileName.StartsWith("appsettings"))
            {
                model.Order = 10;
            }

            return true;
        });
        fileScannerConfigurationBuilder.Build(configurationBuilder);

        var sources = configurationBuilder.Sources;
        Assert.True(sources.Count > 4);

        var lastFourSources = sources.Skip(sources.Count - 4);

        Assert.Equal("test.json", (lastFourSources.ElementAt(0) as JsonConfigurationSource)!.Path);
        Assert.Equal("test.Development.json", (lastFourSources.ElementAt(1) as JsonConfigurationSource)!.Path);
        Assert.Equal("appsettings.json", (lastFourSources.ElementAt(2) as JsonConfigurationSource)!.Path);
        Assert.Equal("appsettings.Development.json", (lastFourSources.ElementAt(3) as JsonConfigurationSource)!.Path);
    }
}