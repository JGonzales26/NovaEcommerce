using EcommerceMVC.Data;
using EcommerceMVC.Models;
using EcommerceMVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Repositories.Implementations;

public sealed class CategoryRepository(ApplicationDbContext db) : ICategoryRepository
{
    public async Task<IReadOnlyList<Category>> GetAllAsync() =>
        await db.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync();

    public async Task<Category?> GetByIdAsync(int id) =>
        await db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
}
