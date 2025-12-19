using cdn.Endpoints.Get;
using cdn.Utils;

// Load environment variables from .env (if present)
EnvLoader.Load();

var builder = WebApplication.CreateBuilder(args);

// Initialize openAPI for automatic documentation
builder.Services.AddOpenApi();

// register handlers
builder.Services.AddSingleton<cdn.Handlers.Get.PingHandler>();

// If a PORT env var is provided, configure Kestrel to listen on it
var port = Environment.GetEnvironmentVariable("PORT") ?? Environment.GetEnvironmentVariable("ASPNETCORE_PORT");
if (!string.IsNullOrEmpty(port) && int.TryParse(port, out var p))
{
	builder.WebHost.UseUrls($"http://*:{p}");
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

// map endpoints
app.MapPingEndpoints();

app.Run();