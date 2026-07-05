using EcommerceMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<AppRole> Roles => Set<AppRole>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();


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

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasOne(cart => cart.User).WithMany().HasForeignKey(cart => cart.UserId);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.Property(item => item.UnitPrice).HasColumnType("decimal(18,2)");
            entity.HasOne(item => item.Cart).WithMany(cart => cart.Items).HasForeignKey(item => item.CartId);
            entity.HasOne(item => item.Product).WithMany().HasForeignKey(item => item.ProductId);
        });
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(order => order.Total).HasColumnType("decimal(18,2)");
            entity.Property(order => order.ShippingFullName).HasMaxLength(120);
            entity.Property(order => order.ShippingAddress).HasMaxLength(240);
            entity.Property(order => order.ShippingCity).HasMaxLength(80);
            entity.Property(order => order.ShippingPhone).HasMaxLength(40);
            entity.HasOne(order => order.User).WithMany(user => user.Orders).HasForeignKey(order => order.UserId);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(detail => detail.UnitPrice).HasColumnType("decimal(18,2)");
            entity.Property(detail => detail.ProductName).HasMaxLength(140);
            entity.HasOne(detail => detail.Order).WithMany(order => order.Details).HasForeignKey(detail => detail.OrderId);
            entity.HasOne(detail => detail.Product).WithMany().HasForeignKey(detail => detail.ProductId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}