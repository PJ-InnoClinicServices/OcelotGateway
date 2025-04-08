using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotGateway;
using OcelotGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddOcelotRoutes();
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddLogging(logging =>
{
    logging.AddConsole(); 
    logging.AddDebug();   
});

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowLocalhost");
app.UseAuthentication();  
app.UseAuthorization();   

await app.UseOcelot();

app.Run();