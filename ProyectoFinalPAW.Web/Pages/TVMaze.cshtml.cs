using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoFinalPAW.Core.Export;
using ProyectoFinalPAW.Infrastructure.Services;

namespace ProyectoFinalPAW.Web.Pages;

public class TVMazeModel(DashboardAggregator aggregator) : PageModel
{
    // Se guarda únicamente la fuente TVMaze que será mostrada en la vista.
    public ExportSource Source { get; private set; } = new()
    {
        Id = "tvmaze",
        Name = "TVMaze",
        Category = Categories.Entretenimiento
    };

    // El dashboard es cargado y se selecciona solo la fuente TVMaze.
    public async Task OnGetAsync(CancellationToken ct)
    {
        var dashboard = await aggregator.BuildAsync(ct);
        Source = dashboard.Sources.FirstOrDefault(s => s.Id == "tvmaze") ?? Source;
    }
}
