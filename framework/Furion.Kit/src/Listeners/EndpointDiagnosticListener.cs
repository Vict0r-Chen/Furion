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
/// 终点路由诊断监听器
/// </summary>
internal sealed class EndpointDiagnosticListener : DiagnosticListenerBase<EndpointDiagnosticModel>
{
    /// <summary>
    /// 终点路由诊断模型缓存集合
    /// </summary>
    internal readonly ConcurrentDictionary<string, EndpointDiagnosticModel> _endpointDiagnosticModelsCache;

    /// <summary>
    /// <inheritdoc cref="EndpointDiagnosticListener" />
    /// </summary>
    /// <param name="capacity">诊断订阅器通道容量</param>
    internal EndpointDiagnosticListener(int capacity = 3000)
        : base("Microsoft.AspNetCore", capacity)
    {
        _endpointDiagnosticModelsCache = new(StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    internal override void OnSubscribe(KeyValuePair<string, object?> data)
    {
        if (data.Key == "Microsoft.AspNetCore.Routing.EndpointMatched")
        {
            RoutingEndpointMatchedHandle(data.Value);
        }

        if (data.Key == "Microsoft.AspNetCore.Mvc.AfterOnActionExecuted")
        {
            MvcAfterOnActionExecutedHandle(data.Value);
        }

        if (data.Key == "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop")
        {
            HostingHttpRequestInStopHandle(data.Value);
        }
    }

    /// <summary>
    /// Microsoft.AspNetCore.Routing.EndpointMatched 事件处理
    /// </summary>
    /// <param name="value">事件负载值</param>
    internal void RoutingEndpointMatchedHandle(object? value)
    {
        // 空检查
        if (value is not HttpContext httpContext)
        {
            return;
        }

        // 初始化终点路由诊断模型
        var endpointDiagnosticModel = new EndpointDiagnosticModel(httpContext);

        // 将终点路由诊断模型缓存到集合中
        if (_endpointDiagnosticModelsCache.TryAdd(httpContext.TraceIdentifier, endpointDiagnosticModel))
        {
            // 将终点路由诊断模型写入诊断订阅器通道
            _ = WriteAsync(endpointDiagnosticModel);
        }
    }

    /// <summary>
    /// Microsoft.AspNetCore.Mvc.AfterOnActionExecuted 事件处理
    /// </summary>
    /// <param name="value">事件负载值</param>
    internal void MvcAfterOnActionExecutedHandle(object? value)
    {
        // 空检查
        if (value is not AfterActionFilterOnActionExecutedEventData eventData)
        {
            return;
        }

        // 空检查
        var httpContext = eventData.ActionExecutedContext.HttpContext;
        if (httpContext is null)
        {
            return;
        }

        // 获取执行异常
        var exception = eventData.ActionExecutedContext.Exception;

        // 更新终点路由诊断模型缓存
        if (_endpointDiagnosticModelsCache.TryUpdate(httpContext.TraceIdentifier, endpointDiagnosticModel =>
        {
            endpointDiagnosticModel.Exception ??= exception is not null
                ? new(exception)
                : null;

            // 添加执行的筛选器名称
            endpointDiagnosticModel.Filters.Add(eventData.Filter.ToString());

            return endpointDiagnosticModel;
        }, out var updatedEndpointDiagnosticModel))
        {
            // 将终点路由诊断模型写入诊断订阅器通道
            _ = WriteAsync(updatedEndpointDiagnosticModel!);
        }
    }

    /// <summary>
    /// Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop 事件处理
    /// </summary>
    /// <param name="value">事件负载值</param>
    internal void HostingHttpRequestInStopHandle(object? value)
    {
        // 空检查
        if (value is not HttpContext httpContext)
        {
            return;
        }

        // 移除终点路由诊断模型缓存
        if (_endpointDiagnosticModelsCache.TryRemove(httpContext.TraceIdentifier, out var endpointDiagnosticModel))
        {
            // 获取响应对象
            var httpResponse = httpContext.Response;

            // 设置响应信息
            endpointDiagnosticModel.SetResponseInfo(httpResponse);

            // 将终点路由诊断模型写入诊断订阅器通道
            _ = WriteAsync(endpointDiagnosticModel);
        }
    }
}