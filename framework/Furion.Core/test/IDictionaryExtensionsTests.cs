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

namespace Furion.Core.Tests;

public class IDictionaryExtensionsTests
{
    [Fact]
    public void AddOrUpdate_IfValueIsNull_WillThrow()
    {
        var dictionary = new Dictionary<string, List<string?>>();

        Assert.Throws<ArgumentNullException>(() =>
        {
            dictionary.AddOrUpdate("null", null);
        });
    }

    [Fact]
    public void AddOrUpdate_IfNotExists_WillAdd()
    {
        var key = "first";
        var value = 1;
        var dictionary = new Dictionary<string, List<int>>();
        dictionary.AddOrUpdate(key, value);

        Assert.Single(dictionary);
        Assert.Equal(key, dictionary.Keys.ElementAt(0));
        Assert.Equal(value, dictionary[key].ElementAt(0));
    }

    [Fact]
    public void AddOrUpdate_IfExists_WillUpdate()
    {
        var key = "first";
        var value = 2;
        var dictionary = new Dictionary<string, List<int>>()
        {
            {"first",new List<int>{ 1 } }
        };
        dictionary.AddOrUpdate(key, value);

        Assert.Single(dictionary);
        Assert.Equal(key, dictionary.Keys.ElementAt(0));
        Assert.Equal(value, dictionary[key].ElementAt(1));
    }

    [Fact]
    public void AddOrUpdate_ConcatNull_Throw()
    {
        var dictionary = new Dictionary<string, List<string?>>();
        Dictionary<string, List<string?>>? concatDictionary = null;

        Assert.Throws<ArgumentNullException>(() =>
        {
            dictionary.AddOrUpdate(concatDictionary!);
        });
    }

    [Fact]
    public void AddOrUpdate_Concat_NewDictionary_WillAdd()
    {
        var dictionary = new Dictionary<string, List<int>>();
        var concatDictionary = new Dictionary<string, List<int>>()
        {
           {"first",new List<int>{ 1 } },
           {"two",new List<int>{ 2 } },
        };
        dictionary.AddOrUpdate(concatDictionary);

        Assert.Equal(2, dictionary.Count);
        Assert.Equal("first", dictionary.Keys.ElementAt(0));
        Assert.Equal("two", dictionary.Keys.ElementAt(1));
        Assert.Equal(1, dictionary["first"].ElementAt(0));
        Assert.Equal(2, dictionary["two"].ElementAt(0));
    }

    [Fact]
    public void AddOrUpdate_Concat_ExistsDictionary_WillUpdate()
    {
        var dictionary = new Dictionary<string, List<int>>()
        {
            {"first",new List<int>{ 1 } }
        };
        var concatDictionary = new Dictionary<string, List<int>>()
        {
           {"first",new List<int>{ 2 } }
        };
        dictionary.AddOrUpdate(concatDictionary);

        Assert.Single(dictionary);
        Assert.Equal("first", dictionary.Keys.ElementAt(0));
        Assert.Equal(2, dictionary["first"].Count);
        Assert.Equal(1, dictionary["first"].ElementAt(0));
        Assert.Equal(2, dictionary["first"].ElementAt(1));
    }

    [Fact]
    public void AddOrUpdate_Concat_ExistsOrNotDictionary_WillAddOrUpdate()
    {
        var dictionary = new Dictionary<string, List<int>>()
        {
            {"first",new List<int>{ 1 } }
        };
        var concatDictionary = new Dictionary<string, List<int>>()
        {
           {"first",new List<int>{ 2 } },
           {"two",new List<int>{ 1 } },
        };
        dictionary.AddOrUpdate(concatDictionary);

        Assert.Equal(2, dictionary.Count);
        Assert.Equal("first", dictionary.Keys.ElementAt(0));
        Assert.Equal("two", dictionary.Keys.ElementAt(1));
        Assert.Equal(2, dictionary["first"].Count);
        Assert.Equal(1, dictionary["first"].ElementAt(0));
        Assert.Equal(2, dictionary["first"].ElementAt(1));
        Assert.Single(dictionary["two"]);
        Assert.Equal(1, dictionary["two"].ElementAt(0));
    }
}