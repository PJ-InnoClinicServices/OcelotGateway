using Ocelot.DependencyInjection;
using Ocelot.Middleware;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration).AddDelegatingHandler<PriceUpdateHandler>();
builder.Services.AddControllers();

var app = builder.Build();
await app.UseOcelot();
app.Run();