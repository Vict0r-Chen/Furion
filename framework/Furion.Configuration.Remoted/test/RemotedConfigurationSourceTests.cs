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

public class RemotedConfigurationSourceTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var remotedConfigurationSource = new RemotedConfigurationSource(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var remotedConfigurationSource = new RemotedConfigurationSource([], null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var remotedConfigurationSource = new RemotedConfigurationSource([], new(new(), new Dictionary<string, string>()));

        Assert.NotNull(remotedConfigurationSource);
        Assert.NotNull(remotedConfigurationSource._remotedConfigurationModels);
        Assert.Empty(remotedConfigurationSource._remotedConfigurationModels);
        Assert.NotNull(remotedConfigurationSource._remotedConfigurationParser);
    }

    [Fact]
    public async Task Load()
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

        var remotedConfigurationModel = new RemotedConfigurationModel($"http://localhost:{port}/return-json", HttpMethod.Get);

        var remotedConfigurationSource = new RemotedConfigurationSource(
        [
            remotedConfigurationModel
        ], new(new(), new Dictionary<string, string>()));

        var configurationBuilder = new ConfigurationBuilder();
        var cbuilder = remotedConfigurationSource.Build(configurationBuilder);
        cbuilder.Load();

        var keys = cbuilder.GetChildKeys(Enumerable.Empty<string>(), null);

        Assert.NotNull(keys);
        Assert.Equal(2, keys.Count());

        Assert.Equal("age", keys.ElementAt(0));
        Assert.Equal("name", keys.ElementAt(1));
    }
}