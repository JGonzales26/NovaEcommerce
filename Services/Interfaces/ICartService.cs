using EcommerceMVC.DTOs;

namespace EcommerceMVC.Services.Interfaces;

public interface ICartService
{
    Task<CartDto> GetCartAsync(int userId);
    Task AddItemAsync(int userId, int productId, int quantity);
    Task UpdateQuantityAsync(int userId, int productId, int quantity);
    Task RemoveItemAsync(int userId, int productId);
    Task<CartDto> MergeCartAsync(int userId, List<CartItemSyncDto> items);
    Task CheckoutAsync(int userId);
}
