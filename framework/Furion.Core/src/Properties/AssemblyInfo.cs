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

// 配置框架友元程序集
[assembly: InternalsVisibleTo("Furion.Authentication")]
[assembly: InternalsVisibleTo("Furion.Authentication.JwtBearer")]
[assembly: InternalsVisibleTo("Furion.Caching")]
[assembly: InternalsVisibleTo("Furion.Component")]
[assembly: InternalsVisibleTo("Furion.Component.AspNetCore")]
[assembly: InternalsVisibleTo("Furion.Component.Hosting")]
[assembly: InternalsVisibleTo("Furion.Configuration")]
[assembly: InternalsVisibleTo("Furion.Configuration.FileScanning")]
[assembly: InternalsVisibleTo("Furion.Configuration.ManifestResource")]
[assembly: InternalsVisibleTo("Furion.Configuration.Remoted")]
[assembly: InternalsVisibleTo("Furion.Crontab")]
[assembly: InternalsVisibleTo("Furion.DependencyInjection")]
[assembly: InternalsVisibleTo("Furion.DependencyInjection.AspNetCore")]
[assembly: InternalsVisibleTo("Furion.DependencyInjection.Hosting")]
[assembly: InternalsVisibleTo("Furion.DependencyInjection.Named")]
[assembly: InternalsVisibleTo("Furion.DependencyInjection.TypeScanning")]
[assembly: InternalsVisibleTo("Furion.Encryption")]
[assembly: InternalsVisibleTo("Furion.EventBus")]
[assembly: InternalsVisibleTo("Furion.Exception")]
[assembly: InternalsVisibleTo("Furion.Exception.AspNetCore")]
[assembly: InternalsVisibleTo("Furion.FileSystem")]
[assembly: InternalsVisibleTo("Furion.Localization")]
[assembly: InternalsVisibleTo("Furion.Localization.AspNetCore")]
[assembly: InternalsVisibleTo("Furion.Logging")]
[assembly: InternalsVisibleTo("Furion.Logging.AspNetCore")]
[assembly: InternalsVisibleTo("Furion.Mail")]
[assembly: InternalsVisibleTo("Furion.Mail.Smtp")]
[assembly: InternalsVisibleTo("Furion.Mail.Imap")]
[assembly: InternalsVisibleTo("Furion.Mail.Pop3")]
[assembly: InternalsVisibleTo("Furion.ObjectMapper")]
[assembly: InternalsVisibleTo("Furion.Options")]
[assembly: InternalsVisibleTo("Furion.Reflection")]
[assembly: InternalsVisibleTo("Furion.Schedule")]
[assembly: InternalsVisibleTo("Furion.Serialization")]
[assembly: InternalsVisibleTo("Furion.Serialization.Json")]
[assembly: InternalsVisibleTo("Furion.Serialization.Xml")]
[assembly: InternalsVisibleTo("Furion.Sockets")]
[assembly: InternalsVisibleTo("Furion.WebSockets")]
[assembly: InternalsVisibleTo("Furion.UnitOfWork")]
[assembly: InternalsVisibleTo("Furion.UnitOfWork.EntityFrameworkCore")]
[assembly: InternalsVisibleTo("Furion.Validation")]
[assembly: InternalsVisibleTo("Furion.Validation.AspNetCore")]
[assembly: InternalsVisibleTo("Furion.Workflow")]

// 配置测试友元程序集
[assembly: InternalsVisibleTo("Furion.Core.Tests")]
[assembly: InternalsVisibleTo("Furion.Component.Tests")]