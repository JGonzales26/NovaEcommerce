using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.ViewModels;

public sealed class RegisterViewModel
{
    [Required, StringLength(120, MinimumLength = 3), Display(Name = "Nombre completo")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, Display(Name = "Correo")]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8), DataType(DataType.Password), Display(Name = "Contrasena")]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Compare(nameof(Password)), Display(Name = "Confirmar contrasena")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
