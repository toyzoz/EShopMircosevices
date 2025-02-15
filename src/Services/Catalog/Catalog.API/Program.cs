using JasperFx;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter(configurator: c =>
{
    var types = typeof(Program).Assembly.GetTypes();
    var modules = types.Where(t =>
            typeof(ICarterModule).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false })
        .ToArray();
    c.WithModules(modules);
});
builder.Services.AddMediatR(config => { config.RegisterServicesFromAssembly(typeof(Program).Assembly); });
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
    opt.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
}).UseLightweightSessions();

var app = builder.Build();


app.MapGet("/", () => "Hello World!");
app.MapCarter();

app.Run();