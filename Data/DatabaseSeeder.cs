using EcommerceMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        if (!await db.Roles.AnyAsync())
        {
            db.Roles.AddRange(
                new AppRole { Name = AppRoles.Admin },
                new AppRole { Name = AppRoles.Customer });
            await db.SaveChangesAsync();
        }

        if (!await db.Categories.AnyAsync())
        {
            db.Categories.AddRange(
                new Category { Name = "Tecnologia", Description = "Equipos y accesorios para productividad diaria." },
                new Category { Name = "Hogar", Description = "Productos utiles para casa y oficina." },
                new Category { Name = "Moda", Description = "Prendas y accesorios contemporaneos." });
            await db.SaveChangesAsync();
        }

        if (!await db.Products.AnyAsync())
        {
            var techId = await db.Categories.Where(c => c.Name == "Tecnologia").Select(c => c.Id).FirstAsync();
            var homeId = await db.Categories.Where(c => c.Name == "Hogar").Select(c => c.Id).FirstAsync();
            var fashionId = await db.Categories.Where(c => c.Name == "Moda").Select(c => c.Id).FirstAsync();

            db.Products.AddRange(
                new Product { Name = "Audifonos Studio", Description = "Cancelacion de ruido, bateria de larga duracion y sonido balanceado.", Price = 249.90m, Stock = 24, CategoryId = techId, ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=900&q=80" },
                new Product { Name = "Mochila Urbana", Description = "Compartimentos protegidos, tela resistente y diseno minimalista.", Price = 139.00m, Stock = 38, CategoryId = fashionId, ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?auto=format&fit=crop&w=900&q=80" },
                new Product { Name = "Lampara Desk", Description = "Luz regulable para escritorio con acabado metalico.", Price = 89.50m, Stock = 17, CategoryId = homeId, ImageUrl = "https://images.unsplash.com/photo-1507473885765-e6ed057f782c?auto=format&fit=crop&w=900&q=80" },
                new Product { Name = "Smart Watch Pulse", Description = "Monitoreo diario, GPS y resistencia al agua.", Price = 319.90m, Stock = 12, CategoryId = techId, ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=900&q=80" });
            await db.SaveChangesAsync();
        }

        if (!await db.Users.AnyAsync())
        {
            var adminRoleId = await db.Roles.Where(r => r.Name == AppRoles.Admin).Select(r => r.Id).FirstAsync();
            var customerRoleId = await db.Roles.Where(r => r.Name == AppRoles.Customer).Select(r => r.Id).FirstAsync();

            db.Users.AddRange(
                new AppUser { FullName = "Administrador", Email = "admin@ecommerce.local", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"), RoleId = adminRoleId },
                new AppUser { FullName = "Cliente Demo", Email = "cliente@ecommerce.local", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Cliente123!"), RoleId = customerRoleId });
            await db.SaveChangesAsync();
        }
    }
}
