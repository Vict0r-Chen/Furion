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

using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Encodings.Web;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Kit 模块 <see cref="WebApplication"/> 拓展类
/// </summary>
public static class KitWebApplicationExtensions
{
    /// <summary>
    /// 添加 Kit 中间件
    /// </summary>
    /// <param name="webApplication"><see cref="WebApplication"/></param>
    /// <returns><see cref="WebApplication"/></returns>
    public static WebApplication UseKit(this WebApplication webApplication)
    {
        // 这里弄一个分组
        webApplication.MapGroup("/furion")
            .MapGet("http-sse", async (HttpContext context, CancellationToken cancellationToken) =>
            {
                var diagnosticListener = new HttpDiagnosticListener();
                diagnosticListener.Observe();
                cancellationToken.Register(() =>
                {
                    diagnosticListener.Dispose();
                });

                // 允许跨域
                context.Response.Headers.AccessControlAllowOrigin = "*";
                context.Response.Headers.AccessControlAllowHeaders = "*";

                // 设置响应头，指定 SSE 响应的 Content-Type
                context.Response.ContentType = "text/event-stream";

                // 启用响应的发送保持活动性
                context.Response.Headers.CacheControl = "no-cache";
                context.Response.Headers.Connection = "keep-alive";

                while (!cancellationToken.IsCancellationRequested)
                {
                    var item = await diagnosticListener.ReadAsync(cancellationToken);

                    await context.Response.WriteAsync("data: " + JsonSerializer.Serialize(item, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    }) + "\n\n", cancellationToken);
                }

                // 关闭连接
                context.Response.ContentType = "text/event-stream";
                context.Response.StatusCode = StatusCodes.Status204NoContent;
                await context.Response.Body.FlushAsync(cancellationToken);

            }).Accepts<NoContent>("text/event-stream")
            .ExcludeFromDescription();

        // 获取当前类型所在程序集
        var currentAssembly = typeof(KitWebApplicationExtensions).Assembly;

        // 添加 Kit 静态资源
        webApplication.UseFileServer(new FileServerOptions
        {
            FileProvider = new EmbeddedFileProvider(currentAssembly, $"{currentAssembly.GetName().Name}.Assets"),
            RequestPath = "/furion",
            EnableDirectoryBrowsing = false
        });

        return webApplication;
    }

    private static string SerializeToJson(object obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}