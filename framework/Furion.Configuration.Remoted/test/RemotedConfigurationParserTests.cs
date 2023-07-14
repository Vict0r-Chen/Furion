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

namespace Furion.Configuration.Remoted.Tests;

public class RemotedConfigurationParserTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var remotedConfigurationParser = new RemotedConfigurationParser(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var remotedConfigurationParser = new RemotedConfigurationParser(new(), null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var remotedConfigurationParser = new RemotedConfigurationParser(new(), new Dictionary<string, string>());

        Assert.NotNull(remotedConfigurationParser);
        Assert.NotNull(remotedConfigurationParser._fileConfigurationParser);
        Assert.NotNull(remotedConfigurationParser._contentTypeMappings);

        Assert.Equal(5, remotedConfigurationParser._contentTypeMappings.Count);

        var contentTypeMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"application/json", ".json" },
            {"application/vnd.api+json", ".json" },
            {"application/x-json", ".json" },
            {"text/json", ".json" },
            {"text/x-json", ".json" }
        };

        Assert.Equal(contentTypeMappings, remotedConfigurationParser._contentTypeMappings);

        var remotedConfigurationParser2 = new RemotedConfigurationParser(new(), new Dictionary<string, string>
        {
            { "text/xml", ".xml" }
        });

        Assert.Equal(6, remotedConfigurationParser2._contentTypeMappings.Count);
        contentTypeMappings.Add("text/xml", ".xml");

        Assert.Equal(contentTypeMappings, remotedConfigurationParser2._contentTypeMappings);
    }

    [Fact]
    public void ReadAsStream_Invalid_Parameters()
    {
        var remotedConfigurationParser = new RemotedConfigurationParser(new(), new Dictionary<string, string>());

        Assert.Throws<ArgumentNullException>(() =>
        {
            remotedConfigurationParser.ReadAsStream(null!, out var extension);
        });
    }

    [Fact]
    public async Task ReadAsStream_NotDefine_ContentType()
    {
        var remotedConfigurationParser = new RemotedConfigurationParser(new(), new Dictionary<string, string>());

        var urls = new[] { "--urls", "http://localhost:5001" };
        var builder = WebApplication.CreateBuilder(urls);
        await using var app = builder.Build();

        app.MapGet("/not-content-type", async context =>
        {
            await context.Response.WriteAsync("text");
        });

        app.MapGet("/not-support-content-type", async context =>
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("text");
        });

        await app.StartAsync();

        var remotedConfigurationModel = new RemotedConfigurationModel("http://localhost:5001/not-found", HttpMethod.Get);
        var exception = Assert.Throws<HttpRequestException>(() =>
        {
            var stream = remotedConfigurationParser.ReadAsStream(remotedConfigurationModel, out var extension);
        });

        var remotedConfigurationModel1 = new RemotedConfigurationModel("http://localhost:5001/not-content-type", HttpMethod.Get);
        var exception1 = Assert.Throws<InvalidOperationException>(() =>
        {
            var stream = remotedConfigurationParser.ReadAsStream(remotedConfigurationModel1, out var extension);
        });

        Assert.Equal("Content-Type definition not found in the response message.", exception1.Message);

        var remotedConfigurationModel2 = new RemotedConfigurationModel("http://localhost:5001/not-support-content-type", HttpMethod.Get);
        var exception2 = Assert.Throws<NotSupportedException>(() =>
        {
            var stream = remotedConfigurationParser.ReadAsStream(remotedConfigurationModel2, out var extension);
        });

        Assert.Equal("`text/plain` is not a supported Content-Type type.", exception2.Message);
    }

    [Fact]
    public async Task ReadAsStream_ReturnOK()
    {
        var remotedConfigurationParser = new RemotedConfigurationParser(new(), new Dictionary<string, string>());

        var urls = new[] { "--urls", "http://localhost:5001" };
        var builder = WebApplication.CreateBuilder(urls);
        await using var app = builder.Build();

        app.MapGet("/return-json", async context =>
        {
            await context.Response.WriteAsJsonAsync(new
            {
                Name = "Furion",
                Age = 3
            });
        });

        await app.StartAsync();

        var remotedConfigurationModel = new RemotedConfigurationModel("http://localhost:5001/return-json", HttpMethod.Get);
        using var stream = remotedConfigurationParser.ReadAsStream(remotedConfigurationModel, out var extension);

        Assert.NotNull(stream);
        Assert.True(stream.Length > 0);
        Assert.NotNull(extension);
        Assert.Equal(".json", extension);
    }

    [Fact]
    public async Task ReadAsStream_ConfigureClient_ReturnOK()
    {
        var remotedConfigurationParser = new RemotedConfigurationParser(new(), new Dictionary<string, string>());

        var urls = new[] { "--urls", "http://localhost:5001" };
        var builder = WebApplication.CreateBuilder(urls);
        await using var app = builder.Build();

        app.MapGet("/return-json", async context =>
        {
            await context.Response.WriteAsJsonAsync(new
            {
                Name = "Furion",
                Age = 3
            });
        });

        await app.StartAsync();

        var remotedConfigurationModel = new RemotedConfigurationModel("/return-json", HttpMethod.Get);
        remotedConfigurationModel.ConfigureClient(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5001");
        });

        using var stream = remotedConfigurationParser.ReadAsStream(remotedConfigurationModel, out var extension);

        Assert.NotNull(stream);
        Assert.True(stream.Length > 0);
        Assert.NotNull(extension);
        Assert.Equal(".json", extension);
    }

    [Fact]
    public void ParseRequestUri_Invalid_Parameters()
    {
        var remotedConfigurationParser = new RemotedConfigurationParser(new(), new Dictionary<string, string>());

        Assert.Throws<ArgumentNullException>(() =>
        {
            var data = remotedConfigurationParser.ParseRequestUri(null!);
        });
    }

    [Fact]
    public async Task ParseRequestUri_ReturnOk()
    {
        var remotedConfigurationParser = new RemotedConfigurationParser(new(), new Dictionary<string, string>());

        var urls = new[] { "--urls", "http://localhost:5001" };
        var builder = WebApplication.CreateBuilder(urls);
        await using var app = builder.Build();

        app.MapGet("/return-json", async context =>
        {
            await context.Response.WriteAsJsonAsync(new
            {
                Name = "Furion",
                Age = 3
            });
        });

        await app.StartAsync();

        var remotedConfigurationModel = new RemotedConfigurationModel("http://localhost:5001/return-json", HttpMethod.Get);
        var data = remotedConfigurationParser.ParseRequestUri(remotedConfigurationModel);

        Assert.NotNull(data);
        Assert.True(data.Count > 0);

        Assert.Equal("name", data.Keys.ElementAt(0));
        Assert.Equal("Furion", data.Values.ElementAt(0));
        Assert.Equal("age", data.Keys.ElementAt(1));
        Assert.Equal("3", data.Values.ElementAt(1));

        Assert.Equal("Furion", data["Name"]);
        Assert.Equal("Furion", data["name"]);
    }

    [Fact]
    public async Task ParseRequestUri_WithPrefix_ReturnOk()
    {
        var remotedConfigurationParser = new RemotedConfigurationParser(new(), new Dictionary<string, string>());

        var urls = new[] { "--urls", "http://localhost:5001" };
        var builder = WebApplication.CreateBuilder(urls);
        await using var app = builder.Build();

        app.MapGet("/return-json", async context =>
        {
            await context.Response.WriteAsJsonAsync(new
            {
                Name = "Furion",
                Age = 3
            });
        });

        await app.StartAsync();

        var remotedConfigurationModel = new RemotedConfigurationModel("http://localhost:5001/return-json", HttpMethod.Get)
        {
            Prefix = "Remoted"
        };
        var data = remotedConfigurationParser.ParseRequestUri(remotedConfigurationModel);

        Assert.NotNull(data);
        Assert.True(data.Count > 0);

        Assert.Equal("Remoted:name", data.Keys.ElementAt(0));
        Assert.Equal("Furion", data.Values.ElementAt(0));
        Assert.Equal("Remoted:age", data.Keys.ElementAt(1));
        Assert.Equal("3", data.Values.ElementAt(1));

        Assert.Equal("Furion", data["Remoted:Name"]);
        Assert.Equal("Furion", data["Remoted:name"]);
        Assert.Equal("Furion", data["remoted:Name"]);
        Assert.Equal("Furion", data["remoted:name"]);
    }
}