using EcommerceMVC.Data;
using EcommerceMVC.Models;
using EcommerceMVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Repositories.Implementations;

public sealed class UserRepository(ApplicationDbContext db) : IUserRepository
{
    public async Task<AppUser?> GetByEmailAsync(string email) =>
        await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

    public async Task<AppUser?> GetByIdAsync(int id) =>
        await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

    public async Task<IReadOnlyList<AppUser>> GetAllAsync() =>
        await db.Users.AsNoTracking().Include(u => u.Role).OrderBy(u => u.FullName).ToListAsync();

    public async Task<AppRole?> GetRoleByNameAsync(string roleName) =>
        await db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

    public async Task AddAsync(AppUser user) => await db.Users.AddAsync(user);
    public void Update(AppUser user) => db.Users.Update(user);
    public async Task SaveChangesAsync() => await db.SaveChangesAsync();


}
