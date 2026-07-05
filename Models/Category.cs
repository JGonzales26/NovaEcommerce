using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.Models;

public sealed class Category
{
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(240)]
    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
