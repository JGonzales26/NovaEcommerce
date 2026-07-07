using EcommerceMVC.Data;
using EcommerceMVC.Services.Interfaces;
using EcommerceMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Services.Implementations;

public sealed class AdminService(ApplicationDbContext db) : IAdminService
{
    public async Task<AdminDashboardViewModel> GetDashboardAsync() => new()
    {
        ProductCount = await db.Products.CountAsync(p => p.IsActive),
        UserCount = await db.Users.CountAsync(u => u.IsActive),
        OrderCount = await db.Orders.CountAsync(),
        Revenue = (decimal)(await db.Orders.SumAsync(o => (double?)o.Total) ?? 0)
    };
}
