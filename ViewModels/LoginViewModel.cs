using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.ViewModels;

public sealed class LoginViewModel
{
    [Required, EmailAddress, Display(Name = "Correo")]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Display(Name = "Contrasena")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Recordarme")]
    public bool RememberMe { get; set; }
}
