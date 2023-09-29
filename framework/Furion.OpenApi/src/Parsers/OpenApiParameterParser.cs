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

namespace Furion.OpenApi;

/// <summary>
/// 开放接口参数解析器
/// </summary>
internal sealed class OpenApiParameterParser
{
    /// <inheritdoc cref="ApiParameterDescription"/>
    internal readonly ApiParameterDescription _apiParameterDescription;

    /// <summary>
    /// 开放接口架构定义集合
    /// </summary>
    internal readonly ConcurrentDictionary<string, object> _definitions;

    /// <summary>
    /// <inheritdoc cref="OpenApiParameterParser"/>
    /// </summary>
    /// <param name="apiParameterDescription"><see cref="ApiParameterDescription"/></param>
    /// <param name="definitions">开放接口架构定义集合</param>
    internal OpenApiParameterParser(ApiParameterDescription apiParameterDescription
        , ConcurrentDictionary<string, object> definitions)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(apiParameterDescription);
        ArgumentNullException.ThrowIfNull(definitions);

        _apiParameterDescription = apiParameterDescription;
        _definitions = definitions;
    }

    /// <summary>
    /// 解析接口参数描述器并返回开放接口参数
    /// </summary>
    /// <returns><see cref="OpenApiParameter"/></returns>
    internal OpenApiParameter? Parse()
    {
        // 检查参数模型元数据是否为空或禁止模型绑定
        if (_apiParameterDescription.ModelMetadata is null or { IsBindingAllowed: false })
        {
            return null;
        }

        // 解析参数模型元数据并返回开放接口参数
        var openApiParameter = OpenApiModelParser.Parse<OpenApiParameter>(_apiParameterDescription.ModelMetadata, _definitions);

        // 设置实际参数名
        openApiParameter.Name = _apiParameterDescription.Name;

        // 设置开放接口参数其他属性
        openApiParameter.Default = _apiParameterDescription.DefaultValue;
        openApiParameter.BindingSource = _apiParameterDescription.Source.DisplayName.ToLowerFirstLetter();
        openApiParameter.Patterns = default;

        // 检查参数是否在路由中存在定义
        if (_apiParameterDescription.Source == BindingSource.Path
            && _apiParameterDescription.RouteInfo is null)
        {
            openApiParameter.Additionals ??= new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            openApiParameter.Additionals.Add("onRoute", false);
        }

        return openApiParameter;
    }
}