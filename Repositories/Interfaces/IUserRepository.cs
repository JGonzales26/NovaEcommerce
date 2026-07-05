using EcommerceMVC.Models;

namespace EcommerceMVC.Repositories.Interfaces;

public interface IUserRepository
{
    Task<AppUser?> GetByEmailAsync(string email);
    Task<AppUser?> GetByIdAsync(int id);
    Task<IReadOnlyList<AppUser>> GetAllAsync();
    Task<AppRole?> GetRoleByNameAsync(string roleName);
    Task AddAsync(AppUser user);
    void Update(AppUser user);
    Task SaveChangesAsync();
}
