using Microsoft.Extensions.DependencyInjection;
using ProyectoFinalPAW.Core.Interfaces;
using ProyectoFinalPAW.Infrastructure.Providers;
using ProyectoFinalPAW.Infrastructure.Services;

namespace ProyectoFinalPAW.Infrastructure;

// Los servicios de Infrastructure son registrados en un solo lugar.
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Se registra la caché en memoria.
        services.AddMemoryCache();

        // Se registra un Provider por cada API (una línea por API).
        services.AddHttpClient<IContentProvider, ImgflipProvider>();
        services.AddHttpClient<IContentProvider, TvMazeProvider>();

        // Se registra el agregador del dashboard.
        services.AddScoped<DashboardAggregator>();

        return services;
    }
}
