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
    /// <param name="obj"><see cref="object"/></param>
    /// <param name="count">数量</param>
    /// <returns><see cref="bool"/></returns>
    internal static bool TryGetCount(this object obj, out int count)
    {
        switch (obj)
        {
            // 检查对象是否是字符类型
            case char:
                count = 1;
                return true;
            // 检查对象是否是字符串类型
            case string text:
                count = text.Length;
                return true;
            // 检查对象是否实现了 ICollection 接口
            case ICollection collection:
                count = collection.Count;
                return true;
        }

        // 反射查找是否存在 Count 属性
        var runtimeProperty = obj.GetType()
            .GetRuntimeProperty("Count");

        // 获取 Count 属性值
        if (runtimeProperty is not null
            && runtimeProperty.CanRead
            && runtimeProperty.PropertyType == typeof(int))
        {
            count = (int)runtimeProperty.GetValue(obj)!;
            return true;
        }

        count = -1;
        return false;
    }
}