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
/// 开放接口请求体解析器
/// </summary>
internal sealed class OpenApiRequestBodyParser
{
    /// <inheritdoc cref="ApiParameterDescription"/>
    internal readonly ApiParameterDescription _apiParameterDescription;

    /// <summary>
    /// 开放接口架构缓存集合
    /// </summary>
    internal readonly ConcurrentDictionary<string, OpenApiSchema> _schemas;

    /// <summary>
    /// <inheritdoc cref="OpenApiRequestBodyParser"/>
    /// </summary>
    /// <param name="apiParameterDescription"><see cref="ApiParameterDescription"/></param>
    /// <param name="schemas">开放接口架构缓存集合</param>
    internal OpenApiRequestBodyParser(ApiParameterDescription apiParameterDescription
        , ConcurrentDictionary<string, OpenApiSchema> schemas)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(apiParameterDescription);
        ArgumentNullException.ThrowIfNull(schemas);

        _apiParameterDescription = apiParameterDescription;
        _schemas = schemas;
    }

    /// <summary>
    /// 检查是否可以执行解析程序
    /// </summary>
    /// <returns><see cref="bool"/></returns>
    internal bool CanParse()
    {
        // 获取接口参数绑定源
        var bindingSource = _apiParameterDescription.Source;

        return bindingSource == BindingSource.Form
            || bindingSource == BindingSource.Body;
    }

    /// <summary>
    /// 解析接口参数描述器并返回开放接口参数
    /// </summary>
    /// <returns><see cref="OpenApiParameter"/></returns>
    internal OpenApiParameter? Parse()
    {
        // 检查是否可以执行解析程序
        if (!CanParse())
        {
            return null;
        }

        // 获取参数绑定源
        var bindingSource = _apiParameterDescription.Source;

        // 获取模型元数据
        var modelMetadata = _apiParameterDescription.ModelMetadata;

        // 检查参数模型元数据是否为空或禁止模型绑定
        if (modelMetadata is null or { IsBindingAllowed: false })
        {
            return null;
        }

        // 解析模型元数据并返回开放接口参数
        var openApiParameter = OpenApiModelParser.Parse<OpenApiParameter>(modelMetadata, _schemas);

        // 设置实际参数名
        openApiParameter.Name = _apiParameterDescription.Name;

        // 设置开放接口参数其他属性
        openApiParameter.Default = _apiParameterDescription.DefaultValue;
        openApiParameter.Binding = bindingSource.DisplayName;
        openApiParameter.Patterns = default;

        // 检查参数是否在路由中存在定义
        if (bindingSource == BindingSource.Path
            && _apiParameterDescription.RouteInfo is null)
        {
            openApiParameter.Additionals ??= new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            openApiParameter.Additionals.Add("onRoute", false);
        }

        return openApiParameter;
    }
}