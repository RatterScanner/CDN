using cdn.Services;
using Microsoft.AspNetCore.StaticFiles;

namespace cdn.Handlers.Get;

public class FilesHandler
{
    private readonly IStorageService _storage;

    public FilesHandler(IStorageService storage)
    {
        _storage = storage;
    }

    // Blacklist reserved paths so file-serving doesn't shadow real endpoints.
    private static readonly HashSet<string> ReservedRootPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "ping"
    };

    public IResult Get(string name, HttpContext ctx)
    {
        // If the client asked for a path that maps to other endpoints, do not serve file.
        if (string.IsNullOrWhiteSpace(name) || ReservedRootPaths.Contains(name))
        {
            return Results.NotFound();
        }

        if (!_storage.Exists(name)) return Results.NotFound();
        var stream = _storage.OpenRead(name);

        // Detect content type by file extension when possible
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(name, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return Results.File(stream, contentType: contentType, fileDownloadName: name);
    }
}
