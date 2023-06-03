using Furion.Tests.DependencyInjection.Named;

namespace Furion.Tests.DependencyInjection;

public class NamedServiceTests
{
    [Fact]
    public void INamedService_ExistsResolve_ReturnOK()
    {
        var name = "name1";
        var services = new ServiceCollection();
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass, NamedClass>());

        var serviceProvider = services.BuildServiceProvider();
        var namedService = serviceProvider.GetRequiredService<INamedService<INamedClass>>();
        Assert.NotNull(namedService);

        var service1 = namedService[name];
        var service2 = namedService.Get(name);
        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.Equal(service1, service2);
    }

    [Fact]
    public void INamedService_EmptyRegister_ReturnOops()
    {
        var services = new ServiceCollection();
        services.AddNamed();

        Assert.Throws<InvalidOperationException>(() =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var namedService = serviceProvider.GetRequiredService<INamedService<INamedClass>>();
            var service1 = namedService["name1"];
        });
    }

    [Fact]
    public void INamedService_EmptyRegister_ReturnOK()
    {
        var services = new ServiceCollection();
        services.AddNamed();

        var serviceProvider = services.BuildServiceProvider();
        var namedService = serviceProvider.GetRequiredService<INamedService<INamedClass>>();
        var service1 = namedService.Get("name1");
        Assert.Null(service1);
    }

    [Fact]
    public void INamedService_GetEnumerator_ReturnOK()
    {
        var name = "name1";
        var guid = Guid.NewGuid();

        var services = new ServiceCollection();
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass, NamedClass>());
        services.AddNamed(name, ServiceDescriptor.Scoped<INamedClass>(sp =>
        {
            return new NamedClass
            {
                Id = guid
            };
        }));

        var serviceProvider = services.BuildServiceProvider();
        var namedService = serviceProvider.GetRequiredService<INamedService<INamedClass>>();
        var service1s = namedService.GetEnumerator(name);
        var count = service1s.LongCount();

        Assert.Equal(2, count);

        var service2 = service1s.Last();
        Assert.Equal(guid, service2.Id);
    }
}