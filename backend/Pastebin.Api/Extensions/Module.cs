namespace Pastebin.Api.Extensions;

public interface IModule
{
    IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}

public static class ModuleExtensions
{
    private static readonly List<IModule> RegisteredModules = [];

    public static IServiceCollection RegisterModules(this IServiceCollection services)
    {
        var modules = DiscoverModules();
        foreach (var module in modules)
        {
            RegisteredModules.Add(module);
        }

        return services;
    }

    public static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder route)
    {
        foreach (var module in RegisteredModules)
        {
            module.MapEndpoints(route);
        }

        return route;
    }

    private static IEnumerable<IModule> DiscoverModules()
    {
        return typeof(IModule).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}