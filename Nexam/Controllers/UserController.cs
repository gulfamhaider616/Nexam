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
        return RedirectToAction(saved ? "UserLogin" : "User_Registration");
    }

    public IActionResult UserLogin() => View();

    public async Task<IActionResult> VerifyUser(string email, string password)
    {
        var user = await users.VerifyAsync(email, password);
        if (user is null)
            return View("UserLogin");

        await HttpContext.SignInAsync(user.FName, AppRoles.User, user.Id);
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> UserDashboard()
    {
        var userId = User.IsInRole(AppRoles.User) ? User.GetUserId() : null;
        if (userId is null)
            return View("UserLogin");

        return View(await users.GetQuizzesAsync(userId.Value));
    }

    public IActionResult GetUserQuiz(int userID) => View();
}
