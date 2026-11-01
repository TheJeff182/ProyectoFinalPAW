using ProyectoFinalPAW.Core.Export;

namespace ProyectoFinalPAW.Core.Interfaces;

// Se define el contrato que es implementado por cada API (patrón Adapter).
public interface IContentProvider
{
    // Se identifica la fuente (minúsculas, números y guiones).
    string SourceId { get; }

    // Se obtiene el contenido de la API ya traducido al formato estándar.
    Task<ExportSource> GetContentAsync(int limit = 12, CancellationToken ct = default);
}
