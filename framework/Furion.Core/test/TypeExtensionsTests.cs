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
    public void IsStatic_ReturnOK(Type type, bool result)
    {
        Assert.Equal(result, type.IsStatic());
    }

    [Fact]
    public void IsAnonymous_ReturnOK()
    {
        Assert.False(typeof(InstanceType).IsAnonymous());
        Assert.False(typeof(StaticType).IsAnonymous());
        Assert.False(typeof(SealedType).IsAnonymous());
        Assert.False(typeof(AbstractType).IsAnonymous());
        Assert.False(typeof(EnumType).IsAnonymous());
        Assert.False(typeof(RecordType).IsAnonymous());
        Assert.False(typeof(StructType).IsAnonymous());
        Assert.False(typeof(object).IsAnonymous());

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

        Assert.True(anonymouse1.GetType().IsAnonymous());
        Assert.True(anonymouse2.GetType().IsAnonymous());
        Assert.True(anonymouse3.GetType().IsAnonymous());
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
    public void IsInstantiable_ReturnOK(Type type, bool result)
    {
        Assert.Equal(result, type.IsInstantiable());
    }

    [Fact]
    public void IsAlienAssignableTo_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            typeof(InstanceType).IsAlienAssignableTo(null!);
        });
    }

    [Theory]
    [InlineData(typeof(InstanceType), false)]
    [InlineData(typeof(Dependency), true)]
    [InlineData(typeof(IDependency), false)]
    public void IsAlienAssignableTo_ReturnOK(Type type, bool result)
    {
        Assert.Equal(result, type.IsAlienAssignableTo(typeof(IDependency)));
    }

    [Theory]
    [InlineData(typeof(WithAttributeClass), true, false)]
    [InlineData(typeof(WithAttributeClass), true, true)]
    [InlineData(typeof(InheritWithAttributeClass), false, false)]
    [InlineData(typeof(InheritWithAttributeClass), true, true)]
    public void GetDefinedCustomAttribute_ReturnOK(Type type, bool notNull, bool inherit)
    {
        var customAttribute = type.GetDefinedCustomAttribute<CustomAttribute>(inherit);
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
    public void HasDefinePublicParameterlessConstructor_ReturnOK(Type type, bool result)
    {
        Assert.Equal(result, type.HasDefinePublicParameterlessConstructor());
    }

    [Fact]
    public void IsDefinitionEqual_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            typeof(IGenericType<string>).IsDefinitionEqual(null!);
        });
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
    public void IsDefinitionEqual_ReturnOK(Type type, Type? compareType, bool result)
    {
        Assert.Equal(result, type.IsDefinitionEqual(compareType));
    }

    [Fact]
    public void IsCompatibilityTo_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            typeof(ImplementationType).IsCompatibilityTo(null!);
        });
    }

    [Theory]
    [InlineData(typeof(ImplementationType))]
    [InlineData(typeof(ImplementationType1), typeof(IServiceType), typeof(ISecondServiceType), typeof(IOtherServiceType), typeof(IGenericServiceType<string>), typeof(IGenericServiceType<string, int>))]
    [InlineData(typeof(ImplementationType2), typeof(BaseServiceType), typeof(IServiceType), typeof(ISecondServiceType), typeof(IOtherServiceType), typeof(IGenericServiceType<string>), typeof(IGenericServiceType<string, int>))]
    [InlineData(typeof(ImplementationType3), typeof(BaseServiceType<string>), typeof(IServiceType), typeof(ISecondServiceType), typeof(IOtherServiceType), typeof(IGenericServiceType<string>), typeof(IGenericServiceType<string, int>))]
    [InlineData(typeof(ImplementationType4), typeof(BaseServiceType<string, int>), typeof(IServiceType), typeof(ISecondServiceType), typeof(IOtherServiceType), typeof(IGenericServiceType<string>), typeof(IGenericServiceType<string, int>))]
    public void IsCompatibilityTo_ReturnOK(Type type, params Type[] types)
    {
        var baseTypes = new[] { type.BaseType }.Concat(type.GetInterfaces());
        var serviceTypes = baseTypes.Where(t => type.IsCompatibilityTo(t)).ToArray();
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
    public void IsCompatibilityTo_WithGenericType_ReturnOK(Type genericType, params Type[] types)
    {
        var type = GetType().Assembly.GetTypes().Single(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericType);
        var baseTypes = new[] { type.BaseType }.Concat(type.GetInterfaces());
        var serviceTypes = baseTypes.Where(t => type.IsCompatibilityTo(t)).Select(t => t!.GetGenericTypeDefinition()).ToArray();
        var isEqual = types.SequenceEqual(serviceTypes);
        Assert.True(isEqual);
    }

    [Fact]
    public void IsDeclarationMethod_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            typeof(DelaryMethodClass).IsDeclarationMethod(null!, BindingFlags.Public, out _);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            typeof(DelaryMethodClass).IsDeclarationMethod(string.Empty, BindingFlags.Public, out _);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            typeof(DelaryMethodClass).IsDeclarationMethod("", BindingFlags.Public, out _);
        });
    }

    [Theory]
    [InlineData(typeof(DelaryMethodClass), true)]
    [InlineData(typeof(NotDelaryMethodClass), false)]
    [InlineData(typeof(OverrideDelaryMethodClass), true)]
    public void IsDeclarationMethod_ReturnOK(Type type, bool isMultiple)
    {
        var result = type.IsDeclarationMethod("Test", BindingFlags.Public, out _);
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
    public void IsInteger_ReturnOK(object value, bool isInteger)
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
    public void IsDecimal_ReturnOK(object value, bool isDecimal)
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
    public void IsNumeric_ReturnOK(object value, bool isNumeric)
    {
        var result = value.GetType().IsNumeric();
        Assert.Equal(isNumeric, result);
    }

    [Fact]
    public void CreatePropertySetter_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var setter = typeof(SetterModel).CreatePropertySetter(null!);
        });
    }

    [Fact]
    public void CreatePropertySetter_ReturnOK()
    {
        var property = typeof(SetterModel).GetProperty(nameof(SetterModel.Name), BindingFlags.Instance | BindingFlags.Public);
        Assert.NotNull(property);

        var setter = typeof(SetterModel).CreatePropertySetter(property);
        Assert.NotNull(setter);

        var model = new SetterModel();
        setter(model, "Furion");

        Assert.Equal("Furion", model.Name);
    }

    [Fact]
    public void CreateFieldSetter_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var setter = typeof(SetterModel).CreateFieldSetter(null!);
        });
    }

    [Fact]
    public void CreateFieldSetter_ReturnOK()
    {
        var field = typeof(SetterModel).GetField(nameof(SetterModel.id), BindingFlags.Instance | BindingFlags.Public);
        Assert.NotNull(field);

        var setter = typeof(SetterModel).CreateFieldSetter(field);
        Assert.NotNull(setter);

        var model = new SetterModel();
        setter(model, 10);

        Assert.Equal(10, model.id);
    }
}