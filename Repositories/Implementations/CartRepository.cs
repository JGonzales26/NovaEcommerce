using EcommerceMVC.Data;
using EcommerceMVC.Models;
using EcommerceMVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Repositories.Implementations;

public sealed class CartRepository(ApplicationDbContext db) : ICartRepository
{
    public async Task<Cart?> GetByUserIdAsync(int userId) =>
        await db.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

    public async Task<Cart?> GetByIdAsync(int id) =>
        await db.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(Cart cart) => await db.Carts.AddAsync(cart);
    public void Update(Cart cart) => db.Carts.Update(cart);
    public void RemoveItem(CartItem item) => db.CartItems.Remove(item);
    public async Task SaveChangesAsync() => await db.SaveChangesAsync();
}
