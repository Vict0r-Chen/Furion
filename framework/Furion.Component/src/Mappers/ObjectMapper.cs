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

namespace Furion.Component;

/// <summary>
/// 简单对象映射
/// </summary>
internal static class ObjectMapper
{
    /// <summary>
    /// 简单对象映射
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <param name="source"><typeparamref name="TSource"/></param>
    /// <param name="destination"><typeparamref name="TDestination"/></param>
    internal static void Map<TSource, TDestination>(TSource source, TDestination destination)
        where TSource : class
        where TDestination : class
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        // 初始化反射搜索成员方式
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        // 获取源类型属性集合和目标类型属性集合
        var sourceProperties = typeof(TSource).GetProperties(bindingFlags);
        var destinationProperties = typeof(TDestination).GetProperties(bindingFlags);

        // 创建表达式入参 s, d
        var sourceParameterExpression = Expression.Parameter(typeof(TSource));
        var destinationParameterExpression = Expression.Parameter(typeof(TDestination));

        // 遍历源对象的属性集合
        foreach (var sourceProperty in sourceProperties)
        {
            // 查找目标类型匹配的属性
            var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name
                && p.PropertyType == sourceProperty.PropertyType
                && p.CanWrite);

            // 空检查
            if (destinationProperty is null)
            {
                continue;
            }

            // 创建表达式 s.Property
            var sourcePropertyExpression = Expression.Property(sourceParameterExpression, sourceProperty);

            // 创建表达式 d.Property
            var destinationPropertyExpression = Expression.Property(destinationParameterExpression, destinationProperty);

            // 创建表达式 d.Property = s.Property
            var assignmentExpression = Expression.Assign(destinationPropertyExpression
                , Expression.Convert(sourcePropertyExpression, destinationPropertyExpression.Type));

            // 创建表达式 (s, d) => d.Property = s.Property
            var lambdaExpression = Expression.Lambda<Action<TSource, TDestination>>(assignmentExpression
                , sourceParameterExpression
                , destinationParameterExpression);

            // 将表达式转换成委托
            var action = lambdaExpression.Compile();

            // 调用委托
            action(source, destination);
        }
    }
}