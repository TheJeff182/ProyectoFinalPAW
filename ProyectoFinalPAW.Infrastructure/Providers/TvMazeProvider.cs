using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ProyectoFinalPAW.Core.Export;
using ProyectoFinalPAW.Core.Interfaces;

namespace ProyectoFinalPAW.Infrastructure.Providers;

// Las series de TVMaze son consumidas y traducidas al formato estándar.
public partial class TvMazeProvider(HttpClient http, ILogger<TvMazeProvider> logger) : IContentProvider
{
    private const string Endpoint = "https://api.tvmaze.com/shows?page=1";

    public string SourceId => "tvmaze";

    public async Task<ExportSource> GetContentAsync(int limit = 12, CancellationToken ct = default)
    {
        // Se crea la fuente con sus datos fijos.
        var source = new ExportSource
        {
            Id = SourceId,
            Name = "TVMaze",
            Url = Endpoint,
            Category = Categories.Entretenimiento
        };

        try
        {
            // La respuesta de la API es descargada y leída como JSON.
            await using var stream = await http.GetStreamAsync(Endpoint, ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

            // Cada show es traducido a un ExportItem; el objeto original es guardado en "data".
            foreach (var show in doc.RootElement.EnumerateArray().Take(limit))
            {
                var originalId = show.GetProperty("id").GetInt32().ToString(CultureInfo.InvariantCulture);
                var url = show.TryGetProperty("url", out var urlElement) ? urlElement.GetString() : null;
                var thumbnail = GetThumbnail(show);
                var premiered = GetPremieredUtc(show);
                var description = GetCleanSummary(show);

                source.Items.Add(new ExportItem
                {
                    Id = $"{SourceId}-{originalId}",
                    Title = show.GetProperty("name").GetString() ?? "Show",
                    MediaType = MediaTypes.Image,
                    CreatedAt = premiered,
                    Category = Categories.Entretenimiento,
                    Url = url,
                    Thumbnail = thumbnail,
                    Description = description,
                    Data = show.Clone()
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

    // Se obtiene la miniatura del show si la API la incluyó.
    private static string? GetThumbnail(JsonElement show)
    {
        if (!show.TryGetProperty("image", out var imageElement))
            return null;

        return imageElement.TryGetProperty("medium", out var mediumElement)
            ? mediumElement.GetString()
            : null;
    }

    // Se convierte la fecha de estreno al formato UTC esperado por el estándar.
    private static DateTime GetPremieredUtc(JsonElement show)
    {
        if (show.TryGetProperty("premiered", out var premieredElement)
            && DateTime.TryParse(premieredElement.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var premiered))
        {
            return premiered.ToUniversalTime();
        }

        return DateTime.UtcNow;
    }

    // Se limpia el resumen para evitar etiquetas HTML en la descripción opcional.
    private static string? GetCleanSummary(JsonElement show)
    {
        if (!show.TryGetProperty("summary", out var summaryElement))
            return null;

        var summary = summaryElement.GetString();
        if (string.IsNullOrWhiteSpace(summary))
            return null;

        return HtmlTagRegex().Replace(summary, string.Empty).Trim();
    }

    [GeneratedRegex("<.*?>", RegexOptions.Compiled)]
    private static partial Regex HtmlTagRegex();
}
