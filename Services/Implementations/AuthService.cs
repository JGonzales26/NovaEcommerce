using EcommerceMVC.Helpers;
using EcommerceMVC.Models;
using EcommerceMVC.Repositories.Interfaces;
using EcommerceMVC.Services.Interfaces;
using EcommerceMVC.ViewModels;

namespace EcommerceMVC.Services.Implementations;

public sealed class AuthService(
    IUserRepository users,
    IPasswordHasher passwordHasher,
    IValidator<RegisterViewModel> registerValidator) : IAuthService
{
    public async Task<(ServiceResult Result, AppUser? User)> LoginAsync(LoginViewModel model)
    {
        var user = await users.GetByEmailAsync(model.Email.Trim());
        if (user is null || !user.IsActive || !passwordHasher.Verify(model.Password, user.PasswordHash))
            return (ServiceResult.Failure("Credenciales invalidas."), null);

        return (ServiceResult.Success(), user);
    }

    public async Task<(ServiceResult Result, AppUser? User)> RegisterAsync(RegisterViewModel model)
    {
        var errors = registerValidator.Validate(model);
        if (errors.Count > 0)
            return (ServiceResult.Failure(string.Join(" ", errors)), null);

        if (await users.GetByEmailAsync(model.Email.Trim()) is not null)
            return (ServiceResult.Failure("Ya existe una cuenta con ese correo."), null);

        var role = await users.GetRoleByNameAsync(AppRoles.Customer);
        if (role is null)
            return (ServiceResult.Failure("No se encontro el rol Cliente."), null);

        var user = new AppUser
        {
            FullName = model.FullName.Trim(),
            Email = model.Email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHasher.Hash(model.Password),
            RoleId = role.Id
        };

        await users.AddAsync(user);
        await users.SaveChangesAsync();
        user.Role = role;

        return (ServiceResult.Success(), user);
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
{
    return await users.GetByIdAsync(id);
}

public async Task UpdateUserAsync(AppUser user)
{
    users.Update(user);
    await users.SaveChangesAsync();
}
    
    
}
