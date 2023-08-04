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

public class DebuggingTests : IDisposable
{
    private readonly StringWriter _stringWriter;
    private readonly TextWriterTraceListener _traceListener;

    public DebuggingTests()
    {
        _stringWriter = new();
        _traceListener = new(_stringWriter);
        Trace.Listeners.Add(_traceListener);
    }

    [Theory]
    [InlineData(1, "🛠️", "跟踪事件信息")]
    [InlineData(2, "ℹ️", "信息事件信息")]
    [InlineData(3, "⚠️", "警告事件信息")]
    [InlineData(4, "❌", "错误事件信息")]
    [InlineData(5, "📄", "文件事件信息")]
    [InlineData(6, "💡", "提示事件信息")]
    [InlineData(7, "🔍", "搜索事件信息")]
    [InlineData(8, "⏱️", "时钟事件信息")]
    [InlineData(9, "", "其他事件信息")]
    public void WriteLine_OuputString(int level, string emoji, string message)
    {
        Debugging.WriteLine(level, message);

        var output = _stringWriter.ToString();
        var expected = $"{emoji}: {message}\r\n";
        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData(1, "🛠️", "跟踪事件信息 {0}-{1}", "one", "two")]
    [InlineData(2, "ℹ️", "信息事件信息 {0}-{1}", "one", "two")]
    [InlineData(3, "⚠️", "警告事件信息 {0}-{1}", "one", "two")]
    [InlineData(4, "❌", "错误事件信息 {0}-{1}", "one", "two")]
    [InlineData(5, "📄", "文件事件信息 {0}-{1}", "one", "two")]
    [InlineData(6, "💡", "提示事件信息 {0}-{1}", "one", "two")]
    [InlineData(7, "🔍", "搜索事件信息 {0}-{1}", "one", "two")]
    [InlineData(8, "⏱️", "时钟事件信息 {0}-{1}", "one", "two")]
    [InlineData(9, "", "其他事件信息 {0}-{1}", "one", "two")]
    public void WriteLine_OuputFormatString(int level, string emoji, string message, params string[] args)
    {
        Debugging.WriteLine(level, message, args);

        var output = _stringWriter.ToString();
        var expected = string.Format($"{emoji}: {message}\r\n", args);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Trace_OutputString()
    {
        var message = "跟踪事件信息";
        Debugging.Trace(message);

        var output = _stringWriter.ToString();
        var expected = $"🛠️: {message}\r\n";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Trace_OutputFormatString()
    {
        string[] args = { "one", "two" };
        var message = "跟踪事件信息 {0}-{1}";
        Debugging.Trace(message, args);

        var output = _stringWriter.ToString();
        var expected = string.Format($"🛠️: {message}\r\n", args);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Info_OutputString()
    {
        var message = "信息事件信息";
        Debugging.Info(message);

        var output = _stringWriter.ToString();
        var expected = $"ℹ️: {message}\r\n";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Info_OutputFormatString()
    {
        string[] args = { "one", "two" };
        var message = "信息事件信息 {0}-{1}";
        Debugging.Info(message, args);

        var output = _stringWriter.ToString();
        var expected = string.Format($"ℹ️: {message}\r\n", args);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Warn_OutputString()
    {
        var message = "警告事件信息";
        Debugging.Warn(message);

        var output = _stringWriter.ToString();
        var expected = $"⚠️: {message}\r\n";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Warn_OutputFormatString()
    {
        string[] args = { "one", "two" };
        var message = "警告事件信息 {0}-{1}";
        Debugging.Warn(message, args);

        var output = _stringWriter.ToString();
        var expected = string.Format($"⚠️: {message}\r\n", args);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Error_OutputString()
    {
        var message = "错误事件信息";
        Debugging.Error(message);

        var output = _stringWriter.ToString();
        var expected = $"❌: {message}\r\n";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Error_OutputFormatString()
    {
        string[] args = { "one", "two" };
        var message = "错误事件信息 {0}-{1}";
        Debugging.Error(message, args);

        var output = _stringWriter.ToString();
        var expected = string.Format($"❌: {message}\r\n", args);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void File_OutputString()
    {
        var message = "文件事件信息";
        Debugging.File(message);

        var output = _stringWriter.ToString();
        var expected = $"📄: {message}\r\n";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void File_OutputFormatString()
    {
        string[] args = { "one", "two" };
        var message = "文件事件信息 {0}-{1}";
        Debugging.File(message, args);

        var output = _stringWriter.ToString();
        var expected = string.Format($"📄: {message}\r\n", args);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Tip_OutputString()
    {
        var message = "提示事件信息";
        Debugging.Tip(message);

        var output = _stringWriter.ToString();
        var expected = $"💡: {message}\r\n";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Tip_OutputFormatString()
    {
        string[] args = { "one", "two" };
        var message = "提示事件信息 {0}-{1}";
        Debugging.Tip(message, args);

        var output = _stringWriter.ToString();
        var expected = string.Format($"💡: {message}\r\n", args);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Search_OutputString()
    {
        var message = "搜索事件信息";
        Debugging.Search(message);

        var output = _stringWriter.ToString();
        var expected = $"🔍: {message}\r\n";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Search_OutputFormatString()
    {
        string[] args = { "one", "two" };
        var message = "搜索事件信息 {0}-{1}";
        Debugging.Search(message, args);

        var output = _stringWriter.ToString();
        var expected = string.Format($"🔍: {message}\r\n", args);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Clock_OutputString()
    {
        var message = "时钟事件信息";
        Debugging.Clock(message);

        var output = _stringWriter.ToString();
        var expected = $"⏱️: {message}\r\n";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Clock_OutputFormatString()
    {
        string[] args = { "one", "two" };
        var message = "时钟事件信息 {0}-{1}";
        Debugging.Clock(message, args);

        var output = _stringWriter.ToString();
        var expected = string.Format($"⏱️: {message}\r\n", args);
        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData(1, "🛠️")]
    [InlineData(2, "ℹ️")]
    [InlineData(3, "⚠️")]
    [InlineData(4, "❌")]
    [InlineData(5, "📄")]
    [InlineData(6, "💡")]
    [InlineData(7, "🔍")]
    [InlineData(8, "⏱️")]
    [InlineData(9, "")]
    public void GetLevelEmoji_InputIntLevel_ReturnEmojiString(int level, string result)
    {
        var emoji = Debugging.GetLevelEmoji(level);
        Assert.Equal(result, emoji);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        Trace.Listeners.Remove(_traceListener);
        _traceListener.Dispose();
        _stringWriter.Dispose();
    }
}