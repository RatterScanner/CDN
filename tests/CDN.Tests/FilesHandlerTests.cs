using System.IO;
using System.Text;
using System.Threading.Tasks;
using cdn.Handlers.Get;
using cdn.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CDN.Tests;

public class FilesHandlerTests
{
    // Helper to create a default HttpContext
    private static DefaultHttpContext CreateContext()
    {
        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();
    // Provide a minimal service provider so IResult.ExecuteAsync can resolve services
    ctx.RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider();
        return ctx;
    }

    [Fact]
    public async Task Get_EmptyName_ReturnsNotFound()
    {
        var storage = new Mock<IStorageService>();
        var handler = new FilesHandler(storage.Object);
        var ctx = CreateContext();

        var result = handler.Get("", ctx);
        await result.ExecuteAsync(ctx);

    Assert.Equal(404, ctx.Response.StatusCode);
    }

    [Fact]
    public async Task Get_ReservedName_ReturnsNotFound()
    {
        var storage = new Mock<IStorageService>();
        var handler = new FilesHandler(storage.Object);
        var ctx = CreateContext();

        var result = handler.Get("ping", ctx);
        await result.ExecuteAsync(ctx);

        Assert.Equal(404, ctx.Response.StatusCode);
    }

    [Fact]
    public async Task Get_NonexistentFile_ReturnsNotFound()
    {
        var storage = new Mock<IStorageService>();
        storage.Setup(s => s.Exists(It.IsAny<string>())).Returns(false);

        var handler = new FilesHandler(storage.Object);
        var ctx = CreateContext();

        var result = handler.Get("missing.txt", ctx);
        await result.ExecuteAsync(ctx);

        Assert.Equal(404, ctx.Response.StatusCode);
    }

    [Fact]
    public async Task Get_KnownExtension_ReturnsFileResultWithCorrectContentType()
    {
        var name = "hello.txt";
        var content = "hello world";
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);

        var storage = new Mock<IStorageService>();
        storage.Setup(s => s.Exists(name)).Returns(true);
        storage.Setup(s => s.OpenRead(name)).Returns(stream);

        var handler = new FilesHandler(storage.Object);
        var ctx = CreateContext();

        var result = handler.Get(name, ctx);
        await result.ExecuteAsync(ctx);

        Assert.Equal(200, ctx.Response.StatusCode);
        Assert.Equal("text/plain", ctx.Response.ContentType);
        // content-disposition should contain filename
        Assert.Contains(name, ctx.Response.Headers["Content-Disposition"].ToString());

        ctx.Response.Body.Seek(0, SeekOrigin.Begin);
        using var ms = new MemoryStream();
        await ctx.Response.Body.CopyToAsync(ms);
        Assert.Equal(bytes, ms.ToArray());
    }

    [Fact]
    public async Task Get_UnknownExtension_DefaultsToOctetStream()
    {
        var name = "file.unknownext";
        var content = "data";
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);

        var storage = new Mock<IStorageService>();
        storage.Setup(s => s.Exists(name)).Returns(true);
        storage.Setup(s => s.OpenRead(name)).Returns(stream);

        var handler = new FilesHandler(storage.Object);
        var ctx = CreateContext();

        var result = handler.Get(name, ctx);
        await result.ExecuteAsync(ctx);

        Assert.Equal(200, ctx.Response.StatusCode);
        Assert.Equal("application/octet-stream", ctx.Response.ContentType);
        Assert.Contains(name, ctx.Response.Headers["Content-Disposition"].ToString());

        ctx.Response.Body.Seek(0, SeekOrigin.Begin);
        using var ms = new MemoryStream();
        await ctx.Response.Body.CopyToAsync(ms);
        Assert.Equal(bytes, ms.ToArray());
    }
}
