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

public class ExceptionSourceCodeParserTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var exceptionSourceCodeParser = new ExceptionSourceCodeParser(null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            var exceptionSourceCodeParser = new ExceptionSourceCodeParser(new("出错了"), 0);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'surroundingLines')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            var exceptionSourceCodeParser = new ExceptionSourceCodeParser(new("出错了"), -1);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'surroundingLines')", exception2.Message);
    }

    [Fact]
    public void New_ReturnOK()
    {
        var exception = Assert.Throws<System.DivideByZeroException>(() =>
        {
            var i = 0;
            var j = 1;
            var k = j / i;
        });

        var exceptionSourceCodeParser = new ExceptionSourceCodeParser(exception);

        Assert.NotNull(exceptionSourceCodeParser);
        Assert.NotNull(exceptionSourceCodeParser._exception);
        Assert.Equal(6, exceptionSourceCodeParser.SurroundingLines);
        Assert.NotNull(exceptionSourceCodeParser.StackFrames);
        Assert.True(exceptionSourceCodeParser.StackFrames.Length > 0);

        var exceptionSourceCodeParser2 = new ExceptionSourceCodeParser(exception, 10);
        Assert.NotNull(exceptionSourceCodeParser2);
        Assert.NotNull(exceptionSourceCodeParser2._exception);
        Assert.Equal(10, exceptionSourceCodeParser2.SurroundingLines);
        Assert.NotNull(exceptionSourceCodeParser2.StackFrames);
        Assert.True(exceptionSourceCodeParser2.StackFrames.Length > 0);
    }

    [Fact]
    public void ReadSurroundingLines_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ExceptionSourceCodeParser.ReadSurroundingLines(null!, 0, 0, out _, out _);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            ExceptionSourceCodeParser.ReadSurroundingLines(string.Empty, 0, 0, out _, out _);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            ExceptionSourceCodeParser.ReadSurroundingLines("", 0, 0, out _, out _);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            ExceptionSourceCodeParser.ReadSurroundingLines("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", 0, 0, out _, out _);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'lineNumber')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            ExceptionSourceCodeParser.ReadSurroundingLines("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", -1, 0, out _, out _);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'lineNumber')", exception2.Message);

        var exception3 = Assert.Throws<ArgumentException>(() =>
        {
            ExceptionSourceCodeParser.ReadSurroundingLines("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", 1, 0, out _, out _);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'surroundingLines')", exception3.Message);

        var exception4 = Assert.Throws<ArgumentException>(() =>
        {
            ExceptionSourceCodeParser.ReadSurroundingLines("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", 1, -1, out _, out _);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'surroundingLines')", exception4.Message);
    }

    [Fact]
    public void Parse_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ExceptionSourceCodeParser.Parse(null!);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            ExceptionSourceCodeParser.Parse(new System.Exception("出错了"), 0);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'surroundingLines')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            ExceptionSourceCodeParser.Parse(new System.Exception("出错了"), -1);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'surroundingLines')", exception2.Message);
    }

    [Fact]
    public void Parse_ReturnOK()
    {
        var exception = Assert.Throws<NotImplementedException>(() =>
        {
            new ExceptionSourceCodeParserModels().TestThrow();
        });

        var list = ExceptionSourceCodeParser.Parse(exception, 1).ToList();

        Assert.NotNull(list);
        Assert.Equal(2, list.Count);

        var firstSourceCode = list[0];
        Assert.Equal(21, firstSourceCode.LineNumber);
        Assert.Equal(20, firstSourceCode.StartingLineNumber);
        Assert.Equal("        throw new NotImplementedException();", firstSourceCode.TargetLineText);
        Assert.Equal("    {\r\n        throw new NotImplementedException();\r\n    }", firstSourceCode.SurroundingLinesText);
    }

    [Fact]
    public void ParseStackFrames_Invalid_Parameters()
    {
        var exceptionSourceCodeParser = new ExceptionSourceCodeParser(new System.Exception("出错了"));

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            exceptionSourceCodeParser.SurroundingLines = 0;
            var list = exceptionSourceCodeParser.ParseStackFrames().ToList();
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'SurroundingLines')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            exceptionSourceCodeParser.SurroundingLines = -1;
            var list = exceptionSourceCodeParser.ParseStackFrames().ToList();
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'SurroundingLines')", exception2.Message);
    }

    [Fact]
    public void ParseStackFrames_ReturnOK()
    {
        var exception = Assert.Throws<NotImplementedException>(() =>
        {
            new ExceptionSourceCodeParserModels().TestThrow();
        });

        var exceptionSourceCodeParser = new ExceptionSourceCodeParser(exception, 1);
        var list = exceptionSourceCodeParser.ParseStackFrames().ToList();

        Assert.NotNull(list);
        Assert.Equal(2, list.Count);

        var firstSourceCode = list[0];
        Assert.Equal(21, firstSourceCode.LineNumber);
        Assert.Equal(20, firstSourceCode.StartingLineNumber);
        Assert.Equal("        throw new NotImplementedException();", firstSourceCode.TargetLineText);
        Assert.Equal("    {\r\n        throw new NotImplementedException();\r\n    }", firstSourceCode.SurroundingLinesText);
    }

    [Fact]
    public void ReadSurroundingLines_FileNotExists_ReturnOK()
    {
        var result = ExceptionSourceCodeParser.ReadSurroundingLines("C:\\Test\\NotFound.cs", 1, 1, out _, out _);

        Assert.Null(result);
    }

    [Fact]
    public void ReadSurroundingLines_ReturnOK()
    {
        var result = ExceptionSourceCodeParser.ReadSurroundingLines("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\test\\ExceptionSourceCodeParserModels.cs", 21, 1, out var targetLineText, out var startingLineNumber);

        Assert.NotNull(result);
        Assert.Equal(20, startingLineNumber);
        Assert.Equal("        throw new NotImplementedException();", targetLineText);
        Assert.Equal("    {\r\n        throw new NotImplementedException();\r\n    }", result);

        var result2 = ExceptionSourceCodeParser.ReadSurroundingLines("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\test\\ExceptionSourceCodeParserModels.cs", 21, 30, out var targetLineText2, out var startingLineNumber2);

        Assert.NotNull(result2);
        Assert.Equal(1, startingLineNumber2);
        Assert.Equal("        throw new NotImplementedException();", targetLineText2);
        Assert.Equal("// 麻省理工学院许可证\r\n//\r\n// 版权所有 © 2020-2023 百小僧，百签科技（广东）有限公司\r\n//\r\n// 特此免费授予获得本软件及其相关文档文件（以下简称“软件”）副本的任何人以处理本软件的权利，\r\n// 包括但不限于使用、复制、修改、合并、发布、分发、再许可、销售软件的副本，\r\n// 以及允许拥有软件副本的个人进行上述行为，但须遵守以下条件：\r\n//\r\n// 在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。\r\n//\r\n// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。\r\n// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，\r\n// 无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。\r\n\r\nnamespace Furion.Exception.Tests;\r\n\r\npublic class ExceptionSourceCodeParserModels\r\n{\r\n    public void TestThrow()\r\n    {\r\n        throw new NotImplementedException();\r\n    }\r\n}", result2);
    }
}