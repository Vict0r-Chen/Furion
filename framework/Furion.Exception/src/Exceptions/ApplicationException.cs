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
/// 应用程序异常抽象基类
/// </summary>
public abstract class ApplicationException : System.Exception
{
    /// <summary>
    /// <inheritdoc cref="ApplicationException" />
    /// </summary>
    public ApplicationException()
        : base()
    {
    }

    /// <summary>
    /// <inheritdoc cref="ApplicationException" />
    /// </summary>
    /// <param name="message">异常信息</param>
    public ApplicationException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// <inheritdoc cref="ApplicationException" />
    /// </summary>
    /// <param name="message">异常信息</param>
    /// <param name="innerException">内部异常</param>
    public ApplicationException(string? message, System.Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// <inheritdoc cref="ApplicationException" />
    /// </summary>
    /// <param name="info"><see cref="SerializationInfo"/></param>
    /// <param name="context"><see cref="StreamingContext"/></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    protected ApplicationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// 异常时间
    /// </summary>
    public DateTimeOffset TimeForException { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// 解决方案
    /// </summary>
    public string? Solution { get; set; }

    /// <summary>
    /// 附加属性
    /// </summary>
    public IDictionary<object, object?>? Properties { get; set; }
}