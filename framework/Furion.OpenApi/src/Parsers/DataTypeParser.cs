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
/// 数据类型解析器
/// </summary>
public static class DataTypeParser
{
    /// <summary>
    /// 根据类型解析对应的数据类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="DataTypes"/></returns>
    public static DataTypes Parse(Type? type)
    {
        // 空检查
        if (type is null)
        {
            return DataTypes.Any;
        }

        return type switch
        {
            _ when typeof(string).IsAssignableFrom(type) || typeof(char).IsAssignableFrom(type) => DataTypes.String,
            _ when type.IsNumeric() => DataTypes.Number,
            _ when typeof(bool).IsAssignableFrom(type) => DataTypes.Boolean,
            _ when typeof(DateTime).IsAssignableFrom(type) || typeof(DateTimeOffset).IsAssignableFrom(type) || typeof(DateOnly).IsAssignableFrom(type) => DataTypes.Date,
            _ when typeof(TimeOnly).IsAssignableFrom(type) => DataTypes.Time,
            _ when type.IsEnum => DataTypes.Enum,
            _ when typeof(IFormFile).IsAssignableFrom(type) => DataTypes.Binary,
            _ when IsFormFileCollection(type) => DataTypes.BinaryCollection,
            _ when type.IsDictionary() => DataTypes.Record,
            _ when typeof(ITuple).IsAssignableFrom(type) => DataTypes.Tuple,
            _ when type.IsArray || (typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType && type.GenericTypeArguments.Length == 1) => DataTypes.Array,
            _ when type != typeof(object) && type.IsClass && Type.GetTypeCode(type) == TypeCode.Object => DataTypes.Object,
            _ when type.IsValueType && !type.IsPrimitive && !type.IsEnum => DataTypes.Struct,
            _ => DataTypes.Any
        };
    }

    /// <summary>
    /// 检查类型是否是 IFormFileCollection 类型
    /// </summary>
    /// <param name="type"><see cref="Type"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsFormFileCollection(Type type)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(type);

        // 如果是 IFormFileCollection 类型则直接返回
        if (typeof(IFormFileCollection).IsAssignableFrom(type))
        {
            return true;
        }

        // 处理 IFormFile 集合类型
        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            // 检查是否是 IFormFile 数组类型
            if (type.IsArray)
            {
                // 获取数组元素类型
                var elementType = type.GetElementType();

                // 检查元素类型是否是 IFormFile 类型
                if (elementType is not null
                    && typeof(IFormFile).IsAssignableFrom(elementType))
                {
                    return true;
                }
            }
            // 检查是否是 IFormFile 集合类型
            else
            {
                // 检查集合项类型是否是 IFormFile 类型
                if (type is { IsGenericType: true, GenericTypeArguments.Length: 1 }
                    && typeof(IFormFile).IsAssignableFrom(type.GenericTypeArguments[0]))
                {
                    return true;
                }
            }
        }

        return false;
    }
}