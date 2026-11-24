using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using ProyectoFinalPAW.Core.Export;
using ProyectoFinalPAW.Infrastructure.Services;

namespace ProyectoFinalPAW.Web.Pages;

public class IndexModel(DashboardAggregator aggregator) : PageModel
{
    // Se guarda el contenido que es mostrado en el dashboard.
    public DashboardExport Dashboard { get; private set; } = new();

    // El contenido de todas las fuentes es cargado al abrir la página.
    public async Task OnGetAsync(CancellationToken ct)
    {
        Dashboard = await aggregator.BuildAsync(ct);
    }

    // Se consulta el contenido actual del dashboard, se convierte a UTF-8 y se entrega como archivo JSON descargable.
    public async Task<IActionResult> OnGetExportAsync(CancellationToken ct)
    {
        var export = await aggregator.BuildAsync(ct);
        var json = export.ToJson();
        var fileName = $"mediahub-{DateTime.UtcNow:yyyyMMdd-HHmm}.json";

        return File(Encoding.UTF8.GetBytes(json), "application/json", fileName);
    }
}
