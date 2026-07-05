using EcommerceMVC.Data;
using EcommerceMVC.DTOs;
using EcommerceMVC.Helpers;
using EcommerceMVC.Models;
using EcommerceMVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Repositories.Implementations;

public sealed class ProductRepository(ApplicationDbContext db) : IProductRepository
{
    public async Task<PagedResult<Product>> GetPagedAsync(ProductQueryDto query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 48);
        var products = db.Products.AsNoTracking().Include(p => p.Category).Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
        }

        if (query.CategoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == query.CategoryId.Value);
        }

        var total = await products.CountAsync();
        var items = await products
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Product> { Items = items, Page = page, PageSize = pageSize, TotalItems = total };
    }

    public async Task<IReadOnlyList<Product>> GetFeaturedAsync(int count) =>
        await db.Products.AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.Stock > 0)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Product product) => await db.Products.AddAsync(product);
    public void Update(Product product) => db.Products.Update(product);
    public void Remove(Product product) => db.Products.Remove(product);
    public async Task<bool> ExistsAsync(int id) => await db.Products.AnyAsync(p => p.Id == id);
    public async Task SaveChangesAsync() => await db.SaveChangesAsync();
}
