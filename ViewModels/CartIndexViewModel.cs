using EcommerceMVC.DTOs;

namespace EcommerceMVC.ViewModels;

public sealed class CartIndexViewModel
{
    public CartDto Cart { get; set; } = new();
    public bool IsAuthenticated { get; set; }
}
