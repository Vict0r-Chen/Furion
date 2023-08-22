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
/// 异常模型
/// </summary>
public sealed class ExceptionModel
{
    /// <summary>
    /// <inheritdoc cref="ExceptionModel"/>
    /// </summary>
    /// <param name="exception"><see cref="System.Exception"/></param>
    public ExceptionModel(System.Exception exception)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(exception);

        Message = exception.Message;
        StackTrace = exception.StackTrace;
        HResult = exception.HResult;
        Source = exception.Source;
        HelpLink = exception.HelpLink;
        Type = exception.GetType().ToString();
        RawText = exception.ToString();

        // 解析异常并返回异常源码集合
        Details = ExceptionSourceCodeParser.Parse(exception);
    }

    /// <summary>
    /// 异常信息
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// 异常堆栈
    /// </summary>
    public string? StackTrace { get; init; }

    /// <summary>
    /// 异常编码数字值
    /// </summary>
    public int HResult { get; init; }

    /// <summary>
    /// 异常对象名称
    /// </summary>
    public string? Source { get; init; }

    /// <summary>
    /// 帮助文件链接
    /// </summary>
    public string? HelpLink { get; init; }

    /// <summary>
    /// 异常类型限定名
    /// </summary>
    public string? Type { get; init; }

    /// <summary>
    /// 原始异常内容
    /// </summary>
    public string? RawText { get; init; }

    /// <summary>
    /// 异常源码集合
    /// </summary>
    public List<ExceptionSourceCode>? Details { get; init; }
}