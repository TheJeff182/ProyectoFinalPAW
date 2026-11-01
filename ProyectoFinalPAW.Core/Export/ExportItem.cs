using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProyectoFinalPAW.Core.Export;

// Se representa un contenido individual (noticia, GIF, meme, etc.).
public class ExportItem
{
    // Campos obligatorios del item.
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
    [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;
    [JsonPropertyName("mediaType")] public string MediaType { get; set; } = MediaTypes.Text;
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }

    // Campos opcionales del item.
    [JsonPropertyName("category")] public string? Category { get; set; }
    [JsonPropertyName("url")] public string? Url { get; set; }
    [JsonPropertyName("thumbnail")] public string? Thumbnail { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }

    // La respuesta original de la API es guardada como objeto JSON.
    [JsonPropertyName("data")] public JsonElement? Data { get; set; }
}
