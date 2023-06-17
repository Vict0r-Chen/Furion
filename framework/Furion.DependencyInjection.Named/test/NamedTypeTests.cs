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

namespace Furion.DependencyInjection.Named.Tests;

public class NamedTypeTests
{
    [Fact]
    public void NewInstance_Parameters_Null_Throw()
    {
        var type = typeof(NamedTypeClass1);

        Assert.Throws<ArgumentNullException>(() =>
        {
            var namedType = new NamedType(null!, type);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var namedType = new NamedType(string.Empty, type);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var namedType = new NamedType("name", null!);
        });
    }

    [Fact]
    public void MemberCheck()
    {
        var name = "class1";
        var type = typeof(NamedTypeClass1);
        var namedType = new NamedType(name, type);

        Assert.NotNull(namedType.name);
        Assert.Equal(name, namedType.name);

        Assert.NotNull(namedType.DelegatingType);
        Assert.Equal(type, namedType.DelegatingType);
        Assert.Equal(type.Name + " (Type 'NamedType')", namedType.Name);
        Assert.Equal(type.FullName + " (Type 'NamedType')", namedType.FullName);
    }

    [Fact]
    public void GetHashCode_Equail_NameHashcode()
    {
        var name = "class1";
        var type = typeof(NamedTypeClass1);
        var namedType = new NamedType(name, type);

        var hashcode = namedType.GetHashCode();
        Assert.Equal(name.GetHashCode() + type.GetHashCode(), hashcode);
    }

    [Fact]
    public void EqualsCheck()
    {
        var name = "class1";
        var type = typeof(NamedTypeClass1);
        var namedType = new NamedType(name, type);
        var namedType1 = new NamedType(name, type);

        Assert.False(namedType.Equals(name));
        Assert.False(namedType.Equals(type));
        Assert.NotEqual(type, namedType);
        Assert.NotEqual(type, namedType1);
        Assert.Equal(namedType, namedType1);
        Assert.True(namedType == namedType1);

        var name1 = "class2";
        var type1 = typeof(NamedTypeClass2);
        var namedType2 = new NamedType(name, type1);
        var namedType3 = new NamedType(name1, type);
        var namedType4 = new NamedType(name1, type1);

        Assert.NotEqual(namedType, namedType2);
        Assert.NotEqual(namedType, namedType3);
        Assert.NotEqual(namedType, namedType4);
    }
}