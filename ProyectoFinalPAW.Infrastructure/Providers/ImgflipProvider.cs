using System.Text.Json;
using Microsoft.Extensions.Logging;
using ProyectoFinalPAW.Core.Export;
using ProyectoFinalPAW.Core.Interfaces;

namespace ProyectoFinalPAW.Infrastructure.Providers;

// Los memes de Imgflip son consumidos y traducidos al formato estándar.
public class ImgflipProvider(HttpClient http, ILogger<ImgflipProvider> logger) : IContentProvider
{
    private const string Endpoint = "https://api.imgflip.com/get_memes";

    public string SourceId => "imgflip";

    public async Task<ExportSource> GetContentAsync(int limit = 12, CancellationToken ct = default)
    {
        // Se crea la fuente con sus datos fijos.
        var source = new ExportSource
        {
            Id = SourceId,
            Name = "Imgflip",
            Url = Endpoint,
            Category = Categories.Memes
        };

        try
        {
            // La respuesta de la API es descargada y leída como JSON.
            await using var stream = await http.GetStreamAsync(Endpoint, ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
            var memes = doc.RootElement.GetProperty("data").GetProperty("memes");

            // Cada meme es traducido a un ExportItem; el objeto original es guardado en "data".
            foreach (var meme in memes.EnumerateArray().Take(limit))
            {
                var url = meme.GetProperty("url").GetString();
                source.Items.Add(new ExportItem
                {
                    Id = $"{SourceId}-{meme.GetProperty("id").GetString()}",
                    Title = meme.GetProperty("name").GetString() ?? "Meme",
                    MediaType = MediaTypes.Image,
                    CreatedAt = DateTime.UtcNow,
                    Category = Categories.Memes,
                    Url = url,
                    Thumbnail = url,
                    Data = meme.Clone()
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
}
