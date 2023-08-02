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

using Furion.Tests.Models;

namespace Furion.Tests.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class HelloController
{
    public HelloController(IServiceProvider _)  // 构造函数注入
    {
    }

    [AutowiredService]  // 属性注入
    private IConfiguration Configuration { get; set; } = null!;

    [HttpGet]
    public string? Get([FromServices] IConfiguration configuration) // 参数注入
    {
        return configuration["Name"] + "Embed: " + Configuration["Embed:Name"];
    }

    [HttpPost]
    public Student Post([FromServices] IObjectValidator<Student> validator, [CustomizeValidator] Student stu)
    {
        var validationResults = validator.GetValidationResults(new Student
        {
            Name = "Furion",
            NickName = "Furion",
            Teacher = new() { Name = "Furion" }
        });

        return stu;
    }

    [HttpGet]
    public void TestRetryPolicy()
    {
        Policy.Retry(3)
            .Handle<System.Exception>()
            .OnRetry(context =>
            {
                Console.WriteLine($"正在重试第 {context.RetryCount} 次...");
            })
            .WaitAndRetry(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3))
            .Execute(() =>
            {
                Console.WriteLine("我正在执行操作...");
                throw new System.InvalidOperationException("我出错了");
            });
    }

    [HttpGet]
    public void TestTimeoutPolicy()
    {
        Policy.Timeout(3000)
            .OnTimeout(context =>
            {
                Console.WriteLine("不好意思，超时了.");
            })
            .Execute(() =>
            {
                Console.WriteLine("我正在执行操作...");
                Thread.Sleep(5000);
            });
    }

    [HttpGet]
    public string? TestFallbackPolicy([FromQuery] string? name = null)
    {
        return Policy<string>.Fallback()
            .Handle<System.Exception>()
            .HandleResult(context => name is null || context.Exception is not null)
            .OnFallback(context =>
            {
                return "百小僧";
            })
            .Execute(() =>
            {
                if (name == "furion")
                {
                    throw new System.Exception("出错了");
                }

                return name;
            });
    }

    [HttpGet]
    public void TestCompositePolicy([FromQuery] int timeout = 5000)
    {
        var timeoutPolicy = Policy.Timeout(timeout)
            .OnTimeout(context =>
            {
                Console.WriteLine("不好意思，超时了.");
            });

        var retryPolicy = Policy.Retry(3)
            .Handle<System.Exception>()
            .OnRetry(context =>
            {
                Console.WriteLine($"正在重试第 {context.RetryCount} 次...");
            })
            .WaitAndRetry(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3));

        Policy.Composite(timeoutPolicy, retryPolicy)
            .OnExecutionFailure(context =>
            {
                Console.WriteLine($"策略 {context.Policy} 执行失败了.");
            })
            .Execute(() =>
            {
                Console.WriteLine("我正在执行组合操作...");
                throw new System.InvalidOperationException("我出错了");
            });
    }
}