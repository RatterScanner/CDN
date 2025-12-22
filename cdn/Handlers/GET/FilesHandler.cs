using cdn.Services;
using Microsoft.AspNetCore.StaticFiles;

namespace cdn.Handlers.Get;

public class FilesHandler
{
    private readonly IStorageService storage;

    public FilesHandler(IStorageService aStorage)
    {
        storage = aStorage;
    }

    // Blacklist reserved paths so file-serving doesn't shadow real endpoints.
    private static readonly HashSet<string> ReservedRootPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "ping"
    };

    public IResult Get(string aName, HttpContext aCTX)
    {
        // If the client asked for a path that maps to other endpoints, do not serve file.
        if (string.IsNullOrWhiteSpace(aName) || ReservedRootPaths.Contains(aName))
        {
            return Results.NotFound();
        }

        if (!storage.Exists(aName)) return Results.NotFound();
        var stream = storage.OpenRead(aName);

        // Detect content type by file extension when possible
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(aName, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return Results.File(stream, contentType: contentType, fileDownloadName: aName);
    }
}
