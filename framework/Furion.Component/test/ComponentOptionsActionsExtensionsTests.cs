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

namespace Furion.Component.Tests;

public class ComponentOptionsActionsExtensionsTests
{
    [Fact]
    public void AddOrUpdate_Generic_ReturnOK()
    {
        var optionsActions = new Dictionary<Type, List<Action<object>>>();

        void action1(DefaultOptions options) { }
        void action2(WithParameterlessOptions options) { }
        static void action3(ManyConstructorOptions options) { }

        optionsActions.AddOrUpdate((Action<DefaultOptions>)action1);
        optionsActions.AddOrUpdate((Action<WithParameterlessOptions>)action2);
        optionsActions.AddOrUpdate((Action<ManyConstructorOptions>)action3);

        void action1Object(object obj) => action1((DefaultOptions)obj);
        void action2Object(object obj) => action2((WithParameterlessOptions)obj);
        void action3Object(object obj) => action3((ManyConstructorOptions)obj);

        optionsActions.AddOrUpdate(typeof(DefaultOptions), action1Object);
        optionsActions.AddOrUpdate(typeof(WithParameterlessOptions), action2Object);
        optionsActions.AddOrUpdate(typeof(ManyConstructorOptions), action3Object);

        Assert.Equal(3, optionsActions.Count);
    }

    [Fact]
    public void AddOrUpdate_Other_ReturnOK()
    {
        var optionsActions = new Dictionary<Type, List<Action<object>>>();

        void action1(DefaultOptions options) { }
        void action2(WithParameterlessOptions options) { }
        static void action3(ManyConstructorOptions options) { }

        optionsActions.AddOrUpdate((Action<DefaultOptions>)action1);
        optionsActions.AddOrUpdate((Action<WithParameterlessOptions>)action2);
        optionsActions.AddOrUpdate((Action<ManyConstructorOptions>)action3);

        var otherOptionsActions = new Dictionary<Type, List<Action<object>>>();

        void action1Object(object obj) => action1((DefaultOptions)obj);
        void action2Object(object obj) => action2((WithParameterlessOptions)obj);
        void action3Object(object obj) => action3((ManyConstructorOptions)obj);

        otherOptionsActions.AddOrUpdate(typeof(DefaultOptions), action1Object);
        otherOptionsActions.AddOrUpdate(typeof(WithParameterlessOptions), action2Object);
        otherOptionsActions.AddOrUpdate(typeof(ManyConstructorOptions), action3Object);

        Assert.Equal(3, optionsActions.Count);
    }

    [Fact]
    public void GetOptions_ReturnOK()
    {
        var optionsActions = new Dictionary<Type, List<Action<object>>>();

        static void action1(DefaultOptions options)
        {
            options.Num += 1;
        }
        static void action2(DefaultOptions options)
        {
            options.Num += 1;
        }

        optionsActions.AddOrUpdate((Action<DefaultOptions>)action1);
        optionsActions.AddOrUpdate((Action<DefaultOptions>)action2);

        var options = optionsActions.GetOptions<DefaultOptions>();
        Assert.NotNull(options);
        Assert.Equal(2, options.Num);
    }

    [Fact]
    public void GetOptions_Null_ReturnOK()
    {
        var optionsActions = new Dictionary<Type, List<Action<object>>>();
        var options = optionsActions.GetOptions<DefaultOptions>();
        Assert.Null(options);
    }
}