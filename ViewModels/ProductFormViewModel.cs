using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceMVC.ViewModels;

public sealed class ProductFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(140, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(1200, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 999999)]
    public decimal Price { get; set; }

    [Range(0, 100000)]
    public int Stock { get; set; }

    [Required, Url, Display(Name = "Imagen URL")]
    public string ImageUrl { get; set; } = string.Empty;

    [Display(Name = "Categoria"), Range(1, int.MaxValue, ErrorMessage = "Selecciona una categoria.")]
    public int CategoryId { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; } = Array.Empty<SelectListItem>();
}
