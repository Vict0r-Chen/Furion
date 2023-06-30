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

namespace Furion.Configuration.ManifestResource.Tests;

public class FileConfigurationParserTests
{
    [Fact]
    public void NewInstance_Default()
    {
        var fileConfigurationParser = new FileConfigurationParser();

        Assert.NotNull(fileConfigurationParser);
        Assert.NotNull(fileConfigurationParser._parsers);
        Assert.NotEmpty(fileConfigurationParser._parsers);
        Assert.Equal(3, fileConfigurationParser._parsers.Count);

        Assert.Equal(".json", fileConfigurationParser._parsers.Keys.ElementAt(0));
        Assert.Equal(".xml", fileConfigurationParser._parsers.Keys.ElementAt(1));
        Assert.Equal(".ini", fileConfigurationParser._parsers.Keys.ElementAt(2));

        Assert.NotNull(fileConfigurationParser._parsers.Values.ElementAt(0));
        Assert.NotNull(fileConfigurationParser._parsers.Values.ElementAt(1));
        Assert.NotNull(fileConfigurationParser._parsers.Values.ElementAt(2));
    }

    [Fact]
    public void CreateJsonParser()
    {
        var fileConfigurationParser = new FileConfigurationParser();
        var jsonParser = fileConfigurationParser.CreateJsonParser();
        Assert.NotNull(jsonParser);
    }

    [Fact]
    public void Parse_Throw()
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
            fileConfigurationParser.Parse(".json", null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            using var stream = new MemoryStream();
            fileConfigurationParser.Parse("json", stream);
        });
        Assert.Equal("`json` is not a valid file extension. (Parameter 'extension')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            using var stream = new MemoryStream();
            fileConfigurationParser.Parse(".yaml", stream);
        });
        Assert.Equal("Parser for configuration files with `.yaml` extension not found. (Parameter 'extension')", exception2.Message);
    }

    [Fact]
    public void Parse_ReturnOK()
    {
        var fileConfigurationParser = new FileConfigurationParser();

        using var stream = GetType().Assembly.GetManifestResourceStream("Furion.Configuration.ManifestResource.Tests.Resources.embed.json");
        Assert.NotNull(stream);
        Assert.True(stream.Length > 0);

        var data = fileConfigurationParser.Parse(".json", stream);
        Assert.NotNull(data);
        Assert.Single(data);
    }
}