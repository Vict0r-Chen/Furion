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

public class ComponentActivatorTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var componentActivator = new ComponentActivator(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            var componentActivator = new ComponentActivator(typeof(IsComponentClass), null!);
        });
    }

    [Fact]
    public void New_ReturnOK()
    {
        var componentActivator = new ComponentActivator(typeof(IsComponentClass), new());

        Assert.NotNull(componentActivator);
        Assert.NotNull(componentActivator._componentType);
        Assert.Equal(typeof(IsComponentClass), componentActivator._componentType);
        Assert.NotNull(componentActivator._componentOptions);
    }

    [Fact]
    public void Create_ReturnOK()
    {
        var componentOptions = new ComponentOptions();
        componentOptions.Props<ComponentOptionsClass1>(p => { });
        var componentActivator = new ComponentActivator(typeof(ComponentWithPropsDefinition), componentOptions);

        var component = componentActivator.Create() as ComponentWithPropsDefinition;
        Assert.NotNull(component);
        Assert.NotNull(component._componentPropsAction1);
        Assert.NotNull(component._componentProps1);
        Assert.NotNull(component.ComponentPropsAction2);
        Assert.NotNull(component._componentProps2);
    }

    [Theory]
    [InlineData(typeof(InvalidComponentPropsConstructor))]
    [InlineData(typeof(InvalidComponentPropsProperty))]
    [InlineData(typeof(InvalidComponentPropsField))]
    [InlineData(typeof(InvalidComponentPropsType))]
    public void Create_Invalid_Props(Type componentType)
    {
        var componentActivator = new ComponentActivator(componentType, new());

        Assert.Throws<InvalidOperationException>(() =>
        {
            var component = componentActivator.Create();
        });
    }

    [Fact]
    public void AutowiredProps_Invalid_Parameters()
    {
        var componentActivator = new ComponentActivator(typeof(ComponentWithAutowiredProps), new());

        Assert.Throws<ArgumentNullException>(() =>
        {
            componentActivator.AutowiredProps(null!, BindingFlags.Public);
        });
    }

    [Fact]
    public void AutowiredProps_ReturnOK()
    {
        var componentOptions = new ComponentOptions();
        componentOptions.Props<ComponentOptionsClass1>(p => { });
        var componentActivator = new ComponentActivator(typeof(ComponentWithAutowiredProps), componentOptions);
        var component = new ComponentWithAutowiredProps
        {
            Options = componentOptions
        };

        componentActivator.AutowiredProps(component, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

        Assert.NotNull(component.ComponentPropsAction2);
        Assert.NotNull(component._componentProps2);
    }

    [Fact]
    public void EnsureLegalComponentType_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ComponentActivator.EnsureLegalComponentType(null!);
        });

        var exception1 = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentActivator.EnsureLegalComponentType(typeof(NotComponentClass));
        });
        Assert.Equal("`Furion.Component.Tests.NotComponentClass` type is not assignable from `Furion.Component.ComponentBase`.", exception1.Message);

        var exception2 = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentActivator.EnsureLegalComponentType(typeof(ComponentBase));
        });
        Assert.Equal("Type cannot be a `Furion.Component.ComponentBase` or `Furion.Component.WebComponent`.", exception2.Message);

        var exception3 = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentActivator.EnsureLegalComponentType(typeof(InheritComponentClass));
        });
        Assert.Equal("`Furion.Component.Tests.InheritComponentClass` type cannot inherit from other component types.", exception3.Message);

        var exception4 = Assert.Throws<InvalidOperationException>(() =>
        {
            ComponentActivator.EnsureLegalComponentType(typeof(AbstractComponentClass));
        });
        Assert.Equal("`Furion.Component.Tests.AbstractComponentClass` type must be able to be instantiated.", exception4.Message);
    }
}