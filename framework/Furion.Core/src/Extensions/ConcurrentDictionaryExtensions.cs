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

namespace System.Collections.Concurrent;

/// <summary>
/// <see cref="ConcurrentDictionary{TKey, TValue}"/> 拓展类
/// </summary>
internal static class ConcurrentDictionaryExtensions
{
    /// <summary>
    /// 根据字典键更新对应的值
    /// </summary>
    /// <typeparam name="TKey">字典键类型</typeparam>
    /// <typeparam name="TValue">字典值类型</typeparam>
    /// <param name="dictionary"><see cref="ConcurrentDictionary{TKey, TValue}"/></param>
    /// <param name="key"><typeparamref name="TKey"/></param>
    /// <param name="updateFactory">自定义更新委托</param>
    /// <param name="value"><typeparamref name="TValue"/></param>
    /// <returns><see cref="bool"/></returns>
    internal static bool TryUpdate<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary
        , TKey key
        , Func<TValue?, TValue> updateFactory
        , out TValue? value)
        where TKey : notnull
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(updateFactory);

        // 查找键值
        if (!dictionary.TryGetValue(key, out var oldValue))
        {
            value = default;
            return false;
        }

        // 调用自定义更新委托
        var newValue = updateFactory(oldValue);

        // 更新键值
        var result = dictionary.TryUpdate(key, newValue, oldValue);
        value = result ? newValue : oldValue;

        return result;
    }
}