using AutoMapper;
using EcommerceMVC.DTOs;
using EcommerceMVC.Exceptions;
using EcommerceMVC.Models;
using EcommerceMVC.Repositories.Interfaces;
using EcommerceMVC.Services.Interfaces;

namespace EcommerceMVC.Services.Implementations;

public sealed class CartService(
    ICartRepository carts,
    IProductRepository products,
    IMapper mapper) : ICartService
{
    public async Task<CartDto> GetCartAsync(int userId)
    {
        var cart = await carts.GetByUserIdAsync(userId);
        return cart is null ? new CartDto() : mapper.Map<CartDto>(cart);
    }

    public async Task AddItemAsync(int userId, int productId, int quantity)
    {
        var product = await products.GetByIdAsync(productId)
            ?? throw new NotFoundException("Producto no encontrado.");

        if (quantity < 1)
            throw new ValidationException("La cantidad debe ser al menos 1.");

        if (product.Stock < quantity)
            throw new ValidationException("Stock insuficiente.");

        var cart = await carts.GetByUserIdAsync(userId);

        if (cart is null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            await carts.AddAsync(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price
            });
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await carts.SaveChangesAsync();
    }

    public async Task UpdateQuantityAsync(int userId, int productId, int quantity)
    {
        if (quantity <= 0)
        {
            await RemoveItemAsync(userId, productId);
            return;
        }

        var cart = await carts.GetByUserIdAsync(userId)
            ?? throw new NotFoundException("Carrito no encontrado.");

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new NotFoundException("Producto no encontrado en el carrito.");

        var product = await products.GetByIdAsync(productId);
        if (product is not null && product.Stock < quantity)
            throw new ValidationException("Stock insuficiente.");

        item.Quantity = quantity;
        cart.UpdatedAt = DateTime.UtcNow;
        await carts.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(int userId, int productId)
    {
        var cart = await carts.GetByUserIdAsync(userId)
            ?? throw new NotFoundException("Carrito no encontrado.");

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new NotFoundException("Producto no encontrado en el carrito.");

        carts.RemoveItem(item);
        cart.UpdatedAt = DateTime.UtcNow;
        await carts.SaveChangesAsync();
    }
}
