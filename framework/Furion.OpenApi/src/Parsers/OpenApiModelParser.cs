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
    /// 开放接口架构缓存集合
    /// </summary>
    internal readonly ConcurrentDictionary<string, OpenApiSchema> _schemas;

    /// <summary>
    /// <inheritdoc cref="OpenApiModelParser"/>
    /// </summary>
    /// <param name="modelMetadata"><see cref="ModelMetadata"/></param>
    /// <param name="schemas">开放接口架构缓存集合</param>
    internal OpenApiModelParser(ModelMetadata modelMetadata
        , ConcurrentDictionary<string, OpenApiSchema> schemas)
    {
        // 获取默认模型元数据
        var defaultModelMetadata = modelMetadata as DefaultModelMetadata;

        // 空检查
        ArgumentNullException.ThrowIfNull(defaultModelMetadata);
        ArgumentNullException.ThrowIfNull(schemas);

        _modelMetadata = defaultModelMetadata;
        _schemas = schemas;
    }

    /// <summary>
    /// 解析模型元数据并返回开放接口模型
    /// </summary>
    /// <typeparam name="TOpenApiModel"><see cref="OpenApiModel"/></typeparam>
    /// <param name="modelMetadata"><see cref="ModelMetadata"/></param>
    /// <param name="schemas">开放接口架构缓存集合</param>
    /// <returns><typeparamref name="TOpenApiModel"/></returns>
    internal static TOpenApiModel Parse<TOpenApiModel>(ModelMetadata modelMetadata, ConcurrentDictionary<string, OpenApiSchema> schemas)
        where TOpenApiModel : OpenApiModel, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(modelMetadata);
        ArgumentNullException.ThrowIfNull(schemas);

        return new OpenApiModelParser(modelMetadata, schemas)
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

        // 获取必填标识
        var required = GetMetadataAttribute<RequiredAttribute>() is not null;

        // 初始化开放接口模型
        var openApiModel = new TOpenApiModel
        {
            Name = _modelMetadata.Name,
            Description = _modelMetadata.Description ?? GetMetadataAttribute<DescriptionAttribute>()?.Description,
            Default = default,
            Required = required,
            Nullable = !required && _modelMetadata.IsReferenceOrNullableType is true,
            Obsolete = GetObsoleteOrNull(GetMetadataAttribute<ObsoleteAttribute>()),
            TypeName = TypeNameParser.Parse(modelType),
            TypeCode = Type.GetTypeCode(modelType),
            RuntimeType = modelType?.ToString(),
        };

        // 解析并生成枚举类型架构
        if (openApiModel.TypeName is TypeName.Enum)
        {
            openApiModel.Schema = ParseEnum(modelType!);
        }

        // 解析并生成对象类型架构
        if (openApiModel.TypeName is TypeName.Object)
        {
            openApiModel.Schema = ParseObject(modelType!);
        }

        return openApiModel;
    }

    /// <summary>
    /// 解析并生成枚举类型架构
    /// </summary>
    /// <param name="modelType">模型类型</param>
    /// <returns><see cref="IDictionary{TKey, TValue}"/></returns>
    /// <exception cref="ArgumentException"></exception>
    internal IDictionary<string, object>? ParseEnum(Type modelType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(modelType);

        // 检查模型类型是否是枚举类型
        if (!modelType.IsEnum)
        {
            throw new ArgumentException("The `modelType` parameter is not a valid enumeration type.", nameof(modelType));
        }

        // 获取架构标识
        var schemaId = modelType.FullName!;

        // 获取或添加枚举类型缓存
        _schemas.GetOrAdd(schemaId, _ =>
        {
            // 获取枚举定义集合
            var values = Enum.GetValues(modelType)
                .Cast<object>();

            // 初始化枚举开放接口属性集合
            var properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // 遍历枚举定义集合
            foreach (var item in values)
            {
                // 获取枚举名、枚举值和枚举字段成员
                var name = item.ToString()!;
                var value = (int)item;
                var field = modelType.GetField(name)!;

                // 添加到枚举开放接口属性集合中
                properties.Add(name, new OpenApiEnum
                {
                    Name = name,
                    Value = value,
                    Description = field.GetCustomAttribute<DescriptionAttribute>()?.Description,
                    Obsolete = GetObsoleteOrNull(field.GetCustomAttribute<ObsoleteAttribute>()),
                });
            }

            return new()
            {
                TypeName = TypeName.Enum,
                Properties = properties
            };
        });

        return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
        {
            { "$ref", $"#/schemas/{schemaId}" }
        };
    }

    /// <summary>
    /// 解析并生成对象类型架构
    /// </summary>
    /// <param name="modelType">模型类型</param>
    /// <returns><see cref="IDictionary{TKey, TValue}"/></returns>
    /// <exception cref="ArgumentException"></exception>
    internal IDictionary<string, object>? ParseObject(Type modelType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(modelType);

        // 检查模型类型是否是对象类型
        if (TypeNameParser.Parse(modelType) is not TypeName.Object)
        {
            throw new ArgumentException("The `modelType` parameter is not a valid object type.", nameof(modelType));
        }

        // 获取架构标识
        var schemaId = modelType.FullName!;

        // 获取或添加对象类型缓存
        _schemas.GetOrAdd(schemaId, _ =>
        {
            // 获取模型元数据的属性模型元数据集合，同时排除贴有 [JsonIgnore] 特性的属性
            var propertyModelMetadata = _modelMetadata.Properties
                .Cast<DefaultModelMetadata>()
                .Where(m => !GetMetadataAttributes<JsonIgnoreAttribute>(m).Any());

            // 初始化对象开放接口属性集合
            var properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // 遍历属性模型元数据集合
            foreach (var modelMetadata in propertyModelMetadata)
            {
                // 解析模型元数据并返回开放接口模型
                var openApiModel = Parse<OpenApiModel>(modelMetadata, _schemas);

                // 添加到对象开放接口属性集合中
                properties.Add(openApiModel.Name.ToLowerFirstLetter()!, openApiModel);
            }

            return new()
            {
                TypeName = TypeName.Object,
                Properties = properties
            };
        });

        return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
        {
            { "$ref", $"#/schemas/{schemaId}" }
        };
    }

    /// <summary>
    /// 获取模型元数据定义的特性
    /// </summary>
    /// <typeparam name="TAttribute"><see cref="Attribute"/></typeparam>
    /// <returns><typeparamref name="TAttribute"/></returns>
    internal TAttribute? GetMetadataAttribute<TAttribute>()
        where TAttribute : Attribute
    {
        return GetMetadataAttributes<TAttribute>(_modelMetadata).FirstOrDefault();
    }

    /// <summary>
    /// 获取模型元数据定义的特性集合
    /// </summary>
    /// <typeparam name="TAttribute"><see cref="Attribute"/></typeparam>
    /// <param name="modelMetadata"><see cref="DefaultModelMetadata"/></param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    internal static IEnumerable<TAttribute> GetMetadataAttributes<TAttribute>(DefaultModelMetadata modelMetadata)
        where TAttribute : Attribute
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(modelMetadata);

        return modelMetadata.Attributes
            .Attributes
            .OfType<TAttribute>();
    }

    /// <summary>
    /// 获取开放接口已过时信息
    /// </summary>
    /// <param name="obsoleteAttribute"><see cref="ObsoleteAttribute"/></param>
    /// <returns><see cref="OpenApiObsolete"/></returns>
    internal static OpenApiObsolete? GetObsoleteOrNull(ObsoleteAttribute? obsoleteAttribute)
    {
        return obsoleteAttribute is null
            ? null
            : new()
            {
                Message = obsoleteAttribute.Message
            };
    }
}