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

namespace Furion.DependencyInjection.TypeScanning.Tests;

public class ExposeServicesAttributeTests
{
    [Fact]
    public void AttributeUsage_Definition()
    {
        var attributeUsageAttribute = typeof(ExposeServicesAttribute).GetCustomAttribute<AttributeUsageAttribute>();

        Assert.NotNull(attributeUsageAttribute);
        Assert.Equal(AttributeTargets.Class, attributeUsageAttribute.ValidOn);
        Assert.False(attributeUsageAttribute.AllowMultiple);
        Assert.True(attributeUsageAttribute.Inherited);
    }

    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var exposeServicesAttribute = new ExposeServicesAttribute(null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var exposeServicesAttribute = new ExposeServicesAttribute();
        Assert.NotNull(exposeServicesAttribute);
        Assert.NotNull(exposeServicesAttribute.ServiceTypes);
        Assert.Empty(exposeServicesAttribute.ServiceTypes);

        var exposeServicesAttribute1 = new ExposeServicesAttribute(typeof(ExportService1));

        Assert.NotNull(exposeServicesAttribute1);
        Assert.NotNull(exposeServicesAttribute1.ServiceTypes);
        Assert.NotEmpty(exposeServicesAttribute1.ServiceTypes);
    }

    [Fact]
    public void NewGeneric_ReturnOK()
    {
        var exposeServicesAttribute1 = new ExposeServicesAttribute<ExportService1>();
        var exposeServicesAttribute2 = new ExposeServicesAttribute<ExportService1, ExportService2>();
        var exposeServicesAttribute3 = new ExposeServicesAttribute<ExportService1, ExportService2, ExportService3>();
        var exposeServicesAttribute4 = new ExposeServicesAttribute<ExportService1, ExportService2, ExportService3, ExportService4>();
        var exposeServicesAttribute5 = new ExposeServicesAttribute<ExportService1, ExportService2, ExportService3, ExportService4, ExportService5>();
        var exposeServicesAttribute6 = new ExposeServicesAttribute<ExportService1, ExportService2, ExportService3, ExportService4, ExportService5, ExportService6>();
        var exposeServicesAttribute7 = new ExposeServicesAttribute<ExportService1, ExportService2, ExportService3, ExportService4, ExportService5, ExportService6, ExportService7>();
        var exposeServicesAttribute8 = new ExposeServicesAttribute<ExportService1, ExportService2, ExportService3, ExportService4, ExportService5, ExportService6, ExportService7, ExportService8>();

        Assert.Single(exposeServicesAttribute1.ServiceTypes);
        Assert.Equal(2, exposeServicesAttribute2.ServiceTypes.Length);
        Assert.Equal(3, exposeServicesAttribute3.ServiceTypes.Length);
        Assert.Equal(4, exposeServicesAttribute4.ServiceTypes.Length);
        Assert.Equal(5, exposeServicesAttribute5.ServiceTypes.Length);
        Assert.Equal(6, exposeServicesAttribute6.ServiceTypes.Length);
        Assert.Equal(7, exposeServicesAttribute7.ServiceTypes.Length);
        Assert.Equal(8, exposeServicesAttribute8.ServiceTypes.Length);
    }
}