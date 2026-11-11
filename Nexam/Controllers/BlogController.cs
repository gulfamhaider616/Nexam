using Microsoft.AspNetCore.Mvc;

namespace Nexam.Controllers;

public class BlogController : Controller
{
    public IActionResult ShowAllBlogs() => View();

    public IActionResult WriteBlog() => View();

    // Blogging is not implemented yet (it never persisted anything); it just returns to the list.
    public IActionResult SaveBlog(string blogid, string uname, string email, string country, string title, string desp) =>
        RedirectToAction("ShowAllBlogs");
}
