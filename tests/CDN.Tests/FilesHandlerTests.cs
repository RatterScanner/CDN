using System.IO;
using System.Text;
using System.Threading.Tasks;
using cdn.Handlers.Get;
using cdn.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;

namespace CDN.Tests;

public class FilesHandlerTests
{
    // Helper to create a default HttpContext
    private static DefaultHttpContext CreateContext()
    {
        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();
        ctx.RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider();
        return ctx;
    }

    // Base path to sample files
    private static FilesHandler CreateHandler() => new FilesHandler(new StorageService());

    // Test for empty file name
    [Fact]
    public async Task Get_EmptyName_Returns404()
    {
        var handler = CreateHandler();
        var ctx = CreateContext();

        var result = handler.Get("", ctx);
        await result.ExecuteAsync(ctx);

        Assert.Equal(404, ctx.Response.StatusCode);
    }

    // Test for illegal file paths
    [Fact]
    public async Task Get_IllegalDirectory_Returns404()
    {
        var handler = CreateHandler();
        var ctx = CreateContext();

        var result = handler.Get("../src/README.md", ctx);
        await result.ExecuteAsync(ctx);

        Assert.Equal(404, ctx.Response.StatusCode);
    }

    // Test for a nonexistent file
    [Fact]
    public async Task Get_NonexistentFile_Returns404()
    {
        var handler = CreateHandler();
        var ctx = CreateContext();

        var result = handler.Get("missing.txt", ctx);
        await result.ExecuteAsync(ctx);

        Assert.Equal(404, ctx.Response.StatusCode);
    }

    // Test for a valid file with known extension
    [Fact]
    public async Task Get_KnownExtension_ReturnsFileResultWithCorrectContentType()
    {
        var fileName = "sample.txt";
        var filePath = Path.Combine(StorageService.STORAGE_PATH, fileName);
        var expectedBytes = await File.ReadAllBytesAsync(filePath);
        
        var handler = CreateHandler();
        var ctx = CreateContext();

        var result = handler.Get(fileName, ctx);
        await result.ExecuteAsync(ctx);

        Assert.Equal(200, ctx.Response.StatusCode);
        Assert.Equal("text/plain", ctx.Response.ContentType);
        Assert.Contains(fileName, ctx.Response.Headers["Content-Disposition"].ToString());

        ctx.Response.Body.Seek(0, SeekOrigin.Begin);
        using var ms = new MemoryStream();
        await ctx.Response.Body.CopyToAsync(ms);
        Assert.Equal(expectedBytes, ms.ToArray());
    }

    // Test for a file with unknown extension
    [Fact]
    public async Task Get_UnknownExtension_DefaultsToOctetStream()
    {
        var fileName = "file.unknownext";
        var filePath = Path.Combine(StorageService.STORAGE_PATH, fileName);
        var expectedBytes = await File.ReadAllBytesAsync(filePath);

        var handler = CreateHandler();
        var ctx = CreateContext();

        var result = handler.Get(fileName, ctx);
        await result.ExecuteAsync(ctx);

        Assert.Equal(200, ctx.Response.StatusCode);
        Assert.Equal("application/octet-stream", ctx.Response.ContentType);
        Assert.Contains(fileName, ctx.Response.Headers["Content-Disposition"].ToString());

        ctx.Response.Body.Seek(0, SeekOrigin.Begin);
        using var ms = new MemoryStream();
        await ctx.Response.Body.CopyToAsync(ms);
        Assert.Equal(expectedBytes, ms.ToArray());
    }
}
