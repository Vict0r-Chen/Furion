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
/// 依赖关系图
/// </summary>
internal sealed class DependencyGraph
{
    /// <summary>
    /// 依赖关系集合
    /// </summary>
    internal readonly Dictionary<Type, Type[]> _dependencies;

    /// <summary>
    /// 祖先节点集合
    /// </summary>
    internal readonly Dictionary<Type, List<Type>> _ancestorsNodes;

    /// <summary>
    /// 后代节点集合
    /// </summary>
    internal readonly Dictionary<Type, List<Type>> _descendantsNodes;

    /// <summary>
    /// <inheritdoc cref="DependencyGraph"/>
    /// </summary>
    /// <param name="dependencies">依赖关系集合</param>
    internal DependencyGraph(Dictionary<Type, Type[]> dependencies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(dependencies);

        _dependencies = dependencies;
        _ancestorsNodes = new();
        _descendantsNodes = new();

        // 构建祖先节点和后代节点集合
        BuildAncestorsAndDescendantsNodes();
    }

    /// <summary>
    /// 构建祖先节点和后代节点集合
    /// </summary>
    internal void BuildAncestorsAndDescendantsNodes()
    {
        // 遍历依赖关系集合
        foreach (var (nodeType, dependencies) in _dependencies)
        {
            // 遍历当前节点的每个依赖关系
            foreach (var currentNode in dependencies)
            {
                // 查找当前节点祖先节点集合
                if (!_ancestorsNodes.TryGetValue(currentNode, out var ancestorsNode))
                {
                    ancestorsNode = new List<Type>();
                    _ancestorsNodes[currentNode] = ancestorsNode;
                }

                // 将当前节点添加到祖先节点集合中
                ancestorsNode.Add(nodeType);

                // 查找当前节点后代节点集合
                if (!_descendantsNodes.TryGetValue(nodeType, out var descendantsNode))
                {
                    descendantsNode = new List<Type>();
                    _descendantsNodes[nodeType] = descendantsNode;
                }

                // 将当前节点添加到后代节点集合中
                descendantsNode.Add(currentNode);
            }
        }
    }

    /// <summary>
    /// 查找节点的所有祖先节点
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<Type> FindAllAncestors(Type nodeType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(nodeType);

        // 初始化祖先节点集合
        var ancestors = new List<Type>();

        // 查找当前节点的祖先节点集合
        if (_ancestorsNodes.TryGetValue(nodeType, out var ancestorsNode))
        {
            // 将当前节点的祖先节点集合到集合中
            ancestors.AddRange(ancestorsNode);

            // 递归查找当前节点的祖先节点集合并添加到集合中
            foreach (var directParent in ancestorsNode)
            {
                ancestors.AddRange(FindAllAncestors(directParent));
            }
        }

        return ancestors;
    }

    /// <summary>
    /// 查找节点的所有后代节点
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<Type> FindAllDescendants(Type nodeType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(nodeType);

        // 初始化后代节点集合
        var descendants = new List<Type>();

        // 查找当前节点的后代节点集合
        if (_descendantsNodes.TryGetValue(nodeType, out var descendantsNode))
        {
            // 将当前节点的后代节点集合到集合中
            descendants.AddRange(descendantsNode);

            // 递归查找当前节点的后代节点集合并添加到集合中
            foreach (var directChild in descendantsNode)
            {
                descendants.AddRange(FindAllDescendants(directChild));
            }
        }

        return descendants;
    }

    /// <summary>
    /// 查找节点的所有祖先节点
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<Type> FindAncestors(Type nodeType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(nodeType);

        // 去重并返回
        return FindAllAncestors(nodeType)
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// 查找节点的所有后代节点
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <returns><see cref="List{T}"/></returns>
    internal List<Type> FindDescendants(Type nodeType)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(nodeType);

        // 去重并返回
        return FindAllDescendants(nodeType)
            .Distinct()
            .ToList();
    }
}