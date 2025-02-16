using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<DiscountContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("Database"));
});
var app = builder.Build();
app.UseMigration();
// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
 

app.Run();