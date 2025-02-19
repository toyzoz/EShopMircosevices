using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Interceptors;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, opt) =>
        {
            var connectionString = configuration.GetConnectionString("Database");
            opt.UseSqlServer(connectionString);

            opt.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}