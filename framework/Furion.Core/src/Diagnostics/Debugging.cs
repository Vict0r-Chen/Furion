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

namespace System;

/// <summary>
/// 内部调试事件消息
/// </summary>
internal static class Debugging
{
    /// <summary>
    /// 输出一行消息
    /// </summary>
    /// <param name="level">调试级别：1/跟踪；2/信息；3/警告；4/错误</param>
    /// <param name="message">消息</param>
    internal static void WriteLine(int level, string message)
    {
        var emoji = level switch
        {
            1 => "🛠️",
            2 => "ℹ️",
            3 => "⚠️",
            4 => "❌",
            _ => "🛠️"
        };

        // 只有调试状态下输出
        if (!Debugger.IsAttached)
        {
            return;
        }

        Debug.WriteLine(message, category: emoji);
    }

    /// <summary>
    /// 输出一行消息
    /// </summary>
    /// <param name="level">调试级别：1/信息；2/警告；3/错误</param>
    /// <param name="message">消息</param>
    /// <param name="args">格式化参数</param>
    internal static void WriteLine(int level, string message, params object?[] args)
    {
        WriteLine(level, string.Format(message, args));
    }

    /// <summary>
    /// 输出跟踪消息
    /// </summary>
    /// <param name="message">消息</param>
    internal static void Trace(string message)
    {
        WriteLine(1, message);
    }

    /// <summary>
    /// 输出跟踪消息
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">格式化参数</param>
    internal static void Trace(string message, params object?[] args)
    {
        Trace(string.Format(message, args));
    }

    /// <summary>
    /// 输出信息消息
    /// </summary>
    /// <param name="message">消息</param>
    internal static void Info(string message)
    {
        WriteLine(2, message);
    }

    /// <summary>
    /// 输出信息消息
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">格式化参数</param>
    internal static void Info(string message, params object?[] args)
    {
        Info(string.Format(message, args));
    }

    /// <summary>
    /// 输出警告消息
    /// </summary>
    /// <param name="message">消息</param>
    internal static void Warn(string message)
    {
        WriteLine(3, message);
    }

    /// <summary>
    /// 输出警告消息
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">格式化参数</param>
    internal static void Warn(string message, params object?[] args)
    {
        Warn(string.Format(message, args));
    }

    /// <summary>
    /// 输出错误消息
    /// </summary>
    /// <param name="message">消息</param>
    internal static void Error(string message)
    {
        WriteLine(4, message);
    }

    /// <summary>
    /// 输出错误消息
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="args">格式化参数</param>
    internal static void Error(string message, params object?[] args)
    {
        Error(string.Format(message, args));
    }
}