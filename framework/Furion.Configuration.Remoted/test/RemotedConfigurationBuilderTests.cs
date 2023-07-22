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

namespace Furion.Configuration.Remoted.Tests;

#pragma warning disable

public class RemotedConfigurationBuilderTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();

        Assert.NotNull(remotedConfigurationBuilder);
        Assert.NotNull(remotedConfigurationBuilder._urlAddresses);
        Assert.Empty(remotedConfigurationBuilder._urlAddresses);
        Assert.NotNull(remotedConfigurationBuilder._mediaTypeMappings);
        Assert.Empty(remotedConfigurationBuilder._mediaTypeMappings);
        Assert.Null(remotedConfigurationBuilder._filterConfigure);
        Assert.Null(remotedConfigurationBuilder._defaultClientConfigurator);

        Assert.Equal(HttpMethod.Get, remotedConfigurationBuilder.DefaultHttpMethod);
        Assert.Equal(TimeSpan.FromSeconds(30), remotedConfigurationBuilder.DefaultTimeout);
        Assert.False(remotedConfigurationBuilder.DefaultOptional);
        Assert.Null(remotedConfigurationBuilder.DefaultPrefix);
    }

    [Fact]
    public void AddFilter_Invalid_Parameters()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            remotedConfigurationBuilder.AddFilter(null!);
        });
    }

    [Fact]
    public void AddFilter_ReturnOK()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        remotedConfigurationBuilder.AddFilter(model => true);

        Assert.NotNull(remotedConfigurationBuilder._filterConfigure);
    }

    [Fact]
    public void ConfigureClient_Invalid_Parameters()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            remotedConfigurationBuilder.ConfigureClient(null!);
        });
    }

    [Fact]
    public void ConfigureClient_ReturnOK()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        remotedConfigurationBuilder.ConfigureClient(client => { });

        Assert.NotNull(remotedConfigurationBuilder._defaultClientConfigurator);
    }

    [Fact]
    public void AddUrlAddresses_Invalid_Paramters()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            remotedConfigurationBuilder.AddUrlAddresses(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            remotedConfigurationBuilder.AddUrlAddresses(new string[] { null! });
        });

        Assert.Throws<ArgumentException>(() =>
        {
            remotedConfigurationBuilder.AddUrlAddresses(new string[] { string.Empty });
        });

        Assert.Throws<ArgumentException>(() =>
        {
            remotedConfigurationBuilder.AddUrlAddresses(new string[] { "" });
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            remotedConfigurationBuilder.AddUrlAddresses(new string[] { "https://furion.net", null! });
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            remotedConfigurationBuilder.AddUrlAddresses(new string[] { "ftp://error" });
        });
        Assert.Equal("The given address `ftp://error` is invalid. (Parameter 'urlAddress')", exception.Message);
    }

    [Fact]
    public void AddUrlAddresses_ReturnOK()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        remotedConfigurationBuilder.AddUrlAddresses("https://furion.net", "http://furion.net", "/get");
        remotedConfigurationBuilder.AddUrlAddresses("https://furion.net");

        Assert.NotEmpty(remotedConfigurationBuilder._urlAddresses);
        Assert.Equal(3, remotedConfigurationBuilder._urlAddresses.Count);
        Assert.Equal("https://furion.net", remotedConfigurationBuilder._urlAddresses.ElementAt(0));
        Assert.Equal("http://furion.net", remotedConfigurationBuilder._urlAddresses.ElementAt(1));
        Assert.Equal("/get", remotedConfigurationBuilder._urlAddresses.ElementAt(2));
    }

    [Fact]
    public void AddUrlAddresses_Enumerable_ReturnOK()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        remotedConfigurationBuilder.AddUrlAddresses(new List<string> { "https://furion.net", "http://furion.net", "/get" });
        remotedConfigurationBuilder.AddUrlAddresses("https://furion.net");

        Assert.NotEmpty(remotedConfigurationBuilder._urlAddresses);
        Assert.Equal(3, remotedConfigurationBuilder._urlAddresses.Count);
        Assert.Equal("https://furion.net", remotedConfigurationBuilder._urlAddresses.ElementAt(0));
        Assert.Equal("http://furion.net", remotedConfigurationBuilder._urlAddresses.ElementAt(1));
        Assert.Equal("/get", remotedConfigurationBuilder._urlAddresses.ElementAt(2));
    }

    [Fact]
    public void AddMediaTypeMapping_Invalid_Parameters()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            remotedConfigurationBuilder.AddMediaTypeMapping(null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            remotedConfigurationBuilder.AddMediaTypeMapping(string.Empty, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            remotedConfigurationBuilder.AddMediaTypeMapping("", null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            remotedConfigurationBuilder.AddMediaTypeMapping("application/json", null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            remotedConfigurationBuilder.AddMediaTypeMapping("application/json", string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            remotedConfigurationBuilder.AddMediaTypeMapping("application/json", "");
        });
    }

    [Fact]
    public void AddMediaTypeMapping_ReturnOK()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        remotedConfigurationBuilder.AddMediaTypeMapping("application/json", ".json");
        remotedConfigurationBuilder.AddMediaTypeMapping("text/xml", ".xml");

        Assert.Equal(2, remotedConfigurationBuilder._mediaTypeMappings.Count);
        Assert.Equal("application/json", remotedConfigurationBuilder._mediaTypeMappings.Keys.ElementAt(0));
        Assert.Equal(".json", remotedConfigurationBuilder._mediaTypeMappings.Values.ElementAt(0));
        Assert.Equal("text/xml", remotedConfigurationBuilder._mediaTypeMappings.Keys.ElementAt(1));
        Assert.Equal(".xml", remotedConfigurationBuilder._mediaTypeMappings.Values.ElementAt(1));
    }

    [Fact]
    public void SetClientConfigurator_Invalid_Parameters()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            remotedConfigurationBuilder.SetClientConfigurator(null!);
        });
    }

    [Fact]
    public void SetClientConfigurator_ReturnOK()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        var remotedConfigurationModel = new RemotedConfigurationModel("https://furion.net", HttpMethod.Get);

        remotedConfigurationBuilder.SetClientConfigurator(remotedConfigurationModel);
        Assert.Null(remotedConfigurationModel.ClientConfigurator);

        var count = 0;
        remotedConfigurationBuilder.ConfigureClient(client =>
        {
            count++;
        });

        remotedConfigurationBuilder.SetClientConfigurator(remotedConfigurationModel);
        Assert.NotNull(remotedConfigurationModel.ClientConfigurator);

        using var httpClient = new HttpClient();
        remotedConfigurationModel.ClientConfigurator(httpClient);

        Assert.Equal(1, count);

        count = 0;
        remotedConfigurationModel.ConfigureClient(client =>
        {
            count++;
        });
        remotedConfigurationBuilder.SetClientConfigurator(remotedConfigurationModel);

        remotedConfigurationModel.ClientConfigurator(httpClient);

        Assert.Equal(2, count);
    }

    [Fact]
    public void CreateModels()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        remotedConfigurationBuilder.AddUrlAddresses(new List<string> { "https://furion.net", "/get" });

        var models = remotedConfigurationBuilder.CreateModels().ToList();
        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Equal(2, models.Count);
    }

    [Fact]
    public void CreateModels_WithFilter()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        remotedConfigurationBuilder.AddUrlAddresses(new List<string> { "https://furion.net", "/get" });
        remotedConfigurationBuilder.AddFilter(model =>
        {
            return model.ToString() == "/get";
        });

        var models = remotedConfigurationBuilder.CreateModels().ToList();
        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Single(models);
        Assert.Equal("/get", models.First().ToString());
    }

    [Fact]
    public void CreateModels_WithDefaultPrefix()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        remotedConfigurationBuilder.AddUrlAddresses(new List<string> { "https://furion.net", "/get" });
        remotedConfigurationBuilder.DefaultPrefix = "Remoted";

        var models = remotedConfigurationBuilder.CreateModels().ToList();
        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Equal(2, models.Count);
        Assert.True(models.First().Prefix == "Remoted");
        Assert.True(models.Last().Prefix == "Remoted");
    }

    [Fact]
    public void Build_Empty()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        var models = remotedConfigurationBuilder.Build(out var remotedConfigurationParser);

        Assert.NotNull(models);
        Assert.Empty(models);
        Assert.Null(remotedConfigurationParser);
    }

    [Fact]
    public void Build_ReturnOK()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        remotedConfigurationBuilder.AddUrlAddresses(new List<string> { "https://furion.net", "/get" });
        var models = remotedConfigurationBuilder.Build(out var remotedConfigurationParser);

        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Equal(2, models.Count);
        Assert.NotNull(remotedConfigurationParser);
    }

    [Fact]
    public void Build_Order()
    {
        var remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        remotedConfigurationBuilder.AddUrlAddresses(new List<string> { "https://furion.net", "/get" });
        remotedConfigurationBuilder.AddFilter(model =>
        {
            if (model.ToString() == "https://furion.net")
            {
                model.Order = 1;
            }
            return true;
        });

        var models = remotedConfigurationBuilder.Build(out var remotedConfigurationParser);

        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Equal(2, models.Count);
        Assert.NotNull(remotedConfigurationParser);
        Assert.Equal("https://furion.net", models.Last().ToString());
    }

    [Fact]
    public void EnsureLegalUrlAddress_Invalid_Paramters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            RemotedConfigurationBuilder.EnsureLegalUrlAddress(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            RemotedConfigurationBuilder.EnsureLegalUrlAddress(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            RemotedConfigurationBuilder.EnsureLegalUrlAddress("");
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            RemotedConfigurationBuilder.EnsureLegalUrlAddress("ftp://error");
        });
        Assert.Equal("The given address `ftp://error` is invalid. (Parameter 'urlAddress')", exception.Message);
    }

    [Theory]
    [InlineData("https://furion.net")]
    [InlineData("http://furion.net")]
    [InlineData("/get")]
    [InlineData("get")]
    public void EnsureLegalUrlAddress_ReturnOK(string urlAddress)
    {
        RemotedConfigurationBuilder.EnsureLegalUrlAddress(urlAddress);
    }
}