namespace EcommerceMVC.Models;

public sealed class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal Total { get; set; }

    public string ShippingFullName { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingPhone { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();
}
