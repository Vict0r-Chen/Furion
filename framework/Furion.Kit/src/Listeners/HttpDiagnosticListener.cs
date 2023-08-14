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
                var httpDiagnosticModel = new HttpDiagnosticModel
                {
                    TraceIdentifier = httpContext.TraceIdentifier,
                    RequestPath = httpContext.Request.Path + httpContext.Request.QueryString,
                    RequestMethod = httpContext.Request.Method,
                    StartTimestamp = DateTimeOffset.UtcNow,
                    Query = httpContext.Request.Query.Select(u => new KeyValueModel { Key = u.Key, Value = u.Value }).ToList(),
                    Cookies = httpContext.Request.Cookies.Select(u => new KeyValueModel { Key = u.Key, Value = u.Value }).ToList(),
                    Headers = httpContext.Request.Headers.Select(u => new KeyValueModel { Key = u.Key, Value = u.Value }).ToList(),
                };

                var endpoint = httpContext.GetEndpoint();
                if (endpoint is not null)
                {
                    var endpointModel = new EndpointModel
                    {
                        DisplayName = endpoint.DisplayName
                    };

                    if (endpoint is RouteEndpoint routeEndpoint)
                    {
                        endpointModel.RoutePattern = routeEndpoint.RoutePattern.RawText;
                        endpointModel.Order = routeEndpoint.Order;
                        var readOnlyList = endpoint.Metadata.GetMetadata<IHttpMethodMetadata>()?.HttpMethods;
                        if (readOnlyList != null)
                        {
                            endpointModel.HttpMethods = string.Join(", ", readOnlyList);
                        }
                    }
                    httpDiagnosticModel.Endpoint = endpointModel;

                    var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                    if (controllerActionDescriptor is not null)
                    {
                        httpDiagnosticModel.DisplayName = controllerActionDescriptor.MethodInfo.GetCustomAttribute<DisplayNameAttribute>(false)?.DisplayName;
                        httpDiagnosticModel.ControllerType = controllerActionDescriptor.ControllerTypeInfo.ToString();
                        httpDiagnosticModel.MethodName = controllerActionDescriptor.MethodInfo.ToString();
                    }
                }

                if (_httpDiagnosticModelsCache.TryAdd(httpDiagnosticModel.TraceIdentifier, httpDiagnosticModel))
                {
                    _ = WriteAsync(httpDiagnosticModel);
                }
            }
        }

        // 监听筛选器执行完成事件
        if (data.Value is AfterActionFilterOnActionExecutedEventData eventData)
        {
            var exception = eventData.ActionExecutedContext.Exception;
            if (exception is not null)
            {
                var httpContext = eventData.ActionExecutedContext.HttpContext;

                if (_httpDiagnosticModelsCache.TryGetValue(httpContext.TraceIdentifier, out var httpDiagnosticModel)
                    && httpDiagnosticModel.Exception is null)
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
                    httpDiagnosticModel.StatusCode = httpContext.Response.StatusCode;
                    httpDiagnosticModel.EndTimestamp = DateTimeOffset.UtcNow;

                    _ = WriteAsync(httpDiagnosticModel);
                }
            }
        }
    }

    /// <summary>
    /// 获取匿名类 httpContext 属性访问器（后续这里抽离出一个对象，包括序列化也是，可以针对每一个类型进行序列化配置）
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