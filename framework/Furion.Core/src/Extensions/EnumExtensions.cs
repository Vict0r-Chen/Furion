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

namespace Furion.Core;

/// <summary>
/// 枚举拓展类
/// </summary>
internal static class EnumExtensions
{
    /// <summary>
    /// 获取枚举值描述
    /// </summary>
    /// <param name="enumValue">枚举值</param>
    /// <returns><see cref="string"/></returns>
    /// <exception cref="ArgumentException"></exception>
    internal static string GetEnumDescription(this object enumValue)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(enumValue);

        // 获取枚举类型
        var enumType = enumValue.GetType();

        // 检查是否是枚举类型
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("The parameter is not an enumeration type.", nameof(enumValue));
        }

        // 获取枚举名称
        var enumName = Enum.GetName(enumType, enumValue);

        // 空检查
        ArgumentNullException.ThrowIfNull(enumName);

        // 获取枚举字段
        var enumField = enumType.GetField(enumName);

        // 空检查
        ArgumentNullException.ThrowIfNull(enumField);

        // 获取 [Description] 特性描述
        return enumField.GetCustomAttribute<DescriptionAttribute>(false)
            ?.Description ?? enumName;
    }
}