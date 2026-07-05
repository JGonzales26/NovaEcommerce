using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceMVC.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<SelectListItem>> GetSelectListAsync(int? selectedId = null);
}
