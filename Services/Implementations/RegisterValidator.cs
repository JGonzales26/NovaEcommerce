using EcommerceMVC.Services.Interfaces;
using EcommerceMVC.ViewModels;

namespace EcommerceMVC.Services.Implementations;

public sealed class RegisterValidator : IValidator<RegisterViewModel>
{
    public IReadOnlyList<string> Validate(RegisterViewModel model)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(model.FullName) || model.FullName.Trim().Length < 3)
            errors.Add("Ingresa un nombre completo valido.");

        if (string.IsNullOrWhiteSpace(model.Email) || !model.Email.Contains('@'))
            errors.Add("Ingresa un correo valido.");

        if (model.Password.Length < 8 || !model.Password.Any(char.IsDigit) || !model.Password.Any(char.IsUpper))
            errors.Add("La contrasena debe tener 8 caracteres, una mayuscula y un numero.");

        if (model.Password != model.ConfirmPassword)
            errors.Add("Las contrasenas no coinciden.");

        return errors;
    }
}
