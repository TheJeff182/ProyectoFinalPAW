using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ProyectoFinalPAW.Core.Export;
using ProyectoFinalPAW.Core.Interfaces;

namespace ProyectoFinalPAW.Infrastructure.Providers;

// Las noticias de Spaceflight News son consumidas y traducidas al formato estándar.
public class SpaceflightNewsProvider(HttpClient http, ILogger<SpaceflightNewsProvider> logger) : IContentProvider
{
    private const string EndpointBase = "https://api.spaceflightnewsapi.net/v4/articles/";

    public string SourceId => "spaceflight-news";

    public async Task<ExportSource> GetContentAsync(int limit = 12, CancellationToken ct = default)
    {
        // Se crea la fuente con sus datos fijos.
        var source = new ExportSource
        {
            Id = SourceId,
            Name = "Spaceflight News",
            Url = EndpointBase,
            Category = Categories.Ciencia
        };

        try
        {
            // La respuesta de la API es descargada y leída como JSON.
            var endpoint = $"{EndpointBase}?limit={limit}";
            await using var stream = await http.GetStreamAsync(endpoint, ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
            var articles = doc.RootElement.GetProperty("results");

            // Cada artículo es traducido a un ExportItem; el objeto original es guardado en "data".
            foreach (var article in articles.EnumerateArray())
            {
                var originalId = article.GetProperty("id").GetInt32().ToString(CultureInfo.InvariantCulture);
                var publishedAt = GetPublishedAtUtc(article);
                var thumbnail = article.TryGetProperty("image_url", out var imageElement) ? imageElement.GetString() : null;
                var url = article.TryGetProperty("url", out var urlElement) ? urlElement.GetString() : null;
                var description = article.TryGetProperty("summary", out var summaryElement) ? summaryElement.GetString() : null;

                source.Items.Add(new ExportItem
                {
                    Id = $"{SourceId}-{originalId}",
                    Title = article.GetProperty("title").GetString() ?? "Artículo",
                    MediaType = MediaTypes.Text,
                    CreatedAt = publishedAt,
                    Category = Categories.Ciencia,
                    Url = url,
                    Thumbnail = thumbnail,
                    Description = description,
                    Data = article.Clone()
                });
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // El error es registrado y se devuelve la fuente vacía para no romper el dashboard.
            logger.LogWarning(ex, "Falló la fuente {Source}", SourceId);
        }

        return source;
    }

    // Se convierte la fecha de publicación al formato UTC esperado por el estándar.
    private static DateTime GetPublishedAtUtc(JsonElement article)
    {
        if (article.TryGetProperty("published_at", out var publishedAtElement)
            && DateTime.TryParse(publishedAtElement.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var publishedAt))
        {
            return publishedAt.ToUniversalTime();
        }

        return DateTime.UtcNow;
    }
}
