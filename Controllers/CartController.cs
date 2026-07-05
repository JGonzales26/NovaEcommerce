using System.Security.Claims;
using System.Text.Json;
using EcommerceMVC.DTOs;
using EcommerceMVC.Services.Interfaces;
using EcommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceMVC.Controllers;

public sealed class CartController(ICartService cartService) : Controller
{
    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = GetUserId();
            var cart = await cartService.GetCartAsync(userId);
            return View(new CartIndexViewModel { Cart = cart, IsAuthenticated = true });
        }

        return View(new CartIndexViewModel { Cart = new CartDto(), IsAuthenticated = false });
    }

    [Authorize(Policy = "CustomerOnly")]
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Sync()
    {
        var userId = GetUserId();

        List<CartItemSyncDto> items;
        try
        {
            items = await JsonSerializer.DeserializeAsync<List<CartItemSyncDto>>(
                Request.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch
        {
            items = new();
        }

        var cart = await cartService.MergeCartAsync(userId, items);
        return Json(new { success = true, totalItems = cart.TotalItems });
    }

    [Authorize(Policy = "CustomerOnly")]
    [HttpPost]
    public async Task<IActionResult> Update(int productId, int quantity)
    {
        var userId = GetUserId();
        await cartService.UpdateQuantityAsync(userId, productId, quantity);
        TempData["Success"] = "Carrito actualizado.";
        return RedirectToAction("Index");
    }

    [Authorize(Policy = "CustomerOnly")]
    [HttpPost]
    public async Task<IActionResult> Remove(int productId)
    {
        var userId = GetUserId();
        await cartService.RemoveItemAsync(userId, productId);
        TempData["Success"] = "Producto eliminado del carrito.";
        return RedirectToAction("Index");
    }

    [Authorize(Policy = "CustomerOnly")]
    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        var userId = GetUserId();
        await cartService.CheckoutAsync(userId);
        TempData["Success"] = "Compra realizada con exito. El stock ha sido actualizado.";
        return RedirectToAction("Index");
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
