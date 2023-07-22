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

namespace Furion.Component.Tests;

public class DependsOnAttributeTests
{
    [Fact]
    public void AttributeUsage_Definition()
    {
        var attributeUsageAttribute = typeof(DependsOnAttribute).GetCustomAttribute<AttributeUsageAttribute>();

        Assert.NotNull(attributeUsageAttribute);
        Assert.Equal(AttributeTargets.Class, attributeUsageAttribute.ValidOn);
        Assert.False(attributeUsageAttribute.AllowMultiple);
        Assert.False(attributeUsageAttribute.Inherited);
    }

    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var dependsOnAttribute = new DependsOnAttribute(null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var dependsOnAttribute = new DependsOnAttribute(typeof(DependsOnComponent1));

        Assert.NotNull(dependsOnAttribute);
        Assert.NotNull(dependsOnAttribute.DependedTypes);
        Assert.NotEmpty(dependsOnAttribute.DependedTypes);
    }

    [Fact]
    public void NewGeneric_ReturnOK()
    {
        var dependsOnAttribute1 = new DependsOnAttribute<DependsOnComponent1>();
        var dependsOnAttribute2 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2>();
        var dependsOnAttribute3 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3>();
        var dependsOnAttribute4 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4>();
        var dependsOnAttribute5 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5>();
        var dependsOnAttribute6 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6>();
        var dependsOnAttribute7 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7>();
        var dependsOnAttribute8 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8>();
        var dependsOnAttribute9 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9>();
        var dependsOnAttribute10 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10>();
        var dependsOnAttribute11 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11>();
        var dependsOnAttribute12 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12>();
        var dependsOnAttribute13 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13>();
        var dependsOnAttribute14 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13, DependsOnComponent14>();
        var dependsOnAttribute15 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13, DependsOnComponent14, DependsOnComponent15>();
        var dependsOnAttribute16 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13, DependsOnComponent14, DependsOnComponent15, DependsOnComponent16>();
        var dependsOnAttribute17 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13, DependsOnComponent14, DependsOnComponent15, DependsOnComponent16, DependsOnComponent17>();
        var dependsOnAttribute18 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13, DependsOnComponent14, DependsOnComponent15, DependsOnComponent16, DependsOnComponent17, DependsOnComponent18>();
        var dependsOnAttribute19 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13, DependsOnComponent14, DependsOnComponent15, DependsOnComponent16, DependsOnComponent17, DependsOnComponent18, DependsOnComponent19>();
        var dependsOnAttribute20 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13, DependsOnComponent14, DependsOnComponent15, DependsOnComponent16, DependsOnComponent17, DependsOnComponent18, DependsOnComponent19, DependsOnComponent20>();
        var dependsOnAttribute21 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13, DependsOnComponent14, DependsOnComponent15, DependsOnComponent16, DependsOnComponent17, DependsOnComponent18, DependsOnComponent19, DependsOnComponent20, DependsOnComponent21>();
        var dependsOnAttribute22 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13, DependsOnComponent14, DependsOnComponent15, DependsOnComponent16, DependsOnComponent17, DependsOnComponent18, DependsOnComponent19, DependsOnComponent20, DependsOnComponent21, DependsOnComponent22>();
        var dependsOnAttribute23 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13, DependsOnComponent14, DependsOnComponent15, DependsOnComponent16, DependsOnComponent17, DependsOnComponent18, DependsOnComponent19, DependsOnComponent20, DependsOnComponent21, DependsOnComponent22, DependsOnComponent23>();
        var dependsOnAttribute24 = new DependsOnAttribute<DependsOnComponent1, DependsOnComponent2, DependsOnComponent3, DependsOnComponent4, DependsOnComponent5, DependsOnComponent6, DependsOnComponent7, DependsOnComponent8, DependsOnComponent9, DependsOnComponent10, DependsOnComponent11, DependsOnComponent12, DependsOnComponent13, DependsOnComponent14, DependsOnComponent15, DependsOnComponent16, DependsOnComponent17, DependsOnComponent18, DependsOnComponent19, DependsOnComponent20, DependsOnComponent21, DependsOnComponent22, DependsOnComponent23, DependsOnComponent24>();

        Assert.Single(dependsOnAttribute1.DependedTypes);
        Assert.Equal(2, dependsOnAttribute2.DependedTypes.Length);
        Assert.Equal(3, dependsOnAttribute3.DependedTypes.Length);
        Assert.Equal(4, dependsOnAttribute4.DependedTypes.Length);
        Assert.Equal(5, dependsOnAttribute5.DependedTypes.Length);
        Assert.Equal(6, dependsOnAttribute6.DependedTypes.Length);
        Assert.Equal(7, dependsOnAttribute7.DependedTypes.Length);
        Assert.Equal(8, dependsOnAttribute8.DependedTypes.Length);
        Assert.Equal(9, dependsOnAttribute9.DependedTypes.Length);
        Assert.Equal(10, dependsOnAttribute10.DependedTypes.Length);
        Assert.Equal(11, dependsOnAttribute11.DependedTypes.Length);
        Assert.Equal(12, dependsOnAttribute12.DependedTypes.Length);
        Assert.Equal(13, dependsOnAttribute13.DependedTypes.Length);
        Assert.Equal(14, dependsOnAttribute14.DependedTypes.Length);
        Assert.Equal(15, dependsOnAttribute15.DependedTypes.Length);
        Assert.Equal(16, dependsOnAttribute16.DependedTypes.Length);
        Assert.Equal(17, dependsOnAttribute17.DependedTypes.Length);
        Assert.Equal(18, dependsOnAttribute18.DependedTypes.Length);
        Assert.Equal(19, dependsOnAttribute19.DependedTypes.Length);
        Assert.Equal(20, dependsOnAttribute20.DependedTypes.Length);
        Assert.Equal(21, dependsOnAttribute21.DependedTypes.Length);
        Assert.Equal(22, dependsOnAttribute22.DependedTypes.Length);
        Assert.Equal(23, dependsOnAttribute23.DependedTypes.Length);
        Assert.Equal(24, dependsOnAttribute24.DependedTypes.Length);
    }
}