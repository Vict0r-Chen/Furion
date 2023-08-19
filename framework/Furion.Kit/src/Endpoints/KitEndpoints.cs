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
/// Kit 模块终点路由配置
/// </summary>
internal static class KitEndpoints
{
    /// <summary>
    /// Kit 模块终点路由配置入口
    /// </summary>
    /// <param name="webApplication"><see cref="WebApplication"/></param>
    /// <param name="kitOptions"><see cref="KitOptions"/></param>
    internal static void Map(WebApplication webApplication, KitOptions kitOptions)
    {
        // 配置终点路由诊断路由
        webApplication.MapGroup(kitOptions.Root)
            .MapGetSSE("endpoint-diagnostic-sse", EndpointDiagnosticSSE);

        // 配置配置诊断路由
        webApplication.MapGroup(kitOptions.Root)
            .MapGet("configuration-diagnostic", ConfigurationDiagnostic)
            .ExcludeFromDescription();
    }

    /// <summary>
    /// 终点路由诊断 SSE 处理程序
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    internal static async Task EndpointDiagnosticSSE(HttpContext httpContext, CancellationToken cancellationToken)
    {
        await new EndpointDiagnosticListener().SSEHandler(httpContext, cancellationToken);
    }

    /// <summary>
    /// 配置诊断处理程序
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns><see cref="Task"/></returns>
    internal static async Task ConfigurationDiagnostic(HttpContext httpContext, IConfiguration configuration)
    {
        var jsonString = configuration.ConvertToJson();

        // 设置响应头，允许跨域请求
        httpContext.Response.AllowCors();

        // 设置响应头，指定 Content-Type 和 Content-Length
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(jsonString);

        // 设置响应头，不缓存请求
        httpContext.Response.Headers.CacheControl = "no-cache";

        // 写入 Body 流
        await httpContext.Response.WriteAsync(jsonString);
    }
}