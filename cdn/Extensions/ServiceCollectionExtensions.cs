using Microsoft.Extensions.DependencyInjection;
using cdn.Handlers.Get;
using cdn.Services;

namespace cdn.Extensions;

public static class ServiceCollectionExtensions
{
    // Register application services and handlers in one place
    public static IServiceCollection AddCdnServices(this IServiceCollection services)
    {
        services.AddSingleton<PingHandler>();
        services.AddScoped<FilesHandler>();
        services.AddSingleton<IStorageService, StorageService>();
        return services;
    }
}
