using EcommerceMVC.Models;
using EcommerceMVC.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceMVC.Controllers;

public sealed class HomeController(IProductService products, ILogger<HomeController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await products.GetFeaturedAsync(8));
    }

    public IActionResult ErrorMessage(string message)
    {
        ViewData["Message"] = message;
        return View("Error");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (feature?.Error is not null)
            logger.LogError(feature.Error, "Unhandled MVC error");

        return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
    }
}
