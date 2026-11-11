using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexam.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult ShowAllBlogs()
        {
            return View();
        }
        public ActionResult WriteBlog ()
        {
            return View();
        }
        public ActionResult SaveBlog(string blogid, string uname, string email, string country, string title, string desp)
        {
            //BlogDO blog = new BlogDO();
            //blog.BlogID = Convert.ToInt32(blogid);
            //blog.UserName = uname;
            //blog.Email = email;
            //blog.Country = country;
            //blog.Title = title;
            //blog.Detail = desp;
            bool result = true;// new MCQPrepBA().SaveQuestion(blog);
            if (result)
            {
                return RedirectToAction("ShowAllBlogs");
            }
            else
            {
                return RedirectToAction("WriteBlog");
            }
        }
    }
}