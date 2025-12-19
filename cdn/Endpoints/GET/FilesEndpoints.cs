using cdn.Handlers.Get;

namespace cdn.Endpoints.Get;

public static class FilesEndpoints
{
    public static void MapFilesEndpoints(this IEndpointRouteBuilder app)
    {
        // Serve files at GET path like http://<domainname>/<filename.ext>
        // This avoids colliding with other routes (ping, openapi, swagger, favicon.ico)
        app.MapGet("/{name}", (FilesHandler filesHandler, string name, HttpContext httpContext) => filesHandler.Get(name, httpContext))
        .WithName("GetFile");
    }
}
