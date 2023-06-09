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
/// 拓扑查找依赖关系
/// </summary>
internal static class Topological
{
    /// <summary>
    /// 创建依赖关系
    /// </summary>
    /// <param name="entryType"></param>
    /// <returns></returns>
    internal static Dictionary<Type, Type[]> CreateDependencies(Type entryType)
    {
        var dependencies = new Dictionary<Type, Type[]>();

        CreateDependenciesHelper(entryType, dependencies);

        return dependencies;
    }

    internal static void CreateDependenciesHelper(Type entryType, Dictionary<Type, Type[]> dependencies)
    {
        var isDefinedDependsOn = entryType.IsDefined(typeof(DependsOnAttribute), true);
        if (!isDefinedDependsOn)
        {
            return;
        }

        var dependsOnAttribute = entryType.GetCustomAttribute<DependsOnAttribute>(true)!;

        if (!dependencies.ContainsKey(entryType))
        {
            dependencies.Add(entryType, dependsOnAttribute.Dependencies);

            Array.ForEach(dependsOnAttribute.Dependencies, t =>
            {
                CreateDependenciesHelper(t, dependencies);
            });
        }
    }

    /// <summary>
    /// 拓扑排序算法
    /// </summary>
    /// <param name="dependencies"></param>
    /// <returns></returns>
    internal static List<Type> TopologicalSort(Dictionary<Type, Type[]> dependencies)
    {
        // 创建一个空列表来存储已排序的节点
        var sortedNodes = new List<Type>();

        // 创建一个set来存储已访问过的节点
        var visited = new HashSet<Type>();

        // 对每个未访问的节点进行深度优先搜索，将搜索到的节点插入到排序列表的头部
        foreach (var node in dependencies.Keys)
        {
            VisitNode(node, dependencies, visited, sortedNodes);
        }

        // 返回已排序的节点列表
        return sortedNodes;
    }

    /// <summary>
    /// 节点搜索
    /// </summary>
    /// <param name="node"></param>
    /// <param name="dependencies"></param>
    /// <param name="visited"></param>
    /// <param name="sortedNodes"></param>
    internal static void VisitNode(Type node, Dictionary<Type, Type[]> dependencies, HashSet<Type> visited, List<Type> sortedNodes)
    {
        // 如果当前节点已经被访问过，则直接返回
        if (visited.Contains(node))
        {
            return;
        }

        // 将当前节点标记为已访问
        visited.Add(node);

        // 访问当前节点所依赖的所有节点
        if (dependencies.TryGetValue(node, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                VisitNode(neighbor, dependencies, visited, sortedNodes);
            }
        }

        // 将当前节点插入到排序列表的尾部
        sortedNodes.Add(node);
    }

    /// <summary>
    /// 检查依赖关系是否存在循环依赖
    /// </summary>
    /// <param name="dependencies"></param>
    /// <returns></returns>
    internal static bool HasCycle(Dictionary<Type, Type[]> dependencies)
    {
        // 创建一个set来存储已访问过的节点
        var visited = new HashSet<Type>();

        // 创建一个set来存储当前遍历路径上的节点
        var currentPath = new HashSet<Type>();

        // 对每个未访问的节点进行深度优先搜索，检查是否存在循环依赖
        foreach (var node in dependencies.Keys)
        {
            if (HasCycleHelper(node, dependencies, visited, currentPath))
            {
                return true;
            }
        }

        // 如果不存在循环依赖，则返回false
        return false;
    }

    internal static bool HasCycleHelper(Type node, Dictionary<Type, Type[]> dependencies, HashSet<Type> visited, HashSet<Type> currentPath)
    {
        // 将当前节点标记为已访问，并将其加入到遍历路径中
        visited.Add(node);
        currentPath.Add(node);

        // 访问当前节点所依赖的所有节点
        if (dependencies.TryGetValue(node, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                // 如果遍历路径上已经包含了该邻居节点，则存在循环依赖，直接返回true
                if (currentPath.Contains(neighbor) ||
                    (!visited.Contains(neighbor) && HasCycleHelper(neighbor, dependencies, visited, currentPath)))
                {
                    return true;
                }
            }
        }

        // 遍历完当前节点的所有邻居节点后，将当前节点从遍历路径中移除
        currentPath.Remove(node);

        // 如果不存在循环依赖，则返回false
        return false;
    }
}