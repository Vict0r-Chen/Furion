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
/// 开放接口属性解析器
/// </summary>
internal sealed class OpenApiPropertyParser
{
    /// <inheritdoc cref="DefaultModelMetadata"/>
    internal readonly DefaultModelMetadata _modelMetadata;

    /// <summary>
    /// <inheritdoc cref="OpenApiPropertyParser"/>
    /// </summary>
    /// <param name="modelMetadata"><see cref="ModelMetadata"/></param>
    internal OpenApiPropertyParser(ModelMetadata modelMetadata)
    {
        // 获取默认模型元数据
        var defaultModelMetadata = modelMetadata as DefaultModelMetadata;

        // 空检查
        ArgumentNullException.ThrowIfNull(defaultModelMetadata);

        _modelMetadata = defaultModelMetadata;
    }

    /// <summary>
    /// 解析模型元数据并返回开放接口属性
    /// </summary>
    /// <returns><see cref="OpenApiProperty"/></returns>
    internal OpenApiProperty Parser()
    {
        // 获取模型类型
        var modelType = _modelMetadata.ModelType;

        // 初始化开放接口属性
        var openApiProperty = new OpenApiProperty
        {
            Name = _modelMetadata.Name,
            Description = _modelMetadata.Description ?? GetMetadataAttribute<DescriptionAttribute>()?.Description,
            DefaultValue = default,
            IsRequired = GetMetadataAttribute<RequiredAttribute>() is not null,
            AllowNullValue = GetMetadataAttribute<RequiredAttribute>() is null && _modelMetadata.IsReferenceOrNullableType is true,
            DataType = DataTypeParser.Parse(modelType),
            RuntimeType = modelType?.ToString(),
            TypeCode = Type.GetTypeCode(modelType),
        };

        // 解析枚举类型模型数据
        if (modelType?.IsEnum is true)
        {
            openApiProperty.Properties = ParseEnum(modelType);
        }

        // 解析对象类型属性模型数据
        if (openApiProperty.DataType is DataTypes.Object)
        {
            openApiProperty.Properties = ParseObject(modelType!);
        }

        return openApiProperty;
    }

    /// <summary>
    /// 解析枚举类型模型数据
    /// </summary>
    /// <param name="modelType"><see cref="Type"/></param>
    /// <returns><see cref="IDictionary{TKey, TValue}"/></returns>
    /// <exception cref="ArgumentException"></exception>
    internal static IDictionary<string, object>? ParseEnum(Type modelType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(modelType);

        // 检查类型是否是枚举类型
        if (!modelType.IsEnum)
        {
            throw new ArgumentException("The `modelType` parameter is not a valid enumeration type.", nameof(modelType));
        }

        // 转换枚举类型为字典类型
        return Enum.GetValues(modelType)
            .Cast<object>()
            .ToDictionary(u => u.ToString()!, u => (object)((int)u), StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 解析对象类型属性模型数据
    /// </summary>
    /// <param name="modelType"><see cref="Type"/></param>
    /// <returns><see cref="IDictionary{TKey, TValue}"/></returns>
    internal IDictionary<string, object>? ParseObject(Type modelType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(modelType);

        // 检查是否是对象类型
        if (DataTypeParser.Parse(modelType) is not DataTypes.Object)
        {
            return null;
        }

        // 初始化对象类型属性模型集合
        var properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        // 获取对象类型属性模型元数据集合
        var propertyModelMetadatas = _modelMetadata.Properties.Cast<DefaultModelMetadata>();

        // 遍历对象类型属性模型元数据集合
        foreach (var propertyModelMetadata in propertyModelMetadatas)
        {
            // 解析模型元数据并返回开放接口属性
            var openApiProperty = new OpenApiPropertyParser(propertyModelMetadata)
                .Parser();

            properties.Add(openApiProperty.Name!, openApiProperty);
        }

        return properties;
    }

    /// <summary>
    /// 获取模型元数据定义的特性
    /// </summary>
    /// <typeparam name="TAttribute"><see cref="Attribute"/></typeparam>
    /// <returns><typeparamref name="TAttribute"/></returns>
    internal TAttribute? GetMetadataAttribute<TAttribute>()
        where TAttribute : Attribute
    {
        return _modelMetadata.Attributes.Attributes.OfType<TAttribute>().FirstOrDefault();
    }
}