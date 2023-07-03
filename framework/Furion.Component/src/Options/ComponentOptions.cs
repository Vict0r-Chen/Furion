﻿// 麻省理工学院许可证
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
/// 组件模块配置选项
/// </summary>
internal sealed class ComponentOptions
{
    /// <summary>
    /// 获取 <see cref="GetPropsAction{TProps}()"/> 方法类型
    /// </summary>
    internal readonly MethodInfo _GetPropsActionMethod;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <remarks>此构造函数只会初始化一次</remarks>
    public ComponentOptions()
    {
        PropsActions ??= new();
        InvokeRecords ??= new();

        _GetPropsActionMethod = GetType().GetMethod(nameof(GetPropsAction)
            , 1
            , BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly
            , null
            , Type.EmptyTypes
            , null)!;
    }

    /// <summary>
    /// 组件配置委托集合
    /// </summary>
    internal Dictionary<Type, List<Delegate>> PropsActions { get; init; }

    /// <summary>
    /// 组件调用记录
    /// </summary>
    /// <remarks>作用于组件重复调用检查</remarks>
    internal ConcurrentBag<string> InvokeRecords { get; init; }

    /// <summary>
    /// 禁用组件重复调用
    /// </summary>
    internal bool SuppressDuplicateInvoke { get; set; } = true;

    /// <summary>
    /// 禁用 Web 组件重复调用
    /// </summary>
    internal bool SuppressDuplicateInvokeForWeb { get; set; } = true;

    /// <summary>
    /// SuppressDuplicateInvoke[ForWeb] 属性索引
    /// </summary>
    /// <param name="propName">属性名</param>
    /// <returns><see cref="bool"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal bool this[string propName] => propName switch
    {
        nameof(SuppressDuplicateInvoke) => SuppressDuplicateInvoke,
        nameof(SuppressDuplicateInvokeForWeb) => SuppressDuplicateInvokeForWeb,
        _ => throw new InvalidOperationException("Unsupported property name index.")
    };

    /// <summary>
    /// 获取组件配置委托
    /// </summary>
    /// <typeparam name="TProps">组件配置类型</typeparam>
    /// <returns><see cref="Action{T}"/></returns>
    internal Action<TProps>? GetPropsAction<TProps>()
        where TProps : class, new()
    {
        // 如果未找到组件类型参数则返回空
        if (!PropsActions.TryGetValue(typeof(TProps), out var values))
        {
            return null;
        }

        // 生成级联调用委托
        var cascadeAction = values.Cast<Action<TProps>>()
                                               .Aggregate((previous, current) => (t) =>
                                               {
                                                   previous(t);
                                                   current(t);
                                               });
        return cascadeAction;
    }

    /// <summary>
    /// 获取组件配置委托
    /// </summary>
    /// <param name="propsType">组件配置类型</param>
    /// <returns><see cref="Delegate"/></returns>
    internal Delegate? GetPropsAction(Type propsType)
    {
        return _GetPropsActionMethod.MakeGenericMethod(propsType)
                                    .Invoke(this, null) as Delegate;
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    internal void Release()
    {
        PropsActions.Clear();
        InvokeRecords.Clear();
    }
}