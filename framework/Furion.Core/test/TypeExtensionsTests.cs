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
    [InlineData(typeof(WithAttributeClass), true, false)]
    [InlineData(typeof(WithAttributeClass), true, true)]
    [InlineData(typeof(InheritWithAttributeClass), false, false)]
    [InlineData(typeof(InheritWithAttributeClass), true, true)]
    public void GetDefinedCustomAttribute(Type type, bool notNull, bool inherit)
    {
        var customAttribute = type.GetDefinedCustomAttribute<CustomAttribute>(inherit);
        Assert.Equal(notNull, customAttribute is not null);
    }

    [Theory]
    [InlineData(typeof(WithAttributeClass), true, false)]
    [InlineData(typeof(WithAttributeClass), true, true)]
    [InlineData(typeof(InheritWithAttributeClass), true, false)]
    [InlineData(typeof(InheritWithAttributeClass), true, true)]
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
    [InlineData(typeof(WithAttributeClass), true)]
    [InlineData(typeof(StaticConstructClass), true)]
    [InlineData(typeof(InternalConstructClass), false)]
    [InlineData(typeof(PrivateConstructClass), false)]
    [InlineData(typeof(WithParameterConstructClass), false)]
    [InlineData(typeof(WithParameterAndParameterlessConstructClass), true)]
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
    public void IsTypeDefinitionEqual(Type type, Type compareType, bool result)
    {
        Assert.Equal(result, type.IsTypeDefinitionEqual(compareType));
    }

    [Theory]
    [InlineData(typeof(ImplementationType))]
    [InlineData(typeof(ImplementationType1), typeof(IServiceType), typeof(ISecondServiceType), typeof(IOtherServiceType), typeof(IGenericServiceType<string>), typeof(IGenericServiceType<string, int>))]
    [InlineData(typeof(ImplementationType2), typeof(BaseServiceType), typeof(IServiceType), typeof(ISecondServiceType), typeof(IOtherServiceType), typeof(IGenericServiceType<string>), typeof(IGenericServiceType<string, int>))]
    [InlineData(typeof(ImplementationType3), typeof(BaseServiceType<string>), typeof(IServiceType), typeof(ISecondServiceType), typeof(IOtherServiceType), typeof(IGenericServiceType<string>), typeof(IGenericServiceType<string, int>))]
    [InlineData(typeof(ImplementationType4), typeof(BaseServiceType<string, int>), typeof(IServiceType), typeof(ISecondServiceType), typeof(IOtherServiceType), typeof(IGenericServiceType<string>), typeof(IGenericServiceType<string, int>))]
    public void IsTypeCompatibilityTo_NonGenericType_ReturnOK(Type type, params Type[] types)
    {
        var baseTypes = new[] { type.BaseType }.Concat(type.GetInterfaces());
        var serviceTypes = baseTypes.Where(t => type.IsTypeCompatibilityTo(t)).ToArray();
        var isEqual = types.SequenceEqual(serviceTypes);
        Assert.True(isEqual);
    }

    [Theory]
    [InlineData(typeof(GenericImplementationType<>), typeof(ISecondGenericServiceType<>), typeof(IGenericServiceType<>))]
    [InlineData(typeof(GenericImplementationTyp1<>), typeof(ISecondGenericServiceType<>), typeof(IGenericServiceType<>))]
    [InlineData(typeof(GenericImplementationType2<>), typeof(ISecondGenericServiceType<>), typeof(IGenericServiceType<>))]
    [InlineData(typeof(GenericImplementationType3<>), typeof(BaseServiceType<>), typeof(ISecondGenericServiceType<>), typeof(IGenericServiceType<>))]
    [InlineData(typeof(GenericImplementationType4<>), typeof(ISecondGenericServiceType<>), typeof(IGenericServiceType<>))]
    [InlineData(typeof(MultiGenericImplementationType<,>), typeof(IGenericServiceType<,>))]
    [InlineData(typeof(MultiGenericImplementationType1<,>), typeof(IGenericServiceType<,>))]
    [InlineData(typeof(MultiGenericImplementationType2<,>), typeof(IGenericServiceType<,>))]
    [InlineData(typeof(MultiGenericImplementationType3<,>), typeof(BaseServiceType<,>), typeof(IGenericServiceType<,>), typeof(ISecondGenericServiceType<,>))]
    [InlineData(typeof(MultiGenericImplementationType4<,>), typeof(IGenericServiceType<,>))]
    [InlineData(typeof(MultiGenericImplementationType5<,>))]
    public void IsTypeCompatibilityTo_GenericType_ReturnOK(Type genericType, params Type[] types)
    {
        var type = GetType().Assembly.GetTypes().Single(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericType);
        var baseTypes = new[] { type.BaseType }.Concat(type.GetInterfaces());
        var serviceTypes = baseTypes.Where(t => type.IsTypeCompatibilityTo(t)).Select(t => t!.GetGenericTypeDefinition()).ToArray();
        var isEqual = types.SequenceEqual(serviceTypes);
        Assert.True(isEqual);
    }

    [Theory]
    [InlineData(typeof(OneAttributeClass), false)]
    [InlineData(typeof(InheritAttributeClass), false)]
    [InlineData(typeof(MultipleAttributeClass), true)]
    [InlineData(typeof(MultipleAndInheritAttributeClass), true)]
    [InlineData(typeof(InheritMultipleAttributeClass), false)]
    public void IsMultipleSameDefined(Type type, bool isMultiple)
    {
        var result = type.IsMultipleSameDefined(typeof(CheckAttribute), true);
        Assert.Equal(isMultiple, result);
    }

    [Theory]
    [InlineData(typeof(DelaryMethodClass), true)]
    [InlineData(typeof(NotDelaryMethodClass), false)]
    [InlineData(typeof(OverrideDelaryMethodClass), true)]
    public void IsDeclareOnlyMethod(Type type, bool isMultiple)
    {
        var result = type.IsDeclareOnlyMethod("Test", BindingFlags.Public, out _);
        Assert.Equal(isMultiple, result);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(1, true)]
    [InlineData((long)1, true)]
    [InlineData(1.0D, false)]
    [InlineData(1.0F, false)]
    [InlineData((byte)1, true)]
    [InlineData(-1, true)]
    [InlineData(0, true)]
    public void IsInteger(object value, bool isInteger)
    {
        var result = value.GetType().IsInteger();
        Assert.Equal(isInteger, result);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(1, false)]
    [InlineData((long)1, false)]
    [InlineData(1.0D, true)]
    [InlineData(1.0F, true)]
    [InlineData((byte)1, false)]
    [InlineData(-1, false)]
    [InlineData(0, false)]
    [InlineData(-123.33, true)]
    public void IsDecimal(object value, bool isDecimal)
    {
        var result = value.GetType().IsDecimal();
        Assert.Equal(isDecimal, result);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(1, true)]
    [InlineData((long)1, true)]
    [InlineData(1.0D, true)]
    [InlineData(1.0F, true)]
    [InlineData((byte)1, true)]
    [InlineData(-1, true)]
    [InlineData(0, true)]
    public void IsNumeric(object value, bool isNumeric)
    {
        var result = value.GetType().IsNumeric();
        Assert.Equal(isNumeric, result);
    }
}