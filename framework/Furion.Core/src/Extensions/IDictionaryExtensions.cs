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

namespace System.Collections.Generic;

/// <summary>
/// <see cref="IDictionary{TKey, TValue}"/> 拓展类
/// </summary>
internal static class IDictionaryExtensions
{
    /// <summary>
    /// 添加或更新
    /// </summary>
    /// <typeparam name="TKey">字典键类型</typeparam>
    /// <typeparam name="TValue">字典值类型</typeparam>
    /// <param name="dictionary"><see cref="IDictionary{TKey, TValue}"/></param>
    /// <param name="key"><typeparamref name="TKey"/></param>
    /// <param name="value"><typeparamref name="TValue"/></param>
    internal static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary
        , TKey key
        , TValue value)
        where TKey : notnull
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(value);

        // 检查键是否存在
        if (!dictionary.TryGetValue(key, out var values))
        {
            values = new();
            dictionary.Add(key, values);
        }

        values.Add(value);
    }

    /// <summary>
    /// 添加或更新
    /// </summary>
    /// <typeparam name="TKey">字典键类型</typeparam>
    /// <typeparam name="TValue">字典值类型</typeparam>
    /// <param name="dictionary"><see cref="IDictionary{TKey, TValue}"/></param>
    /// <param name="concatDictionary"><see cref="IDictionary{TKey, TValue}"/></param>
    internal static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, IDictionary<TKey, List<TValue>> concatDictionary)
         where TKey : notnull
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(concatDictionary);

        // 逐条遍历合并更新
        foreach (var (key, newValues) in concatDictionary)
        {
            // 检查键是否存在
            if (!dictionary.TryGetValue(key, out var values))
            {
                values = new();
                dictionary.Add(key, values);
            }

            values.AddRange(newValues);
        }
    }
}