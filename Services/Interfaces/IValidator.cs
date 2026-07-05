namespace EcommerceMVC.Services.Interfaces;

public interface IValidator<in T>
{
    IReadOnlyList<string> Validate(T instance);
}
