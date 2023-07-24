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
/// 拓扑图
/// </summary>
internal sealed class TopologicalGraph
{
    /// <summary>
    /// 依赖关系集合
    /// </summary>
    internal readonly Dictionary<Type, Type[]> _dependencies;

    /// <summary>
    /// <inheritdoc cref="TopologicalGraph"/>
    /// </summary>
    /// <param name="dependencies">依赖关系集合</param>
    internal TopologicalGraph(Dictionary<Type, Type[]> dependencies)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(dependencies);

        _dependencies = dependencies;
    }

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="ensureLegal">是否启用合法性检查</param>
    /// <returns><see cref="List{T}"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal List<Type> Sort(bool ensureLegal = false)
    {
        // 循环依赖检查
        if (ensureLegal && HasCycle())
        {
            throw new InvalidOperationException("The dependency relationship has a circular dependency.");
        }

        // 初始化已排序的节点集合
        var sortedNodes = new List<Type>();

        // 初始化已访问过的节点集合
        var visitedNodes = new HashSet<Type>();

        // 遍历依赖关系集合中的键进行排序操作
        foreach (var nodeType in _dependencies.Keys)
        {
            // 节点访问记录
            VisitNode(nodeType, visitedNodes, sortedNodes);
        }

        return sortedNodes;
    }

    /// <summary>
    /// 节点访问记录
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <param name="visitedNodes">已访问过的节点集合</param>
    /// <param name="sortedNodes">已排序的节点集合</param>
    internal void VisitNode(Type nodeType
        , HashSet<Type> visitedNodes
        , List<Type> sortedNodes)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(nodeType);
        ArgumentNullException.ThrowIfNull(visitedNodes);
        ArgumentNullException.ThrowIfNull(sortedNodes);

        // 检查当前节点是否已访问过
        if (visitedNodes.Contains(nodeType))
        {
            return;
        }

        // 将当前节点添加到已访问过的节点集合中
        visitedNodes.Add(nodeType);

        // 查找当前节点依赖节点集合
        if (_dependencies.TryGetValue(nodeType, out var dependencies))
        {
            // 遍历当前节点的每个依赖节点
            foreach (var currentNode in dependencies)
            {
                // 节点访问记录
                VisitNode(currentNode, visitedNodes, sortedNodes);
            }
        }

        // 将当前节点添加到已排序的节点集合中
        sortedNodes.Add(nodeType);
    }

    /// <summary>
    /// 循环依赖检查
    /// </summary>
    /// <returns><see cref="bool"/></returns>
    internal bool HasCycle()
    {
        // 初始化已遍历路径的节点集合
        var pathNodes = new HashSet<Type>();

        // 初始化已访问过的节点集合
        var visitedNodes = new HashSet<Type>();

        // 循环依赖检查
        return _dependencies.Keys
            .Any(currentNode => HasCycleHelper(currentNode, visitedNodes, pathNodes));
    }

    /// <summary>
    /// 循环依赖检查（内部方法）
    /// </summary>
    /// <param name="nodeType">节点类型</param>
    /// <param name="visitedNodes">已访问过的节点集合</param>
    /// <param name="pathNodes">已遍历路径的节点集合</param>
    /// <returns><see cref="bool"/></returns>
    internal bool HasCycleHelper(Type nodeType
        , HashSet<Type> visitedNodes
        , HashSet<Type> pathNodes)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(nodeType);
        ArgumentNullException.ThrowIfNull(visitedNodes);
        ArgumentNullException.ThrowIfNull(pathNodes);

        // 将当前节点添加到已访问过的节点集合和已遍历路径的节点集合中
        visitedNodes.Add(nodeType);
        pathNodes.Add(nodeType);

        // 查找当前节点依赖节点集合
        if (_dependencies.TryGetValue(nodeType, out var dependencies))
        {
            // 循环依赖检查
            if (dependencies.Any(currentNode => pathNodes.Contains(currentNode)
                || (!visitedNodes.Contains(currentNode) && HasCycleHelper(currentNode, visitedNodes, pathNodes))))
            {
                Debugging.Error("The node type `{0}` has circular dependencies.", nodeType);

                return true;
            }
        }

        // 将当前节点从已遍历路径的节点集合中移除
        pathNodes.Remove(nodeType);

        return false;
    }
}