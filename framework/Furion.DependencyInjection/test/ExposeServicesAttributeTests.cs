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
    public void New_Parameterless_ReturnOK()
    {
        var exposeServicesAttribute = new ExposeServicesAttribute();
        Assert.NotNull(exposeServicesAttribute.ServiceTypes);
        Assert.Empty(exposeServicesAttribute.ServiceTypes);
    }

    [Fact]
    public void New_Parameter_Null_Throw()
    {
        Assert.Throws<ArgumentNullException>(() => new ExposeServicesAttribute(null!));
    }

    [Fact]
    public void New_Parameter_Set()
    {
        var exposeServicesAttribute = new ExposeServicesAttribute(typeof(IServiceProvider));
        Assert.NotNull(exposeServicesAttribute.ServiceTypes);
        Assert.Single(exposeServicesAttribute.ServiceTypes);
        Assert.Equal(typeof(IServiceProvider), exposeServicesAttribute.ServiceTypes[0]);
    }

    [Fact]
    public void AttributeUsage_Check()
    {
        var attributeUsageAttribute = typeof(ExposeServicesAttribute).GetCustomAttribute<AttributeUsageAttribute>();
        Assert.NotNull(attributeUsageAttribute);
        Assert.Equal(AttributeTargets.Class, attributeUsageAttribute.ValidOn);
        Assert.False(attributeUsageAttribute.AllowMultiple);
        Assert.True(attributeUsageAttribute.Inherited);
    }
}