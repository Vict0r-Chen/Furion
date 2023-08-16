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
/// 终点路由模型
/// </summary>
internal sealed class EndpointModel
{
    /// <inheritdoc cref="Endpoint"/>
    internal readonly Endpoint _endpoint;

    /// <summary>
    /// <inheritdoc cref="EndpointModel"/>
    /// </summary>
    /// <param name="endpoint"><see cref="Endpoint"/></param>
    internal EndpointModel(Endpoint endpoint)
    {
        _endpoint = endpoint;

        // 初始化
        Initialize();
    }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string? DisplayName { get; internal set; }

    /// <summary>
    /// 路由格式
    /// </summary>
    public string? RoutePattern { get; internal set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? Order { get; internal set; }

    /// <summary>
    /// 请求方式集合
    /// </summary>
    public string? HttpMethods { get; internal set; }

    /// <summary>
    /// 初始化
    /// </summary>
    internal void Initialize()
    {
        DisplayName = _endpoint.DisplayName;

        // 判断终点路由是否是 RouteEndpoint 类型
        if (_endpoint is not RouteEndpoint routeEndpoint)
        {
            return;
        }

        RoutePattern = routeEndpoint.RoutePattern.RawText;
        Order = routeEndpoint.Order;

        // 拼接路由 HttpMethod 集合
        var httpMethods = routeEndpoint.Metadata.GetMetadata<IHttpMethodMetadata>()?.HttpMethods;
        if (httpMethods != null)
        {
            HttpMethods = string.Join(", ", httpMethods);
        }
    }
}