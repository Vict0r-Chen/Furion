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

public class RemotedConfigurationBuilderExtensionsTests
{
    [Fact]
    public void AddRemoted_Invalid_Parameters()
    {
        var configurationBuilder = new ConfigurationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            configurationBuilder.AddRemoted((RemotedConfigurationBuilder)null!);
        });
    }

    [Fact]
    public void AddRemoted_Emtpty()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddRemoted(new RemotedConfigurationBuilder());

        Assert.Empty(configurationBuilder.Sources);
    }

    [Fact]
    public async Task AddRemoted_ReturnOK()
    {
        var port = Helpers.GetIdlePort();
        var urls = new[] { "--urls", $"http://localhost:{port}" };
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

        var configurationBuilder = new ConfigurationBuilder();

        RemotedConfigurationBuilder? remotedConfigurationBuilder = new RemotedConfigurationBuilder();
        remotedConfigurationBuilder.AddUrlAddresses($"http://localhost:{port}/return-json");
        configurationBuilder.AddRemoted(remotedConfigurationBuilder);

        Assert.NotEmpty(configurationBuilder.Sources);
        Assert.Single(configurationBuilder.Sources);

        var configuration = configurationBuilder.Build();

        Assert.Equal("Furion", configuration["Name"]);
        Assert.Equal("3", configuration["Age"]);
    }

    [Fact]
    public void AddRemotedAction_Empty_Parameters()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddRemoted();
        Assert.Empty(configurationBuilder.Sources);
    }

    [Fact]
    public async Task AddRemotedAction_ReturnOK()
    {
        var port = Helpers.GetIdlePort();
        var urls = new[] { "--urls", $"http://localhost:{port}" };
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

        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.AddRemoted(builder =>
        {
            builder.AddUrlAddresses($"http://localhost:{port}/return-json");
        });

        Assert.NotEmpty(configurationBuilder.Sources);
        Assert.Single(configurationBuilder.Sources);

        var configuration = configurationBuilder.Build();

        Assert.Equal("Furion", configuration["Name"]);
        Assert.Equal("3", configuration["Age"]);
    }
}