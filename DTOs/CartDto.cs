namespace EcommerceMVC.DTOs;

public sealed class CartItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal => UnitPrice * Quantity;
}

public sealed class CartDto
{
    public int Id { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public int TotalItems => Items.Sum(i => i.Quantity);
    public decimal Total => Items.Sum(i => i.Subtotal);
}

public sealed class CartItemSyncDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
