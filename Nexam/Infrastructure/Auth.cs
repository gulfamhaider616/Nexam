using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Nexam.Infrastructure;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
}

public static class AuthExtensions
{
    /// <summary>The signed-in site user's id (null for admins and anonymous visitors).</summary>
    public static int? GetUserId(this ClaimsPrincipal principal) =>
        int.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

    public static async Task SignInAsync(this HttpContext http, string name, string role, int? userId = null)
    {
        var claims = new List<Claim> { new(ClaimTypes.Name, name ?? ""), new(ClaimTypes.Role, role) };
        if (userId.HasValue)
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
    }
}
