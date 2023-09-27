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
/// 开放接口模型解析器
/// </summary>
internal sealed class OpenApiModelParser
{
    /// <inheritdoc cref="DefaultModelMetadata"/>
    internal readonly DefaultModelMetadata _modelMetadata;

    /// <summary>
    /// <inheritdoc cref="OpenApiModelParser"/>
    /// </summary>
    /// <param name="modelMetadata"><see cref="ModelMetadata"/></param>
    internal OpenApiModelParser(ModelMetadata modelMetadata)
    {
        // 获取默认模型元数据
        var defaultModelMetadata = modelMetadata as DefaultModelMetadata;

        // 空检查
        ArgumentNullException.ThrowIfNull(defaultModelMetadata);

        _modelMetadata = defaultModelMetadata;
    }

    /// <summary>
    /// 解析模型元数据并返回开放接口模型
    /// </summary>
    /// <param name="modelMetadata"><see cref="ModelMetadata"/></param>
    /// <returns><see cref="OpenApiModel"/></returns>
    internal static OpenApiModel Parse(ModelMetadata modelMetadata)
    {
        return Parse<OpenApiModel>(modelMetadata);
    }

    /// <summary>
    /// 解析模型元数据并返回开放接口模型
    /// </summary>
    /// <typeparam name="TOpenApiModel"><see cref="OpenApiModel"/></typeparam>
    /// <param name="modelMetadata"><see cref="ModelMetadata"/></param>
    /// <returns><typeparamref name="TOpenApiModel"/></returns>
    internal static TOpenApiModel Parse<TOpenApiModel>(ModelMetadata modelMetadata)
        where TOpenApiModel : OpenApiModel, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(modelMetadata);

        return new OpenApiModelParser(modelMetadata)
            .ParseModel<TOpenApiModel>();
    }

    /// <summary>
    /// 解析模型元数据并返回开放接口模型
    /// </summary>
    /// <typeparam name="TOpenApiModel"><see cref="OpenApiModel"/></typeparam>
    /// <returns><typeparamref name="TOpenApiModel"/></returns>
    internal TOpenApiModel ParseModel<TOpenApiModel>()
        where TOpenApiModel : OpenApiModel, new()
    {
        // 获取模型类型
        var modelType = _modelMetadata.ModelType;

        // 初始化开放接口模型
        var openApiModel = new TOpenApiModel
        {
            Name = _modelMetadata.Name,
            Description = _modelMetadata.Description ?? GetMetadataAttributes<DescriptionAttribute>().FirstOrDefault()?.Description,
            DefaultValue = default,
            IsRequired = GetMetadataAttributes<RequiredAttribute>().Any(),
            AllowNullValue = !GetMetadataAttributes<RequiredAttribute>().Any() && _modelMetadata.IsReferenceOrNullableType is true,
            DataType = DataTypeParser.Parse(modelType),
            RuntimeType = modelType?.ToString(),
            TypeCode = Type.GetTypeCode(modelType),
        };

        // 解析枚举类型模型元数据
        if (modelType?.IsEnum is true)
        {
            openApiModel.Properties = ParseEnum(modelType);
        }

        // 解析对象类型模型元数据
        if (openApiModel.DataType is DataTypes.Object)
        {
            openApiModel.Properties = ParseObject(modelType!);
        }

        return openApiModel;
    }

    /// <summary>
    /// 解析枚举类型模型元数据
    /// </summary>
    /// <param name="modelType">模型类型</param>
    /// <returns><see cref="IDictionary{TKey, TValue}"/></returns>
    /// <exception cref="ArgumentException"></exception>
    internal static IDictionary<string, object>? ParseEnum(Type modelType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(modelType);

        // 检查模型类型是否是枚举类型
        if (!modelType.IsEnum)
        {
            throw new ArgumentException("The `modelType` parameter is not a valid enumeration type.", nameof(modelType));
        }

        // 将枚举项转换为字典集合
        var properties = Enum.GetValues(modelType)
            .Cast<object>()
            .ToDictionary(u => u.ToString()!, u => (object)(int)u, StringComparer.OrdinalIgnoreCase);

        return properties;
    }

    /// <summary>
    /// 解析对象类型模型元数据
    /// </summary>
    /// <param name="modelType">模型类型</param>
    /// <returns><see cref="IDictionary{TKey, TValue}"/></returns>
    internal IDictionary<string, object>? ParseObject(Type modelType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(modelType);

        // 检查模型类型是否是对象类型
        if (DataTypeParser.Parse(modelType) is not DataTypes.Object)
        {
            return null;
        }

        // 获取对象类型模型属性模型元数据集合
        var propertyModelMetadata = _modelMetadata.Properties
            .Cast<DefaultModelMetadata>();

        // 初始化属性开放接口模型字典集合
        var properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        // 遍历属性模型元数据集合
        foreach (var modelMetadata in propertyModelMetadata)
        {
            // 解析属性模型元数据并返回开放接口模型
            var openApiModel = Parse(modelMetadata);

            // 添加到属性模型元数据集合
            properties.Add(openApiModel.Name!, openApiModel);
        }

        return properties;
    }

    /// <summary>
    /// 获取模型元数据定义的特性集合
    /// </summary>
    /// <typeparam name="TAttribute"><see cref="Attribute"/></typeparam>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal IEnumerable<TAttribute> GetMetadataAttributes<TAttribute>()
        where TAttribute : Attribute
    {
        return _modelMetadata.Attributes
            .Attributes
            .OfType<TAttribute>();
    }
}