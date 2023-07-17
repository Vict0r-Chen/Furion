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

public class ExposeServicesAttributeTests
{
    [Fact]
    public void NewInstance_With_Parameterless()
    {
        var exposeServicesAttribute = new ExposeServicesAttribute();
        Assert.NotNull(exposeServicesAttribute.ServiceTypes);
        Assert.Empty(exposeServicesAttribute.ServiceTypes);
    }

    [Fact]
    public void NewInstance_With_Null_Parameter_Throw()
    {
        Assert.Throws<ArgumentNullException>(() => new ExposeServicesAttribute(null!));
    }

    [Fact]
    public void NewInstance_With_Parameters()
    {
        var exposeServicesAttribute = new ExposeServicesAttribute(typeof(ExposeService1));
        Assert.NotNull(exposeServicesAttribute.ServiceTypes);
        Assert.Single(exposeServicesAttribute.ServiceTypes);
        Assert.Equal(typeof(ExposeService1), exposeServicesAttribute.ServiceTypes[0]);
    }

    [Fact]
    public void AttributeUsage_Attribute_Check()
    {
        var attributeUsageAttribute = typeof(ExposeServicesAttribute).GetCustomAttribute<AttributeUsageAttribute>();
        Assert.NotNull(attributeUsageAttribute);
        Assert.Equal(AttributeTargets.Class, attributeUsageAttribute.ValidOn);
        Assert.False(attributeUsageAttribute.AllowMultiple);
        Assert.True(attributeUsageAttribute.Inherited);
    }

    [Fact]
    public void GenericType()
    {
        var exposeServicesAttribute1 = new ExposeServicesAttribute<ExposeService1>();
        var exposeServicesAttribute2 = new ExposeServicesAttribute<ExposeService1, ExposeService2>();
        var exposeServicesAttribute3 = new ExposeServicesAttribute<ExposeService1, ExposeService2, ExposeService3>();
        var exposeServicesAttribute4 = new ExposeServicesAttribute<ExposeService1, ExposeService2, ExposeService3, ExposeService4>();
        var exposeServicesAttribute5 = new ExposeServicesAttribute<ExposeService1, ExposeService2, ExposeService3, ExposeService4, ExposeService5>();
        var exposeServicesAttribute6 = new ExposeServicesAttribute<ExposeService1, ExposeService2, ExposeService3, ExposeService4, ExposeService5, ExposeService6>();
        var exposeServicesAttribute7 = new ExposeServicesAttribute<ExposeService1, ExposeService2, ExposeService3, ExposeService4, ExposeService5, ExposeService6, ExposeService7>();
        var exposeServicesAttribute8 = new ExposeServicesAttribute<ExposeService1, ExposeService2, ExposeService3, ExposeService4, ExposeService5, ExposeService6, ExposeService7, ExposeService8>();

        Assert.True(typeof(ExposeServicesAttribute).IsAssignableFrom(exposeServicesAttribute1.GetType()));
        Assert.True(typeof(ExposeServicesAttribute).IsAssignableFrom(exposeServicesAttribute2.GetType()));
        Assert.True(typeof(ExposeServicesAttribute).IsAssignableFrom(exposeServicesAttribute3.GetType()));
        Assert.True(typeof(ExposeServicesAttribute).IsAssignableFrom(exposeServicesAttribute4.GetType()));
        Assert.True(typeof(ExposeServicesAttribute).IsAssignableFrom(exposeServicesAttribute5.GetType()));
        Assert.True(typeof(ExposeServicesAttribute).IsAssignableFrom(exposeServicesAttribute6.GetType()));
        Assert.True(typeof(ExposeServicesAttribute).IsAssignableFrom(exposeServicesAttribute7.GetType()));
        Assert.True(typeof(ExposeServicesAttribute).IsAssignableFrom(exposeServicesAttribute8.GetType()));

        var serviceTypes = new[] { typeof(ExposeService1), typeof(ExposeService2), typeof(ExposeService3), typeof(ExposeService4), typeof(ExposeService5), typeof(ExposeService6), typeof(ExposeService7), typeof(ExposeService8) };
        Assert.True(exposeServicesAttribute1.ServiceTypes.SequenceEqual(serviceTypes.Take(1)));
        Assert.True(exposeServicesAttribute2.ServiceTypes.SequenceEqual(serviceTypes.Take(2)));
        Assert.True(exposeServicesAttribute3.ServiceTypes.SequenceEqual(serviceTypes.Take(3)));
        Assert.True(exposeServicesAttribute4.ServiceTypes.SequenceEqual(serviceTypes.Take(4)));
        Assert.True(exposeServicesAttribute5.ServiceTypes.SequenceEqual(serviceTypes.Take(5)));
        Assert.True(exposeServicesAttribute6.ServiceTypes.SequenceEqual(serviceTypes.Take(6)));
        Assert.True(exposeServicesAttribute7.ServiceTypes.SequenceEqual(serviceTypes.Take(7)));
        Assert.True(exposeServicesAttribute8.ServiceTypes.SequenceEqual(serviceTypes.Take(8)));
    }
}