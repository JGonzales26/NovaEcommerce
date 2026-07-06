using EcommerceMVC.DTOs;
using EcommerceMVC.Models;

namespace EcommerceMVC.Services.Interfaces;

public interface IOrderService
{
    Task<Order> CheckoutAsync(int userId, string sessionId, CheckoutDto checkout);
    Task<IReadOnlyList<Order>> GetUserOrdersAsync(int userId);
    Task<IReadOnlyList<Order>> GetAllAsync();
}
