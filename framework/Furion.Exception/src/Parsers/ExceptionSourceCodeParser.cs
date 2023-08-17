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
/// 异常源码解析器
/// </summary>
public sealed class ExceptionSourceCodeParser
{
    /// <inheritdoc cref="System.Exception"/>
    internal readonly System.Exception _exception;

    /// <summary>
    /// <inheritdoc cref="ExceptionCodeParser"/>
    /// </summary>
    /// <param name="exception"><see cref="System.Exception"/></param>
    public ExceptionSourceCodeParser(System.Exception exception)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(exception);

        _exception = exception;
        StackFrames = new StackTrace(exception, true).GetFrames();
    }

    /// <summary>
    /// <inheritdoc cref="ExceptionCodeParser"/>
    /// </summary>
    /// <param name="exception"><see cref="System.Exception"/></param>
    /// <param name="surroundingLines">目标行号上下行数</param>
    /// <exception cref="ArgumentException"></exception>
    public ExceptionSourceCodeParser(System.Exception exception, int surroundingLines)
        : this(exception)
    {
        // 检查行号合法性
        ExceptionSourceCode.EnsureLegalLineNumber(surroundingLines);

        SurroundingLines = surroundingLines;
    }

    /// <summary>
    /// 堆栈帧集合
    /// </summary>
    public StackFrame[] StackFrames { get; init; }

    /// <summary>
    /// 目标行号上下行数
    /// </summary>
    public int SurroundingLines { get; set; } = 6;

    /// <summary>
    /// 解析异常并返回异常源码集合
    /// </summary>
    /// <param name="exception"><see cref="System.Exception"/></param>
    /// <param name="surroundingLines">目标行号上下行数</param>
    /// <returns><see cref="List{T}"/></returns>
    public static List<ExceptionSourceCode> Parse(System.Exception exception, int surroundingLines = 6)
    {
        return new ExceptionSourceCodeParser(exception, surroundingLines)
            .ParseStackFrames()
            .ToList();
    }

    /// <summary>
    /// 解析堆栈帧集合并创建异常源码集合
    /// </summary>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    public IEnumerable<ExceptionSourceCode> ParseStackFrames()
    {
        // 检查行号合法性
        ExceptionSourceCode.EnsureLegalLineNumber(SurroundingLines);

        // 遍历堆栈帧集合并创建异常源码集合
        foreach (var stackFrame in StackFrames)
        {
            // 获取文件名和目标行号
            var (fileName, lineNumber) = (stackFrame.GetFileName(), stackFrame.GetFileLineNumber());

            // 空检查
            if (string.IsNullOrWhiteSpace(fileName) || lineNumber <= 0)
            {
                continue;
            }

            // 读取目标上下行（含目标行）内容
            var surroundingLinesText = ReadSurroundingLines(fileName
                , lineNumber
                , SurroundingLines
                , out var targetLineText
                , out var startingLineNumber);

            // 初始化异常源码对象
            var exceptionSourceCode = new ExceptionSourceCode(fileName
                , lineNumber
                , startingLineNumber!.Value)
            {
                SurroundingLinesText = surroundingLinesText,
                TargetLineText = targetLineText
            };

            yield return exceptionSourceCode;
        }
    }

    /// <summary>
    /// 读取目标上下行（含目标行）内容
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="lineNumber">目标行号</param>
    /// <param name="surroundingLines">目标上下行（含目标行）内容</param>
    /// <param name="targetLineText">目标行内容</param>
    /// <param name="startingLineNumber">起始行号</param>
    /// <returns><see cref="string"/></returns>
    internal static string ReadSurroundingLines(string fileName, int lineNumber, int surroundingLines
        , out string? targetLineText
        , out int? startingLineNumber)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        // 检查行号合法性
        ExceptionSourceCode.EnsureLegalLineNumber(lineNumber);
        ExceptionSourceCode.EnsureLegalLineNumber(surroundingLines);

        // 初始化 out 返回值参数
        (targetLineText, startingLineNumber) = (default, default);

        // 初始化当前行号和索引值
        int currentLine = 1, currentIndex = 0;

        // 初始化最小和最大可读行号
        int minLineNumber = lineNumber - surroundingLines
            , maxLineNumber = lineNumber + surroundingLines;

        // 初始化上下行内容数组
        var lines = new string?[surroundingLines * 2 + 1];

        // 初始化文件流读写器
        using var streamReader = new StreamReader(fileName);

        // 循环读取文件每一行内容直至结尾
        while (!streamReader.EndOfStream)
        {
            // 读取当前行内容
            var line = streamReader.ReadLine();

            // 检查当前行号是否在可读行号范围内
            if (currentLine >= minLineNumber && currentLine <= maxLineNumber)
            {
                // 设置起始行号
                startingLineNumber ??= currentLine;

                // 存储当前行内容
                lines[currentIndex] = line;

                // 设置目标行内容
                if (currentLine == lineNumber)
                {
                    targetLineText = line;
                }

                // 检查是否达到最大可读行
                if (currentLine == maxLineNumber)
                {
                    break;
                }

                // 递增数组索引值
                currentIndex++;
            }

            // 递增当前行数
            currentLine++;
        }

        // 初始化字符串构建器
        var stringBuilder = new StringBuilder();

        // 构建目标上下行（含目标行）内容字符串
        foreach (var line in lines)
        {
            // 空检查
            if (line is null)
            {
                break;
            }

            stringBuilder.AppendLine(line);
        }

        return stringBuilder.ToString();
    }
}