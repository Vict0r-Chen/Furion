// 麻省理工学院许可证
//
// 版权所有 © 2020-2023 百小僧，百签科技（广东）有限公司
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

using Microsoft.Extensions.Configuration.Memory;
using System.Reflection;

namespace Furion.Configuration.Tests;

public class ConfigurationMetadataTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var configurationMetadata = new ConfigurationMetadata(null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"Furion:Name", "Furion" },
            {"Furion:Version", "5.0.0" },
            {"Furion:Homepage", "https://furion.net" }
        });
        var configuration = configurationBuilder.Build();

        var configurationMetadata = new ConfigurationMetadata(configuration.Providers.First());

        Assert.Equal(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, ConfigurationMetadata._bindingAttr);
        Assert.NotNull(configurationMetadata);
        Assert.NotNull(configurationMetadata.Provider);
        Assert.NotNull(configurationMetadata.Data);
        Assert.NotEmpty(configurationMetadata.Data);
        Assert.Equal(3, configurationMetadata.Data.Count);
        Assert.Null(configurationMetadata.Source);
        Assert.Null(configurationMetadata.Metadata);
    }

    [Fact]
    public void InitializeProperties_ReturnOK()
    {
        var builder = WebApplication.CreateBuilder();
        var webApplication = builder.Build();

        var configuration = (IConfigurationRoot)webApplication.Services.GetRequiredService<IConfiguration>();

        var configurationMetadata = new ConfigurationMetadata(configuration.Providers.OfType<MemoryConfigurationProvider>().Last());
        Assert.NotNull(configurationMetadata.Data);
        Assert.NotEmpty(configurationMetadata.Data);

        configurationMetadata.Data.Clear();
        configurationMetadata.InitializeProperties();
        Assert.Empty(configurationMetadata.Data);

        var configurationMetadata2 = new ConfigurationMetadata(new ChainedConfigurationProvider(new ChainedConfigurationSource
        {
            Configuration = builder.Configuration
        }));
        Assert.NotNull(configurationMetadata2.Data);
        Assert.NotEmpty(configurationMetadata2.Data);
        Assert.NotNull(configurationMetadata2.Metadata);
        Assert.NotEmpty(configurationMetadata2.Metadata);
    }

    [Fact]
    public void ConvertToJson_ReturnOK()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"Furion:Name", "Furion" },
            {"Furion:Version", "5.0.0" },
            {"Furion:Homepage", "https://furion.net" }
        });
        var configuration = configurationBuilder.Build();
        var configurationMetadata = new ConfigurationMetadata(configuration.Providers.First());

        var jsonString = configurationMetadata.ConvertToJson();
        Assert.Equal("{\"Furion\":{\"Name\":\"Furion\",\"Version\":\"5.0.0\",\"Homepage\":\"https://furion.net\"}}", jsonString);

        var jsonString2 = configurationMetadata.ConvertToJson(new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        Assert.Equal("{\r\n  \"Furion\": {\r\n    \"Name\": \"Furion\",\r\n    \"Version\": \"5.0.0\",\r\n    \"Homepage\": \"https://furion.net\"\r\n  }\r\n}", jsonString2);
    }
}