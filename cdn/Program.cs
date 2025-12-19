using cdn.Endpoints.Get;
using cdn.Handlers.Get;
using cdn.Services;
using cdn.Extensions;
using cdn.Utils;

// Load environment variables from .env (if present)
EnvLoader.Load();

var builder = WebApplication.CreateBuilder(args);

// register app services and handlers
builder.Services.AddCdnServices();

// If a PORT env is provided, configure Kestrel to listen on it
var port = Environment.GetEnvironmentVariable("PORT") ?? Environment.GetEnvironmentVariable("ASPNETCORE_PORT");
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

// map endpoints
app.MapPingEndpoints();
app.MapFilesEndpoints();

app.Run();