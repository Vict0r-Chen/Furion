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

namespace Furion.DependencyInjection.Tests;

public class AddNamedTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void AddNamed_EmptyName_ReturnOops(string? name)
    {
        var services = new ServiceCollection();

        if (name == null)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                services.AddNamed(name: name!, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
            });
        }

        if (name == string.Empty)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                services.AddNamed(name!, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
            });
        }
    }

    [Fact]
    public void AddNamed_EmptyServiceDescriptor_ReturnOops()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentNullException>(() =>
        {
            services.AddNamed("name1", null!);
        });
    }

    [Theory]
    [InlineData("name1", "name2")]
    [InlineData("name3", "name4")]
    public void AddNamed_NotRepeatName_ReturnOK(string name1, string name2)
    {
        var services = new ServiceCollection();
        services.AddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.AddNamed(name2, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var count = services.Count;
        Assert.Equal(3, count);
    }

    [Fact]
    public void AddNamed_RepeatName_ReturnOK()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass2, NamedClass2>());

        var count = services.Count;
        Assert.Equal(4, count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TryAddNamed_EmptyName_ReturnOops(string? name)
    {
        var services = new ServiceCollection();

        if (name == null)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                services.TryAddNamed(name!, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
            });
        }

        if (name == string.Empty)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                services.TryAddNamed(name!, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
            });
        }
    }

    [Fact]
    public void TryAddNamed_EmptyServiceDescriptor_ReturnOops()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentNullException>(() =>
        {
            services.TryAddNamed("name1", null!);
        });
    }

    [Theory]
    [InlineData("name1", "name2")]
    [InlineData("name3", "name4")]
    public void TryAddNamed_NotRepeatName_ReturnOK(string name1, string name2)
    {
        var services = new ServiceCollection();
        services.TryAddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.TryAddNamed(name2, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var count = services.Count;
        Assert.Equal(3, count);
    }

    [Fact]
    public void TryAddNamed_RepeatName_ReturnOK()
    {
        var name1 = "name1";
        var services = new ServiceCollection();
        services.TryAddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.TryAddNamed(name1, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.TryAddNamed(name1, ServiceDescriptor.Scoped<INamedClass2, NamedClass2>());

        var count = services.Count;
        Assert.Equal(3, count);
    }
}