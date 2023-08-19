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

namespace Furion.Configuration.Tests;

public class IConfigurationExtensionsTests
{
    [Fact]
    public void Exists_Invalid_Parameters()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        Assert.Throws<ArgumentNullException>(() =>
        {
            configuration.Exists(null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Exists(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Exists("");
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Exists("   ");
        });
    }

    [Theory]
    [InlineData("Furion", true)]
    [InlineData("Furion:Name", true)]
    [InlineData("Furion:Version", true)]
    [InlineData("Furion:Homepage", true)]
    [InlineData("Furion:License", false)]
    public void Exists_Check(string key, bool exists)
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"Furion:Name", "Furion" },
            {"Furion:Version", "5.0.0" },
            {"Furion:Homepage", "https://furion.net" }
        });
        var configuration = configurationBuilder.Build();

        Assert.Equal(exists, configuration.Exists(key));
    }

    [Fact]
    public void GetOfT_Invalid_Parameters()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        Assert.Throws<ArgumentNullException>(() =>
        {
            configuration.Get<string>((string)null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get<string>(string.Empty);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get<string>("");
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get<string>("   ");
        });
    }

    [Fact]
    public void GetOfT_ReturnData()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var data = new Dictionary<string, string?>
        {
            {"Furion:Name", "Furion" },
            {"Furion:Version", "5.0.0" },
            {"Furion:Homepage", "https://furion.net" },
            {"Furion:Age", "3" },
            {"Furion:Price", "0.00" },
            {"Furion:IsMIT", "true" },

            {"Furion:Tags:0", "Furion" },
            {"Furion:Tags:1", "MonkSoul" },

            {"Furion:Properties:Gitee", "https://gitee.com/dotnetchina/Furion" },
            {"Furion:Properties:Github", "https://github.com/MonkSoul/Furion" },

            {"Furion:Author:Name", "百小僧" },
            {"Furion:Author:Age", "31" },

            {"Furion:NotPublic", "NotPublic" }
        };
        configurationBuilder.AddInMemoryCollection(data);
        var configuration = configurationBuilder.Build();

        var model = configuration.Get<FurionModel>("Furion");

        Assert.NotNull(model);
        Assert.Equal(configuration.Get<string>(data.Keys.ElementAt(0)), model.Name);
        Assert.Equal(configuration.Get<Version>(data.Keys.ElementAt(1)), model.Version);
        Assert.Equal(configuration.Get<Uri>(data.Keys.ElementAt(2)), model.Homepage);
        Assert.Equal(configuration.Get<int>(data.Keys.ElementAt(3)), model.Age);
        Assert.Equal(configuration.Get<decimal>(data.Keys.ElementAt(4)), model.Price);
        Assert.Equal(configuration.Get<bool>(data.Keys.ElementAt(5)), model.IsMIT);

        var tags = configuration.Get<string[]>("Furion:Tags");
        Assert.NotNull(tags);
        Assert.NotNull(model.Tags);
        Assert.Equal(tags, model.Tags);
        Assert.Equal("Furion", model.Tags[0]);
        Assert.Equal("MonkSoul", model.Tags[1]);

        var properties = configuration.Get<Dictionary<string, object?>>("Furion:Properties");
        Assert.NotNull(properties);
        Assert.Equal(properties, model.Properties);
        Assert.NotNull(model.Properties);
        Assert.Equal(properties, model.Properties);
        Assert.Equal("Gitee", model.Properties.Keys.ElementAt(0));
        Assert.Equal("Github", model.Properties.Keys.ElementAt(1));
        Assert.Equal("https://gitee.com/dotnetchina/Furion", model.Properties.Values.ElementAt(0));
        Assert.Equal("https://github.com/MonkSoul/Furion", model.Properties.Values.ElementAt(1));

        var author = configuration.Get<FurionAuthor>("Furion:Author");
        Assert.NotNull(author);
        Assert.Equal("百小僧", author.Name);
        Assert.Equal(31, author.Age);

        Assert.Null(model.NotPublic);

        Assert.Null(configuration.Get<string>("Furion:Unknown"));
    }

    [Fact]
    public void GetOfT_With_Action_Invalid_Parameters()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        Assert.Throws<ArgumentNullException>(() =>
        {
            configuration.Get<string>(null!, null);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get<string>(string.Empty, null);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get<string>("", null);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get<string>("   ", null);
        });
    }

    [Fact]
    public void GetOfT_With_Action_ReturnData()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var data = new Dictionary<string, string?>
        {
            {"Furion:Name", "Furion" },
            {"Furion:Version", "5.0.0" },
            {"Furion:Homepage", "https://furion.net" },
            {"Furion:Age", "3" },
            {"Furion:Price", "0.00" },
            {"Furion:IsMIT", "true" },

            {"Furion:Tags:0", "Furion" },
            {"Furion:Tags:1", "MonkSoul" },

            {"Furion:Properties:Gitee", "https://gitee.com/dotnetchina/Furion" },
            {"Furion:Properties:Github", "https://github.com/MonkSoul/Furion" },

            {"Furion:Author:Name", "百小僧" },
            {"Furion:Author:Age", "31" },

            {"Furion:NotPublic", "NotPublic" }
        };
        configurationBuilder.AddInMemoryCollection(data);
        var configuration = configurationBuilder.Build();

        var model = configuration.Get<FurionModel>("Furion", null);

        Assert.NotNull(model);
        Assert.Equal(configuration.Get<string>(data.Keys.ElementAt(0), null), model.Name);
        Assert.Equal(configuration.Get<Version>(data.Keys.ElementAt(1), null), model.Version);
        Assert.Equal(configuration.Get<Uri>(data.Keys.ElementAt(2), null), model.Homepage);
        Assert.Equal(configuration.Get<int>(data.Keys.ElementAt(3), null), model.Age);
        Assert.Equal(configuration.Get<decimal>(data.Keys.ElementAt(4), null), model.Price);
        Assert.Equal(configuration.Get<bool>(data.Keys.ElementAt(5), null), model.IsMIT);

        var tags = configuration.Get<string[]>("Furion:Tags", null);
        Assert.NotNull(tags);
        Assert.NotNull(model.Tags);
        Assert.Equal(tags, model.Tags);
        Assert.Equal("Furion", model.Tags[0]);
        Assert.Equal("MonkSoul", model.Tags[1]);

        var properties = configuration.Get<Dictionary<string, object?>>("Furion:Properties", null);
        Assert.NotNull(properties);
        Assert.Equal(properties, model.Properties);
        Assert.NotNull(model.Properties);
        Assert.Equal(properties, model.Properties);
        Assert.Equal("Gitee", model.Properties.Keys.ElementAt(0));
        Assert.Equal("Github", model.Properties.Keys.ElementAt(1));
        Assert.Equal("https://gitee.com/dotnetchina/Furion", model.Properties.Values.ElementAt(0));
        Assert.Equal("https://github.com/MonkSoul/Furion", model.Properties.Values.ElementAt(1));

        var author = configuration.Get<FurionAuthor>("Furion:Author", null);
        Assert.NotNull(author);
        Assert.Equal("百小僧", author.Name);
        Assert.Equal(31, author.Age);

        Assert.Null(model.NotPublic);

        Assert.Null(configuration.Get<string>("Furion:Unknown", null));
    }

    [Fact]
    public void GetOfT_With_Action_BinderOptions_BindNonPublicProperties()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var data = new Dictionary<string, string?>
        {
            {"Furion:NotPublic", "NotPublic" }
        };
        configurationBuilder.AddInMemoryCollection(data);
        var configuration = configurationBuilder.Build();

        var model = configuration.Get<FurionModel>("Furion", options =>
        {
            options.BindNonPublicProperties = true;
        });
        Assert.NotNull(model);

        Assert.NotNull(model.NotPublic);
        Assert.Equal("NotPublic", model.NotPublic);
    }

    [Fact]
    public void GetOfT_With_Action_BinderOptions_ErrorOnUnknownConfiguration_Set()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var data = new Dictionary<string, string?>
        {
            {"Furion:Unknown", "Unknown" }
        };
        configurationBuilder.AddInMemoryCollection(data);
        var configuration = configurationBuilder.Build();

        Assert.Throws<InvalidOperationException>(() =>
        {
            var model = configuration.Get<FurionModel>("Furion", options =>
            {
                options.ErrorOnUnknownConfiguration = true;
            });
        });
    }

    [Fact]
    public void GetOfType_Invalid_Parameters()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        Assert.Throws<ArgumentNullException>(() =>
        {
            configuration.Get((string)null!, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get(string.Empty, null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get("", null!);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get("   ", null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            configuration.Get("Furion", null!);
        });
    }

    [Fact]
    public void GetOfType_ReturnData()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var data = new Dictionary<string, string?>
        {
            {"Furion:Name", "Furion" },
            {"Furion:Version", "5.0.0" },
            {"Furion:Homepage", "https://furion.net" },
            {"Furion:Age", "3" },
            {"Furion:Price", "0.00" },
            {"Furion:IsMIT", "true" },

            {"Furion:Tags:0", "Furion" },
            {"Furion:Tags:1", "MonkSoul" },

            {"Furion:Properties:Gitee", "https://gitee.com/dotnetchina/Furion" },
            {"Furion:Properties:Github", "https://github.com/MonkSoul/Furion" },

            {"Furion:Author:Name", "百小僧" },
            {"Furion:Author:Age", "31" },

            {"Furion:NotPublic", "NotPublic" }
        };
        configurationBuilder.AddInMemoryCollection(data);
        var configuration = configurationBuilder.Build();

        var model = configuration.Get("Furion", typeof(FurionModel)) as FurionModel;

        Assert.NotNull(model);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(0), typeof(string)), model.Name);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(1), typeof(Version)), model.Version);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(2), typeof(Uri)), model.Homepage);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(3), typeof(int)), model.Age);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(4), typeof(decimal)), model.Price);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(5), typeof(bool)), model.IsMIT);

        var tags = configuration.Get("Furion:Tags", typeof(string[])) as string[];
        Assert.NotNull(tags);
        Assert.NotNull(model.Tags);
        Assert.Equal(tags, model.Tags);
        Assert.Equal("Furion", model.Tags[0]);
        Assert.Equal("MonkSoul", model.Tags[1]);

        var properties = configuration.Get("Furion:Properties", typeof(Dictionary<string, object?>)) as Dictionary<string, object?>;
        Assert.NotNull(properties);
        Assert.Equal(properties, model.Properties);
        Assert.NotNull(model.Properties);
        Assert.Equal(properties, model.Properties);
        Assert.Equal("Gitee", model.Properties.Keys.ElementAt(0));
        Assert.Equal("Github", model.Properties.Keys.ElementAt(1));
        Assert.Equal("https://gitee.com/dotnetchina/Furion", model.Properties.Values.ElementAt(0));
        Assert.Equal("https://github.com/MonkSoul/Furion", model.Properties.Values.ElementAt(1));

        var author = configuration.Get("Furion:Author", typeof(FurionAuthor)) as FurionAuthor;
        Assert.NotNull(author);
        Assert.Equal("百小僧", author.Name);
        Assert.Equal(31, author.Age);

        Assert.Null(model.NotPublic);

        Assert.Null(configuration.Get("Furion:Unknown", typeof(string)));
    }

    [Fact]
    public void GetOfType_With_Action_Invalid_Parameters()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        Assert.Throws<ArgumentNullException>(() =>
        {
            configuration.Get(null!, null!, null);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get(string.Empty, null!, null);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get("", null!, null);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            configuration.Get("   ", null!, null);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            configuration.Get("Furion", null!, null);
        });
    }

    [Fact]
    public void GetOfType_With_Action_ReturnData()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var data = new Dictionary<string, string?>
        {
            {"Furion:Name", "Furion" },
            {"Furion:Version", "5.0.0" },
            {"Furion:Homepage", "https://furion.net" },
            {"Furion:Age", "3" },
            {"Furion:Price", "0.00" },
            {"Furion:IsMIT", "true" },

            {"Furion:Tags:0", "Furion" },
            {"Furion:Tags:1", "MonkSoul" },

            {"Furion:Properties:Gitee", "https://gitee.com/dotnetchina/Furion" },
            {"Furion:Properties:Github", "https://github.com/MonkSoul/Furion" },

            {"Furion:Author:Name", "百小僧" },
            {"Furion:Author:Age", "31" },

            {"Furion:NotPublic", "NotPublic" }
        };
        configurationBuilder.AddInMemoryCollection(data);
        var configuration = configurationBuilder.Build();

        var model = configuration.Get("Furion", typeof(FurionModel), null) as FurionModel;

        Assert.NotNull(model);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(0), typeof(string), null), model.Name);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(1), typeof(Version), null), model.Version);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(2), typeof(Uri), null), model.Homepage);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(3), typeof(int), null), model.Age);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(4), typeof(decimal), null), model.Price);
        Assert.Equal(configuration.Get(data.Keys.ElementAt(5), typeof(bool), null), model.IsMIT);

        var tags = configuration.Get("Furion:Tags", typeof(string[]), null) as string[];
        Assert.NotNull(tags);
        Assert.NotNull(model.Tags);
        Assert.Equal(tags, model.Tags);
        Assert.Equal("Furion", model.Tags[0]);
        Assert.Equal("MonkSoul", model.Tags[1]);

        var properties = configuration.Get("Furion:Properties", typeof(Dictionary<string, object?>), null) as Dictionary<string, object?>;
        Assert.NotNull(properties);
        Assert.Equal(properties, model.Properties);
        Assert.NotNull(model.Properties);
        Assert.Equal(properties, model.Properties);
        Assert.Equal("Gitee", model.Properties.Keys.ElementAt(0));
        Assert.Equal("Github", model.Properties.Keys.ElementAt(1));
        Assert.Equal("https://gitee.com/dotnetchina/Furion", model.Properties.Values.ElementAt(0));
        Assert.Equal("https://github.com/MonkSoul/Furion", model.Properties.Values.ElementAt(1));

        var author = configuration.Get("Furion:Author", typeof(FurionAuthor), null) as FurionAuthor;
        Assert.NotNull(author);
        Assert.Equal("百小僧", author.Name);
        Assert.Equal(31, author.Age);

        Assert.Null(model.NotPublic);

        Assert.Null(configuration.Get("Furion:Unknown", typeof(string), null));
    }

    [Fact]
    public void GetOfType_With_Action_BinderOptions_BindNonPublicProperties()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var data = new Dictionary<string, string?>
        {
            {"Furion:NotPublic", "NotPublic" }
        };
        configurationBuilder.AddInMemoryCollection(data);
        var configuration = configurationBuilder.Build();

        var model = configuration.Get("Furion", typeof(FurionModel), options =>
        {
            options.BindNonPublicProperties = true;
        }) as FurionModel;
        Assert.NotNull(model);

        Assert.NotNull(model.NotPublic);
        Assert.Equal("NotPublic", model.NotPublic);
    }

    [Fact]
    public void GetOfType_With_Action_BinderOptions_ErrorOnUnknownConfiguration_Set()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var data = new Dictionary<string, string?>
        {
            {"Furion:Unknown", "Unknown" }
        };
        configurationBuilder.AddInMemoryCollection(data);
        var configuration = configurationBuilder.Build();

        Assert.Throws<InvalidOperationException>(() =>
        {
            var model = configuration.Get("Furion", typeof(FurionModel), options =>
            {
                options.ErrorOnUnknownConfiguration = true;
            });
        });
    }

    [Fact]
    public void Reload_ReturnOK()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.Add(new ReloadConfigurationSource());
        var configuration = configurationBuilder.Build() as IConfiguration;

        Assert.Equal("1", configuration["Count"]);

        configuration.Reload();

        Assert.Equal("2", configuration["Count"]);
    }

    [Fact]
    public void ConvertToJson_ReturnOK()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var data = new Dictionary<string, string?>
        {
            {"Furion:Name", "Furion" },
            {"Furion:Version", "5.0.0" },
            {"Furion:Homepage", "https://furion.net" },
            {"Furion:Age", "3" },
            {"Furion:Price", "0.00" },
            {"Furion:IsMIT", "true" },

            {"Furion:Tags:0", "Furion" },
            {"Furion:Tags:1", "MonkSoul" },

            {"Furion:Properties:Gitee", "https://gitee.com/dotnetchina/Furion" },
            {"Furion:Properties:Github", "https://github.com/MonkSoul/Furion" },

            {"Furion:Author:Name", "百小僧" },
            {"Furion:Author:Age", "31" },

            {"Furion:NotPublic", "NotPublic" }
        };
        configurationBuilder.AddInMemoryCollection(data);
        var configuration = configurationBuilder.Build();

        var jsonString = configuration.ConvertToJson();
        Assert.Equal("{\"Furion\":{\"Age\":\"3\",\"Author\":{\"Age\":\"31\",\"Name\":\"\\u767E\\u5C0F\\u50E7\"},\"Homepage\":\"https://furion.net\",\"IsMIT\":\"true\",\"Name\":\"Furion\",\"NotPublic\":\"NotPublic\",\"Price\":\"0.00\",\"Properties\":{\"Gitee\":\"https://gitee.com/dotnetchina/Furion\",\"Github\":\"https://github.com/MonkSoul/Furion\"},\"Tags\":{\"0\":\"Furion\",\"1\":\"MonkSoul\"},\"Version\":\"5.0.0\"}}", jsonString);

        var jsonString2 = configuration.ConvertToJson(new JsonWriterOptions
        {
            Indented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        Assert.Equal("{\r\n  \"Furion\": {\r\n    \"Age\": \"3\",\r\n    \"Author\": {\r\n      \"Age\": \"31\",\r\n      \"Name\": \"百小僧\"\r\n    },\r\n    \"Homepage\": \"https://furion.net\",\r\n    \"IsMIT\": \"true\",\r\n    \"Name\": \"Furion\",\r\n    \"NotPublic\": \"NotPublic\",\r\n    \"Price\": \"0.00\",\r\n    \"Properties\": {\r\n      \"Gitee\": \"https://gitee.com/dotnetchina/Furion\",\r\n      \"Github\": \"https://github.com/MonkSoul/Furion\"\r\n    },\r\n    \"Tags\": {\r\n      \"0\": \"Furion\",\r\n      \"1\": \"MonkSoul\"\r\n    },\r\n    \"Version\": \"5.0.0\"\r\n  }\r\n}", jsonString2);
    }

    [Fact]
    public void BuildJson_Invalid_Parameters()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            IConfigurationExtensions.BuildJson(null!, null!);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            IConfigurationExtensions.BuildJson(new ConfigurationManager(), null!);
        });
    }

    [Fact]
    public void BuildJson_ReturnOK()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var data = new Dictionary<string, string?>
        {
            {"Furion:Name", "Furion" },
            {"Furion:Version", "5.0.0" },
            {"Furion:Homepage", "https://furion.net" },
            {"Furion:Age", "3" },
            {"Furion:Price", "0.00" },
            {"Furion:IsMIT", "true" },

            {"Furion:Tags:0", "Furion" },
            {"Furion:Tags:1", "MonkSoul" },

            {"Furion:Properties:Gitee", "https://gitee.com/dotnetchina/Furion" },
            {"Furion:Properties:Github", "https://github.com/MonkSoul/Furion" },

            {"Furion:Author:Name", "百小僧" },
            {"Furion:Author:Age", "31" },

            {"Furion:NotPublic", "NotPublic" }
        };
        configurationBuilder.AddInMemoryCollection(data);
        var configuration = configurationBuilder.Build();

        using var stream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(stream))
        {
            IConfigurationExtensions.BuildJson(configuration, jsonWriter);
        }

        var jsonString = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal("{\"Furion\":{\"Age\":\"3\",\"Author\":{\"Age\":\"31\",\"Name\":\"\\u767E\\u5C0F\\u50E7\"},\"Homepage\":\"https://furion.net\",\"IsMIT\":\"true\",\"Name\":\"Furion\",\"NotPublic\":\"NotPublic\",\"Price\":\"0.00\",\"Properties\":{\"Gitee\":\"https://gitee.com/dotnetchina/Furion\",\"Github\":\"https://github.com/MonkSoul/Furion\"},\"Tags\":{\"0\":\"Furion\",\"1\":\"MonkSoul\"},\"Version\":\"5.0.0\"}}", jsonString);
    }

    [Fact]
    public void GetMetadata_ReturnOK()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"Furion:Name", "Furion" },
            {"Furion:Version", "5.0.0" },
            {"Furion:Homepage", "https://furion.net" }
        });
        var configuration = configurationBuilder.Build();

        var metadata = configuration.GetMetadata();
        Assert.NotNull(metadata);
        Assert.Single(metadata);
    }
}