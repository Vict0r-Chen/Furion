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

namespace Furion.Component.Tests;

#pragma warning disable

public class NotComponentClass
{ }

public class ComponentClass : ComponentBase
{ }

public class InheritComponentClass : ComponentClass
{ }

public abstract class AbstractComponentClass : ComponentBase
{ }

public class ComponentWithPropsDefinition : ComponentBase
{
    internal readonly Action<ComponentOptionsClass1> _componentPropsAction1;

    internal readonly ComponentOptionsClass1 _componentProps1;

    public ComponentWithPropsDefinition(Action<ComponentOptionsClass1> componentPropsAction1
        , ComponentOptionsClass1 componentProps1)
    {
        _componentPropsAction1 = componentPropsAction1;
        _componentProps1 = componentProps1;
    }

    [ComponentProps]
    public Action<ComponentOptionsClass1>? ComponentPropsAction2 { get; set; }

    [ComponentProps]
    internal ComponentOptionsClass1? _componentProps2;
}

public class InvalidComponentPropsConstructor : ComponentBase
{
    public InvalidComponentPropsConstructor(Action<ComponentOptionsClass3> componentPropsAction3)
    {
        Assert.NotNull(componentPropsAction3);
    }
}

public class InvalidComponentPropsProperty : ComponentBase
{
    [ComponentProps]
    public Action<ComponentOptionsClass1>? ComponentPropsAction2 { get; }
}

public class InvalidComponentPropsField : ComponentBase
{
    [ComponentProps]
    internal readonly ComponentOptionsClass1? _componentProps2;
}

public class InvalidComponentPropsType : ComponentBase
{
    [ComponentProps]
    public Action<ComponentOptionsClass3>? ComponentPropsAction3 { get; set; }
}

public class ComponentWithAutowiredProps : ComponentBase
{
    [ComponentProps]
    public Action<ComponentOptionsClass1>? ComponentPropsAction2 { get; set; }

    [ComponentProps]
    internal ComponentOptionsClass1? _componentProps2;
}

public class ComponentWithMultipleConstructor : ComponentBase
{
    public ComponentWithMultipleConstructor(Action<ComponentOptionsClass1> action1)
    {
    }

    public ComponentWithMultipleConstructor(Action<ComponentOptionsClass1> action1, ComponentOptionsClass1 props1)
    {
    }
}

public class ComponentWithActivatorComponentConstructor : ComponentBase
{
    [ActivatorComponentConstructor]
    public ComponentWithActivatorComponentConstructor(Action<ComponentOptionsClass1> action1)
    {
    }

    public ComponentWithActivatorComponentConstructor(Action<ComponentOptionsClass1> action1, ComponentOptionsClass1 props1)
    {
    }
}