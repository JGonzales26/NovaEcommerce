using EcommerceMVC.Models;

namespace EcommerceMVC.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
}
