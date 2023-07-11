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

namespace System.Linq.Expressions;

/// <summary>
/// <see cref="Expression"/> 拓展类
/// </summary>
internal static class ExpressionExtensions
{
    /// <summary>
    /// 解析表达式属性名称
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <typeparam name="TProperty">属性类型</typeparam>
    /// <param name="propertySelector">属性选择器</param>
    /// <returns><see cref="string"/></returns>
    /// <exception cref="ArgumentException"></exception>
    internal static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty?>> propertySelector)
        where T : class
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(propertySelector, nameof(propertySelector));

        // 检查 Lambda 表达式的主体是否是 MemberExpression 类型
        if (propertySelector.Body is MemberExpression memberExpression)
        {
            // 获取 MemberExpression 的 Member 属性，返回属性的名称
            return memberExpression.Member.Name;
        }
        // 如果主体是 UnaryExpression 类型，则继续解析
        else if (propertySelector.Body is UnaryExpression unaryExpression
            && unaryExpression.Operand is MemberExpression nestedMemberExpression)
        {
            // 获取嵌套的 MemberExpression 的 Member 属性，返回属性的名称
            return nestedMemberExpression.Member.Name;
        }

        // 如果无法解析属性名称，抛出 ArgumentException 异常
        throw new ArgumentException($"The property name for type {typeof(T).Name} cannot be resolved.");
    }
}