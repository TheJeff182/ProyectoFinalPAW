using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoFinalPAW.Core.Export;
using ProyectoFinalPAW.Infrastructure.Services;

namespace ProyectoFinalPAW.Web.Pages;

public class SpaceflightNewsModel(DashboardAggregator aggregator) : PageModel
{
    // Se guarda únicamente la fuente Spaceflight News que será mostrada en la vista.
    public ExportSource Source { get; private set; } = new()
    {
        Id = "spaceflight-news",
        Name = "Spaceflight News",
        Category = Categories.Ciencia
    };

    // El dashboard es cargado y se selecciona solo la fuente Spaceflight News.
    public async Task OnGetAsync(CancellationToken ct)
    {
        var dashboard = await aggregator.BuildAsync(ct);
        Source = dashboard.Sources.FirstOrDefault(s => s.Id == "spaceflight-news") ?? Source;
    }
}
