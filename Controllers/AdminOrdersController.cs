using EcommerceMVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceMVC.Controllers;

[Authorize(Policy = "AdminOnly")]
[Route("Admin/Orders")]
public sealed class AdminOrdersController(IOrderService orders) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await orders.GetAllAsync());
}
