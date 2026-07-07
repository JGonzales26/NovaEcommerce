using EcommerceMVC.ViewModels;

namespace EcommerceMVC.Services.Interfaces;

public interface IAdminService
{
    Task<AdminDashboardViewModel> GetDashboardAsync();
}
