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

public class AutowiredMemberActivatorTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var autowiredMemberActivator = new AutowiredMemberActivator();

        Assert.NotNull(autowiredMemberActivator);
        Assert.NotNull(autowiredMemberActivator._typeAutowiredPropertiesCache);
        Assert.Empty(autowiredMemberActivator._typeAutowiredPropertiesCache);
        Assert.NotNull(autowiredMemberActivator._typeAutowiredFieldsCache);
        Assert.Empty(autowiredMemberActivator._typeAutowiredFieldsCache);
        Assert.NotNull(autowiredMemberActivator._getAutowiredPropertiesFactory);
        Assert.NotNull(autowiredMemberActivator._getAutowiredFieldsFactory);
    }

    [Fact]
    public void GetBindingFlags()
    {
        var autowiredMemberActivator = new AutowiredMemberActivator();
        var bindingFlags = autowiredMemberActivator.GetBindingFlags();

        Assert.Equal(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic, bindingFlags);
    }

    [Fact]
    public void AutowriedProperties_Invalid_Parameters()
    {
        var autowiredMemberActivator = new AutowiredMemberActivator();

        Assert.Throws<ArgumentNullException>(() =>
        {
            autowiredMemberActivator.AutowriedProperties(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            autowiredMemberActivator.AutowriedProperties(new AutowiredService(), null!);
        });
    }

    [Fact]
    public void AutowriedProperties_ReturnOK()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        var autowiredMemberActivator = new AutowiredMemberActivator();
        var autowiredService = new AutowiredService();

        autowiredMemberActivator.AutowriedProperties(autowiredService, app.Services);

        Assert.NotNull(autowiredService.Configuration);
        Assert.NotNull(autowiredService.NotPublicConfiguration);
        Assert.Null(autowiredService.BaseConfiguration);

        Assert.NotEmpty(autowiredMemberActivator._typeAutowiredPropertiesCache);
    }

    [Theory]
    [InlineData(typeof(ReadonlyPropertyAutowiredService))]
    public void AutowriedProperties_Readonly_Throw(Type type)
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        var autowiredMemberActivator = new AutowiredMemberActivator();
        var instance = Activator.CreateInstance(type);
        Assert.NotNull(instance);

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            autowiredMemberActivator.AutowriedProperties(instance, app.Services);
        });

        Assert.Equal($"Cannot automatically assign read-only property `{nameof(ReadonlyPropertyAutowiredService.Configuration)}` of type `{typeof(ReadonlyPropertyAutowiredService)}`.", exception.Message);
    }

    [Fact]
    public void AutowriedFields_Invalid_Parameters()
    {
        var autowiredMemberActivator = new AutowiredMemberActivator();

        Assert.Throws<ArgumentNullException>(() =>
        {
            autowiredMemberActivator.AutowriedFields(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            autowiredMemberActivator.AutowriedFields(new AutowiredService(), null!);
        });
    }

    [Fact]
    public void AutowriedFields_ReturnOK()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        var autowiredMemberActivator = new AutowiredMemberActivator();
        var autowiredService = new AutowiredService();

        autowiredMemberActivator.AutowriedFields(autowiredService, app.Services);

        Assert.NotNull(autowiredService._services);
        Assert.NotNull(autowiredService._notPublicServices);
        Assert.Null(autowiredService._baseServices);

        Assert.NotEmpty(autowiredMemberActivator._typeAutowiredFieldsCache);
    }

    [Theory]
    [InlineData(typeof(ReadonlyFiedAutowiredService))]
    public void AutowriedFields_Readonly_Throw(Type type)
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        var autowiredMemberActivator = new AutowiredMemberActivator();
        var instance = Activator.CreateInstance(type);
        Assert.NotNull(instance);

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            autowiredMemberActivator.AutowriedFields(instance, app.Services);
        });

        Assert.Equal($"Cannot automatically assign read-only field `{nameof(ReadonlyFiedAutowiredService._services)}` of type `{typeof(ReadonlyFiedAutowiredService)}`.", exception.Message);
    }

    [Fact]
    public void AutowiredMembers_Invalid_Parameters()
    {
        var autowiredMemberActivator = new AutowiredMemberActivator();

        Assert.Throws<ArgumentNullException>(() =>
        {
            autowiredMemberActivator.AutowiredMembers(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            autowiredMemberActivator.AutowiredMembers(new AutowiredService(), null!);
        });
    }

    [Fact]
    public void AutowiredMembers_ReturnOK()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        var autowiredMemberActivator = new AutowiredMemberActivator();
        var autowiredService = new AutowiredService();

        autowiredMemberActivator.AutowiredMembers(autowiredService, app.Services);

        Assert.NotNull(autowiredService.Configuration);
        Assert.NotNull(autowiredService.NotPublicConfiguration);
        Assert.NotNull(autowiredService._services);
        Assert.NotNull(autowiredService._notPublicServices);
        Assert.Null(autowiredService.BaseConfiguration);
        Assert.Null(autowiredService._baseServices);

        Assert.NotEmpty(autowiredMemberActivator._typeAutowiredPropertiesCache);
        Assert.NotEmpty(autowiredMemberActivator._typeAutowiredFieldsCache);
    }

    [Fact]
    public void AutowiredMembers_CanBeNull_IsFalse()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var autowiredMemberActivator = new AutowiredMemberActivator();
        var autowiredService = new NotRegisterAutowiredService();

        Assert.Throws<InvalidOperationException>(() =>
        {
            autowiredMemberActivator.AutowiredMembers(autowiredService, serviceProvider);
        });
    }

    [Fact]
    public void AutowiredMembers_CanBeNull_IsTrue()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var autowiredMemberActivator = new AutowiredMemberActivator();
        var autowiredService = new AllowNotRegisterAutowiredService();

        autowiredMemberActivator.AutowiredMembers(autowiredService, serviceProvider);

        Assert.Null(autowiredService.Configuration);
    }
}