using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.ViewModels;

public sealed class CheckoutViewModel
{
    [Required, StringLength(120), Display(Name = "Nombre completo")]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(240), Display(Name = "Direccion")]
    public string Address { get; set; } = string.Empty;

    [Required, StringLength(80), Display(Name = "Ciudad")]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(40), Phone, Display(Name = "Telefono")]
    public string Phone { get; set; } = string.Empty;
}
