using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.Models;

public sealed class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [Range(1, 100000)]
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
