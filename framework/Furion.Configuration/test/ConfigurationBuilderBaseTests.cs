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

public class ConfigurationBuilderBaseTests
{
    [Fact]
    public void New_Default()
    {
        var configurationBuilder = new TestConfigurationBuilder();

        Assert.NotNull(configurationBuilder);

        Assert.NotNull(configurationBuilder._parsers);
        Assert.Empty(configurationBuilder._parsers);
        Assert.NotNull(configurationBuilder._sourceTypes);
        Assert.Empty(configurationBuilder._sourceTypes);
    }

    [Fact]
    public void AddParser_Invalid_Parameters()
    {
        var configurationBuilder = new TestConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            configurationBuilder.AddParser(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configurationBuilder.AddParser(string.Empty, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configurationBuilder.AddParser("", null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            configurationBuilder.AddParser(".json", null!);
        });
    }

    [Fact]
    public void AddParser_ReturnOK()
    {
        var configurationBuilder = new TestConfigurationBuilder();

        configurationBuilder.AddParser(".json", stream => new Dictionary<string, string?>());

        Assert.NotEmpty(configurationBuilder._parsers);
        Assert.Single(configurationBuilder._parsers);
    }

    [Fact]
    public void AddSource_Invalid_Parameters()
    {
        var configurationBuilder = new TestConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            configurationBuilder.AddSource(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configurationBuilder.AddSource(string.Empty, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configurationBuilder.AddSource("", null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            configurationBuilder.AddSource(".json", null!);
        });
    }

    [Fact]
    public void AddSource_ReturnOK()
    {
        var configurationBuilder = new TestConfigurationBuilder();

        configurationBuilder.AddSource(".json", typeof(JsonConfigurationSource));

        Assert.NotEmpty(configurationBuilder._sourceTypes);
        Assert.Single(configurationBuilder._sourceTypes);
    }

    [Fact]
    public void Initialize_Invalid_Parameters()
    {
        var configurationBuilder = new TestConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            configurationBuilder.Initialize(null!);
        });
    }

    [Fact]
    public void Initialize_ReturnOK()
    {
        var configurationBuilder = new TestConfigurationBuilder();
        configurationBuilder.AddParser(".xml", stream => XmlStreamConfigurationProvider.Read(stream, XmlDocumentDecryptor.Instance));
        configurationBuilder.AddSource(".xml", typeof(XmlConfigurationSource));

        var fileConfigurationParser = new FileConfigurationParser();

        Assert.Single(fileConfigurationParser._parsers);
        Assert.Single(fileConfigurationParser._sourceTypes);

        configurationBuilder.Initialize(fileConfigurationParser);

        Assert.Equal(2, fileConfigurationParser._parsers.Count);
        Assert.Equal(2, fileConfigurationParser._sourceTypes.Count);

        Assert.Empty(configurationBuilder._parsers);
        Assert.Empty(configurationBuilder._sourceTypes);
    }
}