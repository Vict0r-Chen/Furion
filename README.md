## 先知

一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。

---

## 开发背景

当前的 **[Furion v4](https://gitee.com/dotnetchina/Furion/tree/v4/)** 版本已经变得成熟且稳定，但仍存在以下问题：

- 早期开发进展迅速，很多代码存在仓促上线的情况，缺乏充分思考和考虑
- 所有模块都包含在一个项目中，无法按需加载和安装
- 过度使用静态类和静态内存存储，不利于进行单元测试和数据隔离
- 对于 .NET Core 的掌握程度有限，导致历史代码臃肿且高度耦合
- 代码架构和设计模式缺乏统一性，可以说是一个杂烩
- 在早期对用户需求掌握不足，导致后期不断打补丁来进行改进，稍有改动就可能引发破坏性的修改
- 模块、类型、属性、方法、属性等命名混乱，很难从字面上理解其功能含义
- 框架示例混乱，用户只能自行摸索最佳实践
- 虽有单元测试，但是非常混乱

## 框架技术

Furion v5 版本采用 C# 12 和 .NET 8 进行开发。

## 开发指导

- 实现完全无第三方依赖（除微软官方提供外）
- 实现彻底模块化，每个模块都是独立的项目
- 每个模块的单元测试覆盖率要达到 92%以上
- 确保每个类型、属性、字段、方法都有详细的注释
- 尽可能避免使用静态内存存储
- 所有模块都采用上下文和构建器模式进行设计
- 所有模块都采用依赖注入/控制反转的设计模式
- 尽可能为每个模块提供看板功能
- 提供所有模块最佳实践示例

## 模块状态

| 模块名称                                                                                                                                  | 状态 | 单元测试 | 版本                                                                                                                                                                | 文档 |
| ----------------------------------------------------------------------------------------------------------------------------------------- | ---- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---- |
| [Furion.Core](https://gitee.com/dotnetchina/Furion/tree/v5-dev/framework/Furion.Core)                                                     | ✅   | ✅       | [![nuget](https://img.shields.io/nuget/v/Furion.Core.svg?cacheSeconds=10800)](https://www.nuget.org/packages/Furion.Core)                                           | ⚠️   |
| [Furion.DependencyInjection](https://gitee.com/dotnetchina/Furion/tree/v5-dev/framework/Furion.DependencyInjection)                       | ✅   | ✅       | [![nuget](https://img.shields.io/nuget/v/Furion.DependencyInjection.svg?cacheSeconds=10800)](https://www.nuget.org/packages/Furion.DependencyInjection)             | ⚠️   |
| [Furion.DependencyInjection.Named](https://gitee.com/dotnetchina/Furion/tree/v5-dev/framework/Furion.DependencyInjection.Named)           | ✅   | ✅       | [![nuget](https://img.shields.io/nuget/v/Furion.DependencyInjection.Named.svg?cacheSeconds=10800)](https://www.nuget.org/packages/Furion.DependencyInjection.Named) | ⚠️   |
| [Furion.Component](https://gitee.com/dotnetchina/Furion/tree/v5-dev/framework/Furion.Component)                                           | ✅   | ✅       | [![nuget](https://img.shields.io/nuget/v/Furion.Component.svg?cacheSeconds=10800)](https://www.nuget.org/packages/Furion.Component)                                 | ⚠️   |
| [Furion.Component.AspNetCore](https://gitee.com/dotnetchina/Furion/tree/v5-dev/framework/Furion.Component.AspNetCore)                     | ✅   | ✅       | [![nuget](https://img.shields.io/nuget/v/Furion.Component.AspNetCore.svg?cacheSeconds=10800)](https://www.nuget.org/packages/Furion.Component.AspNetCore)           | ⚠️   |
| [Furion.Component.Hosting](https://gitee.com/dotnetchina/Furion/tree/v5-dev/framework/Furion.Component.Hosting)                           | ✅   | ✅       | [![nuget](https://img.shields.io/nuget/v/Furion.Component.Hosting.svg?cacheSeconds=10800)](https://www.nuget.org/packages/Furion.Component.Hosting)                 | ⚠️   |
| [Furion.Configuration](https://gitee.com/dotnetchina/Furion/tree/v5-dev/framework/Furion.Configuration)                                   | ✅   | ✅       | [![nuget](https://img.shields.io/nuget/v/Furion.Configuration.svg?cacheSeconds=10800)](https://www.nuget.org/packages/Furion.Configuration)                         | ⚠️   |
| [Furion.Configuration.ManifestResource](https://gitee.com/dotnetchina/Furion/tree/v5-dev/framework/Furion.Configuration.ManifestResource) | ⏳   | ⏳       |                                                                                                                                                                     | ⚠️   |

> 状态说明
>
> | 图标 | 描述     |
> | ---- | -------- |
> | ⚠️   | 待定     |
> | ⏳   | 进行中   |
> | ✅   | 完成     |
> | 💔   | 随时抛弃 |
