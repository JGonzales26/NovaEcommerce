using EcommerceMVC.DTOs;
using EcommerceMVC.Helpers;
using EcommerceMVC.ViewModels;

namespace EcommerceMVC.Services.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetPagedAsync(ProductQueryDto query);
    Task<IReadOnlyList<ProductDto>> GetFeaturedAsync(int count);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductFormViewModel?> GetFormAsync(int id);
    Task CreateAsync(ProductFormViewModel model);
    Task UpdateAsync(ProductFormViewModel model);
    Task DeleteAsync(int id);
}
