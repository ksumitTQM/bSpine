using System.Diagnostics;
using bSpine.Models.ViewModels;
using bSpine.Services;
using Microsoft.AspNetCore.Mvc;

namespace bSpine.Controllers;

public class HomeController : Controller
{
    private readonly IDashboardService _dashboard;

    public HomeController(IDashboardService dashboard)
    {
        _dashboard = dashboard;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _dashboard.GetDashboardAsync();
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
