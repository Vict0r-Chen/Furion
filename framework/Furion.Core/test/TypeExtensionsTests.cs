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
    [InlineData(typeof(InstanceType), false)]
    [InlineData(typeof(StaticType), true)]
    [InlineData(typeof(SealedType), false)]
    [InlineData(typeof(AbstractType), false)]
    [InlineData(typeof(EnumType), false)]
    [InlineData(typeof(RecordType), false)]
    [InlineData(typeof(StructType), false)]
    [InlineData(typeof(string), false)]
    [InlineData(typeof(int), false)]
    [InlineData(typeof(bool), false)]
    [InlineData(typeof(object), false)]
    [InlineData(typeof(IDependency), false)]
    public void IsStatic(Type type, bool result)
    {
        Assert.Equal(result, type.IsStatic());
    }

    [Fact]
    public void IsAnonymousType()
    {
        Assert.False(typeof(InstanceType).IsAnonymousType());
        Assert.False(typeof(StaticType).IsAnonymousType());
        Assert.False(typeof(SealedType).IsAnonymousType());
        Assert.False(typeof(AbstractType).IsAnonymousType());
        Assert.False(typeof(EnumType).IsAnonymousType());
        Assert.False(typeof(RecordType).IsAnonymousType());
        Assert.False(typeof(StructType).IsAnonymousType());
        Assert.False(typeof(object).IsAnonymousType());

        var anonymouse1 = new
        {
            Name = "Furion"
        };
        var anonymouse2 = new { };
        var anonymouse3 = new
        {
            Name = "Furion",
            Age = DateTime.Now.Year - DateTime.Parse("2020/06/22").Year
        };

        Assert.True(anonymouse1.GetType().IsAnonymousType());
        Assert.True(anonymouse2.GetType().IsAnonymousType());
        Assert.True(anonymouse3.GetType().IsAnonymousType());
    }

    [Theory]
    [InlineData(typeof(InstanceType), true)]
    [InlineData(typeof(StaticType), false)]
    [InlineData(typeof(SealedType), true)]
    [InlineData(typeof(AbstractType), false)]
    [InlineData(typeof(EnumType), false)]
    [InlineData(typeof(RecordType), true)]
    [InlineData(typeof(StructType), false)]
    [InlineData(typeof(string), true)]
    [InlineData(typeof(int), false)]
    [InlineData(typeof(bool), false)]
    [InlineData(typeof(object), true)]
    [InlineData(typeof(IDependency), false)]
    public void IsInstantiable(Type type, bool result)
    {
        Assert.Equal(result, type.IsInstantiable());
    }

    [Theory]
    [InlineData(typeof(InstanceType), false)]
    [InlineData(typeof(Dependency), true)]
    [InlineData(typeof(IDependency), false)]
    public void IsAlienAssignableTo(Type type, bool result)
    {
        Assert.Equal(result, type.IsAlienAssignableTo(typeof(IDependency)));
    }

    [Theory]
    [InlineData(typeof(WithAttribute), true, false)]
    [InlineData(typeof(WithAttribute), true, true)]
    [InlineData(typeof(InheritWithAttribute), false, false)]
    [InlineData(typeof(InheritWithAttribute), true, true)]
    public void GetDefinedCustomAttribute(Type type, bool notNull, bool inherit)
    {
        var customAttribute = type.GetDefinedCustomAttribute<CustomAttribute>(inherit);
        Assert.Equal(notNull, customAttribute is not null);
    }

    [Theory]
    [InlineData(typeof(WithAttribute), true, false)]
    [InlineData(typeof(WithAttribute), true, true)]
    [InlineData(typeof(InheritWithAttribute), true, false)]
    [InlineData(typeof(InheritWithAttribute), true, true)]
    public void GetDefinedCustomAttributeOrNew(Type type, bool notNull, bool inherit)
    {
        var customAttribute = type.GetDefinedCustomAttributeOrNew<CustomAttribute>(inherit);
        Assert.Equal(notNull, customAttribute is not null);
    }

    [Theory]
    [InlineData(typeof(InstanceType), true)]
    [InlineData(typeof(StaticType), false)]
    [InlineData(typeof(SealedType), true)]
    [InlineData(typeof(AbstractType), false)]
    [InlineData(typeof(EnumType), false)]
    [InlineData(typeof(RecordType), true)]
    [InlineData(typeof(StructType), false)]
    [InlineData(typeof(IDependency), false)]
    [InlineData(typeof(WithAttribute), true)]
    [InlineData(typeof(StaticConstruct), true)]
    [InlineData(typeof(InternalConstruct), false)]
    [InlineData(typeof(PrivateConstruct), false)]
    [InlineData(typeof(WithParameterConstruct), false)]
    [InlineData(typeof(WithParameterAndParameterlessConstruct), true)]
    public void HasParameterlessConstructorDefined(Type type, bool result)
    {
        Assert.Equal(result, type.HasParameterlessConstructorDefined());
    }

    [Theory]
    [InlineData(typeof(IGenericType<string>), typeof(IEnumerable<string>), false)]
    [InlineData(typeof(IGenericType<string>), typeof(IGenericType<string>), true)]
    [InlineData(typeof(IGenericType<string>), typeof(IGenericType<int>), false)]
    [InlineData(typeof(IGenericType<>), typeof(IGenericType<>), true)]
    [InlineData(typeof(IGenericType<>), typeof(IGenericType<string>), true)]
    [InlineData(typeof(IGenericType<string>), typeof(IGenericType<>), false)]
    [InlineData(typeof(IGenericType<>), typeof(IGenericType<int>), true)]
    [InlineData(typeof(IGenericType<,>), typeof(IGenericType<,>), true)]
    [InlineData(typeof(IGenericType<string, int>), typeof(IGenericType<string, int>), true)]
    [InlineData(typeof(IGenericType<int, string>), typeof(IGenericType<string, int>), false)]
    [InlineData(typeof(IGenericType<,>), typeof(IGenericType<>), false)]
    [InlineData(typeof(IGenericType<,>), typeof(IGenericType<string, int>), true)]
    [InlineData(typeof(IGenericType<,>), typeof(IGenericType<int, string>), true)]
    [InlineData(typeof(IGenericType<>), typeof(GenericType<>), false)]
    [InlineData(typeof(IGenericType<,>), typeof(GenericType<,>), false)]
    [InlineData(typeof(InstanceType), typeof(InstanceType), true)]
    [InlineData(typeof(InstanceType), typeof(SealedType), false)]
    public void IsEqualTypeDefinition(Type type, Type compareType, bool result)
    {
        Assert.Equal(result, type.IsEqualTypeDefinition(compareType));
    }

    [Fact]
    public void IsTypeCompatibilityTo()
    {
        var types = GetType().Assembly.GetTypes().Where(t => t.IsDefined(typeof(ScanningAttribute), false));

        foreach (var type in types)
        {
            var firstInterface = type.GetInterfaces()[0];
            var secondInterface = type.GetInterfaces()[1];
            var lastInterface = type.GetInterfaces()[2];

            Assert.True(type.IsTypeCompatibilityTo(firstInterface));
            Assert.False(type.IsTypeCompatibilityTo(secondInterface));
            Assert.False(type.IsTypeCompatibilityTo(lastInterface));
        }
    }
}