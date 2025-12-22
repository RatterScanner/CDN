using cdn.Endpoints.Get;
using cdn.Handlers.Get;
using cdn.Services;
using cdn.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddEnvironmentVariables();

// register app services and handlers
builder.Services.AddCdnServices();

// If a PORT env is provided, configure Kestrel to listen on it
var port = builder.Configuration["PORT"] ?? builder.Configuration["ASPNETCORE_PORT"];
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

// map endpoints
app.MapPingEndpoints();
app.MapFilesEndpoints();

app.Run();