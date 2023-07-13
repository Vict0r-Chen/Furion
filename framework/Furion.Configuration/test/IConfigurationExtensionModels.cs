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

namespace Furion.Configuration.Tests;

public class IConfigurationExtensionModels
{
    public class FurionModel
    {
        public string? Name { get; set; }
        public Version? Version { get; set; }
        public Uri? Homepage { get; set; }
        public int Age { get; set; }
        public decimal Price { get; set; }
        public bool IsMIT { get; set; }

        public string[]? Tags { get; set; }
        public Dictionary<string, object?>? Properties { get; set; }

        public FurionAuthor? Author { get; set; }

        internal string? NotPublic { get; set; }
    }

    public class FurionAuthor
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    public class ReloadConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ReloadConfigurationProvider();
        }
    }

    public class ReloadConfigurationProvider : ConfigurationProvider
    {
        public int Count { get; set; }

        public override void Load()
        {
            Count++;
            Data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
            {
                {nameof(Count), $"{Count}"}
            };
        }
    }

    public class UnknownConfigurationSource
    {
    }
}