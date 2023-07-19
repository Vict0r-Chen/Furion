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
/// 依赖关系图算法
/// </summary>
internal sealed class DependencyGraph
{
    /// <summary>
    /// 依赖关系集合
    /// </summary>
    internal readonly Dictionary<Type, Type[]> _dependencies;

    /// <summary>
    /// 祖先节点关系集合
    /// </summary>
    internal readonly Dictionary<Type, List<Type>> _ancestorsNodes;

    /// <summary>
    /// 后代节点关系集合
    /// </summary>
    internal readonly Dictionary<Type, List<Type>> _descendantsNodes;

    /// <summary>
    /// <inheritdoc cref="DependencyGraph"/>
    /// </summary>
    /// <param name="dependencies">依赖关系集合</param>
    internal DependencyGraph(Dictionary<Type, Type[]> dependencies)
    {
        _dependencies = dependencies;
        _ancestorsNodes = new Dictionary<Type, List<Type>>();
        _descendantsNodes = new Dictionary<Type, List<Type>>();

        // 构建祖先节点和后代节点集合
        BuildAncestorsAndDescendantsNodes();
    }

    /// <summary>
    /// 构建祖先节点和后代节点集合
    /// </summary>
    internal void BuildAncestorsAndDescendantsNodes()
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
                if (!_ancestorsNodes.TryGetValue(dependency, out var ancestorsNode))
                {
                    ancestorsNode = new List<Type>();
                    _ancestorsNodes[dependency] = ancestorsNode;
                }

                // 将当前节点设为依赖节点的祖先节点
                ancestorsNode.Add(dependent);

                if (!_descendantsNodes.TryGetValue(dependent, out var descendantsNode))
                {
                    descendantsNode = new List<Type>();
                    _descendantsNodes[dependent] = descendantsNode;
                }

                // 将依赖节点设为当前节点的后代节点
                descendantsNode.Add(dependency);
            }
        }
    }

    /// <summary>
    /// 查找给定节点类型的所有祖先节点（内部方法）
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<Type> FindAllAncestors(Type nodeType)
    {
        var ancestors = new List<Type>();

        if (_ancestorsNodes.TryGetValue(nodeType, out var ancestorsNode))
        {
            // 直接祖先节点
            var directAncestors = ancestorsNode;
            ancestors.AddRange(directAncestors);

            foreach (var directParent in directAncestors)
            {
                // 递归查找直接祖先节点的祖先节点
                ancestors.AddRange(FindAllAncestors(directParent));
            }
        }

        return ancestors;
    }

    /// <summary>
    /// 查找给定节点类型的所有后代节点（内部方法）
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<Type> FindAllDescendants(Type nodeType)
    {
        var descendants = new List<Type>();

        if (_descendantsNodes.TryGetValue(nodeType, out var descendantsNode))
        {
            // 直接后代节点
            var directDescendants = descendantsNode;
            descendants.AddRange(directDescendants);

            foreach (var directChild in directDescendants)
            {
                // 递归查找直接后代节点的后代节点
                descendants.AddRange(FindAllDescendants(directChild));
            }
        }

        return descendants;
    }

    /// <summary>
    /// 查找指定节点类型的所有祖先节点
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<Type> FindAncestors(Type nodeType)
    {
        // 去重并返回结果
        return FindAllAncestors(nodeType)
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// 查找指定节点类型的所有后代节点
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<Type> FindDescendants(Type nodeType)
    {
        // 去重并返回结果
        return FindAllDescendants(nodeType)
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    internal void Release()
    {
        _dependencies.Clear();
        _ancestorsNodes.Clear();
        _descendantsNodes.Clear();
    }
}