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

namespace Furion.Configuration;

/// <summary>
/// 远程配置提供器
/// </summary>
internal sealed class RemotedConfigurationProvider : ConfigurationProvider
{
    /// <summary>
    /// <see cref="RemotedConfigurationModel"/> 集合
    /// </summary>
    internal readonly List<RemotedConfigurationModel> _remotedConfigurationModels;

    /// <inheritdoc cref="FileConfigurationParser" />
    internal readonly FileConfigurationParser _fileConfigurationParser;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="remotedConfigurationModels"><see cref="RemotedConfigurationModel"/> 集合</param>
    /// <param name="fileConfigurationParser"><see cref="FileConfigurationParser"/></param>
    internal RemotedConfigurationProvider(List<RemotedConfigurationModel> remotedConfigurationModels
        , FileConfigurationParser fileConfigurationParser)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(remotedConfigurationModels);
        ArgumentNullException.ThrowIfNull(fileConfigurationParser);

        _remotedConfigurationModels = remotedConfigurationModels;
        _fileConfigurationParser = fileConfigurationParser;
    }

    /// <inheritdoc />
    public override void Load()
    {
        // 初始化远程配置解析器对象
        var remotedConfigurationParser = new RemotedConfigurationParser(_fileConfigurationParser);

        // 解析远程请求地址并返回配置集合
        var data = _remotedConfigurationModels.SelectMany(remotedConfigurationParser.ParseRequestUri)
            .ToDictionary(u => u.Key
            , u => u.Value
            , StringComparer.OrdinalIgnoreCase);

        Data = data;
    }
}