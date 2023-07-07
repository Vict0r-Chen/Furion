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
/// 依赖关系图
/// </summary>
internal sealed class DependencyGraph
{
    /// <summary>
    /// 依赖关系集合
    /// </summary>
    internal readonly Dictionary<Type, List<Type>> _dependencies;

    /// <summary>
    /// 父节点关系集合
    /// </summary>
    internal readonly Dictionary<Type, List<Type>> _parentNodes;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dependencies">依赖关系集合</param>
    internal DependencyGraph(Dictionary<Type, List<Type>> dependencies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(dependencies, nameof(dependencies));

        _dependencies = dependencies;
        _parentNodes = new Dictionary<Type, List<Type>>();

        // 构建父节点字典
        BuildParentNodes();
    }

    /// <summary>
    /// 构建父节点字典
    /// </summary>
    internal void BuildParentNodes()
    {
        // 遍历依赖关系集合
        foreach (var entry in _dependencies)
        {
            // 当前节点
            var dependent = entry.Key;

            // 当前节点的依赖关系
            var dependencies = entry.Value;

            // 遍历当前节点的每个依赖关系
            foreach (var dependency in dependencies)
            {
                if (!_parentNodes.TryGetValue(dependency, out var value))
                {
                    value = new List<Type>();
                    _parentNodes[dependency] = value;
                }

                // 将当前节点设为依赖节点的父节点
                value.Add(dependent);
            }
        }
    }

    /// <summary>
    /// 查找给定输入类型的所有父节点
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<Type> FindAllParents(Type nodeType)
    {
        var parents = new List<Type>();

        if (_parentNodes.TryGetValue(nodeType, out List<Type>? value))
        {
            // 直接父节点
            var directParents = value;
            parents.AddRange(directParents);

            foreach (var directParent in directParents)
            {
                // 递归查找直接父节点的父节点
                parents.AddRange(FindAllParents(directParent));
            }
        }

        return parents;
    }

    /// <summary>
    /// 查找指定输入类型的所有父节点
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<Type> FindParents(Type nodeType)
    {
        // 去重并返回结果
        return FindAllParents(nodeType)
            .Distinct()
            .ToList();
    }
}