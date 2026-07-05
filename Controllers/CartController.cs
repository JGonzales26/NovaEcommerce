using System.Security.Claims;
using EcommerceMVC.Services.Interfaces;
using EcommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceMVC.Controllers;

[Authorize(Policy = "CustomerOnly")]
public sealed class CartController(ICartService cartService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var cart = await cartService.GetCartAsync(userId);
        return View(new CartIndexViewModel { Cart = cart });
    }

    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var userId = GetUserId();
        await cartService.AddItemAsync(userId, productId, quantity);
        TempData["Success"] = "Producto agregado al carrito.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(int productId, int quantity)
    {
        var userId = GetUserId();
        await cartService.UpdateQuantityAsync(userId, productId, quantity);
        TempData["Success"] = "Carrito actualizado.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int productId)
    {
        var userId = GetUserId();
        await cartService.RemoveItemAsync(userId, productId);
        TempData["Success"] = "Producto eliminado del carrito.";
        return RedirectToAction("Index");
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
