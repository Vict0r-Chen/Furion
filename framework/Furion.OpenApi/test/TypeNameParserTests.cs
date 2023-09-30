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

public class TypeNameParserTests
{
    [Fact]
    public void Parse_Null_ReturnOK()
    {
        Assert.Equal(TypeName.Any, TypeNameParser.Parse(null));
    }

    [Theory]
    [InlineData(typeof(bool), TypeName.Boolean)]
    [InlineData(typeof(byte), TypeName.Number)]
    [InlineData(typeof(char), TypeName.String)]
    [InlineData(typeof(decimal), TypeName.Number)]
    [InlineData(typeof(double), TypeName.Number)]
    [InlineData(typeof(float), TypeName.Number)]
    [InlineData(typeof(int), TypeName.Number)]
    [InlineData(typeof(long), TypeName.Number)]
    [InlineData(typeof(sbyte), TypeName.Number)]
    [InlineData(typeof(short), TypeName.Number)]
    [InlineData(typeof(uint), TypeName.Number)]
    [InlineData(typeof(ulong), TypeName.Number)]
    [InlineData(typeof(ushort), TypeName.Number)]
    [InlineData(typeof(string), TypeName.String)]
    [InlineData(typeof(object), TypeName.Any)]
    [InlineData(typeof(DateTime), TypeName.Date)]
    [InlineData(typeof(DateTimeOffset), TypeName.Date)]
    [InlineData(typeof(DateOnly), TypeName.Date)]
    [InlineData(typeof(TimeOnly), TypeName.Time)]
    [InlineData(typeof(TypeName), TypeName.Enum)]
    [InlineData(typeof(IFormFile), TypeName.Binary)]
    [InlineData(typeof(IFormFileCollection), TypeName.BinaryCollection)]
    [InlineData(typeof(IFormFile[]), TypeName.BinaryCollection)]
    [InlineData(typeof(List<IFormFile>), TypeName.BinaryCollection)]
    [InlineData(typeof(IList<IFormFile>), TypeName.BinaryCollection)]
    [InlineData(typeof(IEnumerable<IFormFile>), TypeName.BinaryCollection)]
    [InlineData(typeof(ICollection<IFormFile>), TypeName.BinaryCollection)]
    [InlineData(typeof(Dictionary<string, string>), TypeName.Record)]
    [InlineData(typeof(IEnumerable<KeyValuePair<string, string>>), TypeName.Record)]
    [InlineData(typeof(IList<KeyValuePair<string, string>>), TypeName.Record)]
    [InlineData(typeof(List<KeyValuePair<string, string>>), TypeName.Record)]
    [InlineData(typeof(ICollection<KeyValuePair<string, string>>), TypeName.Record)]
    [InlineData(typeof(KeyValuePair<string, string>[]), TypeName.Record)]
    [InlineData(typeof(ValueTuple<string, string>), TypeName.Tuple)]
    [InlineData(typeof(Tuple<string, string>), TypeName.Tuple)]
    [InlineData(typeof(Type), TypeName.Object)]
    [InlineData(typeof(DataTable), TypeName.Object)]
    [InlineData(typeof(DataSet), TypeName.Object)]
    [InlineData(typeof(string[]), TypeName.Array)]
    [InlineData(typeof(int[]), TypeName.Array)]
    [InlineData(typeof(byte[]), TypeName.Array)]
    [InlineData(typeof(StructObject), TypeName.Struct)]
    [InlineData(typeof(RecordObject), TypeName.Object)]
    public void Parse_ReturnOK(Type? type, TypeName dataType)
    {
        Assert.Equal(dataType, TypeNameParser.Parse(type));
    }

    [Fact]
    public void IsFormFileCollection_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var result = TypeNameParser.IsFormFileCollection(null!);
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
        Assert.Equal(result, TypeNameParser.IsFormFileCollection(type));
    }
}