using EcommerceMVC.Models;

namespace EcommerceMVC.Repositories.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(int id);
    Task<IReadOnlyList<Order>> GetByUserAsync(int userId);
    Task<IReadOnlyList<Order>> GetAllAsync();
    Task SaveChangesAsync();
}
