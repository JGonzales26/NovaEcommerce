using System.Security.Claims;
using EcommerceMVC.Services.Interfaces;
using EcommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // ➔ Soluciona [Authorize]
using EcommerceMVC.Models;                 // ➔ Soluciona 'AppUser'
using System.Threading.Tasks;              // ➔ Por si falta para el Task

namespace EcommerceMVC.Controllers;

public sealed class AccountController(IAuthService auth) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var (result, user) = await auth.LoginAsync(model);
        if (!result.Succeeded || user is null)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "No se pudo iniciar sesion.");
            return View(model);
        }

        await SignInAsync(user.Id, user.FullName, user.Email, user.Role.Name, model.RememberMe);
        return LocalRedirect(returnUrl ?? Url.Action("Index", "Home")!);
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (result, user) = await auth.RegisterAsync(model);
        if (!result.Succeeded || user is null)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "No se pudo registrar el usuario.");
            return View(model);
        }

        await SignInAsync(user.Id, user.FullName, user.Email, user.Role.Name, false);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    [Authorize]
    [Route("/Account/Profile")]
    public async Task<IActionResult> Profile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
        {
            return RedirectToAction("Login");
        }

        var user = await auth.GetUserByIdAsync(userId);
        if (user is null)
        {
            return NotFound("Usuario no encontrado.");
        }

        ViewData["FullName"] = user.FullName;
        ViewData["Email"] = user.Email;
        ViewData["Role"] = user.Role?.Name ?? "Cliente";
        ViewData["PasswordHash"] = "••••••••••••"; 

        return View();
    }

    [HttpPost]
    [Authorize]
    [Route("/Account/UpdateProfile")]
    public async Task<IActionResult> UpdateProfile(string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
        {
            TempData["Error"] = "Todos los campos son obligatorios.";
            return RedirectToAction("Profile");
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
        {
            return RedirectToAction("Login");
        }

        var userInDb = await auth.GetUserByIdAsync(userId);
        if (userInDb is null)
        {
            return NotFound("Usuario no encontrado.");
        }

        userInDb.FullName = fullName.Trim();
        userInDb.Email = email.Trim().ToLowerInvariant();

        await auth.UpdateUserAsync(userInDb);

        var currentRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "Cliente";
        await SignInAsync(userInDb.Id, userInDb.FullName, userInDb.Email, currentRole, false);

        TempData["Success"] = "¡Perfil actualizado con éxito!";
        return RedirectToAction("Profile");
    }

    public IActionResult AccessDenied() => View();

    private async Task SignInAsync(int id, string fullName, string email, string role, bool rememberMe)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id.ToString()),
            new(ClaimTypes.Name, fullName),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = rememberMe });
    }
}
