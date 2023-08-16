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

namespace Furion.Kit;

/// <summary>
/// 异常详细模型
/// </summary>
internal sealed class ExceptionDetailModel
{
    /// <inheritdoc cref="StackFrame"/>
    internal readonly StackFrame _stackFrame;

    /// <summary>
    /// <inheritdoc cref="ExceptionDetailModel"/>
    /// </summary>
    /// <param name="stackFrame"><see cref="StackFrame"/></param>
    internal ExceptionDetailModel(StackFrame stackFrame)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(stackFrame);

        _stackFrame = stackFrame;

        // 初始化
        Initialize();
    }

    /// <summary>
    /// 文件名
    /// </summary>
    public string? FileName { get; internal set; }

    /// <summary>
    /// 出错行号
    /// </summary>
    public int? LineNumber { get; internal set; }

    /// <summary>
    /// 出错行的起始行号
    /// </summary>
    public int? StartLineNumber { get; internal set; }

    /// <summary>
    /// 出错代码行周围内容
    /// </summary>
    public string? SurroundingLinesText { get; internal set; }

    /// <summary>
    /// 出错代码行内容
    /// </summary>
    public string? TargetLineText { get; internal set; }

    /// <summary>
    /// 初始化
    /// </summary>
    internal void Initialize()
    {
        // 获取文件名和出错代码行号
        var fileName = _stackFrame.GetFileName();
        var lineNumber = _stackFrame.GetFileLineNumber();

        // 空检查
        if (!string.IsNullOrEmpty(fileName) && lineNumber != 0)
        {
            // 读取文件出错代码行周围内容
            var surroundingLinesText = ReadSurroundingLines(fileName
                , lineNumber
                , 6
                , out var targetLineText
                , out var startLineNumber);

            FileName = fileName;
            LineNumber = lineNumber;
            StartLineNumber = startLineNumber;
            SurroundingLinesText = surroundingLinesText;
            TargetLineText = targetLineText;
        }
    }

    /// <summary>
    /// 读取文件出错代码行周围内容
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="targetLine">目标行数</param>
    /// <param name="surroundingLines">周围行数</param>
    /// <param name="targetLineContent">出错代码行内容</param>
    /// <param name="startLineNumber">出错行的起始行号</param>
    /// <returns><see cref="string"/></returns>
    internal static string ReadSurroundingLines(string filePath
        , int targetLine
        , int surroundingLines
        , out string? targetLineContent
        , out int? startLineNumber)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        // 初始化目标行内容
        targetLineContent = default;
        startLineNumber = default;

        // 总共要读取的行数
        var linesToRead = surroundingLines * 2 + 1;

        // 初始化字符串构建器
        var stringBuilder = new StringBuilder();

        // 通过流的方式读取文件
        using (var reader = new StreamReader(filePath))
        {
            // 当前行号
            var currentLine = 1;

            // 存储上下行文本
            var lines = new string?[linesToRead];

            // 当前行的索引
            var currentIndex = 0;

            while (!reader.EndOfStream)
            {
                // 读取当前行内容
                var line = reader.ReadLine();

                // 设置目标行的内容
                if (currentLine == targetLine)
                {
                    targetLineContent = line;
                }

                // 检查目标行周围行数边界
                if (currentLine >= targetLine - surroundingLines
                    && currentLine <= targetLine + surroundingLines)
                {
                    // 设置出错行的起始行号
                    startLineNumber ??= currentLine;

                    // 存储上下行文本
                    lines[currentIndex] = line;
                    currentIndex++;
                }

                currentLine++;
            }

            // 构建结果字符串
            foreach (var line in lines)
            {
                if (line is not null)
                {
                    stringBuilder.AppendLine(line);
                }
            }
        }

        return stringBuilder.ToString();
    }
}