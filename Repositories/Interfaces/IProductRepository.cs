using EcommerceMVC.DTOs;
using EcommerceMVC.Helpers;
using EcommerceMVC.Models;

namespace EcommerceMVC.Repositories.Interfaces;

public interface IProductRepository
{
    Task<PagedResult<Product>> GetPagedAsync(ProductQueryDto query);
    Task<IReadOnlyList<Product>> GetFeaturedAsync(int count);
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    void Update(Product product);
    void Remove(Product product);
    Task<bool> ExistsAsync(int id);
    Task SaveChangesAsync();
}
