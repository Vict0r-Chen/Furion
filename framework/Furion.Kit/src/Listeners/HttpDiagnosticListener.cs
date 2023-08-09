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
/// HTTP 诊断监听器
/// </summary>
internal sealed class HttpDiagnosticListener : DiagnosticListenerBase<HttpDiagnosticModel>
{
    /// <summary>
    /// HTTP 诊断模型缓存集合
    /// </summary>
    internal readonly ConcurrentDictionary<string, HttpDiagnosticModel> _httpDiagnosticModelsCache;

    /// <summary>
    /// 匿名类 httpContext 属性访问器
    /// </summary>
    internal Delegate? _anonymousHttpContextGetter;

    /// <summary>
    /// <inheritdoc cref="HttpDiagnosticListener"/>
    /// </summary>
    /// <param name="capacity">诊断订阅器通道容量</param>
    internal HttpDiagnosticListener(int capacity = 3000)
        : base("Microsoft.AspNetCore", capacity)
    {
        _httpDiagnosticModelsCache = new(StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    internal override void OnSubscribe(KeyValuePair<string, object?> data)
    {
        // 监听路由匹配事件
        if (data.Key == "Microsoft.AspNetCore.Routing.EndpointMatched")
        {
            if (data.Value is HttpContext httpContext)
            {
                var httpDiagnosticModel = _httpDiagnosticModelsCache.GetOrAdd(httpContext.TraceIdentifier, new HttpDiagnosticModel
                {
                    TraceIdentifier = httpContext.TraceIdentifier,
                    RequestPath = httpContext.Request.Path + httpContext.Request.QueryString,
                    RequestHttpMethod = httpContext.Request.Method
                });

                _ = WriteAsync(httpDiagnosticModel);
            }
        }

        // 监听筛选器执行完成事件
        if (data.Value is AfterActionFilterOnActionExecutedEventData eventData)
        {
            var exception = eventData.ActionExecutedContext.Exception;
            if (exception is not null)
            {
                var httpContext = eventData.ActionExecutedContext.HttpContext;

                if (_httpDiagnosticModelsCache.TryGetValue(httpContext.TraceIdentifier, out var httpDiagnosticModel))
                {
                    httpDiagnosticModel.Exception = exception?.ToString();

                    if (_httpDiagnosticModelsCache.TryUpdate(httpContext.TraceIdentifier, httpDiagnosticModel, httpDiagnosticModel))
                    {
                        _ = WriteAsync(httpDiagnosticModel);
                    }
                }
            }
        }

        // 监听请求完成事件
        if (data.Key == "Microsoft.AspNetCore.Hosting.EndRequest")
        {
            var httpContext = GetAnonymousHttpContextValue(data.Value!) as HttpContext;
            if (httpContext is not null)
            {
                if (_httpDiagnosticModelsCache.TryRemove(httpContext.TraceIdentifier, out var httpDiagnosticModel))
                {
                    httpDiagnosticModel.ResponseStatusCode = httpContext.Response.StatusCode;

                    _ = WriteAsync(httpDiagnosticModel);
                }
            }
        }
    }

    /// <summary>
    /// 获取匿名类 httpContext 属性访问器
    /// </summary>
    /// <param name="anonymousObject">匿名类对象</param>
    /// <returns><see cref="Func{T, TResult}"/></returns>
    internal object? GetAnonymousHttpContextValue(object anonymousObject)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(anonymousObject);

        // 空检查
        if (_anonymousHttpContextGetter is null)
        {
            // 创建 obj 表达式
            var paramExpression = Expression.Parameter(anonymousObject.GetType(), "obj");

            // 创建 obj.httpContext 表达式
            var propertyExpression = Expression.Property(paramExpression, "httpContext");

            // 创建 obj => obj.httpContext 表达式
            var lambdaExpression = Expression.Lambda(propertyExpression, paramExpression);

            // 编译表达式
            _anonymousHttpContextGetter = lambdaExpression.Compile();
        }

        return _anonymousHttpContextGetter.DynamicInvoke(anonymousObject);
    }
}