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

namespace Furion.Core.Tests;

public class IDictionaryExtensionsTests
{
    [Fact]
    public void AddOrUpdate_Invalid_Parameters()
    {
        var dic = new Dictionary<string, List<object>>();
        Assert.Throws<ArgumentNullException>(() =>
        {
            dic.AddOrUpdate("key", null!);
        });
    }

    [Fact]
    public void AddOrUpdate_ReturnOK()
    {
        var dic = new Dictionary<string, List<object>>();

        dic.AddOrUpdate("key1", 1);
        dic.AddOrUpdate("key1", 2);
        dic.AddOrUpdate("key2", 1);

        Assert.Equal(2, dic.Count);
        Assert.Equal([1, 2], dic["key1"]);
        Assert.Equal([1], dic["key2"]);
    }

    [Fact]
    public void AddOrUpdateDictionary_Invalid_Parameters()
    {
        var dic = new Dictionary<string, List<object>>();
        Assert.Throws<ArgumentNullException>(() =>
        {
            dic.AddOrUpdate(null!);
        });
    }

    [Fact]
    public void AddOrUpdateDictionary_ReturnOK()
    {
        var dic = new Dictionary<string, List<object>>
        {
            {"key1", new List<object>{ 1, 2 } },
            {"key2", new List<object>{ 1 } },
        };

        var dic2 = new Dictionary<string, List<object>>
        {
            {"key1", new List<object>{ 3, 4 } },
            {"key2", new List<object>{ 2 } },
            {"key3", new List<object>{ 1 } },
        };

        dic.AddOrUpdate(dic2);

        Assert.Equal(3, dic.Count);
        Assert.Equal([1, 2, 3, 4], dic["key1"]);
        Assert.Equal([1, 2], dic["key2"]);
        Assert.Equal([1], dic["key3"]);
    }

    [Fact]
    public void AddOrUpdateDictionary2_Invalid_Parameters()
    {
        var dic = new Dictionary<string, string>();
        Assert.Throws<ArgumentNullException>(() =>
        {
            dic.AddOrUpdate(null!);
        });
    }

    [Fact]
    public void AddOrUpdateDictionary2_ReturnOK()
    {
        var dic = new Dictionary<string, string>
        {
            {"key1", "1" },
            {"key2", "2" },
        };

        var dic2 = new Dictionary<string, string>
        {
            {"key1", "3" },
            {"key2", "4" },
            {"key3", "5" },
        };

        dic.AddOrUpdate(dic2);

        Assert.Equal(3, dic.Count);
        Assert.Equal("3", dic["key1"]);
        Assert.Equal("4", dic["key2"]);
        Assert.Equal("5", dic["key3"]);
    }
}