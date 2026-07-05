using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.Models;

public sealed class Product
{
    public int Id { get; set; }

    [Required, MaxLength(140)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(1200)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 999999)]
    public decimal Price { get; set; }

    [Range(0, 100000)]
    public int Stock { get; set; }

    [MaxLength(600)]
    public string ImageUrl { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
