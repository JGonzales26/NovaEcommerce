using EcommerceMVC.Services.Interfaces;
using EcommerceMVC.ViewModels;

namespace EcommerceMVC.Services.Implementations;

public sealed class ProductValidator : IValidator<ProductFormViewModel>
{
    public IReadOnlyList<string> Validate(ProductFormViewModel product)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(product.Name) || product.Name.Trim().Length < 3)
            errors.Add("El nombre debe tener al menos 3 caracteres.");

        if (string.IsNullOrWhiteSpace(product.Description) || product.Description.Trim().Length < 10)
            errors.Add("La descripcion debe tener al menos 10 caracteres.");

        if (product.Price <= 0)
            errors.Add("El precio debe ser mayor que cero.");

        if (product.Stock < 0)
            errors.Add("El stock no puede ser negativo.");

        if (!Uri.TryCreate(product.ImageUrl, UriKind.Absolute, out _))
            errors.Add("La URL de imagen no es valida.");

        if (product.CategoryId <= 0)
            errors.Add("Selecciona una categoria.");

        return errors;
    }
}
