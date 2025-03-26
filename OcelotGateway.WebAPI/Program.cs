using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotGateway;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddOcelotRoutes();
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddLogging(logging =>
{
    logging.AddConsole(); 
    logging.AddDebug();   
});

builder.Services.AddControllers();

var app = builder.Build();

await app.UseOcelot();
app.Run();