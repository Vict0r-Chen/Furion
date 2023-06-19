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

        Assert.NotNull(options.OptionsActions);
        Assert.Empty(options.OptionsActions);
        Assert.NotNull(options.CallRecords);
        Assert.Empty(options.CallRecords);
        Assert.True(options.SuppressDuplicateCall);
        Assert.True(options.SuppressDuplicateCallForWeb);

        Assert.NotNull(options._GetOptionsActionOrNewMethod);
    }

    [Fact]
    public void GetOptionsAction_NotExists_ReturnNull()
    {
        var options = new ComponentOptions();
        var action = options.GetOptionsAction<ComponentActionOptions>();
        Assert.Null(action);
    }

    [Fact]
    public void GetOptionsAction_Exists_ReturnAction()
    {
        var options = new ComponentOptions();
        static void Action(ComponentActionOptions options)
        {
        }
        options.OptionsActions.Add(typeof(ComponentActionOptions), new List<Delegate> { Action });

        var action = options.GetOptionsAction<ComponentActionOptions>();
        Assert.NotNull(action);
    }

    [Fact]
    public void GetOptionsAction_MultipleActions()
    {
        var callRecord = new List<string>();
        var options = new ComponentOptions();

        void Action(ComponentActionOptions options)
        {
            callRecord.Add(nameof(Action));
        }

        void Action2(ComponentActionOptions options)
        {
            callRecord.Add(nameof(Action2));
        }
        options.OptionsActions.Add(typeof(ComponentActionOptions), new List<Delegate> { Action, Action2 });

        var action = options.GetOptionsAction<ComponentActionOptions>();
        Assert.NotNull(action);

        action(new ComponentActionOptions());

        Assert.Equal(2, callRecord.Count);
        Assert.Equal(nameof(Action), callRecord[0]);
        Assert.Equal(nameof(Action2), callRecord[1]);
    }

    [Fact]
    public void GetOptionsActionOrNew_NotExists_ReturnAction()
    {
        var options = new ComponentOptions();
        var action = options.GetOptionsActionOrNew<ComponentActionOptions>();
        Assert.NotNull(action);
    }

    [Fact]
    public void GetOptionsActionOrNew_ForType()
    {
        var options = new ComponentOptions();
        var @delegate = options.GetOptionsActionOrNew(typeof(ComponentActionOptions));
        Assert.NotNull(@delegate);

        var action = (Action<ComponentActionOptions>)@delegate;
        Assert.NotNull(action);
    }

    [Fact]
    public void CheckIndex()
    {
        var options = new ComponentOptions()
        {
            SuppressDuplicateCall = false,
            SuppressDuplicateCallForWeb = false
        };

        var suppressDuplicateCall = options["SuppressDuplicateCall"];
        var suppressDuplicateCallForWeb = options["SuppressDuplicateCallForWeb"];
        Assert.False(suppressDuplicateCall);
        Assert.False(suppressDuplicateCallForWeb);

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var undefined = options["Undefined"];
        });
        Assert.Equal("Unsupported property name index.", exception.Message);
    }
}