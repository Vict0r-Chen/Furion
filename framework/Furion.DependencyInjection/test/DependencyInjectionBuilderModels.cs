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

public interface IService
{ }

internal interface IService<T>
{ }

public interface IService<T, U>
{ }

public interface ISecondService
{ }

internal interface ISecondService<T>
{ }

public interface ISecondService<T, U>
{ }

public class BaseService
{ }

public abstract class BaseService<T>
{ }

internal class BaseService<T, U>
{ }

public class Service : IService, ITransientDependency, IService<string>, ISecondService<string, int>
{
}

public class Service1 : IService, IScopedDependency, ISecondService, IService<string>, ISecondService<string, int>
{
}

public class Service2 : IService, ISingletonDependency, ITransientDependency
{
}

public class Service3 : IService, IScopedDependency, IDisposable
{
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

public class Service4 : IScopedDependency
{
}

public class ServiceBase : IScopedDependency
{
}

[ServiceInjection(IncludeSelf = true)]
public class Service5 : ServiceBase, IService
{
}

[ServiceInjection(IncludeBase = true)]
public class Service6 : ServiceBase, IService, ITransientDependency, IDependency
{
}

[ServiceInjection(IncludeSelf = true, IncludeBase = true)]
public class Service7 : ServiceBase, IService, ITransientDependency
{
}

public class Service8 : ServiceBase, IService, IScopedDependency
{
}

public class NonLifetimeClass : IDependency, IService
{
}

public class GenericService<T> : IService, IService<string>, ISecondService<T>, IScopedDependency
{
}

public class GenericService1<T> : IService, IService<T>, ISecondService<T>, IScopedDependency
{
}

public class GenericService2<T, U> : IService, IService<string>, IService<T, U>, ISecondService<T>, ISecondService<T, U>, ISingletonDependency
{
}

public class GenericService3<T, U> : IService, IService<U, T>, ISecondService<T>, ISecondService<T, U>, IService<string>, IScopedDependency
{
}

[ServiceInjection(Ignore = true)]
public class IgnoreService : IService, IScopedDependency
{
}

[ServiceInjection(Order = 2)]
public class Order1Service : IService, IScopedDependency
{ }

public class Order2Service : IService, IScopedDependency
{ }

[ServiceInjection(Order = 1)]
public class Order3Service : IService, IScopedDependency
{ }

internal class NotPublicService : IScopedDependency
{
}

[ExposeServices<ISecondService>]
public class ExposeServiceClass : IService, ISecondService, ITransientDependency
{
}

[ExposeServices(typeof(IService))]
[ExposeServices<ISecondService>]
public class ExposeServiceClass2 : IService, ISecondService, ITransientDependency
{
}