using AutoMapper;
using EcommerceMVC.DTOs;
using EcommerceMVC.Exceptions;
using EcommerceMVC.Helpers;
using EcommerceMVC.Models;
using EcommerceMVC.Repositories.Interfaces;
using EcommerceMVC.Services.Interfaces;
using EcommerceMVC.ViewModels;

namespace EcommerceMVC.Services.Implementations;

public sealed class ProductService(
    IProductRepository products,
    ICategoryService categories,
    IValidator<ProductFormViewModel> validator,
    IMapper mapper) : IProductService
{
    public async Task<PagedResult<ProductDto>> GetPagedAsync(ProductQueryDto query)
    {
        var result = await products.GetPagedAsync(query);
        return new PagedResult<ProductDto>
        {
            Items = mapper.Map<IReadOnlyList<ProductDto>>(result.Items),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalItems = result.TotalItems
        };
    }

    public async Task<IReadOnlyList<ProductDto>> GetFeaturedAsync(int count) =>
        mapper.Map<IReadOnlyList<ProductDto>>(await products.GetFeaturedAsync(count));

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await products.GetByIdAsync(id);
        return product is null || !product.IsActive ? null : mapper.Map<ProductDto>(product);
    }

    public async Task<ProductFormViewModel?> GetFormAsync(int id)
    {
        var product = await products.GetByIdAsync(id);
        if (product is null) return null;

        var form = mapper.Map<ProductFormViewModel>(product);
        form.Categories = await categories.GetSelectListAsync(form.CategoryId);
        return form;
    }

    public async Task CreateAsync(ProductFormViewModel model)
    {
        ThrowIfInvalid(model);
        var product = mapper.Map<Product>(model);
        product.CreatedAt = DateTime.UtcNow;
        await products.AddAsync(product);
        await products.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProductFormViewModel model)
    {
        ThrowIfInvalid(model);
        var product = await products.GetByIdAsync(model.Id) ?? throw new NotFoundException("Producto no encontrado.");

        product.Name = model.Name.Trim();
        product.Description = model.Description.Trim();
        product.Price = model.Price;
        product.Stock = model.Stock;
        product.ImageUrl = model.ImageUrl.Trim();
        product.CategoryId = model.CategoryId;

        products.Update(product);
        await products.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await products.GetByIdAsync(id) ?? throw new NotFoundException("Producto no encontrado.");
        product.IsActive = false;
        products.Update(product);
        await products.SaveChangesAsync();
    }

    private void ThrowIfInvalid(ProductFormViewModel model)
    {
        var errors = validator.Validate(model);
        if (errors.Count > 0)
            throw new ValidationException(string.Join(" ", errors));
    }
}
