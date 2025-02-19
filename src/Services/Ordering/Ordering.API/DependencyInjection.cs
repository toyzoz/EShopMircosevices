using System.Reflection;
using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter(configurator: cfg =>
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            var modules = types.Where(t =>
                    typeof(ICarterModule).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false })
                .ToArray();
            cfg.WithModules(modules);
        });

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddHealthChecks().AddSqlServer(configuration.GetConnectionString("Database")!);
        return services;
    }

    public static IApplicationBuilder UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(opt => { });
        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        return app;
    }
}