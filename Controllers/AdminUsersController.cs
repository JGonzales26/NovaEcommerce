using EcommerceMVC.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceMVC.Controllers;

[Authorize(Policy = "AdminOnly")]
[Route("Admin/Users")]
public sealed class AdminUsersController(IUserRepository users) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await users.GetAllAsync());
}
