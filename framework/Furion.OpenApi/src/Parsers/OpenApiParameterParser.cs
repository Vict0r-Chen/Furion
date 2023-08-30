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
public sealed class OpenApiParameterParser
{
    /// <inheritdoc cref="ApiParameterDescription"/>
    internal readonly ApiParameterDescription _apiParameterDescription;

    /// <summary>
    /// <inheritdoc cref="OpenApiParameterParser"/>
    /// </summary>
    /// <param name="apiParameterDescription"><see cref="ApiParameterDescription"/></param>
    public OpenApiParameterParser(ApiParameterDescription apiParameterDescription)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(apiParameterDescription);

        _apiParameterDescription = apiParameterDescription;
    }

    /// <summary>
    /// 解析 <see cref="ApiParameterDescription"/> 并返回开放接口参数
    /// </summary>
    /// <returns></returns>
    public OpenApiParameter? Parse()
    {
        // 检查参数绑定来源是否是 Query 或 Path
        if (_apiParameterDescription.Source != BindingSource.Query
            && _apiParameterDescription.Source != BindingSource.Path)
        {
            return null;
        }

        // 检查是否允许模型绑定
        if (_apiParameterDescription.ModelMetadata is { IsBindingAllowed: false })
        {
            return null;
        }

        // 获取默认模型元数据
        var modelMetadata = _apiParameterDescription.ModelMetadata as DefaultModelMetadata;

        // 初始化开放接口参数
        var openApiRouteParameter = new OpenApiParameter
        {
            Name = _apiParameterDescription.Name,
            Description = modelMetadata?.Description ?? modelMetadata?.Attributes?.Attributes?.OfType<DescriptionAttribute>()?.FirstOrDefault()?.Description,
            DefaultValue = _apiParameterDescription.DefaultValue,
            AllowNullValue = modelMetadata?.IsReferenceOrNullableType is true,
            IsRequired = modelMetadata?.Attributes?.Attributes?.OfType<RequiredAttribute>()?.Any() is true,
            DataType = DataTypeParser.Parse(_apiParameterDescription.Type),
            RuntimeType = _apiParameterDescription.Type?.ToString(),
            TypeCode = Type.GetTypeCode(_apiParameterDescription.Type),
            BindingSource = _apiParameterDescription.Source.DisplayName.ToLowerFirstLetter(),
            Patterns = null,
        };

        // 检查参数是否在路由中定义
        if (_apiParameterDescription.Source == BindingSource.Path
            && _apiParameterDescription.RouteInfo is not null)
        {
            openApiRouteParameter.Properties = new Dictionary<string, object?>
            {
                { nameof(_apiParameterDescription.RouteInfo), _apiParameterDescription.RouteInfo }
            };
        }

        return openApiRouteParameter;
    }
}