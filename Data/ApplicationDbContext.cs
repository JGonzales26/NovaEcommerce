using EcommerceMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<AppRole> Roles => Set<AppRole>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.Email).UseCollation("NOCASE");
            entity.HasOne(user => user.Role).WithMany(role => role.Users).HasForeignKey(user => user.RoleId);
        });

        modelBuilder.Entity<AppRole>().HasIndex(role => role.Name).IsUnique();
        modelBuilder.Entity<Category>().HasIndex(category => category.Name).IsUnique();

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(product => product.Price).HasColumnType("decimal(18,2)");
            entity.HasOne(product => product.Category).WithMany(category => category.Products).HasForeignKey(product => product.CategoryId);
        });
    }
}