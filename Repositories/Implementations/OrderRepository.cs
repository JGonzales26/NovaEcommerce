using EcommerceMVC.Data;
using EcommerceMVC.Models;
using EcommerceMVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Repositories.Implementations;

public sealed class OrderRepository(ApplicationDbContext db) : IOrderRepository
{
    public async Task AddAsync(Order order) => await db.Orders.AddAsync(order);

    public async Task<Order?> GetByIdAsync(int id) =>
        await db.Orders.Include(o => o.User).Include(o => o.Details).ThenInclude(d => d.Product).FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IReadOnlyList<Order>> GetByUserAsync(int userId) =>
        await db.Orders.AsNoTracking().Include(o => o.Details).Where(o => o.UserId == userId).OrderByDescending(o => o.CreatedAt).ToListAsync();

    public async Task<IReadOnlyList<Order>> GetAllAsync() =>
        await db.Orders.AsNoTracking().Include(o => o.User).Include(o => o.Details).OrderByDescending(o => o.CreatedAt).ToListAsync();

    public async Task SaveChangesAsync() => await db.SaveChangesAsync();
}
