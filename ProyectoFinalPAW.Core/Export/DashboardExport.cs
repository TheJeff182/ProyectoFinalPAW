using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProyectoFinalPAW.Core.Export;

// Se representa el archivo raíz del estándar JSON 1.0.
public class DashboardExport
{
    public const string CurrentVersion = "1.0";

    // Campos del nivel archivo.
    [JsonPropertyName("version")] public string Version { get; set; } = CurrentVersion;
    [JsonPropertyName("exportedAt")] public DateTime? ExportedAt { get; set; }
    [JsonPropertyName("exportedBy")] public string? ExportedBy { get; set; }
    [JsonPropertyName("sources")] public List<ExportSource> Sources { get; set; } = [];

    // Se configuran las opciones comunes: fechas con "Z", tildes legibles y campos desconocidos ignorados.
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new UtcDateConverter() }
    };

    // Se convierte un texto JSON en un objeto DashboardExport.
    public static DashboardExport? FromJson(string json)
        => JsonSerializer.Deserialize<DashboardExport>(json, JsonOptions);

    // Se convierte el objeto en texto JSON con el formato estándar.
    public string ToJson() => JsonSerializer.Serialize(this, JsonOptions);
}
