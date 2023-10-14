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

namespace Furion.Component.Tests;

public class ObjectMapperTests
{
    [Fact]
    public void Map_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ObjectMapper.Map<ObjectMapper1, ObjectMapper2>(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            ObjectMapper.Map<ObjectMapper1, ObjectMapper2>(new(), null!);
        });
    }

    [Fact]
    public void Map_ReturnOK()
    {
        var object1 = new ObjectMapper1
        {
            Id = 1,
            Name = "Furion",
            Age = 3,
            Address = "广东省中山市",
            Tags = ["Furion", "百小僧"]
        };

        var object2 = new ObjectMapper2();
        ObjectMapper.Map(object1, object2);

        Assert.Equal(1, object2.Id);
        Assert.Equal("Furion", object2.Name);
        Assert.Equal(3, object2.Age);
        Assert.Null(object2.Address);
        Assert.Equal(["Furion", "百小僧"], object2.Tags);
        Assert.Null(object2.Custom);
    }
}