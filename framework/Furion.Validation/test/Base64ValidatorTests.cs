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

namespace Furion.Validation.Tests;

public class Base64ValidatorTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("data:image/gif;base64,xxxx==", true)]
    [InlineData("data:text/plain;base64,SGVsbG8gd29ybGQh", true)]
    [InlineData("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAyElEQVR42mL8DwEPqSq5OTk4+cwEggBpBDCMvYBSpAU4B41iQB1kF6oIhNC+UUrWASregVC7JYjAK8SQRjSOI+UQAUYF0pFSyDcDizO4vhBDXTBEoDqSI0FoXBFXWgqdAbVEEoITpAo3IEINAFS4j7UYJiBuBAgqipjX0GosULf4GQAAv3R3kLG6s6wAAAABJRU5ErkJggg==", true)]
    [InlineData("data:text/html;base64,PGh0bWw+PGJvZHk+SGVsbG8gd29ybGQhPC9ib2R5PjwvaHRtbD4=", true)]
    [InlineData("data:application/pdf;base64,JVBERi0xLjMNCiXi48/TDQoNCjEgMCBvYmoNCjw8DQovVHlwZSAvQ2F0YWxvZw0KL091dGxpbmVzIDIgMCBSDQovUGFnZXMgNCAwIFIgDQo+Pg0Kc3RyZWFtDQo8Y2FzZSByYW5nZSBTVERLIC0gRm9udCBuYW1lIG9mIHRoZSBkYXRhIG5hbWUuIC0gVmlydHVhbCBtZXRob2QgZGF0YS4NCnN0cmVhbQ0KPC9jYXNlPj4NCnN0cmVhbQ0KPC9jYXNlPj4NCnN0cmVhbQ0KPC9jYXNlPj4NCi9YTUwvVHJhbnNwb3JTIb3Zlci9UeXBlL0ZvbnQvVG9Vbmljb2RlMT4+DQplbmRvYmoNCg0KMiAwIG9iag0KPDwNCiAgIC9UeXBlIC9Gb250DQogICAxIDAgUg0KICA+Pg0KZW5kb2JqDQoNCjQgMCBvYmoNCjw8DQogIC9MZW5ndGggMTENCiAgICBzDQogIC9GaWx0ZXIgL0ZsYXRlRGVjb2RlDQogID4+DQplbmRvYmoNCg0KNiAwIG9iag0KPDwNCiAgL1R5cGUgL0ZvbnQNCiAgL1N1YnR5cGUgL1Jlc291cmNlRGVjb2RlDQogIC9CYXNlRm9udCAvVG86DQogID4+DQplbmRvYmoNCg0KOSAwIG9iag0KPDwNCiAgL1R5cGUgL0ZvbnQNCiAgL1N1YnR5cGUgL1Jlc291cmNlRGVjb2RlIDw8DQogIC9CYXNlRm9udCA8PA0KL0NvbnRlbnRzIDggMCBSDQogIC9Sb290IDcgMCBSDQogIC9Sb291bmQgIDwgMCANCiAgL0JvbWJXIDQgMCBSIA0KPj4NCmVuZG9iag0KDQp0cmFpbGVyDQo8PA0KICAvRjEgMCBvYmoNCjw8DQogIC9MZW5ndGggMzQNCiAgICBzDQogIC9Gb250IDw8DQogIC9GMSA3IDAgUg0KICAvRjEgMSAwIFINCiAgPj4NCm", true)]
    [InlineData("datax:image/gif;base64,xxxx==", false)]
    [InlineData("V2VsY29tZSB0byBKU09OLg==", false)]
    public void IsValid(object? value, bool result)
    {
        var validator = new Base64Validator();
        Assert.Equal(result, validator.IsValid(value));
    }

    [Fact]
    public void Default_ErrorMessage()
    {
        var validator = new Base64Validator();

        var failure = validator.GetValidationResult("datax:image/gif;base64,xxxx==");
        Assert.NotNull(failure);
        Assert.Equal("The field is not a valid base64 format.", failure.ErrorMessage);

        var failure2 = validator.GetValidationResult("datax:image/gif;base64,xxxx==", new List<string> { "Value" });
        Assert.NotNull(failure2);
        Assert.Equal("The field Value is not a valid base64 format.", failure2.ErrorMessage);
    }

    [Fact]
    public void Custom_ErrorMessage()
    {
        var validator = new Base64Validator
        {
            ErrorMessage = "不是一个有效的 Base64 格式"
        };

        var failure = validator.GetValidationResult("datax:image/gif;base64,xxxx==");
        Assert.NotNull(failure);

        Assert.Equal("不是一个有效的 Base64 格式", failure.ErrorMessage);
    }
}