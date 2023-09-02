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
/// 终点路由诊断模型
/// </summary>
internal sealed class EndpointDiagnosticModel
{
    /// <inheritdoc cref="HttpContext" />
    internal readonly HttpContext _httpContext;

    /// <summary>
    /// <inheritdoc cref="EndpointDiagnosticModel"/>
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/></param>
    internal EndpointDiagnosticModel(HttpContext httpContext)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(httpContext);

        _httpContext = httpContext;

        Filters = new();

        // 初始化
        Initialize();
    }

    /// <summary>
    /// 活动标识
    /// </summary>
    public string? TraceId { get; private set; }

    /// <summary>
    /// 请求标识
    /// </summary>
    public string? TraceIdentifier { get; private set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    public string? HttpMethod { get; private set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    public string? Path { get; private set; }

    /// <summary>
    /// 请求地址
    /// </summary>
    public string? UrlAddress { get; private set; }

    /// <summary>
    /// 请求开始时间
    /// </summary>
    public DateTimeOffset? RequestStartTime { get; private set; }

    /// <summary>
    /// 请求 URL 参数集合
    /// </summary>
    public IDictionary<string, string>? Query { get; private set; }

    /// <summary>
    /// 请求 Cookies 集合
    /// </summary>
    public IDictionary<string, string>? Cookies { get; private set; }

    /// <summary>
    /// 请求 Headers 集合
    /// </summary>
    public IDictionary<string, string>? RequestHeaders { get; private set; }

    /// <summary>
    /// 路由信息集合
    /// </summary>
    public IDictionary<string, object?>? RouteValues { get; private set; }

    /// <inheritdoc cref="EndpointModel"/>
    public EndpointModel? Endpoint { get; private set; }

    /// <inheritdoc cref="ControllerActionModel"/>
    public ControllerActionModel? ControllerAction { get; private set; }

    /// <summary>
    /// 请求结束时间
    /// </summary>
    public DateTimeOffset? RequestEndTime { get; private set; }

    /// <summary>
    /// 状态码
    /// </summary>
    public int? StatusCode { get; private set; }

    /// <summary>
    /// 状态文本
    /// </summary>
    public string? StatusText { get; private set; }

    /// <summary>
    /// 内容类型
    /// </summary>
    public string? ContentType { get; private set; }

    /// <summary>
    /// 响应 Headers 集合
    /// </summary>
    public IDictionary<string, string>? ResponseHeaders { get; private set; }

    /// <summary>
    /// 筛选器集合
    /// </summary>
    public List<string?> Filters { get; init; }

    /// <inheritdoc cref="ExceptionModel"/>
    public ExceptionModel? Exception { get; internal set; }

    /// <summary>
    /// 初始化
    /// </summary>
    internal void Initialize()
    {
        // 获取请求对象
        var httpRequest = _httpContext.Request;

        TraceId = Activity.Current?.Id ?? _httpContext?.TraceIdentifier;
        TraceIdentifier = _httpContext.TraceIdentifier;
        Path = httpRequest.Path;
        UrlAddress = httpRequest.GetUrlAddress();
        HttpMethod = httpRequest.Method;
        RequestStartTime = DateTimeOffset.UtcNow;

        Query = httpRequest.Query.ToDictionary(u => u.Key, u => u.Value.ToString());
        Cookies = httpRequest.Cookies.ToDictionary(u => u.Key, u => u.Value);
        RequestHeaders = httpRequest.Headers.ToDictionary(u => u.Key, u => u.Value.ToString());
        RouteValues = httpRequest.RouteValues.ToDictionary(u => u.Key, u => u.Value);

        // 获取终点路由
        if (_httpContext.GetEndpoint() is Endpoint endpoint)
        {
            // 设置终点路由模型
            Endpoint = new(endpoint);

            // 获取控制器操作描述器
            if (endpoint.Metadata.GetMetadata<ControllerActionDescriptor>() is ControllerActionDescriptor controllerActionDescriptor)
            {
                // 设置操作控制器模型
                ControllerAction = new(controllerActionDescriptor);
            }
        }
    }

    /// <summary>
    /// 同步响应数据
    /// </summary>
    /// <param name="httpResponse"><see cref="HttpResponse"/></param>
    internal void SyncResponseData(HttpResponse httpResponse)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(httpResponse);

        RequestEndTime = DateTimeOffset.UtcNow;
        StatusCode = httpResponse.StatusCode;
        StatusText = httpResponse.GetStatusText();
        ContentType = httpResponse.ContentType;
        ResponseHeaders = httpResponse.Headers.ToDictionary(u => u.Key, u => u.Value.ToString());
    }
}