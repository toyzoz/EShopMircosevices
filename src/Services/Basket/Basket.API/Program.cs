using BuildingBlocks.Messaging.Extensions;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
//=================================
builder.Services.AddCarter(configurator: c =>
{
    var types = assembly.GetTypes();
    var modules = types.Where(t =>
            typeof(ICarterModule).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false })
        .ToArray();
    c.WithModules(modules);
});
//=================================
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
//=================================
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddMarten(opt =>
    {
        opt.Connection(connectionString!);
        opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    })
    .UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket";
});

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
{
    opt.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
});

//=================================
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
//=================================
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!)
;
// add message broker
builder.Services.AddMessageBroker(builder.Configuration);

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(opt => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });


app.Run();