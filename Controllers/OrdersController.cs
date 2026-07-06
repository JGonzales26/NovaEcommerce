using AutoMapper;
using EcommerceMVC.DTOs;
using EcommerceMVC.Helpers;
using EcommerceMVC.Services.Interfaces;
using EcommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceMVC.Controllers;

[Authorize(Policy = "CustomerOnly")]
public sealed class OrdersController(IOrderService orders, ICartService cart, IMapper mapper) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = CurrentUser.GetUserId(User)!.Value;
        return View(await orders.GetUserOrdersAsync(userId));
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var model = await cart.GetCartAsync(CurrentUser.GetUserId(User)!.Value, SessionId());
        if (model.Items.Count == 0) return RedirectToAction("Index", "Cart");
        return View(new CheckoutViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var order = await orders.CheckoutAsync(CurrentUser.GetUserId(User)!.Value, SessionId(), mapper.Map<CheckoutDto>(model));
        TempData["Success"] = $"Pedido #{order.Id} creado correctamente.";
        return RedirectToAction("Index");
    }

    private string SessionId()
    {
        var value = HttpContext.Session.GetString(SessionKeys.CartSessionId);
        if (!string.IsNullOrWhiteSpace(value)) return value;

        value = Guid.NewGuid().ToString("N");
        HttpContext.Session.SetString(SessionKeys.CartSessionId, value);
        return value;
    }
}
