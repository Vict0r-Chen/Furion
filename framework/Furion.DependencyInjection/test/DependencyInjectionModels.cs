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

public interface ITestClass
{
}

public class PublicBaseClass
{
}

internal class NotPublicBaseClass
{
}

public abstract class AbstractBaseClass
{
}

public class PublicTestClass : ITestClass, IScopedDependency
{
}

internal class NotPublicTestClass : ITestClass, IScopedDependency
{
}

public abstract class AbstractPublicTestClass : ITestClass, IScopedDependency
{
}

[ServiceInjection(IncludingBase = true)]
internal class IncludePublicBaseClass : PublicBaseClass, ITestClass, IScopedDependency
{
}

[ServiceInjection(IncludingBase = true)]
internal class IncludeNotPublicBaseClass : NotPublicBaseClass, ITestClass, IScopedDependency
{
}

[ServiceInjection(IncludingBase = true)]
internal class IncludeAbstractBaseClass : AbstractBaseClass, ITestClass, IScopedDependency
{
}

[ServiceInjection(IncludingSelf = true)]
internal class IncludeSelfClass : ITestClass, IScopedDependency
{
}

internal class IncludeSelfClassWithNotInterface : IScopedDependency
{
}

[ServiceInjection(IncludingBase = true)]
internal class IncludeSelfClassWithBaseClass : PublicBaseClass, IScopedDependency
{
}

[ServiceInjection(Order = 2)]
internal class OrderClass2 : ITestClass, IScopedDependency
{
}

[ServiceInjection(Order = 1)]
internal class OrderClass1 : ITestClass, IScopedDependency
{
}

public interface ISuppressDerivedType
{
}

[ServiceInjection(SuppressDerivedTypes = new[] { typeof(ISuppressDerivedType) })]
internal class SuppressDerivedTypeClass : ISuppressDerivedType, ITestClass, IScopedDependency
{
}

[ServiceInjection(Ignore = true)]
internal class IgnoreClass : ITestClass, IScopedDependency
{
}

public interface IGenericClass<T>
{
}

public class BaseGenericClass<T>
{
}

internal class NormalClassWithGenericClass : IGenericClass<string>, IScopedDependency
{
}

[ServiceInjection(IncludingBase = true)]
internal class NormalClassWithBaseClass : BaseGenericClass<string>, IScopedDependency
{
}

internal class GenericClass<T> : IGenericClass<T>, IScopedDependency
{
}

internal class GenericClassWithFixedGenericClass<T> : IGenericClass<string>, IScopedDependency
{
}

[ServiceInjection(IncludingBase = true)]
internal class GenericClassAll<T> : BaseGenericClass<T>, IGenericClass<T>, IScopedDependency
{
}

internal class NotTGenericClass<T2> : IGenericClass<T2>, IScopedDependency
{
}

internal class GenericClassIncludeSelf<T2> : IScopedDependency
{
}

public interface IMultipleGenericClass<T, M>
{
}

internal class MultipleGenericClass : IMultipleGenericClass<string, object>, IScopedDependency
{
}

internal class MultipleGenericClass<T, M> : IMultipleGenericClass<T, M>, IScopedDependency
{
}

public interface IManyGenericClass<T, M, Z>
{
}

internal class ManyGenericClass<T, M> : IManyGenericClass<T, M, string>, IScopedDependency
{
}

public interface IAddClass
{
}

internal class AddClass1 : IAddClass, IScopedDependency
{
}

[ServiceInjection(ServiceAddition.Add, Order = 2)]
internal class AddClass2 : IAddClass, IScopedDependency
{
}

public interface ITryAddClass
{
}

internal class TryAddClass1 : ITryAddClass, IScopedDependency
{
}

[ServiceInjection(ServiceAddition.TryAdd, Order = 2)]
internal class TryAddClass2 : ITryAddClass, IScopedDependency
{
}

public interface ITryAddEnumerableClass
{
}

internal class TryAddEnumerableClass1 : ITryAddEnumerableClass, IScopedDependency
{
}

[ServiceInjection(ServiceAddition.TryAddEnumerable, Order = 2)]
internal class TryAddEnumerableClass2 : ITryAddEnumerableClass, IScopedDependency
{
}

public interface IReplaceClass
{
}

internal class ReplaceClass1 : IReplaceClass, IScopedDependency
{
}

[ServiceInjection(ServiceAddition.Replace, Order = 2)]
internal class ReplaceClass2 : IReplaceClass, IScopedDependency
{
}

internal class FilterConfigureClass : ITestClass, IScopedDependency
{
}

public interface IMultiple1
{
}

public interface IMultiple2
{
}

public interface IMultiple3
{
}

internal class MulipleClass : IMultiple1, IMultiple2, IMultiple3, IScopedDependency
{
}

public interface IGlobalSuppressDerivedType1
{
}

public interface IGlobalSuppressDerivedType2
{
}

internal class GlobalSuppressDerivedType : IGlobalSuppressDerivedType1, IGlobalSuppressDerivedType2, IScopedDependency
{
}