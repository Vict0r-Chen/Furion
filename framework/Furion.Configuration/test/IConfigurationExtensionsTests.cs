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

public class IConfigurationExtensionsTests
{
    [Theory]
    [InlineData("Name", true)]
    [InlineData("Age", false)]
    [InlineData("Version", true)]
    [InlineData("Author:Name", true)]
    public void Exists(string key, bool exists)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "appsettings.json");
        Assert.True(File.Exists(filePath));

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile(filePath);
        var configuration = configurationBuilder.Build();

        Assert.NotNull(configuration);

        Assert.Equal(exists, configuration.Exists(key));
    }

    [Fact]
    public void GetOfT()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "appsettings.json");
        Assert.True(File.Exists(filePath));

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile(filePath);
        var configuration = configurationBuilder.Build();

        Assert.NotNull(configuration);

        Assert.Equal("Furion", configuration.Get<string>("Name"));
        Assert.Equal("5.0.0", configuration.Get<string>("Version"));
        Assert.Equal("百小僧", configuration.Get<string>("Author:Name"));
        Assert.Equal(31, configuration.Get<int>("Author:Age"));
        Assert.True(configuration.Get<bool>("IsMIT"));
        Assert.Equal(DateTime.Parse("2020.09.01"), configuration.Get<DateTime>("Birthday"));
        var arr = new[] { "Furion", "Fur", "百小僧", "MonkSoul" };
        Assert.Equal(arr, configuration.Get<string[]>("Tags"));
        Assert.Equal(0, configuration.Get<decimal>("Price"));

        Assert.Null(configuration.Get<string>("Unknown"));

        var furionAuthor = configuration.Get<FurionAuthor>("Author");
        Assert.NotNull(furionAuthor);
        Assert.Equal("百小僧", furionAuthor.Name);
        Assert.Equal(0, furionAuthor.Age);
        Assert.Null(furionAuthor.Unknown);
    }

    [Fact]
    public void GetOfT_With_Action()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "appsettings.json");
        Assert.True(File.Exists(filePath));

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile(filePath);
        var configuration = configurationBuilder.Build();

        Assert.NotNull(configuration);

        var furionAuthor = configuration.Get<FurionAuthor>("Author", options =>
        {
            options.BindNonPublicProperties = true;
        });
        Assert.NotNull(furionAuthor);
        Assert.Equal("百小僧", furionAuthor.Name);
        Assert.Equal(31, furionAuthor.Age);
        Assert.Null(furionAuthor.Unknown);

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var furionAuthor2 = configuration.Get<FurionAuthor>("Author", options =>
            {
                options.BindNonPublicProperties = true;
                options.ErrorOnUnknownConfiguration = true;
            });
        });

        Assert.Equal("'ErrorOnUnknownConfiguration' was set on the provided BinderOptions, but the following properties were not found on the instance of Furion.Configuration.Tests.FurionAuthor: 'Email'", exception.Message);
    }

    [Fact]
    public void GetOfType()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "appsettings.json");
        Assert.True(File.Exists(filePath));

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile(filePath);
        var configuration = configurationBuilder.Build();

        Assert.NotNull(configuration);

        Assert.Equal("Furion", configuration.Get("Name", typeof(string)));
        Assert.Equal("5.0.0", configuration.Get("Version", typeof(string)));
        Assert.Equal("百小僧", configuration.Get("Author:Name", typeof(string)));
        Assert.Equal(31, configuration.Get("Author:Age", typeof(int)));
        Assert.Equal(true, configuration.Get("IsMIT", typeof(bool)));
        Assert.Equal(DateTime.Parse("2020.09.01"), configuration.Get("Birthday", typeof(DateTime)));
        var arr = new[] { "Furion", "Fur", "百小僧", "MonkSoul" };
        Assert.Equal(arr, configuration.Get("Tags", typeof(string[])));
        Assert.Equal("0.00", configuration.Get("Price", typeof(decimal))!.ToString());

        Assert.Null(configuration.Get("Unknown", typeof(string)));

        var furionAuthor = configuration.Get("Author", typeof(FurionAuthor)) as FurionAuthor;
        Assert.NotNull(furionAuthor);
        Assert.Equal("百小僧", furionAuthor.Name);
        Assert.Equal(0, furionAuthor.Age);
        Assert.Null(furionAuthor.Unknown);
    }

    [Fact]
    public void GetOfType_With_Action()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "appsettings.json");
        Assert.True(File.Exists(filePath));

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile(filePath);
        var configuration = configurationBuilder.Build();

        Assert.NotNull(configuration);

        var furionAuthor = configuration.Get("Author", typeof(FurionAuthor), options =>
        {
            options.BindNonPublicProperties = true;
        }) as FurionAuthor;
        Assert.NotNull(furionAuthor);
        Assert.Equal("百小僧", furionAuthor.Name);
        Assert.Equal(31, furionAuthor.Age);
        Assert.Null(furionAuthor.Unknown);

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var furionAuthor2 = configuration.Get("Author", typeof(FurionAuthor), options =>
            {
                options.BindNonPublicProperties = true;
                options.ErrorOnUnknownConfiguration = true;
            });
        });

        Assert.Equal("'ErrorOnUnknownConfiguration' was set on the provided BinderOptions, but the following properties were not found on the instance of Furion.Configuration.Tests.FurionAuthor: 'Email'", exception.Message);
    }

    [Fact]
    public void Reload()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "appsettings.json");
        Assert.True(File.Exists(filePath));

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile(filePath);
        var configuration = configurationBuilder.Build();

        Assert.NotNull(configuration);
        configuration.Reload();
    }
}