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
/// 模型元数据解析器
/// </summary>
public sealed class ModelMetadataParser
{
    /// <inheritdoc cref="ModelMetadata"/>
    internal readonly ModelMetadata _modelMetadata;

    /// <inheritdoc cref="ApiParameterDescription"/>
    internal readonly ApiParameterDescription? _apiParameterDescription;

    /// <summary>
    /// <inheritdoc cref="ModelMetadataParser"/>
    /// </summary>
    /// <param name="modelMetadata"><see cref="ModelMetadata"/></param>
    /// <param name="apiParameterDescription"><see cref="ApiParameterDescription"/></param>
    public ModelMetadataParser(ModelMetadata modelMetadata, ApiParameterDescription? apiParameterDescription)
    {
        _modelMetadata = modelMetadata;
        _apiParameterDescription = apiParameterDescription;
    }

    /// <summary>
    /// 解析模型元数据
    /// </summary>
    /// <returns></returns>
    public OpenApiProperty? Parse()
    {
        return _apiParameterDescription switch
        {
            _ when _apiParameterDescription?.Source == BindingSource.Query => ParseQueryAndPath(),
            _ when _apiParameterDescription?.Source == BindingSource.Path => ParseQueryAndPath(),
            _ when _apiParameterDescription?.Source == BindingSource.Body => ParseBody(),
            _ when _apiParameterDescription?.Source == BindingSource.Header => ParseHeader(),
            _ => null,
        };
    }

    /// <summary>
    /// 解析 Query 和 Path 模型元数据
    /// </summary>
    /// <returns><see cref="OpenApiProperty"/></returns>
    public OpenApiProperty? ParseQueryAndPath()
    {
        // 检查绑定源是否是 Query 或 Path
        if (_apiParameterDescription?.Source != BindingSource.Query && _apiParameterDescription?.Source != BindingSource.Path)
        {
            return null;
        }

        // 初始化开放接口属性
        var openApiProperty = new OpenApiProperty
        {
            Name = _apiParameterDescription.Name,
            Description = _modelMetadata.Description,   // 解析注释或 [Description] 特性
            DefaultValue = _apiParameterDescription?.DefaultValue,
            AllowNullValue = _modelMetadata.IsNullableValueType,
            IsRequired = _modelMetadata.IsRequired,
            AsPathParameter = _apiParameterDescription?.RouteInfo is not null,
            DataType = DataTypeParser.Parse(_modelMetadata.ModelType),
            Format = "",
            SourceId = _apiParameterDescription?.Source?.Id,
            RuntimeType = _modelMetadata.ModelType.ToString(),
            Patterns = null,
            Properties = null
        };

        return openApiProperty;
    }

    /// <summary>
    /// 解析 Body 模型元数据
    /// </summary>
    /// <returns></returns>
    public OpenApiProperty? ParseBody()
    {
        return null;
    }

    /// <summary>
    /// 解析 Header 模型元数据
    /// </summary>
    /// <returns></returns>
    public OpenApiProperty? ParseHeader()
    {
        return null;
    }

    /// <summary>
    /// 解析 Form 模型元数据
    /// </summary>
    /// <returns></returns>
    public OpenApiProperty? ParseForm()
    {
        return null;
    }
}