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

namespace Furion.Core.Tests;

public class TypeExtensionsTests
{
    [Theory]
    [InlineData(typeof(StaticClass), true)]
    [InlineData(typeof(InstanceClass), false)]
    [InlineData(typeof(SealedClass), false)]
    [InlineData(typeof(AbstraceClass), false)]
    public void IsStatic_ReturnOK(Type type, bool ok)
    {
        var result = type.IsStatic();
        Assert.Equal(ok, result);
    }

    [Fact]
    public void IsAnonymousType_ReturnOK()
    {
        var obj1 = new
        {
        };
        var result1 = obj1.GetType().IsAnonymousType();
        Assert.True(result1);

        var obj2 = new
        {
            Name = "Furion"
        };
        var result2 = obj2.GetType().IsAnonymousType();
        Assert.True(result2);

        var obj3 = new InstanceClass();
        var result3 = obj3.GetType().IsAnonymousType();
        Assert.False(result3);
    }

    [Theory]
    [InlineData(typeof(StaticClass), false)]
    [InlineData(typeof(InstanceClass), true)]
    [InlineData(typeof(SealedClass), true)]
    [InlineData(typeof(AbstraceClass), false)]
    public void IsInstantiatedType_ReturnOK(Type type, bool ok)
    {
        var result = type.IsInstantiatedType();
        Assert.Equal(ok, result);
    }

    [Theory]
    [InlineData(typeof(StaticClass), false)]
    [InlineData(typeof(InstanceClass), false)]
    [InlineData(typeof(SealedClass), false)]
    [InlineData(typeof(AbstraceClass), false)]
    [InlineData(typeof(DerivedType), true)]
    public void IsInstantiatedTypeWithAssignableFrom_ReturnOK(Type type, bool ok)
    {
        var result = type.IsInstantiatedTypeWithAssignableFrom(typeof(IDerivedType));
        Assert.Equal(ok, result);
    }

    [Fact]
    public void IsInstantiatedTypeWithAssignableFrom_EmptyDerivedType_ReturnOops()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            var result = typeof(DerivedType).IsInstantiatedTypeWithAssignableFrom(null!);
        });
    }

    [Fact]
    public void GetCustomAttributeIfIsDefined_ReturnOK()
    {
        var attribute1 = typeof(NotAttributeClass).GetCustomAttributeIfIsDefined<DisplayNameAttribute>(true);
        var attribute2 = typeof(HasAttributeClass).GetCustomAttributeIfIsDefined<DisplayNameAttribute>(true);
        var attribute3 = typeof(InheritAttributeClass).GetCustomAttributeIfIsDefined<DisplayNameAttribute>(true);
        var attribute4 = typeof(InheritAttributeClass).GetCustomAttributeIfIsDefined<DisplayNameAttribute>(false);

        Assert.Null(attribute1);
        Assert.NotNull(attribute2);
        Assert.NotNull(attribute3);
        Assert.Null(attribute4);
    }

    [Theory]
    [InlineData(nameof(WithMethodClass.GetStatic), false)]
    [InlineData(nameof(WithMethodClass.GetInternal), false)]
    [InlineData(nameof(WithMethodClass.GetPublic), true)]
    public void GetPublicInstanceMethod_ReturnOK(string methodName, bool isPublicInstance)
    {
        var type = typeof(WithMethodClass);
        var method = type.GetPublicInstanceMethod(methodName);
        Assert.Equal(isPublicInstance, method != null);
    }
}