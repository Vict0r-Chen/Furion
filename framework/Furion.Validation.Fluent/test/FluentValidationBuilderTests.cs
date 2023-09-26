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

namespace Furion.Validation.Fluent.Tests;

public class FluentValidationBuilderTests
{
    [Fact]
    public void New_ReturnOK()
    {
        var fluentValidationBuilder = new FluentValidationBuilder();

        Assert.NotNull(fluentValidationBuilder);
        Assert.NotNull(fluentValidationBuilder._assemblies);
        Assert.Empty(fluentValidationBuilder._assemblies);
        Assert.NotNull(fluentValidationBuilder._validatorTypes);
        Assert.Empty(fluentValidationBuilder._validatorTypes);
        Assert.Null(fluentValidationBuilder._typeFilterConfigure);
        Assert.False(fluentValidationBuilder.SuppressAssemblyScanning);
        Assert.False(fluentValidationBuilder.SuppressNonPublicType);
    }

    [Fact]
    public void AddTypeFilter_Invalid_Parameters()
    {
        var fluentValidationBuilder = new FluentValidationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fluentValidationBuilder.AddTypeFilter(null!);
        });
    }

    [Fact]
    public void AddTypeFilter_ReturnOK()
    {
        var fluentValidationBuilder = new FluentValidationBuilder();
        fluentValidationBuilder.AddTypeFilter(model => true);

        Assert.NotNull(fluentValidationBuilder._typeFilterConfigure);
    }

    [Fact]
    public void AddAssemblies_Invalid_Paramters()
    {
        var fluentValidationBuilder = new FluentValidationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fluentValidationBuilder.AddAssemblies(null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fluentValidationBuilder.AddAssemblies([null!]);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            fluentValidationBuilder.AddAssemblies([GetType().Assembly, null!]);
        });
    }

    [Fact]
    public void AddAssemblies_ReturnOK()
    {
        var fluentValidationBuilder = new FluentValidationBuilder();
        fluentValidationBuilder.AddAssemblies(GetType().Assembly);
        fluentValidationBuilder.AddAssemblies(GetType().Assembly);

        Assert.NotEmpty(fluentValidationBuilder._assemblies);
        Assert.Single(fluentValidationBuilder._assemblies);
        Assert.Equal(GetType().Assembly, fluentValidationBuilder._assemblies.ElementAt(0));
    }

    [Fact]
    public void AddAssemblies_Enumerable_ReturnOK()
    {
        var fluentValidationBuilder = new FluentValidationBuilder();
        fluentValidationBuilder.AddAssemblies(new List<Assembly> { GetType().Assembly });
        fluentValidationBuilder.AddAssemblies(new List<Assembly> { GetType().Assembly });

        Assert.NotEmpty(fluentValidationBuilder._assemblies);
        Assert.Single(fluentValidationBuilder._assemblies);
        Assert.Equal(GetType().Assembly, fluentValidationBuilder._assemblies.ElementAt(0));
    }

    [Fact]
    public void AddValidator_Invalid_Parameters()
    {
        var fluentValidationBuilder = new FluentValidationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fluentValidationBuilder.AddValidator(null!);
        });

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            fluentValidationBuilder.AddValidator(typeof(FluentModelValidator5));
        });
        Assert.Equal("`Furion.Validation.Fluent.Tests.FluentModelValidator5` type is not assignable from `Furion.Validation.AbstractValidator`1[T]`.", exception.Message);

        var exception2 = Assert.Throws<InvalidOperationException>(() =>
        {
            fluentValidationBuilder.AddValidator(typeof(FluentModelValidator4));
        });
        Assert.Equal($"`Furion.Validation.Fluent.Tests.FluentModelValidator4` type must be able to be instantiated.", exception2.Message);
    }

    [Fact]
    public void AddValidator_ReturnOK()
    {
        var fluentValidationBuilder = new FluentValidationBuilder();
        fluentValidationBuilder.AddValidator(typeof(FluentModelValidator));

        Assert.Single(fluentValidationBuilder._validatorTypes);
        Assert.Equal(typeof(FluentModelValidator), fluentValidationBuilder._validatorTypes.First());
    }

    [Fact]
    public void AddValidatorGeneric_ReturnOK()
    {
        var fluentValidationBuilder = new FluentValidationBuilder();
        fluentValidationBuilder.AddValidator<FluentModelValidator>();

        Assert.Single(fluentValidationBuilder._validatorTypes);
        Assert.Equal(typeof(FluentModelValidator), fluentValidationBuilder._validatorTypes.First());
    }

    [Fact]
    public void Build_Invalid_Parameters()
    {
        var fluentValidationBuilder = new FluentValidationBuilder();

        Assert.Throws<ArgumentNullException>(() =>
        {
            fluentValidationBuilder.Build(null!);
        });
    }

    [Fact]
    public void Build()
    {
        var services = new ServiceCollection();
        var fluentValidationBuilder = new FluentValidationBuilder();
        fluentValidationBuilder.AddAssemblies(GetType().Assembly);

        fluentValidationBuilder.Build(services);

        Assert.NotEmpty(services);

        _ = services.BuildServiceProvider();
    }
}