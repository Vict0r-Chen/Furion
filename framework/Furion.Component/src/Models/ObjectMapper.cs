// 麻省理工学院许可证
//
// 版权所有 (c) 2020-2023 百小僧，百签科技（广东）有限公司
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
    /// 对象映射
    /// </summary>
    /// <typeparam name="TSource">源对象类型</typeparam>
    /// <typeparam name="TDestination">目标对象类型</typeparam>
    /// <param name="source"><typeparamref name="TSource"/></param>
    /// <param name="destination"><typeparamref name="TSource"/></param>
    internal static void Map<TSource, TDestination>(TSource source, TDestination destination)
        where TSource : class, new()
        where TDestination : class, new()
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(destination, nameof(destination));

        // 反射查找成员绑定标记
        var bindingFlags = BindingFlags.Instance | BindingFlags.Public;

        // 获取源对象和目标对象的属性集合
        var sourceProperties = typeof(TSource).GetProperties(bindingFlags);
        var destinationProperties = typeof(TDestination).GetProperties(bindingFlags);

        // 创建源对象参数和目标对象参数的表达式参数
        var sourceParameter = Expression.Parameter(typeof(TSource), nameof(source));
        var destinationParameter = Expression.Parameter(typeof(TDestination), nameof(destination));

        // 遍历源对象的属性集合
        foreach (var sourceProperty in sourceProperties)
        {
            // 在目标对象的属性集合中查找与源对象属性匹配的属性
            var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);

            // 空检查
            if (destinationProperty == null)
            {
                continue;
            }

            // 创建表达式树的成员访问表达式，表示源对象的属性值
            var sourceValue = Expression.Property(sourceParameter, sourceProperty);

            // 创建表达式树的成员访问表达式，表示目标对象的属性
            var destinationValue = Expression.Property(destinationParameter, destinationProperty);

            // 创建表达式树的赋值表达式，将源对象的属性值赋给目标对象的属性
            var assignment = Expression.Assign(destinationValue, Expression.Convert(sourceValue, destinationValue.Type));

            // 创建表达式树的 Lambda 表达式，表示源对象和目标对象的参数以及赋值表达式
            var lambda = Expression.Lambda<Action<TSource, TDestination>>(assignment, sourceParameter, destinationParameter);

            // 编译 Lambda 表达式生成委托，并执行委托，实现属性值的映射
            var compiledLambda = lambda.Compile();
            compiledLambda(source, destination);
        }
    }
}