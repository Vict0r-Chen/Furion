namespace Furion.Tests.Framework.Extensions;

public class TypeExtensionsTests
{
    /// <summary>
    /// 判断是否是静态类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ok"></param>
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

    /// <summary>
    /// 判断是否是匿名类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ok"></param>
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

    /// <summary>
    /// 判断是否是可实例化类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ok"></param>
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

    /// <summary>
    /// 判断是否是可实例化类型且派生自特定类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ok"></param>
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

    /// <summary>
    /// 判断是否是可实例化类型且派生自特定类型且派生类型为 null
    /// </summary>
    [Fact]
    public void IsInstantiatedTypeWithAssignableFrom_EmptyDerivedType_ReturnOops()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            var result = typeof(DerivedType).IsInstantiatedTypeWithAssignableFrom(null!);
        });
    }
}