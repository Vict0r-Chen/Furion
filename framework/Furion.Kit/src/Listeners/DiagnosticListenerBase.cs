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
/// 诊断监听器抽象基类
/// </summary>
/// <typeparam name="TData">诊断数据通道数据类型</typeparam>
internal abstract class DiagnosticListenerBase<TData> : IDisposable
{
    /// <summary>
    /// 并发锁标识
    /// </summary>
    internal readonly object _lockObject = new();

    /// <summary>
    /// 诊断数据通道
    /// </summary>
    internal readonly Channel<TData> _diagnosticChannel;

    /// <summary>
    /// 诊断订阅器引用
    /// </summary>
    internal IDisposable? _subscription;

    /// <summary>
    /// 诊断观察者引用
    /// </summary>
    internal IDisposable? _listenerSubscription;

    /// <summary>
    /// 诊断监听类别
    /// </summary>
    internal readonly string _listenerCategory;

    /// <summary>
    /// <inheritdoc cref="DiagnosticListenerBase{T}" />
    /// </summary>
    /// <param name="listenerCategory">诊断监听类别</param>
    /// <param name="capacity">诊断数据通道容量</param>
    internal DiagnosticListenerBase(string listenerCategory, int capacity = 3000)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(listenerCategory);

        _listenerCategory = listenerCategory;

        _diagnosticChannel = Channel.CreateBounded<TData>(new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        });
    }

    /// <summary>
    /// 开始监听
    /// </summary>
    public void Listening()
    {
        // 初始化诊断观察者
        var diagnosticObserver = new DiagnosticObserver<DiagnosticListener>(AddSubscription, OnCompleted);

        // 注册诊断观察者
        _listenerSubscription = DiagnosticListener.AllListeners.Subscribe(diagnosticObserver);
    }

    /// <summary>
    /// 添加诊断订阅器
    /// </summary>
    /// <param name="diagnosticListener"><see cref="DiagnosticListener"/></param>
    internal void AddSubscription(DiagnosticListener diagnosticListener)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(diagnosticListener);

        // 检查诊断监听器类别是否一致
        if (diagnosticListener.Name != _listenerCategory)
        {
            return;
        }

        // 注册诊断订阅器
        lock (_lockObject)
        {
            _subscription?.Dispose();
            _subscription = diagnosticListener.Subscribe(new DiagnosticObserver<KeyValuePair<string, object?>>(OnNext, OnCompleted));
        }
    }

    /// <summary>
    /// 监听生产者发布新消息
    /// </summary>
    /// <param name="data">通知信息</param>
    internal abstract void OnNext(KeyValuePair<string, object?> data);

    /// <summary>
    /// 监听生产者停止发送消息
    /// </summary>
    protected virtual void OnCompleted()
    {
    }

    /// <summary>
    /// 将数据写入诊断数据通道
    /// </summary>
    /// <param name="data"><typeparamref name="TData"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    internal virtual async Task WriteAsync(TData data, CancellationToken cancellationToken = default)
    {
        await _diagnosticChannel.Writer.WriteAsync(data, cancellationToken);
    }

    /// <summary>
    /// 读取诊断数据通道数据
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task{TResult}"/></returns>
    internal virtual async Task<TData> ReadAsync(CancellationToken cancellationToken = default)
    {
        return await _diagnosticChannel.Reader.ReadAsync(cancellationToken);
    }

    /// <summary>
    /// 构建 SSE 终点路由处理程序
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    internal virtual async Task BuildSSEEndpointRouteHandler(HttpContext httpContext, CancellationToken cancellationToken)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(httpContext);

        // 开始观察
        Listening();

        // 检查请求是否已终止并释放诊断观察者
        cancellationToken.Register(Dispose);

        // 设置响应头，允许跨域请求
        httpContext.Response.AllowCors();

        // 设置响应头，指定 Content-Type
        httpContext.Response.ContentType = "text/event-stream";

        // 设置响应头，启用响应发送保持活动性
        httpContext.Response.Headers.CacheControl = "no-cache";
        httpContext.Response.Headers.Connection = "keep-alive";

        // 在请求未终止前持续推送
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // 读取诊断数据通道数据
                var data = await ReadAsync(cancellationToken);

                // 持续推送至连接客户端
                await httpContext.Response.WriteAsync("data: " + Helpers.SerializeObject(data) + "\n\n", cancellationToken);
            }
            catch { }
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _listenerSubscription?.Dispose();
    }
}