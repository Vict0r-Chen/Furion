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

namespace Furion.Exception.Tests;

public class ExceptionCodeParserTests : IDisposable
{
    [Fact]
    public void Instance_ReturnOK()
    {
        Assert.NotNull(ExceptionCodeParser._instance);
        Assert.NotNull(ExceptionCodeParser.Instance);
        Assert.NotNull(ExceptionCodeParser.Instance._codeMessagesCache);
        Assert.Empty(ExceptionCodeParser.Instance._codeMessagesCache);
    }

    [Fact]
    public void Parse_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ExceptionCodeParser.Parse(null!);
        });
    }

    [Theory]
    [InlineData("出错啦", "出错啦")]
    [InlineData(true, "True")]
    [InlineData(false, "False")]
    [InlineData(0, "0")]
    [InlineData(ExceptionCodeModel.None, "None")]
    [InlineData(ExceptionCodeModel.Default, "缺省值")]
    [InlineData(ExceptionCodeModel.Other, "其他  的", "")]
    [InlineData("出错啦 {0} ~", "出错啦 furion ~", "furion")]
    [InlineData(ExceptionCodeModel.Other, "其他 furion 的", "furion")]
    public void Parse_ReturnOK(object code, string result, params object?[] args)
    {
        Assert.Equal(result, ExceptionCodeParser.Parse(code, args));
    }

    [Fact]
    public void ParseEnum_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ExceptionCodeParser.Instance.ParseEnum(null!);
        });
    }

    [Fact]
    public void ParseEnum_ReturnOK()
    {
        Assert.Equal("None", ExceptionCodeParser.Instance.ParseEnum(ExceptionCodeModel.None));
        Assert.Single(ExceptionCodeParser.Instance._codeMessagesCache);
        Assert.Equal("Furion.Exception.Tests.ExceptionCodeModel.None", ExceptionCodeParser.Instance._codeMessagesCache.Keys.Last());
        Assert.Equal("None", ExceptionCodeParser.Instance._codeMessagesCache.Values.Last());

        Assert.Equal("缺省值", ExceptionCodeParser.Instance.ParseEnum(ExceptionCodeModel.Default));
        Assert.Equal(2, ExceptionCodeParser.Instance._codeMessagesCache.Count);
        Assert.Equal("Furion.Exception.Tests.ExceptionCodeModel.Default", ExceptionCodeParser.Instance._codeMessagesCache.Keys.First());
        Assert.Equal("缺省值", ExceptionCodeParser.Instance._codeMessagesCache.Values.First());
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        Thread.Sleep(10);
        ExceptionCodeParser.Instance._codeMessagesCache.Clear();
    }
}