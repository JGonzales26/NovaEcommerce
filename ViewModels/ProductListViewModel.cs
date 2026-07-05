using EcommerceMVC.DTOs;
using EcommerceMVC.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceMVC.ViewModels;

public sealed class ProductListViewModel
{
    public PagedResult<ProductDto> Products { get; set; } = new();
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public IEnumerable<SelectListItem> Categories { get; set; } = Array.Empty<SelectListItem>();
}
