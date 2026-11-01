using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProyectoFinalPAW.Core.Export;

// Las fechas son convertidas a UTC y escritas en formato ISO 8601 terminado en "Z".
public class UtcDateConverter : JsonConverter<DateTime>
{
    // La fecha leída del JSON es convertida a UTC.
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetDateTimeOffset().UtcDateTime;

    // La fecha es escrita como "AAAA-MM-DDTHH:MM:SSZ".
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var utc = value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
            : value.ToUniversalTime();
        writer.WriteStringValue(utc.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture));
    }
}
