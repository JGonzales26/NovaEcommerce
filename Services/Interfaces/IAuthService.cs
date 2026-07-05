using EcommerceMVC.Helpers;
using EcommerceMVC.Models;
using EcommerceMVC.ViewModels;

namespace EcommerceMVC.Services.Interfaces;

public interface IAuthService
{
    Task<(ServiceResult Result, AppUser? User)> LoginAsync(LoginViewModel model);
    Task<(ServiceResult Result, AppUser? User)> RegisterAsync(RegisterViewModel model);
}
