using EcommerceMVC.Repositories.Interfaces;
using EcommerceMVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceMVC.Services.Implementations;

public sealed class CategoryService(ICategoryRepository categories) : ICategoryService
{
    public async Task<IEnumerable<SelectListItem>> GetSelectListAsync(int? selectedId = null)
    {
        var items = await categories.GetAllAsync();
        return items.Select(c => new SelectListItem(c.Name, c.Id.ToString(), c.Id == selectedId)).ToList();
    }
}
