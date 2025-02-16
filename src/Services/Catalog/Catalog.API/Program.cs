using Catalog.API.Data;
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
builder.Services.AddMarten(opt => { opt.Connection(connectionString!); })
    .UseLightweightSessions();

if (builder.Environment.IsDevelopment()) builder.Services.InitializeMartenWith<CatalogInitialData>();

//=================================
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
//=================================
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString!);

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(opt => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();