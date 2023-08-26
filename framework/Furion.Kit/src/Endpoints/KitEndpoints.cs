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

using Furion.OpenApi;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

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
        // 终点路由诊断路由配置
        webApplication.MapGroup(kitOptions.Root)
            .MapGet("endpoint-diagnostic-sse", EndpointDiagnosticHandler)
            .Accepts<NoContent>("text/event-stream")
            .ExcludeFromDescription();

        // 配置诊断路由配置
        webApplication.MapGroup(kitOptions.Root)
            .MapGet("configuration-diagnostic", ConfigurationDiagnosticHandler)
            .ExcludeFromDescription();

        // 配置提供器诊断路由配置
        webApplication.MapGroup(kitOptions.Root)
            .MapGet("configuration-provider-diagnostic", ConfigurationProviderDiagnosticHandler)
            .ExcludeFromDescription();

        // 组件诊断路由配置
        webApplication.MapGroup(kitOptions.Root)
            .MapGet("component-diagnostic", ComponentDiagnosticHandler)
            .ExcludeFromDescription();

        // 开放接口路由配置
        webApplication.MapGroup(kitOptions.Root)
            .MapGet("openapi", (HttpContext httpContext, IApiDescriptionGroupCollectionProvider provider) =>
            {
                var apiDescriptionParser = new ApiDescriptionParser(provider);

                httpContext.Response.AllowCors();
                httpContext.Response.Headers.CacheControl = "no-cache";

                return Results.Json(apiDescriptionParser.Parse());
            });
    }

    /// <summary>
    /// 终点路由诊断处理程序
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    internal static async Task EndpointDiagnosticHandler(HttpContext httpContext, CancellationToken cancellationToken)
    {
        await new EndpointDiagnosticListener().BuildSSEEndpointRouteHandler(httpContext, cancellationToken);
    }

    /// <summary>
    /// 配置诊断路由处理程序
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="hostEnvironment"><see cref="IHostEnvironment"/></param>
    /// <returns><see cref="Task"/></returns>
    internal static async Task ConfigurationDiagnosticHandler(HttpContext httpContext, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        // 添加运行环境响应头导出
        httpContext.Response.Headers.AppendExpose(Constants.ENVIRONMENT_NAME_KEY, hostEnvironment.EnvironmentName);

        // 写入 Body 流
        await httpContext.Response.WriteAsJsonAsync(configuration.ConvertToJson());
    }

    /// <summary>
    /// 配置提供器诊断路由处理程序
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns><see cref="Task"/></returns>
    internal static async Task ConfigurationProviderDiagnosticHandler(HttpContext httpContext, IConfiguration configuration)
    {
        // 获取配置元数据集合
        var metadatas = configuration.GetMetadata();

        // 创建一个内存流，用于存储生成的 JSON 数据
        using var stream = new MemoryStream();

        // 创建一个 Utf8JsonWriter 对象来写入 JSON 数据到内存流中
        using (var jsonWriter = new Utf8JsonWriter(stream))
        {
            // 写入 JSON 数组的起始括号
            jsonWriter.WriteStartArray();

            // 遍历配置原始集合构建 JSON 字符串
            foreach (var metadata in metadatas)
            {
                // 写入 JSON 对象的起始括号
                jsonWriter.WriteStartObject();

                // 输出基础属性 JSON 字符串
                jsonWriter.WriteNumber("id", metadata.Provider.GetHashCode());
                jsonWriter.WriteString("provider", metadata.Provider.ToString());
                jsonWriter.WriteBoolean("isFileConfiguration", metadata.IsFileConfiguration);

                // 输出配置元数据 JSON 字符串
                jsonWriter.WritePropertyName("metadata");
                jsonWriter.WriteRawValue(metadata.ConvertToJson());

                // 写入 JSON 对象的结束括号
                jsonWriter.WriteEndObject();
            }

            // 写入 JSON 数组的结束括号
            jsonWriter.WriteEndArray();
        }

        // 将内存流中的数据转换为字符串
        var jsonString = Encoding.UTF8.GetString(stream.ToArray());

        // 写入 Body 流
        await httpContext.Response.WriteAsJsonAsync(jsonString);
    }

    /// <summary>
    /// 组件诊断路由处理程序
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/></param>
    /// <param name="coreOptions"><see cref="CoreOptions"/></param>
    /// <returns><see cref="IResult"/></returns>
    internal static IResult ComponentDiagnosticHandler(HttpContext httpContext, CoreOptions coreOptions)
    {
        // 初始化组件诊断模型
        var componentDiagnosticModel = new ComponentDiagnosticModel();

        // 遍历入口组件类型集合
        foreach (var componentType in coreOptions.EntryComponentTypes)
        {
            componentDiagnosticModel.Components.Add(
                CreateComponentModel(componentType, DependencyGraph.Create(componentType)));
        }

        // 配置允许跨域响应头
        httpContext.Response.AllowCors();

        // 设置响应头，不缓存请求
        httpContext.Response.Headers.CacheControl = "no-cache";

        // 添加项目名称响应头导出
        httpContext.Response.Headers.AppendExpose(Constants.PROJECT_NAME_KEY, Assembly.GetEntryAssembly()?.GetName()?.Name ?? nameof(Furion));

        // 返回 application/json 响应流数据
        return Results.Json(componentDiagnosticModel);
    }

    /// <summary>
    /// 创建组件模型
    /// </summary>
    /// <param name="componentType">组件类型</param>
    /// <param name="dependencies">组件依赖关系集合</param>
    /// <returns><see cref="ComponentModel"/></returns>
    internal static ComponentModel CreateComponentModel(Type componentType, Dictionary<Type, Type[]> dependencies)
    {
        return new(componentType)
        {
            Dependencies = dependencies[componentType]
                .Select(type => CreateComponentModel(type, dependencies))
                .ToList()
        };
    }
}