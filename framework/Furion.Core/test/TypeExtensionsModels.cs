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

public class InstanceType
{ }

public static class StaticType
{ }

public sealed class SealedType
{ }

public abstract class AbstractType
{ }

public enum EnumType
{ }

public record RecordType { }

public struct StructType
{ }

public interface IDependency
{ }

public class Dependency : IDependency
{ }

[AttributeUsage(AttributeTargets.Class)]
public class CustomAttribute : Attribute
{ }

[Custom]
public class WithAttributeClass
{
}

public class InheritWithAttributeClass : WithAttributeClass
{
}

public class StaticConstructClass
{
    static StaticConstructClass()
    {
    }
}

public class InternalConstructClass
{
    internal InternalConstructClass()
    {
    }
}

public class PrivateConstructClass
{
    private PrivateConstructClass()
    {
    }
}

public class WithParameterConstructClass
{
    public WithParameterConstructClass(string _)
    {
    }
}

public class WithParameterAndParameterlessConstructClass
{
    public WithParameterAndParameterlessConstructClass()
    {
    }

    public WithParameterAndParameterlessConstructClass(string _)
    {
    }
}

public interface IServiceType
{ }

public interface ISecondServiceType
{ }

public interface IOtherServiceType
{ }

public interface IGenericServiceType<T>
{ }

public interface IGenericServiceType<T, U>
{ }

public interface ISecondGenericServiceType<T>
{ }

public interface ISecondGenericServiceType<T, U>
{ }

public interface IOtherGenericServiceType<T>
{ }

public interface IOtherGenericServiceType<T, U>
{ }

public abstract class BaseServiceType
{ }

public class BaseServiceType<T>
{ }

public abstract class BaseServiceType<T, U>
{ }

public class ImplementationType
{ }

public class ImplementationType1 : IServiceType, ISecondServiceType, IOtherServiceType, IGenericServiceType<string>, IGenericServiceType<string, int>
{ }

public class ImplementationType2 : BaseServiceType, IServiceType, ISecondServiceType, IOtherServiceType, IGenericServiceType<string>, IGenericServiceType<string, int>
{ }

public class ImplementationType3 : BaseServiceType<string>, IServiceType, ISecondServiceType, IOtherServiceType, IGenericServiceType<string>, IGenericServiceType<string, int>
{ }

public class ImplementationType4 : BaseServiceType<string, int>, IServiceType, ISecondServiceType, IOtherServiceType, IGenericServiceType<string>, IGenericServiceType<string, int>
{ }

public class GenericImplementationType<T> : IServiceType, ISecondServiceType, IOtherServiceType, IOtherGenericServiceType<string>, ISecondGenericServiceType<T>, IGenericServiceType<T>, IGenericServiceType<T, int>
{ }

public class GenericImplementationTyp1<T> : BaseServiceType, IServiceType, ISecondServiceType, IOtherServiceType, IOtherGenericServiceType<string>, ISecondGenericServiceType<T>, IGenericServiceType<T>, IGenericServiceType<T, int>
{ }

public class GenericImplementationType2<T> : BaseServiceType<string>, IServiceType, ISecondServiceType, IOtherServiceType, IOtherGenericServiceType<string>, ISecondGenericServiceType<T>, IGenericServiceType<T>, IGenericServiceType<T, int>
{ }

public class GenericImplementationType3<T> : BaseServiceType<T>, IServiceType, ISecondServiceType, IOtherServiceType, IOtherGenericServiceType<string>, ISecondGenericServiceType<T>, IGenericServiceType<T>, IGenericServiceType<T, int>
{ }

public class GenericImplementationType4<T> : BaseServiceType<T, string>, IServiceType, ISecondServiceType, IOtherServiceType, IOtherGenericServiceType<string>, ISecondGenericServiceType<T>, IGenericServiceType<T>, IGenericServiceType<T, int>
{ }

public class MultiGenericImplementationType<T, U> : IServiceType, ISecondServiceType, IOtherServiceType, IOtherGenericServiceType<string, int>, ISecondGenericServiceType<T>, IGenericServiceType<T, U>, ISecondGenericServiceType<T, int>
{ }

public class MultiGenericImplementationType1<T, U> : BaseServiceType, IServiceType, ISecondServiceType, IOtherServiceType, IOtherGenericServiceType<string, int>, ISecondGenericServiceType<T>, IGenericServiceType<T, U>, ISecondGenericServiceType<T, int>
{ }

public class MultiGenericImplementationType2<T, U> : BaseServiceType<string>, IServiceType, ISecondServiceType, IOtherServiceType, IOtherGenericServiceType<string, int>, ISecondGenericServiceType<T>, IGenericServiceType<T, U>, ISecondGenericServiceType<T, int>
{ }

public class MultiGenericImplementationType3<T, U> : BaseServiceType<T, U>, IServiceType, ISecondServiceType, IOtherServiceType, IOtherGenericServiceType<string, int>, ISecondGenericServiceType<T>, IGenericServiceType<T, U>, ISecondGenericServiceType<T, U>
{ }

public class MultiGenericImplementationType4<T, U> : BaseServiceType<T, string>, IServiceType, ISecondServiceType, IOtherServiceType, IOtherGenericServiceType<string, int>, ISecondGenericServiceType<T>, IGenericServiceType<T, U>, ISecondGenericServiceType<T, int>
{ }

public class MultiGenericImplementationType5<T, U> : IGenericServiceType<U, T>
{ }

public interface IGenericType<T>
{ }

public interface IGenericType<T, U>
{ }

public class GenericType<T> : IGenericType<T>, IGenericType<T, string>
{ }

public class GenericType<T, U> : IGenericType<T, U>, IGenericType<T>
{ }