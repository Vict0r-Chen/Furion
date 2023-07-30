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

public class FluentValidatorScannerTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var fluentValidatorScanner = new FluentValidatorScanner(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var fluentValidatorScanner = new FluentValidatorScanner(new ServiceCollection(), null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var services = new ServiceCollection();
        var fluentValidationBuilder = new FluentValidationBuilder();

        var fluentValidatorScanner = new FluentValidatorScanner(services, fluentValidationBuilder);

        Assert.NotNull(fluentValidatorScanner._services);
        Assert.NotNull(fluentValidatorScanner._fluentValidationBuilder);
        Assert.Single(fluentValidatorScanner._fluentValidationBuilder._assemblies);
    }

    [Fact]
    public void Initialize_ReturnOK()
    {
        var services = new ServiceCollection();
        var fluentValidationBuilder = new FluentValidationBuilder();

        var fluentValidatorScanner = new FluentValidatorScanner(services, fluentValidationBuilder);
        fluentValidatorScanner._fluentValidationBuilder._assemblies.Clear();

        fluentValidatorScanner.Initialize();
        Assert.Single(fluentValidatorScanner._fluentValidationBuilder._assemblies);
    }

    [Fact]
    public void ScanToAddServices()
    {
        var services = new ServiceCollection();
        var fluentValidationBuilder = new FluentValidationBuilder();
        fluentValidationBuilder.AddAssemblies(GetType().Assembly);
        var fluentValidatorScanner = new FluentValidatorScanner(services, fluentValidationBuilder);

        fluentValidatorScanner.ScanToAddServices();
        Assert.NotNull(services);
        Assert.NotEmpty(services);
    }

    [Fact]
    public void ScanToAddServices_SuppressAssemblyScanning()
    {
        var services = new ServiceCollection();
        var fluentValidationBuilder = new FluentValidationBuilder
        {
            SuppressAssemblyScanning = true
        };
        fluentValidationBuilder.AddAssemblies(GetType().Assembly);
        var fluentValidatorScanner = new FluentValidatorScanner(services, fluentValidationBuilder);

        fluentValidatorScanner.ScanToAddServices();
        Assert.NotNull(services);
        Assert.Empty(services);
    }

    [Fact]
    public void GetValidatorTypes_ReturnOK()
    {
        var services = new ServiceCollection();
        var fluentValidationBuilder = new FluentValidationBuilder();
        fluentValidationBuilder.AddAssemblies(GetType().Assembly);
        fluentValidationBuilder.AddTypeFilter(type => type != typeof(TestModelValidator) && type != typeof(SubModelValidator));

        var fluentValidatorScanner = new FluentValidatorScanner(services, fluentValidationBuilder);
        var validatorTypes = fluentValidatorScanner.GetValidatorTypes();

        Assert.Equal(3, validatorTypes.Count());
        Assert.Equal(typeof(FluentModelValidator), validatorTypes.ElementAt(0));
        Assert.Equal(typeof(FluentModelValidator2), validatorTypes.ElementAt(1));
        Assert.Equal(typeof(FluentModelValidator3), validatorTypes.ElementAt(2));
    }

    [Fact]
    public void GetValidatorTypes_WithSuppressNonPublicType_ReturnOK()
    {
        var services = new ServiceCollection();
        var fluentValidationBuilder = new FluentValidationBuilder
        {
            SuppressNonPublicType = true
        };
        fluentValidationBuilder.AddAssemblies(GetType().Assembly);
        fluentValidationBuilder.AddTypeFilter(type => type != typeof(TestModelValidator) && type != typeof(SubModelValidator));

        var fluentValidatorScanner = new FluentValidatorScanner(services, fluentValidationBuilder);
        var validatorTypes = fluentValidatorScanner.GetValidatorTypes();

        Assert.Equal(2, validatorTypes.Count());
        Assert.Equal(typeof(FluentModelValidator), validatorTypes.ElementAt(0));
        Assert.Equal(typeof(FluentModelValidator2), validatorTypes.ElementAt(1));
    }

    [Fact]
    public void AddService_Invalid_Parameters()
    {
        var services = new ServiceCollection();
        var fluentValidationBuilder = new FluentValidationBuilder();
        var fluentValidatorScanner = new FluentValidatorScanner(services, fluentValidationBuilder);

        Assert.Throws<ArgumentNullException>(() =>
        {
            fluentValidatorScanner.AddService(null!);
        });
    }

    [Fact]
    public void AddService_ReturnOK()
    {
        var services = new ServiceCollection();
        var fluentValidationBuilder = new FluentValidationBuilder();
        var fluentValidatorScanner = new FluentValidatorScanner(services, fluentValidationBuilder);

        fluentValidatorScanner.AddService(typeof(FluentModelValidator));

        Assert.Equal(2, services.Count);
        Assert.Equal(typeof(IObjectValidator<FluentModel>), services.ElementAt(0).ServiceType);
        Assert.Equal(ServiceLifetime.Transient, services.ElementAt(0).Lifetime);
        Assert.Equal(typeof(IObjectValidator), services.ElementAt(1).ServiceType);
        Assert.Equal(ServiceLifetime.Transient, services.ElementAt(1).Lifetime);
    }
}