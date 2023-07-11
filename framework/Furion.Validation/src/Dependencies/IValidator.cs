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

namespace Furion.Validation;

/// <summary>
/// 类型验证器接口
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
public interface IValidator<T>
   where T : class
{
    /// <summary>
    /// 禁止注解（特性）验证
    /// </summary>
    bool SuppressAnnotations { get; set; }

    /// <summary>
    /// 配置条件
    /// </summary>
    /// <param name="condition">条件委托</param>
    /// <returns><see cref="IValidator{T}"/></returns>
    IValidator<T> When(Func<T, bool> condition);

    /// <summary>
    /// 配置条件
    /// </summary>
    /// <param name="condition">条件委托</param>
    /// <returns><see cref="IValidator{T}"/></returns>
    IValidator<T> WhenContext(Func<ValidationContext, bool> condition);

    /// <summary>
    /// 清空条件
    /// </summary>
    /// <returns><see cref="IValidator{T}"/></returns>
    IValidator<T> Reset();

    /// <summary>
    /// 检查值有效性
    /// </summary>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <returns><see cref="bool"/></returns>
    bool IsValid(T instance);

    /// <summary>
    /// 获取验证结果
    /// </summary>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <returns><see cref="ValidationResult"/> 集合</returns>
    List<ValidationResult>? GetValidationResults(T instance);

    /// <summary>
    /// 执行验证
    /// </summary>
    /// <param name="instance"><typeparamref name="T"/></param>
    /// <exception cref="AggregateValidationException"></exception>
    void Validate(T instance);
}