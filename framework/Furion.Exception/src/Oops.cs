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

namespace System;

/// <summary>
/// 异常静态类
/// </summary>
public static class Oops
{
    /// <summary>
    /// 抛出用户友好异常
    /// </summary>
    [DoesNotReturn]
    public static void Throw()
    {
        Throw(Constants.DEFAULT_EXCEPTION_MESSAGE);
    }

    /// <summary>
    /// 抛出用户友好异常
    /// </summary>
    /// <param name="code">异常编码</param>
    /// <param name="args">格式化参数</param>
    [DoesNotReturn]
    public static void Throw(object? code, params object?[] args)
    {
        Throw<object>(code, args);
    }

    /// <summary>
    /// 抛出用户友好异常
    /// </summary>
    /// <typeparam name="TResult">异常返回类型</typeparam>
    /// <param name="code">异常编码</param>
    /// <param name="args">格式化参数</param>
    /// <returns><typeparamref name="TResult"/></returns>
    /// <exception cref="UserFriendlyException"></exception>
    [DoesNotReturn]
    public static TResult Throw<TResult>(object? code, params object?[] args)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(code);

        // 解析异常编码并返回异常信息
        var errorMessage = ExceptionCodeParser.Parse(code, args);

        // 返回用户友好异常
        throw new UserFriendlyException(errorMessage)
        {
            Code = code
        };
    }

    /// <summary>
    /// 抛出用户友好异常
    /// </summary>
    /// <typeparam name="TResult">异常返回类型</typeparam>
    /// <param name="code">异常编码</param>
    /// <param name="exceptionType">异常类型</param>
    /// <param name="args">格式化参数</param>
    /// <returns><typeparamref name="TResult"/></returns>
    /// <exception cref="UserFriendlyException"></exception>
    [DoesNotReturn]
    public static TResult Throw<TResult>(object? code, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type exceptionType, params object?[] args)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(code);

        // 解析异常编码并返回异常信息
        var errorMessage = ExceptionCodeParser.Parse(code, args);

        // 反射创建异常实例
        var exception = Activator.CreateInstance(exceptionType, new[] { errorMessage }) as System.Exception;

        // 空检查
        ArgumentNullException.ThrowIfNull(exception);

        // 返回用户友好异常
        throw new UserFriendlyException(null, exception)
        {
            Code = code
        };
    }

    /// <summary>
    /// 若参数值为 <see langword="null"/> 则抛出用户友好参数异常
    /// </summary>
    /// <param name="argument">参数值</param>
    /// <param name="paramName">参数名称</param>
    public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument == null)
        {
            ArgumentThrowHelpers.Throw(paramName!);
        }
    }

    /// <summary>
    /// 若参数值为 <see langword="null"/> 或空白字符串则抛出用户友好参数异常
    /// </summary>
    /// <param name="argument">参数值</param>
    /// <param name="paramName">参数名称</param>
    public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            ArgumentThrowHelpers.ThrowNullOrWhiteSpaceException(argument!, paramName!);
        }
    }

    /// <summary>
    /// 若参数值为 <see langword="null"/> 或空字符串则抛出用户友好参数异常
    /// </summary>
    /// <param name="argument">参数值</param>
    /// <param name="paramName">参数名称</param>
    public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            ArgumentThrowHelpers.ThrowNullOrEmptyException(argument!, paramName!);
        }
    }

    /// <summary>
    /// 若参数值为 <see langword="null"/> 或空集合则抛出用户友好参数异常
    /// </summary>
    /// <typeparam name="T">集合项类型</typeparam>
    /// <param name="argument">参数值</param>
    /// <param name="paramName">参数名称</param>
    public static void ThrowIfNullOrEmpty<T>([NotNull] ICollection<T>? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null or { Count: 0 })
        {
            ArgumentThrowHelpers.ThrowNullOrEmptyException(argument, paramName!);
        }
    }
}