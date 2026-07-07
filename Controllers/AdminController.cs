using EcommerceMVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceMVC.Controllers;

[Authorize(Policy = "AdminOnly")]
public sealed class AdminController(IAdminService admin) : Controller
{
    public async Task<IActionResult> Index() => View(await admin.GetDashboardAsync());
}
