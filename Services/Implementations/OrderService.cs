using EcommerceMVC.DTOs;
using EcommerceMVC.Exceptions;
using EcommerceMVC.Models;
using EcommerceMVC.Repositories.Interfaces;
using EcommerceMVC.Services.Interfaces;

namespace EcommerceMVC.Services.Implementations;

public sealed class OrderService(ICartRepository cart, IOrderRepository orders, IProductRepository products) : IOrderService
{
    public async Task<Order> CheckoutAsync(int userId, string sessionId, CheckoutDto checkout)
    {
        var cartItems = await cart.GetItemsAsync(userId, sessionId);
        if (cartItems.Count() == 0) throw new ValidationException("El carrito esta vacio.");

        foreach (var item in cartItems)
        {
            if (!item.Product.IsActive || item.Product.Stock < item.Quantity)
                throw new ValidationException($"Stock insuficiente para {item.Product.Name}.");
        }

        var order = new Order
        {
            UserId = userId,
            ShippingFullName = checkout.FullName.Trim(),
            ShippingAddress = checkout.Address.Trim(),
            ShippingCity = checkout.City.Trim(),
            ShippingPhone = checkout.Phone.Trim(),
            Status = OrderStatus.Pending
        };

        foreach (var item in cartItems)
        {
            order.Details.Add(new OrderDetail
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                UnitPrice = item.Product.Price,
                Quantity = item.Quantity
            });

            item.Product.Stock -= item.Quantity;
            products.Update(item.Product);
        }

        order.Total = order.Details.Sum(d => d.UnitPrice * d.Quantity);
        await orders.AddAsync(order);
        cart.RemoveRange(cartItems);
        await orders.SaveChangesAsync();
        return order;
    }

    public async Task<IReadOnlyList<Order>> GetUserOrdersAsync(int userId) => await orders.GetByUserAsync(userId);
    public async Task<IReadOnlyList<Order>> GetAllAsync() => await orders.GetAllAsync();
}
