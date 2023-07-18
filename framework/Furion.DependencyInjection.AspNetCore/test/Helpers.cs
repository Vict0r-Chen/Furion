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

namespace Furion.DependencyInjection.AspNetCore.Tests;

public class Helpers
{
    public static ControllerContext CreateControllerContext(ServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        var httpContext = new DefaultHttpContext()
        {
            RequestServices = serviceProvider
        };
        var actionDescriptor = new ControllerActionDescriptor()
        {
            ControllerTypeInfo = typeof(AutowiredController).GetTypeInfo()
        };

        var context = new ControllerContext(new ActionContext(httpContext, new(), actionDescriptor))
        {
            ActionDescriptor = actionDescriptor,
            HttpContext = httpContext
        };
        return context;
    }

    public static int GetIdlePort()
    {
        var fromPort = 10000;
        var toPort = 65535;
        var randomPort = RandomNumberGenerator.GetInt32(fromPort, toPort);

        while (IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any(p => p.Port == randomPort))
        {
            randomPort = RandomNumberGenerator.GetInt32(fromPort, toPort);
        }

        return randomPort;
    }
}