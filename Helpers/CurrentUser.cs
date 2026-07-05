using System.Security.Claims;

namespace EcommerceMVC.Helpers;

public static class CurrentUser
{
    public static int? GetUserId(ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out var id) ? id : null;
    }
}
