using Microsoft.AspNetCore.Mvc.RazorPages;
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
}
