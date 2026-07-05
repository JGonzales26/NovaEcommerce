using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.Models;

public sealed class AppRole
{
    public int Id { get; set; }

    [Required, MaxLength(40)]
    public string Name { get; set; } = string.Empty;

    public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}
