using System.Text.Json.Serialization;

namespace ProyectoFinalPAW.Core.Export;

// Se representa una fuente (una API) con sus items.
public class ExportSource
{
    // Campos obligatorios de la fuente.
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    // Campos opcionales de la fuente. La url es guardada sin API keys.
    [JsonPropertyName("url")] public string? Url { get; set; }
    [JsonPropertyName("category")] public string? Category { get; set; }

    // Se guardan los contenidos de la fuente.
    [JsonPropertyName("items")] public List<ExportItem> Items { get; set; } = [];
}
