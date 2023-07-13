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

public class FileConfigurationParserTests : IDisposable
{
    [Fact]
    public void New_Default()
    {
        Assert.Null(FileConfigurationParser._jsonParser);

        var fileConfigurationParser = new FileConfigurationParser();

        Assert.NotNull(fileConfigurationParser);
        Assert.NotNull(fileConfigurationParser._parsers);
        Assert.NotNull(fileConfigurationParser._sourceTypes);
        Assert.NotNull(FileConfigurationParser._jsonParser);

        Assert.Single(fileConfigurationParser._parsers);
        Assert.Single(fileConfigurationParser._sourceTypes);

        Assert.Equal(".json", fileConfigurationParser._parsers.Keys.ElementAt(0));
        Assert.NotNull(fileConfigurationParser._parsers.Values.ElementAt(0));

        Assert.Equal(".json", fileConfigurationParser._sourceTypes.Keys.ElementAt(0));
        Assert.Equal(typeof(JsonConfigurationSource), fileConfigurationParser._sourceTypes.Values.ElementAt(0));
    }

    [Fact]
    public void AddParser_Invalid_Parameters()
    {
        var fileConfigurationParser = new FileConfigurationParser();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationParser.AddParser(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationParser.AddParser(string.Empty, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationParser.AddParser(".xml", null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationParser.AddParser("xml", stream => new Dictionary<string, string?>());
        });
        Assert.Equal("The `xml` is not a valid file extension. (Parameter 'extension')", exception.Message);
    }

    [Fact]
    public void AddParser_ReturnOK()
    {
        var fileConfigurationParser = new FileConfigurationParser();

        fileConfigurationParser.AddParser(".xml", stream => XmlStreamConfigurationProvider.Read(stream, XmlDocumentDecryptor.Instance));
        fileConfigurationParser.AddParser(".ini", IniStreamConfigurationProvider.Read);

        Assert.Equal(3, fileConfigurationParser._parsers.Count);

        Assert.Equal(".xml", fileConfigurationParser._parsers.Keys.ElementAt(1));
        Assert.NotNull(fileConfigurationParser._parsers.Values.ElementAt(1));

        Assert.Equal(".ini", fileConfigurationParser._parsers.Keys.ElementAt(2));
        Assert.NotNull(fileConfigurationParser._parsers.Values.ElementAt(2));
    }

    [Fact]
    public void AddSource_Invalid_Parameters()
    {
        var fileConfigurationParser = new FileConfigurationParser();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationParser.AddSource(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationParser.AddSource(string.Empty, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationParser.AddSource(".xml", null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationParser.AddSource("xml", typeof(JsonConfigurationSource));
        });
        Assert.Equal("The `xml` is not a valid file extension. (Parameter 'extension')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationParser.AddSource(".xml", typeof(UnknownConfigurationSource));
        });
        Assert.Equal("The `UnknownConfigurationSource` type is not assignable from `FileConfigurationSource`. (Parameter 'sourceType')", exception2.Message);
    }

    [Fact]
    public void AddSource_ReturnOK()
    {
        var fileConfigurationParser = new FileConfigurationParser();

        fileConfigurationParser.AddSource(".xml", typeof(XmlConfigurationSource));
        fileConfigurationParser.AddSource(".ini", typeof(IniConfigurationSource));

        Assert.Equal(3, fileConfigurationParser._sourceTypes.Count);

        Assert.Equal(".xml", fileConfigurationParser._sourceTypes.Keys.ElementAt(1));
        Assert.Equal(typeof(XmlConfigurationSource), fileConfigurationParser._sourceTypes.Values.ElementAt(1));

        Assert.Equal(".ini", fileConfigurationParser._sourceTypes.Keys.ElementAt(2));
        Assert.Equal(typeof(IniConfigurationSource), fileConfigurationParser._sourceTypes.Values.ElementAt(2));
    }

    [Fact]
    public void CreateSourceInstance_Invalid_Parameters()
    {
        var fileConfigurationParser = new FileConfigurationParser();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationParser.CreateSourceInstance(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationParser.CreateSourceInstance(string.Empty, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationParser.CreateSourceInstance(".xml", null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationParser.CreateSourceInstance("xml", source => { });
        });
        Assert.Equal("The `xml` is not a valid file extension. (Parameter 'extension')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationParser.CreateSourceInstance(".xml", source => { });
        });
        Assert.Equal("Configuration source for file with the extension `.xml` not found. (Parameter 'extension')", exception2.Message);
    }

    [Fact]
    public void CreateSourceInstance_Default()
    {
        var fileConfigurationParser = new FileConfigurationParser();

        var fileConfiguration = fileConfigurationParser.CreateSourceInstance(".json", source =>
        {
            source.Optional = true;
        });

        Assert.NotNull(fileConfiguration);
        Assert.True(fileConfiguration.Optional);
    }

    [Fact]
    public void CreateSourceInstance_Additional()
    {
        var fileConfigurationParser = new FileConfigurationParser();

        fileConfigurationParser.AddSource(".xml", typeof(XmlConfigurationSource));
        fileConfigurationParser.AddSource(".ini", typeof(IniConfigurationSource));

        var xmlFileConfiguration = fileConfigurationParser.CreateSourceInstance(".xml", source =>
        {
            source.Optional = true;
        });

        Assert.NotNull(xmlFileConfiguration);
        Assert.True(xmlFileConfiguration.Optional);

        var iniFileConfiguration = fileConfigurationParser.CreateSourceInstance(".ini", source =>
        {
            source.Optional = true;
        });

        Assert.NotNull(iniFileConfiguration);
        Assert.True(iniFileConfiguration.Optional);
    }

    [Fact]
    public void Parse_Invalid_Parameters()
    {
        var fileConfigurationParser = new FileConfigurationParser();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationParser.Parse(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            fileConfigurationParser.Parse(string.Empty, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fileConfigurationParser.Parse(".xml", null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            using var stream = new MemoryStream();
            fileConfigurationParser.Parse("xml", stream);
        });
        Assert.Equal("The `xml` is not a valid file extension. (Parameter 'extension')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            using var stream = new MemoryStream();
            fileConfigurationParser.Parse(".xml", stream);
        });
        Assert.Equal("Configuration parser for file with the extension `.xml` not found. (Parameter 'extension')", exception2.Message);
    }

    [Fact]
    public void Parse_Default()
    {
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "config.json");
        using var fileStream = File.OpenRead(jsonFilePath);

        var fileConfigurationParser = new FileConfigurationParser();
        var data = fileConfigurationParser.Parse(".json", fileStream);

        Assert.NotNull(data);
        Assert.Single(data);
        Assert.Equal("Name", data.Keys.ElementAt(0));
        Assert.Equal("Furion", data.Values.ElementAt(0));
    }

    [Fact]
    public void Parse_Additional()
    {
        var fileConfigurationParser = new FileConfigurationParser();

        fileConfigurationParser.AddParser(".xml", stream => XmlStreamConfigurationProvider.Read(stream, XmlDocumentDecryptor.Instance));
        fileConfigurationParser.AddParser(".ini", IniStreamConfigurationProvider.Read);

        var xmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "config.xml");
        using var xmlFileStream = File.OpenRead(xmlFilePath);

        var xmlData = fileConfigurationParser.Parse(".xml", xmlFileStream);

        Assert.NotNull(xmlData);
        Assert.Single(xmlData);
        Assert.Equal("Name", xmlData.Keys.ElementAt(0));
        Assert.Equal("Furion", xmlData.Values.ElementAt(0));

        var iniFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "config.ini");
        using var iniFileStream = File.OpenRead(iniFilePath);

        var iniData = fileConfigurationParser.Parse(".ini", iniFileStream);

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
            FileConfigurationParser.EnsureLegalExtension(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            FileConfigurationParser.EnsureLegalExtension(string.Empty);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            FileConfigurationParser.EnsureLegalExtension("xml");
        });
        Assert.Equal("The `xml` is not a valid file extension. (Parameter 'extension')", exception.Message);
    }

    [Fact]
    public void EnsureLegalExtension_ReturnOK()
    {
        FileConfigurationParser.EnsureLegalExtension(".xml");
    }

    [Fact]
    public void ResolveJsonParser()
    {
        Assert.Null(FileConfigurationParser._jsonParser);

        var @delegate = FileConfigurationParser.ResolveJsonParser();

        Assert.NotNull(@delegate);
        Assert.NotNull(FileConfigurationParser._jsonParser);
    }

    public void Dispose()
    {
        FileConfigurationParser._jsonParser = null;
    }
}