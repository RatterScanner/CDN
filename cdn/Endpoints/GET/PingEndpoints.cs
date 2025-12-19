using cdn.Handlers.Get;

namespace cdn.Endpoints.Get;

public static class PingEndpoints
{
    public static void MapPingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/ping", (PingHandler handler) => handler.Handle())
           .WithName("Ping")
           .WithOpenApi();
    }
}
