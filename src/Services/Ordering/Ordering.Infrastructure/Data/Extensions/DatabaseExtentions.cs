using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
        await SeedAsync(dbContext);
    }

    private static async Task SeedAsync(this ApplicationDbContext dbContext)
    {
        await SeedCustomerAsync(dbContext);
        await SeedProductAsync(dbContext);
        await SeedOrdersWithItemsAsync(dbContext);

        Console.WriteLine("Ordering data have been successfully seeded");
    }

    private static async Task SeedProductAsync(ApplicationDbContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext context)
    {
        if (!await context.Orders.AnyAsync())
        {
            await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedCustomerAsync(ApplicationDbContext context)
    {
        if (!await context.Customers.AnyAsync())
        {
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
    }
}