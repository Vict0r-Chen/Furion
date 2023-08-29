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

namespace Furion.OpenApi.Tests;

public class DataTypeParserTests
{
    [Fact]
    public void Parse_Null_ReturnOK()
    {
        Assert.Equal(DataTypes.Any, DataTypeParser.Parse(null));
    }

    [Theory]
    [InlineData(typeof(bool), DataTypes.Boolean)]
    [InlineData(typeof(byte), DataTypes.Number)]
    [InlineData(typeof(char), DataTypes.String)]
    [InlineData(typeof(decimal), DataTypes.Number)]
    [InlineData(typeof(double), DataTypes.Number)]
    [InlineData(typeof(float), DataTypes.Number)]
    [InlineData(typeof(int), DataTypes.Number)]
    [InlineData(typeof(long), DataTypes.Number)]
    [InlineData(typeof(sbyte), DataTypes.Number)]
    [InlineData(typeof(short), DataTypes.Number)]
    [InlineData(typeof(uint), DataTypes.Number)]
    [InlineData(typeof(ulong), DataTypes.Number)]
    [InlineData(typeof(ushort), DataTypes.Number)]
    [InlineData(typeof(string), DataTypes.String)]
    [InlineData(typeof(object), DataTypes.Any)]
    [InlineData(typeof(DateTime), DataTypes.Date)]
    [InlineData(typeof(DateTimeOffset), DataTypes.Date)]
    [InlineData(typeof(DateOnly), DataTypes.Date)]
    [InlineData(typeof(TimeOnly), DataTypes.Time)]
    [InlineData(typeof(DataTypes), DataTypes.Enum)]
    [InlineData(typeof(IFormFile), DataTypes.Binary)]
    [InlineData(typeof(IFormFileCollection), DataTypes.BinaryCollection)]
    [InlineData(typeof(IFormFile[]), DataTypes.BinaryCollection)]
    [InlineData(typeof(List<IFormFile>), DataTypes.BinaryCollection)]
    [InlineData(typeof(IList<IFormFile>), DataTypes.BinaryCollection)]
    [InlineData(typeof(IEnumerable<IFormFile>), DataTypes.BinaryCollection)]
    [InlineData(typeof(ICollection<IFormFile>), DataTypes.BinaryCollection)]
    [InlineData(typeof(Dictionary<string, string>), DataTypes.Record)]
    [InlineData(typeof(IEnumerable<KeyValuePair<string, string>>), DataTypes.Record)]
    [InlineData(typeof(IList<KeyValuePair<string, string>>), DataTypes.Record)]
    [InlineData(typeof(List<KeyValuePair<string, string>>), DataTypes.Record)]
    [InlineData(typeof(ICollection<KeyValuePair<string, string>>), DataTypes.Record)]
    [InlineData(typeof(KeyValuePair<string, string>[]), DataTypes.Record)]
    [InlineData(typeof(ValueTuple<string, string>), DataTypes.Tuple)]
    [InlineData(typeof(Tuple<string, string>), DataTypes.Tuple)]
    [InlineData(typeof(Type), DataTypes.Object)]
    [InlineData(typeof(DataTable), DataTypes.Object)]
    [InlineData(typeof(DataSet), DataTypes.Object)]
    [InlineData(typeof(string[]), DataTypes.Array)]
    [InlineData(typeof(int[]), DataTypes.Array)]
    [InlineData(typeof(byte[]), DataTypes.Array)]
    [InlineData(typeof(StructObject), DataTypes.Struct)]
    [InlineData(typeof(RecordObject), DataTypes.Object)]
    public void Parse_ReturnOK(Type? type, DataTypes dataType)
    {
        Assert.Equal(dataType, DataTypeParser.Parse(type));
    }

    [Fact]
    public void IsFormFileCollection_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var result = DataTypeParser.IsFormFileCollection(null!);
        });
    }

    [Theory]
    [InlineData(typeof(IFormFile), false)]
    [InlineData(typeof(IFormFileCollection), true)]
    [InlineData(typeof(IFormFile[]), true)]
    [InlineData(typeof(List<IFormFile>), true)]
    [InlineData(typeof(IList<IFormFile>), true)]
    [InlineData(typeof(IEnumerable<IFormFile>), true)]
    [InlineData(typeof(ICollection<IFormFile>), true)]
    public void IsFormFileCollection_ReturnOK(Type type, bool result)
    {
        Assert.Equal(result, DataTypeParser.IsFormFileCollection(type));
    }
}