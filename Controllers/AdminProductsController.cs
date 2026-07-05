using EcommerceMVC.DTOs;
using EcommerceMVC.Services.Interfaces;
using EcommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceMVC.Controllers;

[Authorize(Policy = "AdminOnly")]
[Route("Admin/Products")]
public sealed class AdminProductsController(IProductService products, ICategoryService categories) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1) => View(await products.GetPagedAsync(new ProductQueryDto(null, null, page, 20)));

    [HttpGet("Create")]
    public async Task<IActionResult> Create() => View(await BuildFormAsync(new ProductFormViewModel()));

    [HttpPost("Create")]
    public async Task<IActionResult> Create(ProductFormViewModel model)
    {
        if (!ModelState.IsValid) return View(await BuildFormAsync(model));
        await products.CreateAsync(model);
        TempData["Success"] = "Producto creado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await products.GetFormAsync(id);
        return model is null ? NotFound() : View(model);
    }

    [HttpPost("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, ProductFormViewModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(await BuildFormAsync(model));
        await products.UpdateAsync(model);
        TempData["Success"] = "Producto actualizado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await products.DeleteAsync(id);
        TempData["Success"] = "Producto desactivado.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<ProductFormViewModel> BuildFormAsync(ProductFormViewModel model)
    {
        model.Categories = await categories.GetSelectListAsync(model.CategoryId);
        return model;
    }
}
