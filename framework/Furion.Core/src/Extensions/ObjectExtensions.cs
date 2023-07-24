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

namespace System;

/// <summary>
/// <see cref="object"/> 拓展类
/// </summary>
internal static class ObjectExtensions
{
    /// <summary>
    /// 尝试获取对象的数量
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="count">数量</param>
    /// <returns><see cref="bool"/></returns>
    internal static bool TryGetCount(this object obj, out int count)
    {
        switch (obj)
        {
            // 处理字符类型
            case char:
                count = 1;
                return true;
            // 处理字符串类型
            case string text:
                count = text.Length;
                return true;
            // 检查对象是否实现了 ICollection 接口，如果是则直接获取 Count 属性值并返回
            case ICollection collection:
                count = collection.Count;
                return true;
        }

        // 如果对象没有实现 ICollection 接口，则通过反射来获取 Count 属性值
        var runtimeProperty = obj.GetType().GetRuntimeProperty("Count");

        // 检查是否获取到 Count 属性，且该属性可读且类型为 int
        if (runtimeProperty is not null
            && runtimeProperty.CanRead
            && runtimeProperty.PropertyType == typeof(int))
        {
            // 通过反射获取 Count 属性的值，并将其转换为 int 类型
            count = (int)runtimeProperty.GetValue(obj)!;
            return true;
        }

        // 如果无法获取到对象的数量，则将 count 的值设为 -1，表示获取失败
        count = -1;

        // 返回 false，表示无法获取对象的数量
        return false;
    }
}