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

namespace Furion.Component.Tests;

public class ObjectMapperTests
{
    [Fact]
    public void Map_Null_Throw()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ObjectMapper.Map((OptionsModel1)null!, (OptionsModel2)null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            ObjectMapper.Map(new OptionsModel1(), (OptionsModel2)null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            ObjectMapper.Map((OptionsModel1)null!, new OptionsModel2());
        });
    }

    [Fact]
    public void Map()
    {
        var optionsModel1 = new OptionsModel1
        {
            Id = 1,
            Name = "Furion",
            Age = 31
        };

        var optionsModel2 = new OptionsModel2();
        ObjectMapper.Map(optionsModel1, optionsModel2);

        Assert.Equal(optionsModel1.Id, optionsModel2.Id);
        Assert.Equal(optionsModel1.Name, optionsModel2.Name);
        Assert.Equal(optionsModel1.Age, optionsModel2.Age);

        var optionsModel3 = new OptionsModel3();
        ObjectMapper.Map(optionsModel1, optionsModel3);

        Assert.Equal(optionsModel1.Id, optionsModel3.Id);
        Assert.Equal(optionsModel1.Name, optionsModel3.Name);

        var optionsModel4 = new OptionsModel4();
        ObjectMapper.Map(optionsModel1, optionsModel4);

        Assert.Equal(optionsModel1.Id, optionsModel4.Id);
        Assert.Equal(optionsModel1.Name, optionsModel4.Name);
        Assert.Equal(optionsModel1.Age, optionsModel4.Age);
        Assert.Null(optionsModel4.Address);

        var optionsModel5 = new OptionsModel5();
        ObjectMapper.Map(optionsModel1, optionsModel5);

        Assert.Equal(optionsModel1.Id, optionsModel5.Id);
        Assert.NotEqual(optionsModel1.Name, optionsModel5.Name);
        Assert.Null(optionsModel5.Name);
        Assert.Equal(optionsModel1.Age, optionsModel5.Age);
    }
}