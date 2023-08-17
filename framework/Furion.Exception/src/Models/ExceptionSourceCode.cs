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

namespace Furion.Exception;

/// <summary>
/// 异常源码
/// </summary>
public sealed class ExceptionSourceCode
{
    /// <summary>
    /// <inheritdoc cref="ExceptionSourceCode"/>
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="lineNumber">目标行号</param>
    /// <param name="startingLineNumber">起始行号</param>
    /// <exception cref="ArgumentException"></exception>
    public ExceptionSourceCode(string fileName, int lineNumber, int startingLineNumber)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        // 检查代码行号合法性
        EnsureLegalLineNumber(lineNumber);
        EnsureLegalLineNumber(startingLineNumber);

        // 检查起始行号是否大于等于目标行号
        if (startingLineNumber >= lineNumber)
        {
            throw new ArgumentException("The starting line number cannot be greater than or equal to the line number.", nameof(startingLineNumber));
        }

        FileName = fileName;
        LineNumber = lineNumber;
        StartingLineNumber = startingLineNumber;
    }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; init; }

    /// <summary>
    /// 目标行号
    /// </summary>
    public int LineNumber { get; init; }

    /// <summary>
    /// 起始行号
    /// </summary>
    public int StartingLineNumber { get; init; }

    /// <summary>
    /// 目标行内容
    /// </summary>
    public string? TargetLineText { get; set; }

    /// <summary>
    /// 目标上下行（含目标行）内容
    /// </summary>
    public string? SurroundingLinesText { get; set; }

    /// <summary>
    /// 检查代码行号合法性
    /// </summary>
    /// <param name="lineNumber">代码行号</param>
    /// <param name="paramName">参数名</param>
    /// <exception cref="ArgumentException"></exception>
    internal static void EnsureLegalLineNumber(int lineNumber, [CallerArgumentExpression(nameof(lineNumber))] string? paramName = null)
    {
        if (lineNumber > 0)
        {
            return;
        }

        throw new ArgumentException("Cannot be less than or equal to `0`.", paramName);
    }
}