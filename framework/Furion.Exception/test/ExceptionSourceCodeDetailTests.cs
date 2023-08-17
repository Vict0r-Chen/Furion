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

public class ExceptionSourceCodeDetailTests
{
    [Fact]
    public void New_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var exceptionSourceCodeDetail = new ExceptionSourceCodeDetail(null!, 0, 0);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var exceptionSourceCodeDetail = new ExceptionSourceCodeDetail(string.Empty, 0, 0);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var exceptionSourceCodeDetail = new ExceptionSourceCodeDetail("", 0, 0);
        });

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            var exceptionSourceCodeDetail = new ExceptionSourceCodeDetail("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", 0, 0);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'lineNumber')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            var exceptionSourceCodeDetail = new ExceptionSourceCodeDetail("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", -1, 0);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'lineNumber')", exception2.Message);

        var exception3 = Assert.Throws<ArgumentException>(() =>
        {
            var exceptionSourceCodeDetail = new ExceptionSourceCodeDetail("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", 4, 0);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'startingLineNumber')", exception3.Message);

        var exception4 = Assert.Throws<ArgumentException>(() =>
        {
            var exceptionSourceCodeDetail = new ExceptionSourceCodeDetail("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", 4, 0);
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'startingLineNumber')", exception4.Message);

        var exception5 = Assert.Throws<ArgumentException>(() =>
        {
            var exceptionSourceCodeDetail = new ExceptionSourceCodeDetail("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", 4, 4);
        });
        Assert.Equal("The starting line number cannot be greater than or equal to the line number. (Parameter 'startingLineNumber')", exception5.Message);

        var exception6 = Assert.Throws<ArgumentException>(() =>
        {
            var exceptionSourceCodeDetail = new ExceptionSourceCodeDetail("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", 4, 6);
        });
        Assert.Equal("The starting line number cannot be greater than or equal to the line number. (Parameter 'startingLineNumber')", exception6.Message);
    }

    [Fact]
    public void New_ReturnOK()
    {
        var exceptionSourceCodeDetail = new ExceptionSourceCodeDetail("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", 5, 2)
        {
            TargetLineText = "var i = 10;",
            SurroundingLinesText = "{\r\n\r\n var i = 10; \r\n\r\n}"
        };

        Assert.NotNull(exceptionSourceCodeDetail);
        Assert.Equal("C:\\Workspace\\furion.net\\Furion\\framework\\Furion.Exception\\src\\Exceptions\\UserFriendlyException.cs", exceptionSourceCodeDetail.FileName);
        Assert.Equal(5, exceptionSourceCodeDetail.LineNumber);
        Assert.Equal(2, exceptionSourceCodeDetail.StartingLineNumber);
        Assert.Equal("var i = 10;", exceptionSourceCodeDetail.TargetLineText);
        Assert.Equal("{\r\n\r\n var i = 10; \r\n\r\n}", exceptionSourceCodeDetail.SurroundingLinesText);
    }

    [Fact]
    public void EnsureLegalLineNumber_Invalid_Parameters()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            ExceptionSourceCodeDetail.EnsureLegalLineNumber(0, "lineNumber");
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'lineNumber')", exception.Message);

        var exception2 = Assert.Throws<ArgumentException>(() =>
        {
            ExceptionSourceCodeDetail.EnsureLegalLineNumber(-1, "lineNumber");
        });
        Assert.Equal("Cannot be less than or equal to `0`. (Parameter 'lineNumber')", exception2.Message);
    }

    [Fact]
    public void EnsureLegalLineNumber_ReturnOK()
    {
        ExceptionSourceCodeDetail.EnsureLegalLineNumber(1);
    }
}