using EcommerceMVC.Models;

namespace EcommerceMVC.Repositories.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(int userId);
    Task<Cart?> GetByIdAsync(int id);
    Task AddAsync(Cart cart);
    void Update(Cart cart);
    void RemoveItem(CartItem item);
    Task SaveChangesAsync();
}
