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

public class FileConfigurationProviderTests : IDisposable
{
    [Fact]
    public void New_Default()
    {
        Assert.Null(FileConfigurationProvider._jsonParser);

        var fileConfigurationProvider = new FileConfigurationProvider();

        Assert.NotNull(fileConfigurationProvider);
        Assert.NotNull(fileConfigurationProvider._parsers);
        Assert.NotNull(fileConfigurationProvider._sourceTypes);
        Assert.NotNull(FileConfigurationProvider._jsonParser);

        Assert.Single(fileConfigurationProvider._parsers);
        Assert.Single(fileConfigurationProvider._sourceTypes);

        Assert.Equal(".json", fileConfigurationProvider._parsers.Keys.ElementAt(0));
        Assert.NotNull(fileConfigurationProvider._parsers.Values.ElementAt(0));

        Assert.Equal(".json", fileConfigurationProvider._sourceTypes.Keys.ElementAt(0));
        Assert.Equal(typeof(JsonConfigurationSource), fileConfigurationProvider._sourceTypes.Values.ElementAt(0));
    }

    [Fact]
    public void AddParser_Invalid_Parameters()
    {
        var fileConfigurationProvider = new FileConfigurationProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationProvider.AddParser(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationProvider.AddParser(string.Empty, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationProvider.AddParser(".xml", null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationProvider.AddParser("xml", stream => new Dictionary<string, string?>());
        });
        Assert.Equal("The `xml` is not a valid file extension. (Parameter 'extension')", exception.Message);
    }

    [Fact]
    public void AddParser_ReturnOK()
    {
        var fileConfigurationProvider = new FileConfigurationProvider();

        fileConfigurationProvider.AddParser(".xml", stream => XmlStreamConfigurationProvider.Read(stream, XmlDocumentDecryptor.Instance));
        fileConfigurationProvider.AddParser(".ini", IniStreamConfigurationProvider.Read);

        Assert.Equal(3, fileConfigurationProvider._parsers.Count);

        Assert.Equal(".xml", fileConfigurationProvider._parsers.Keys.ElementAt(1));
        Assert.NotNull(fileConfigurationProvider._parsers.Values.ElementAt(1));

        Assert.Equal(".ini", fileConfigurationProvider._parsers.Keys.ElementAt(2));
        Assert.NotNull(fileConfigurationProvider._parsers.Values.ElementAt(2));
    }

    [Fact]
    public void AddSource_Invalid_Parameters()
    {
        var fileConfigurationProvider = new FileConfigurationProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationProvider.AddSource(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationProvider.AddSource(string.Empty, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationProvider.AddSource(".xml", null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationProvider.AddSource("xml", typeof(JsonConfigurationSource));
        });
        Assert.Equal("The `xml` is not a valid file extension. (Parameter 'extension')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationProvider.AddSource(".xml", typeof(UnknownConfigurationSource));
        });
        Assert.Equal("The `UnknownConfigurationSource` type is not assignable from `FileConfigurationSource`. (Parameter 'sourceType')", exception2.Message);
    }

    [Fact]
    public void AddSource_ReturnOK()
    {
        var fileConfigurationProvider = new FileConfigurationProvider();

        fileConfigurationProvider.AddSource(".xml", typeof(XmlConfigurationSource));
        fileConfigurationProvider.AddSource(".ini", typeof(IniConfigurationSource));

        Assert.Equal(3, fileConfigurationProvider._sourceTypes.Count);

        Assert.Equal(".xml", fileConfigurationProvider._sourceTypes.Keys.ElementAt(1));
        Assert.Equal(typeof(XmlConfigurationSource), fileConfigurationProvider._sourceTypes.Values.ElementAt(1));

        Assert.Equal(".ini", fileConfigurationProvider._sourceTypes.Keys.ElementAt(2));
        Assert.Equal(typeof(IniConfigurationSource), fileConfigurationProvider._sourceTypes.Values.ElementAt(2));
    }

    [Fact]
    public void CreateSourceInstance_Invalid_Parameters()
    {
        var fileConfigurationProvider = new FileConfigurationProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationProvider.CreateSourceInstance(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationProvider.CreateSourceInstance(string.Empty, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationProvider.CreateSourceInstance(".xml", null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationProvider.CreateSourceInstance("xml", source => { });
        });
        Assert.Equal("The `xml` is not a valid file extension. (Parameter 'extension')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationProvider.CreateSourceInstance(".xml", source => { });
        });
        Assert.Equal("Configuration source for file with the extension `.xml` not found. (Parameter 'extension')", exception2.Message);
    }

    [Fact]
    public void CreateSourceInstance_Default()
    {
        var fileConfigurationProvider = new FileConfigurationProvider();

        var fileConfiguration = fileConfigurationProvider.CreateSourceInstance(".json", source =>
        {
            source.Optional = true;
        });

        Assert.NotNull(fileConfiguration);
        Assert.True(fileConfiguration.Optional);
    }

    [Fact]
    public void CreateSourceInstance_Additional()
    {
        var fileConfigurationProvider = new FileConfigurationProvider();

        fileConfigurationProvider.AddSource(".xml", typeof(XmlConfigurationSource));
        fileConfigurationProvider.AddSource(".ini", typeof(IniConfigurationSource));

        var xmlFileConfiguration = fileConfigurationProvider.CreateSourceInstance(".xml", source =>
        {
            source.Optional = true;
        });

        Assert.NotNull(xmlFileConfiguration);
        Assert.True(xmlFileConfiguration.Optional);

        var iniFileConfiguration = fileConfigurationProvider.CreateSourceInstance(".ini", source =>
        {
            source.Optional = true;
        });

        Assert.NotNull(iniFileConfiguration);
        Assert.True(iniFileConfiguration.Optional);
    }

    [Fact]
    public void Parse_Invalid_Parameters()
    {
        var fileConfigurationProvider = new FileConfigurationProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationProvider.Parse(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationProvider.Parse(string.Empty, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationProvider.Parse(".xml", null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            using var stream = new MemoryStream();
            fileConfigurationProvider.Parse("xml", stream);
        });
        Assert.Equal("The `xml` is not a valid file extension. (Parameter 'extension')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            using var stream = new MemoryStream();
            fileConfigurationProvider.Parse(".xml", stream);
        });
        Assert.Equal("Configuration parser for file with the extension `.xml` not found. (Parameter 'extension')", exception2.Message);
    }

    [Fact]
    public void Parse_Default()
    {
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "config.json");
        using var fileStream = File.OpenRead(jsonFilePath);

        var fileConfigurationProvider = new FileConfigurationProvider();
        var data = fileConfigurationProvider.Parse(".json", fileStream);

        Assert.NotNull(data);
        Assert.Single(data);
        Assert.Equal("Name", data.Keys.ElementAt(0));
        Assert.Equal("Furion", data.Values.ElementAt(0));
    }

    [Fact]
    public void Parse_Additional()
    {
        var fileConfigurationProvider = new FileConfigurationProvider();

        fileConfigurationProvider.AddParser(".xml", stream => XmlStreamConfigurationProvider.Read(stream, XmlDocumentDecryptor.Instance));
        fileConfigurationProvider.AddParser(".ini", IniStreamConfigurationProvider.Read);

        var xmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "config.xml");
        using var xmlFileStream = File.OpenRead(xmlFilePath);

        var xmlData = fileConfigurationProvider.Parse(".xml", xmlFileStream);

        Assert.NotNull(xmlData);
        Assert.Single(xmlData);
        Assert.Equal("Name", xmlData.Keys.ElementAt(0));
        Assert.Equal("Furion", xmlData.Values.ElementAt(0));

        var iniFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "config.ini");
        using var iniFileStream = File.OpenRead(iniFilePath);

        var iniData = fileConfigurationProvider.Parse(".ini", iniFileStream);

        Assert.NotNull(iniData);
        Assert.Single(iniData);
        Assert.Equal("Name", iniData.Keys.ElementAt(0));
        Assert.Equal("Furion", iniData.Values.ElementAt(0));
    }

    [Fact]
    public void EnsureLegalExtension_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            FileConfigurationProvider.EnsureLegalExtension(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            FileConfigurationProvider.EnsureLegalExtension(string.Empty);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            FileConfigurationProvider.EnsureLegalExtension("xml");
        });
        Assert.Equal("The `xml` is not a valid file extension. (Parameter 'extension')", exception.Message);
    }

    [Fact]
    public void EnsureLegalExtension_ReturnOK()
    {
        FileConfigurationProvider.EnsureLegalExtension(".xml");
    }

    [Fact]
    public void ResolveJsonParser()
    {
        Assert.Null(FileConfigurationProvider._jsonParser);

        var @delegate = FileConfigurationProvider.ResolveJsonParser();

        Assert.NotNull(@delegate);
        Assert.NotNull(FileConfigurationProvider._jsonParser);
    }

    public void Dispose()
    {
        FileConfigurationProvider._jsonParser = null;
    }
}