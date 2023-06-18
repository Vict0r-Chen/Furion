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

namespace Furion.Component.Tests;

public class DependsOnAttributeTests
{
    [Fact]
    public void NewInstance_With_Parameterless()
    {
        var dependsOnAttribute = new DependsOnAttribute();
        Assert.NotNull(dependsOnAttribute.DependedTypes);
        Assert.Empty(dependsOnAttribute.DependedTypes);
    }

    [Fact]
    public void NewInstance_With_Null_Parameter_Throw()
    {
        Assert.Throws<ArgumentNullException>(() => new DependsOnAttribute(null!));
    }

    [Fact]
    public void NewInstance_With_Parameters()
    {
        var dependsOnAttribute = new DependsOnAttribute(typeof(IServiceProvider));
        Assert.NotNull(dependsOnAttribute.DependedTypes);
        Assert.Single(dependsOnAttribute.DependedTypes);
        Assert.Equal(typeof(IServiceProvider), dependsOnAttribute.DependedTypes[0]);
    }

    [Fact]
    public void AttributeUsage_Attribute_Check()
    {
        var attributeUsageAttribute = typeof(DependsOnAttribute).GetCustomAttribute<AttributeUsageAttribute>();
        Assert.NotNull(attributeUsageAttribute);
        Assert.Equal(AttributeTargets.Class, attributeUsageAttribute.ValidOn);
        Assert.False(attributeUsageAttribute.AllowMultiple);
        Assert.False(attributeUsageAttribute.Inherited);
    }

    [Fact]
    public void GenericType()
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

        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute1.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute2.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute3.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute4.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute5.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute6.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute7.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute8.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute9.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute10.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute11.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute12.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute13.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute14.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute15.GetType()));
        Assert.True(typeof(DependsOnAttribute).IsAssignableFrom(dependsOnAttribute16.GetType()));

        var serviceTypes = new[] { typeof(DependsOnComponent1), typeof(DependsOnComponent2), typeof(DependsOnComponent3), typeof(DependsOnComponent4), typeof(DependsOnComponent5), typeof(DependsOnComponent6), typeof(DependsOnComponent7), typeof(DependsOnComponent8), typeof(DependsOnComponent9), typeof(DependsOnComponent10), typeof(DependsOnComponent11), typeof(DependsOnComponent12), typeof(DependsOnComponent13), typeof(DependsOnComponent14), typeof(DependsOnComponent15), typeof(DependsOnComponent16) };
        Assert.True(dependsOnAttribute1.DependedTypes.SequenceEqual(serviceTypes.Take(1)));
        Assert.True(dependsOnAttribute2.DependedTypes.SequenceEqual(serviceTypes.Take(2)));
        Assert.True(dependsOnAttribute3.DependedTypes.SequenceEqual(serviceTypes.Take(3)));
        Assert.True(dependsOnAttribute4.DependedTypes.SequenceEqual(serviceTypes.Take(4)));
        Assert.True(dependsOnAttribute5.DependedTypes.SequenceEqual(serviceTypes.Take(5)));
        Assert.True(dependsOnAttribute6.DependedTypes.SequenceEqual(serviceTypes.Take(6)));
        Assert.True(dependsOnAttribute7.DependedTypes.SequenceEqual(serviceTypes.Take(7)));
        Assert.True(dependsOnAttribute8.DependedTypes.SequenceEqual(serviceTypes.Take(8)));
        Assert.True(dependsOnAttribute9.DependedTypes.SequenceEqual(serviceTypes.Take(9)));
        Assert.True(dependsOnAttribute10.DependedTypes.SequenceEqual(serviceTypes.Take(10)));
        Assert.True(dependsOnAttribute11.DependedTypes.SequenceEqual(serviceTypes.Take(11)));
        Assert.True(dependsOnAttribute12.DependedTypes.SequenceEqual(serviceTypes.Take(12)));
        Assert.True(dependsOnAttribute13.DependedTypes.SequenceEqual(serviceTypes.Take(13)));
        Assert.True(dependsOnAttribute14.DependedTypes.SequenceEqual(serviceTypes.Take(14)));
        Assert.True(dependsOnAttribute15.DependedTypes.SequenceEqual(serviceTypes.Take(15)));
        Assert.True(dependsOnAttribute16.DependedTypes.SequenceEqual(serviceTypes.Take(16)));
    }
}