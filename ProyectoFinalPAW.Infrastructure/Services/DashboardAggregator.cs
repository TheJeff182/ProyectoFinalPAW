using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using ProyectoFinalPAW.Core.Export;
using ProyectoFinalPAW.Core.Interfaces;

namespace ProyectoFinalPAW.Infrastructure.Services;

// El contenido de todas las fuentes es reunido en un solo DashboardExport.
public class DashboardAggregator(IEnumerable<IContentProvider> providers, IMemoryCache cache, IConfiguration config)
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

    public async Task<DashboardExport> BuildAsync(CancellationToken ct = default)
    {
        // Todas las APIs son consultadas en paralelo.
        var sources = await Task.WhenAll(providers.Select(p => GetCachedAsync(p, ct)));

        // Se arma el archivo raíz con la fecha actual y el nombre del grupo.
        return new DashboardExport
        {
            ExportedAt = DateTime.UtcNow,
            ExportedBy = config["Dashboard:GroupName"] ?? "ProyectoFinalPAW",
            Sources = [.. sources]
        };
    }

    // La respuesta de cada fuente es guardada en caché para no gastar la cuota de la API.
    private async Task<ExportSource> GetCachedAsync(IContentProvider provider, CancellationToken ct)
    {
        var key = $"source:{provider.SourceId}";
        if (cache.TryGetValue(key, out ExportSource? cached) && cached is not null)
            return cached;

        var source = await provider.GetContentAsync(ct: ct);

        // Las fuentes vacías no son guardadas, para reintentar en la próxima carga.
        if (source.Items.Count > 0)
            cache.Set(key, source, CacheDuration);

        return source;
    }
}
