using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public static class Extentions
{
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateAsyncScope();
        var discountContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();
        discountContext.Database.MigrateAsync();

        return app;
    }
}