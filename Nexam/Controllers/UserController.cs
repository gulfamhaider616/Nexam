using Microsoft.AspNetCore.Mvc;
using Nexam.Business.Interfaces;
using Nexam.Infrastructure;
using Nexam.Models.Entities;

namespace Nexam.Controllers;

public class UserController(IUserService users) : Controller
{
    public IActionResult User_Registration() => View();

    public async Task<IActionResult> SaveUser(string fname, string lname, string email, string password,
        string pnumber, string country, string city)
    {
        var saved = await users.RegisterAsync(new UserAccount
        {
            FName = fname, LName = lname, Email = email, Password = password,
            PNumber = pnumber, Country = country, City = city
        });
        return saved ? RedirectToAction("Login", "Account") : RedirectToAction("User_Registration");
    }

    // The old separate user login now lives at /Login (kept so bookmarks still work).
    public IActionResult UserLogin() => RedirectToActionPermanent("Login", "Account");

    public async Task<IActionResult> UserDashboard()
    {
        var userId = User.IsInRole(AppRoles.User) ? User.GetUserId() : null;
        if (userId is null)
            return RedirectToAction("Login", "Account");

        return View(await users.GetQuizzesAsync(userId.Value));
    }

    public IActionResult GetUserQuiz(int userID) => View();
}
