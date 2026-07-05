using EcommerceMVC.DTOs;
using EcommerceMVC.Services.Interfaces;
using EcommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceMVC.Controllers;

public sealed class ProductsController(IProductService products, ICategoryService categories) : Controller
{
    public async Task<IActionResult> Index(string? search, int? categoryId, int page = 1)
    {
        var model = new ProductListViewModel
        {
            Search = search,
            CategoryId = categoryId,
            Categories = await categories.GetSelectListAsync(categoryId),
            Products = await products.GetPagedAsync(new ProductQueryDto(search, categoryId, page))
        };

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await products.GetByIdAsync(id);
        return product is null ? NotFound() : View(product);
    }
}
