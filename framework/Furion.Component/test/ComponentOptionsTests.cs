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

public class ComponentOptionsTests
{
    [Fact]
    public void NewInstance_Default()
    {
        var options = new ComponentOptions();
        Assert.NotNull(options);

        Assert.NotNull(options.PropsActions);
        Assert.Empty(options.PropsActions);
        Assert.NotNull(options.InvokeRecords);
        Assert.Empty(options.InvokeRecords);
        Assert.True(options.SuppressDuplicateInvoke);
        Assert.True(options.SuppressDuplicateInvokeForWeb);

        Assert.NotNull(options._GetPropsActionMethod);
    }

    [Fact]
    public void GetPropsAction_NotExists_ReturnNull()
    {
        var options = new ComponentOptions();
        var action = options.GetPropsAction<ComponentActionOptions>();
        Assert.Null(action);
    }

    [Fact]
    public void GetPropsAction_Exists_ReturnAction()
    {
        var options = new ComponentOptions();
        static void Action(ComponentActionOptions options)
        {
        }
        options.PropsActions.Add(typeof(ComponentActionOptions), new List<Delegate> { Action });

        var action = options.GetPropsAction<ComponentActionOptions>();
        Assert.NotNull(action);
    }

    [Fact]
    public void GetPropsAction_MultipleActions()
    {
        var invokeRecord = new List<string>();
        var options = new ComponentOptions();

        void Action(ComponentActionOptions options)
        {
            invokeRecord.Add(nameof(Action));
        }

        void Action2(ComponentActionOptions options)
        {
            invokeRecord.Add(nameof(Action2));
        }
        options.PropsActions.Add(typeof(ComponentActionOptions), new List<Delegate> { Action, Action2 });

        var action = options.GetPropsAction<ComponentActionOptions>();
        Assert.NotNull(action);

        action(new ComponentActionOptions());

        Assert.Equal(2, invokeRecord.Count);
        Assert.Equal(nameof(Action), invokeRecord[0]);
        Assert.Equal(nameof(Action2), invokeRecord[1]);
    }

    [Fact]
    public void GetPropsAction_ForType()
    {
        var options = new ComponentOptions();
        var @delegate = options.GetPropsAction(typeof(ComponentActionOptions));
        Assert.Null(@delegate);

        static void Action(ComponentActionOptions options)
        {
        }

        options.PropsActions.Add(typeof(ComponentActionOptions), new List<Delegate> { Action });
        var delegate2 = options.GetPropsAction(typeof(ComponentActionOptions));
        Assert.NotNull(delegate2);
    }

    [Fact]
    public void CheckIndex()
    {
        var options = new ComponentOptions()
        {
            SuppressDuplicateInvoke = false,
            SuppressDuplicateInvokeForWeb = false
        };

        var suppressDuplicateInvoke = options["SuppressDuplicateInvoke"];
        var suppressDuplicateInvokeForWeb = options["SuppressDuplicateInvokeForWeb"];
        Assert.False(suppressDuplicateInvoke);
        Assert.False(suppressDuplicateInvokeForWeb);

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var undefined = options["Undefined"];
        });
        Assert.Equal("Unsupported property name index.", exception.Message);
    }

    [Fact]
    public void Release()
    {
        var options = new ComponentOptions();
        static void Action(ComponentActionOptions options)
        {
        }
        options.PropsActions.Add(typeof(ComponentActionOptions), new List<Delegate> { Action });

        Assert.Single(options.PropsActions);

        options.Release();
        Assert.Empty(options.PropsActions);
        Assert.Empty(options.InvokeRecords);
    }
}