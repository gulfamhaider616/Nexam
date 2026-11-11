using Microsoft.AspNetCore.Mvc;
using Nexam.Business.Interfaces;
using Nexam.Infrastructure;

namespace Nexam.Controllers;

/// <summary>The one login page for administrators and regular users; each role lands on its own dashboard.</summary>
public class AccountController(IAccountService accounts) : Controller
{
    private const string AdminHome = "/Admin/GetAllSubjects";
    private const string UserHome = "/User/UserDashboard";

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        // Already signed in: go straight to the right dashboard.
        if (User.IsInRole(AppRoles.Admin)) return LocalRedirect(AdminHome);
        if (User.IsInRole(AppRoles.User)) return LocalRedirect(UserHome);

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
    {
        var account = await accounts.LoginAsync(email, password);
        if (account is null)
        {
            ViewBag.Error = "That email and password don't match. Please try again.";
            ViewBag.Email = email;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        await HttpContext.SignInAsync(account.Name, account.Role, account.UserId);

        // Honour a page the visitor was sent here from (local URLs only); otherwise use the role's dashboard.
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);
        return LocalRedirect(account.Role == AppRoles.Admin ? AdminHome : UserHome);
    }
}
